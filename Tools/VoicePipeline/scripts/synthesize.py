#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
火山豆包语音：制作期批量 TTS（新版 API Key）。
用法：
  python scripts/synthesize.py --probe
  python scripts/synthesize.py
  python scripts/synthesize.py --only whoosh_good_01

环境变量：
  VOLC_TTS_API_KEY   必填
  VOLC_TTS_SPEAKER   默认 zh_female_vv_uranus_bigtts
  VOLC_TTS_RESOURCE_ID  默认 seed-tts-2.0（Vivi/uranus 家族）
  VOLC_TTS_MODEL     可选，写入请求备注；Resource-Id 以 RESOURCE_ID 为准
"""

from __future__ import annotations

import argparse
import base64
import hashlib
import json
import os
import shutil
import sys
import uuid
from pathlib import Path

try:
    import urllib.request
    import urllib.error
except ImportError:
    raise

try:
    import yaml
except ImportError:
    print("缺少 PyYAML：pip install pyyaml", file=sys.stderr)
    sys.exit(1)

ROOT = Path(__file__).resolve().parents[1]
CATALOG = ROOT / "catalog" / "voice_lines.yaml"
OUT_DIR = ROOT / "out"
MANIFEST_PATH = ROOT / "manifests" / "generated.json"
# 按仓库产物规范：最终交付到 Assets/TestAudio/
REPO_ROOT = ROOT.parents[1]
DELIVER_DIR = REPO_ROOT / "Assets" / "TestAudio"
# 密钥优先读本地 .env（勿提交 git）：Tools/.env 或 Tools/VoicePipeline/.env
ENV_CANDIDATES = (
    REPO_ROOT / "Tools" / ".env",
    ROOT / ".env",
)

ENDPOINT = "https://openspeech.bytedance.com/api/v3/tts/unidirectional"
REQUIRED_SLOTS = [
    "GreetingLevel",
    "GreetingEndless",
    "WhooshGood",
    "WhooshAwesome",
    "WhooshExquisite",
    "WhooshPerfect",
    "Fever",
    "Milestone",
    "IdleNudge",
    "Fail",
    "Clear",
    "Continue",
]
MAX_CHARS = 18


def env(name: str, default: str = "") -> str:
    return os.environ.get(name, default).strip()


def load_dotenv_files() -> None:
    """把本地 .env 灌入 os.environ（已有环境变量不覆盖）。不打印密钥内容。"""
    for path in ENV_CANDIDATES:
        if not path.is_file():
            continue
        try:
            text = path.read_text(encoding="utf-8")
        except OSError as exc:
            print(f"警告: 无法读取 {path}: {exc}", file=sys.stderr)
            continue
        loaded = 0
        for raw in text.splitlines():
            line = raw.strip()
            if not line or line.startswith("#") or "=" not in line:
                continue
            key, _, value = line.partition("=")
            key = key.strip()
            value = value.strip().strip('"').strip("'")
            if not key:
                continue
            # 已有系统/会话环境变量优先，避免误覆盖
            if key in os.environ and os.environ[key].strip():
                continue
            os.environ[key] = value
            loaded += 1
        print(f"已加载本地密钥文件: {path}（{loaded} 项，未覆盖已有环境变量）")


def load_catalog() -> dict:
    with CATALOG.open("r", encoding="utf-8") as f:
        return yaml.safe_load(f)


def validate(data: dict) -> None:
    lines = data.get("lines") or []
    if not lines:
        raise SystemExit("voice_lines.yaml 无 lines")
    counts: dict[str, int] = {s: 0 for s in REQUIRED_SLOTS}
    ids = set()
    for row in lines:
        lid = row.get("id")
        slot = row.get("slot")
        text = (row.get("text") or "").strip()
        if not lid or not slot or not text:
            raise SystemExit(f"非法行（缺 id/slot/text）: {row}")
        if lid in ids:
            raise SystemExit(f"重复 id: {lid}")
        ids.add(lid)
        if len(text) > MAX_CHARS:
            raise SystemExit(f"{lid} 字数 {len(text)} > {MAX_CHARS}: {text}")
        if slot not in counts:
            raise SystemExit(f"{lid} 未知 slot: {slot}")
        counts[slot] += 1
    missing = [s for s, n in counts.items() if n < 2]
    if missing:
        raise SystemExit(f"以下 slot 少于 2 句: {missing}")


def content_hash(line_id: str, text: str, speaker: str, resource_id: str) -> str:
    raw = f"{line_id}|{text}|{speaker}|{resource_id}|wav|24000"
    return hashlib.sha256(raw.encode("utf-8")).hexdigest()[:16]


def load_manifest() -> dict:
    if MANIFEST_PATH.exists():
        return json.loads(MANIFEST_PATH.read_text(encoding="utf-8"))
    return {"lines": {}}


def save_manifest(manifest: dict) -> None:
    MANIFEST_PATH.parent.mkdir(parents=True, exist_ok=True)
    MANIFEST_PATH.write_text(json.dumps(manifest, ensure_ascii=False, indent=2), encoding="utf-8")


def parse_chunked_audio(raw: bytes) -> bytes:
    """解析 unidirectional chunked JSON，拼接 base64 音频。"""
    text = raw.decode("utf-8", errors="replace")
    audio = bytearray()
    decoder = json.JSONDecoder()
    idx = 0
    n = len(text)
    while idx < n:
        while idx < n and text[idx].isspace():
            idx += 1
        if idx >= n:
            break
        try:
            obj, end = decoder.raw_decode(text, idx)
        except json.JSONDecodeError:
            break
        idx = end
        if not isinstance(obj, dict):
            continue
        code = obj.get("code")
        if code not in (None, 0, 20000000):
            # 部分实现用 0 表示成功；非 0 且无 data 则报错
            if "data" not in obj and code not in (0, 20000000):
                raise RuntimeError(f"TTS 错误 code={code} msg={obj.get('message') or obj}")
        data = obj.get("data")
        if isinstance(data, str) and data:
            audio.extend(base64.b64decode(data))
        # 结束标记
        if obj.get("code") == 20000000 or obj.get("finished") is True:
            break
    if not audio:
        raise RuntimeError(f"未解析到音频，原始响应前 400 字: {text[:400]}")
    return bytes(audio)


def synthesize_one(text: str, speaker: str, resource_id: str, api_key: str) -> bytes:
    req_id = str(uuid.uuid4())
    body = {
        "user": {"uid": "chillysnow_voice_pipeline"},
        "req_params": {
            "text": text,
            "speaker": speaker,
            "audio_params": {
                "format": "wav",
                "sample_rate": 24000,
            },
        },
    }
    payload = json.dumps(body, ensure_ascii=False).encode("utf-8")
    headers = {
        "Content-Type": "application/json",
        "X-Api-Key": api_key,
        "X-Api-Resource-Id": resource_id,
        "X-Api-Request-Id": req_id,
    }
    req = urllib.request.Request(ENDPOINT, data=payload, headers=headers, method="POST")
    try:
        with urllib.request.urlopen(req, timeout=60) as resp:
            raw = resp.read()
            logid = resp.headers.get("X-Tt-Logid") or resp.headers.get("X-Tt-LogId") or ""
            if logid:
                print(f"  logid={logid}")
            return parse_chunked_audio(raw)
    except urllib.error.HTTPError as e:
        err = e.read().decode("utf-8", errors="replace")
        logid = e.headers.get("X-Tt-Logid") or ""
        raise RuntimeError(f"HTTP {e.code} logid={logid} body={err[:500]}") from e


def slot_to_filename(slot: str, line_id: str) -> str:
    # vc_{slot_snake}_{id尾}.wav
    snake = "".join(("_" + c.lower() if c.isupper() else c) for c in slot).lstrip("_")
    return f"vc_{snake}_{line_id}.wav"


def deliver_copy(src: Path) -> None:
    DELIVER_DIR.mkdir(parents=True, exist_ok=True)
    dst = DELIVER_DIR / src.name
    shutil.copy2(src, dst)
    print(f"  → {dst.relative_to(REPO_ROOT)}")


def main() -> None:
    parser = argparse.ArgumentParser(description="ChillySnow 陪玩语音批量合成")
    parser.add_argument("--probe", action="store_true", help="只合成一句探针")
    parser.add_argument("--only", type=str, default="", help="只合成指定 line id")
    parser.add_argument("--no-deliver", action="store_true", help="不拷贝到 Assets/TestAudio")
    args = parser.parse_args()

    load_dotenv_files()
    api_key = env("VOLC_TTS_API_KEY")
    if not api_key:
        raise SystemExit(
            "未找到 VOLC_TTS_API_KEY。请写入 Tools/.env 或 Tools/VoicePipeline/.env，"
            "或设置本机环境变量。"
        )

    speaker = env("VOLC_TTS_SPEAKER", "zh_female_vv_uranus_bigtts")
    resource_id = env("VOLC_TTS_RESOURCE_ID") or env("VOLC_TTS_MODEL") or "seed-tts-2.0"
    # 控制台若写 seed-audio-1.0，Vivi 仍常用 seed-tts-2.0；探针失败请改 RESOURCE_ID
    if resource_id == "seed-audio-1.0":
        print("提示: RESOURCE_ID=seed-audio-1.0，若探针失败请改 VOLC_TTS_RESOURCE_ID=seed-tts-2.0")

    data = load_catalog()
    validate(data)
    OUT_DIR.mkdir(parents=True, exist_ok=True)

    if args.probe:
        text = "漂亮！"
        print(f"探针合成 speaker={speaker} resource={resource_id} text={text}")
        audio = synthesize_one(text, speaker, resource_id, api_key)
        path = OUT_DIR / "_probe.wav"
        path.write_bytes(audio)
        print(f"探针成功: {path} ({len(audio)} bytes)")
        if not args.no_deliver:
            deliver_copy(path)
        return

    lines = data["lines"]
    if args.only:
        lines = [r for r in lines if r["id"] == args.only]
        if not lines:
            raise SystemExit(f"找不到 id: {args.only}")

    manifest = load_manifest()
    done = 0
    skipped = 0
    for row in lines:
        lid = row["id"]
        text = row["text"].strip()
        slot = row["slot"]
        h = content_hash(lid, text, speaker, resource_id)
        fname = slot_to_filename(slot, lid)
        out_path = OUT_DIR / fname
        prev = manifest.get("lines", {}).get(lid)
        if prev and prev.get("hash") == h and out_path.exists():
            print(f"跳过 {lid}（hash 未变）")
            skipped += 1
            if not args.no_deliver:
                deliver_copy(out_path)
            continue
        print(f"合成 {lid}: {text}")
        audio = synthesize_one(text, speaker, resource_id, api_key)
        out_path.write_bytes(audio)
        manifest.setdefault("lines", {})[lid] = {
            "hash": h,
            "file": fname,
            "slot": slot,
            "text": text,
            "speaker": speaker,
            "resource_id": resource_id,
        }
        done += 1
        if not args.no_deliver:
            deliver_copy(out_path)

    save_manifest(manifest)
    print(f"完成：新合成 {done}，跳过 {skipped}")


if __name__ == "__main__":
    main()
