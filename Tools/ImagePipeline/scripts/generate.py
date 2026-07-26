#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
即梦图片生成 4.6（jimeng_seedream46_cvtob）制作期批量工具。

文档：https://docs.volcengine.com/docs/85621/2275082?lang=zh

用法：
  python scripts/generate.py --probe
  python scripts/generate.py --probe-ref
  python scripts/generate.py
  python scripts/generate.py --only concept_mode_btn_level
  python scripts/generate.py --force   # 忽略增量缓存强制重跑

环境变量（写入 Tools/.env，勿提交）：
  VOLC_ACCESS_KEY / VOLC_SECRET_KEY   必填（亦接受 VOLCENGINE_AK / VOLCENGINE_SK）
  VOLC_SECURITY_TOKEN                 可选，STS 临时凭证

鉴权：Header SigV4（公共参数 Header 场景），Query 仅 Action + Version。
"""

from __future__ import annotations

import argparse
import base64
import hashlib
import json
import os
import shutil
import struct
import sys
import time
import urllib.error
import urllib.parse
import urllib.request
from pathlib import Path
from typing import Any, Dict, List, Optional, Tuple

try:
    import yaml
except ImportError:
    print("缺少 PyYAML：pip install pyyaml", file=sys.stderr)
    sys.exit(1)

# 同目录签名模块
sys.path.insert(0, str(Path(__file__).resolve().parent))
from volc_sign import sign_headers  # noqa: E402

ROOT = Path(__file__).resolve().parents[1]
REPO_ROOT = ROOT.parents[1]
CATALOG_PATH = ROOT / "catalog" / "image_jobs.yaml"
STYLE_PREFIX_PATH = ROOT / "catalog" / "style_prefix.txt"
PROBE_REF_PATH = ROOT / "catalog" / "refs" / "probe_ref.png"
OUT_DIR = ROOT / "out"
MANIFEST_PATH = ROOT / "manifests" / "generated.json"
DELIVER_DIR = REPO_ROOT / "Assets" / "TestImage"

ENV_CANDIDATES = (
    REPO_ROOT / "Tools" / ".env",
    ROOT / ".env",
)

HOST = "visual.volcengineapi.com"
ENDPOINT = f"https://{HOST}/"
REGION = "cn-north-1"
SERVICE = "cv"
API_VERSION = "2022-08-31"
REQ_KEY = "jimeng_seedream46_cvtob"
ACTION_SUBMIT = "CVSync2AsyncSubmitTask"
ACTION_QUERY = "CVSync2AsyncGetResult"

# 即梦 4.6 文档限制：输入最多 14 张；单张 ≤15MB；分辨率 ≤4096。
MAX_REFS = 14
MAX_REF_BYTES = 15 * 1024 * 1024
MAX_DIM = 4096
MIN_AREA = 1024 * 1024
MAX_AREA = 4096 * 4096
DEFAULT_SIZE_AREA = 4194304  # 2048*2048，文档默认 2K 面积
ALLOWED_EXT = {".jpg", ".jpeg", ".png"}
DEFAULT_SCALE = 50  # 4.6: int [1,100]，默认 50（不是 4.0 的 0~1 浮点）

POLL_INTERVAL_SEC = 2.0
POLL_MAX_ATTEMPTS = 90  # ~3 分钟


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
            if key in os.environ and os.environ[key].strip():
                continue
            os.environ[key] = value
            loaded += 1
        print(f"已加载本地密钥文件: {path}（{loaded} 项，未覆盖已有环境变量）")


def resolve_credentials() -> Tuple[str, str, str]:
    ak = env("VOLC_ACCESS_KEY") or env("VOLCENGINE_AK")
    sk = env("VOLC_SECRET_KEY") or env("VOLCENGINE_SK")
    token = env("VOLC_SECURITY_TOKEN") or env("VOLCENGINE_TOKEN")
    if not ak or not sk:
        raise SystemExit(
            "未找到 VOLC_ACCESS_KEY / VOLC_SECRET_KEY。\n"
            "请写入 Tools/.env（勿提交）：\n"
            "  VOLC_ACCESS_KEY=你的AccessKeyId\n"
            "  VOLC_SECRET_KEY=你的SecretAccessKey"
        )
    return ak, sk, token


def load_style_prefix() -> str:
    """读取风格前缀；忽略 # 注释行与空行（避免把说明文字送进 API prompt）。"""
    if not STYLE_PREFIX_PATH.is_file():
        return ""
    lines = []
    for line in STYLE_PREFIX_PATH.read_text(encoding="utf-8").splitlines():
        s = line.strip()
        if not s or s.startswith("#"):
            continue
        lines.append(s)
    return " ".join(lines).strip()


def load_catalog() -> dict:
    if not CATALOG_PATH.is_file():
        raise SystemExit(f"缺少 catalog: {CATALOG_PATH}")
    with CATALOG_PATH.open("r", encoding="utf-8") as f:
        data = yaml.safe_load(f) or {}
    jobs = data.get("jobs") or []
    if not isinstance(jobs, list) or not jobs:
        raise SystemExit("image_jobs.yaml 无 jobs")
    return data


def load_manifest() -> dict:
    if not MANIFEST_PATH.is_file():
        return {"items": {}}
    try:
        return json.loads(MANIFEST_PATH.read_text(encoding="utf-8"))
    except (OSError, json.JSONDecodeError):
        return {"items": {}}


def save_manifest(manifest: dict) -> None:
    MANIFEST_PATH.parent.mkdir(parents=True, exist_ok=True)
    MANIFEST_PATH.write_text(
        json.dumps(manifest, ensure_ascii=False, indent=2) + "\n",
        encoding="utf-8",
    )


def sha256_bytes(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def sha256_text(text: str) -> str:
    return hashlib.sha256(text.encode("utf-8")).hexdigest()


def read_png_size(data: bytes) -> Optional[Tuple[int, int]]:
    if len(data) < 24 or data[:8] != b"\x89PNG\r\n\x1a\n":
        return None
    if data[12:16] != b"IHDR":
        return None
    w, h = struct.unpack(">II", data[16:24])
    return int(w), int(h)


def read_jpeg_size(data: bytes) -> Optional[Tuple[int, int]]:
    if len(data) < 4 or data[:2] != b"\xff\xd8":
        return None
    i = 2
    n = len(data)
    while i + 9 < n:
        if data[i] != 0xFF:
            i += 1
            continue
        marker = data[i + 1]
        i += 2
        # 无长度的标记
        if marker in (0xD8, 0xD9) or (0xD0 <= marker <= 0xD7):
            continue
        if i + 2 > n:
            break
        seg_len = struct.unpack(">H", data[i : i + 2])[0]
        if seg_len < 2:
            break
        # SOF0..SOF3 / SOF5..SOF7 / SOF9..SOF11 / SOF13..SOF15
        if marker in (
            0xC0,
            0xC1,
            0xC2,
            0xC3,
            0xC5,
            0xC6,
            0xC7,
            0xC9,
            0xCA,
            0xCB,
            0xCD,
            0xCE,
            0xCF,
        ):
            if i + 7 > n:
                break
            h, w = struct.unpack(">HH", data[i + 3 : i + 7])
            return int(w), int(h)
        i += seg_len
    return None


def detect_image_size(data: bytes) -> Optional[Tuple[int, int]]:
    return read_png_size(data) or read_jpeg_size(data)


def resolve_repo_path(rel: str) -> Path:
    p = Path(rel)
    if p.is_absolute():
        return p
    return (REPO_ROOT / p).resolve()


def validate_local_ref(path: Path) -> bytes:
    if not path.is_file():
        raise FileNotFoundError(f"参考图不存在: {path}")
    ext = path.suffix.lower()
    if ext not in ALLOWED_EXT:
        raise ValueError(f"参考图仅支持 JPEG/PNG，收到: {path.name}")
    size = path.stat().st_size
    if size > MAX_REF_BYTES:
        raise ValueError(f"参考图超过 15MB: {path} ({size} bytes)")
    data = path.read_bytes()
    dims = detect_image_size(data)
    if dims is None:
        print(f"警告: 无法解析分辨率，仍提交: {path}", file=sys.stderr)
    else:
        w, h = dims
        if w > MAX_DIM or h > MAX_DIM:
            raise ValueError(f"参考图分辨率超过 {MAX_DIM}: {path} ({w}x{h})")
        if w <= 0 or h <= 0:
            raise ValueError(f"参考图分辨率非法: {path}")
    return data


def normalize_scale(value: Any) -> int:
    """
    按 4.6 文档规范化 scale：int，范围 [1, 100]，默认 50。
    兼容误写成 0~1 浮点的旧配置（自动 *100）。
    """
    if value is None or value == "":
        return DEFAULT_SCALE
    try:
        v = float(value)
    except (TypeError, ValueError) as exc:
        raise ValueError(f"scale 非法: {value!r}") from exc
    if 0 < v <= 1.0:
        v = round(v * 100)
    scaled = int(round(v))
    if scaled < 1 or scaled > 100:
        raise ValueError(f"scale 须在 [1, 100]，收到: {value}")
    return scaled


def validate_output_size(width: int, height: int) -> None:
    """宽高须同时传入才生效；面积 ∈ [1024², 4096²]，宽高比 ∈ [min_ratio, max_ratio] 默认约 [1/3, 3]。"""
    if width <= 0 or height <= 0:
        raise ValueError(f"非法宽高: {width}x{height}")
    if width > MAX_DIM or height > MAX_DIM:
        raise ValueError(f"输出宽高超过 {MAX_DIM}: {width}x{height}")
    area = width * height
    if area < MIN_AREA or area > MAX_AREA:
        raise ValueError(
            f"输出面积须在 [{MIN_AREA}, {MAX_AREA}]，当前 {width}x{height}={area}"
        )
    ratio = width / float(height)
    # 文档默认 min_ratio=1/3、max_ratio=3；允许用户通过 job 放宽，这里按宽范围 [1/16,16] 兜底
    if ratio < (1.0 / 16.0) or ratio > 16.0:
        raise ValueError(f"输出宽高比超出 [1/16, 16]: {ratio}")


def validate_size_area(size: Optional[int]) -> Optional[int]:
    if size is None:
        return None
    area = int(size)
    if area < MIN_AREA or area > MAX_AREA:
        raise ValueError(f"size 面积须在 [{MIN_AREA}, {MAX_AREA}]，收到: {area}")
    return area


def parse_refs(job: dict) -> Tuple[Optional[List[str]], Optional[List[str]], List[str]]:
    """
    解析 refs → (binary_data_base64, image_urls, hash_parts)。
    同 job 不允许混用 path 与 url。
    缺失本地 path：返回空并在外层 warning（降级文生图）。
    """
    refs = job.get("refs") or []
    if not refs:
        return None, None, []

    if not isinstance(refs, list):
        raise ValueError(f"job {job.get('id')}: refs 必须是列表")

    if len(refs) > MAX_REFS:
        raise ValueError(f"job {job.get('id')}: 参考图最多 {MAX_REFS} 张，收到 {len(refs)}")

    modes = set()
    for item in refs:
        if not isinstance(item, dict):
            raise ValueError(f"job {job.get('id')}: refs 项须为 {{path:}} 或 {{url:}}")
        has_path = bool((item.get("path") or "").strip())
        has_url = bool((item.get("url") or "").strip())
        if has_path and has_url:
            raise ValueError(f"job {job.get('id')}: 单条 ref 不能同时有 path 与 url")
        if has_path:
            modes.add("path")
        elif has_url:
            modes.add("url")
        else:
            raise ValueError(f"job {job.get('id')}: ref 缺少 path/url")

    if len(modes) > 1:
        raise ValueError(
            f"job {job.get('id')}: 同一 job 不允许混用 path 与 url，请拆成两条任务"
        )

    hash_parts: List[str] = []
    if "path" in modes:
        b64_list: List[str] = []
        missing: List[str] = []
        for item in refs:
            rel = (item.get("path") or "").strip()
            path = resolve_repo_path(rel)
            if not path.is_file():
                missing.append(rel)
                continue
            data = validate_local_ref(path)
            hash_parts.append(f"path:{rel}:{sha256_bytes(data)}")
            # 火山 Visual 通常要纯 base64，不要 data: 前缀
            b64_list.append(base64.b64encode(data).decode("ascii"))
        if missing:
            print(
                f"警告: job {job.get('id')} 参考图缺失 {missing}，降级为纯文生图",
                file=sys.stderr,
            )
            return None, None, []
        return b64_list, None, hash_parts

    urls: List[str] = []
    for item in refs:
        url = (item.get("url") or "").strip()
        urls.append(url)
        hash_parts.append(f"url:{url}")
    return None, urls, hash_parts


def build_prompt(job: dict, style_prefix: str) -> str:
    parts = []
    # 场景/背景贴图可设 skip_style_prefix: true，避免首页 UI 前缀污染
    if style_prefix and not bool(job.get("skip_style_prefix", False)):
        parts.append(style_prefix)
    prompt = (job.get("prompt") or "").strip()
    if not prompt:
        raise ValueError(f"job {job.get('id')}: 缺少 prompt")
    parts.append(prompt)
    full = "\n".join(parts).strip()
    if len(full) > 800:
        print(
            f"警告: job {job.get('id')} prompt 超过 800 字符（{len(full)}），可能异常",
            file=sys.stderr,
        )
    return full


def job_content_hash(job: dict, prompt: str, ref_hash_parts: List[str]) -> str:
    payload = {
        "req_key": REQ_KEY,
        "prompt": prompt,
        "width": int(job.get("width") or 0),
        "height": int(job.get("height") or 0),
        "size": job.get("size"),
        "force_single": bool(job.get("force_single", True)),
        "scale": normalize_scale(job.get("scale", DEFAULT_SCALE)),
        "min_ratio": job.get("min_ratio"),
        "max_ratio": job.get("max_ratio"),
        "refs": ref_hash_parts,
    }
    return sha256_text(json.dumps(payload, ensure_ascii=False, sort_keys=True))


def api_post(action: str, body_obj: dict, ak: str, sk: str, token: str) -> dict:
    query = {"Action": action, "Version": API_VERSION}
    body = json.dumps(body_obj, ensure_ascii=False, separators=(",", ":"))
    headers = sign_headers(
        method="POST",
        host=HOST,
        path="/",
        query=query,
        body=body,
        access_key=ak,
        secret_key=sk,
        region=REGION,
        service=SERVICE,
        content_type="application/json",
        security_token=token,
    )
    url = ENDPOINT + "?" + urllib.parse.urlencode(query)
    req = urllib.request.Request(
        url,
        data=body.encode("utf-8"),
        headers=headers,
        method="POST",
    )
    try:
        with urllib.request.urlopen(req, timeout=120) as resp:
            raw = resp.read().decode("utf-8")
    except urllib.error.HTTPError as exc:
        err_body = exc.read().decode("utf-8", errors="replace")
        hint = ""
        if exc.code == 401 or "Access Denied" in err_body or "50400" in err_body:
            hint = (
                "\n提示: AK/SK 可能有效但无即梦/视觉权限，或控制台未开通「即梦AI-图片生成4.6」。"
                "\n请到火山引擎控制台：开通即梦图片 4.6，并确认该 Access Key 所属账号/子用户有 cv/视觉智能权限。"
            )
        raise RuntimeError(f"HTTP {exc.code}: {err_body}{hint}") from exc
    except urllib.error.URLError as exc:
        raise RuntimeError(f"网络错误: {exc}") from exc

    try:
        return json.loads(raw)
    except json.JSONDecodeError as exc:
        raise RuntimeError(f"响应非 JSON: {raw[:500]}") from exc


def summarize_error(resp: dict) -> str:
    return (
        f"code={resp.get('code')} message={resp.get('message')} "
        f"request_id={resp.get('request_id')}"
    )


def submit_task(
    *,
    prompt: str,
    width: Optional[int],
    height: Optional[int],
    size: Optional[int],
    force_single: bool,
    scale: int,
    min_ratio: Optional[float],
    max_ratio: Optional[float],
    binary_data_base64: Optional[List[str]],
    image_urls: Optional[List[str]],
    ak: str,
    sk: str,
    token: str,
) -> str:
    """
    按 4.6 文档组装提交 Body：
      req_key / prompt / image_urls|binary_data_base64 / width&height|size /
      scale / force_single / min_ratio / max_ratio
    面积与宽高二选一；同时传时优先宽高。
    """
    body: Dict[str, Any] = {
        "req_key": REQ_KEY,
        "prompt": prompt,
        "force_single": bool(force_single),
        "scale": int(scale),
    }
    # 宽高优先；否则传 size 面积；都不传则走文档默认 4194304
    if width and height:
        body["width"] = int(width)
        body["height"] = int(height)
    elif size is not None:
        body["size"] = int(size)

    if min_ratio is not None:
        body["min_ratio"] = float(min_ratio)
    if max_ratio is not None:
        body["max_ratio"] = float(max_ratio)

    # 参考图：文档主推 image_urls（0~14）；本地文件走 binary_data_base64（视觉接口惯例）
    if binary_data_base64:
        body["binary_data_base64"] = binary_data_base64
    if image_urls:
        body["image_urls"] = image_urls

    resp = api_post(ACTION_SUBMIT, body, ak, sk, token)
    if resp.get("code") != 10000:
        raise RuntimeError(f"提交失败: {summarize_error(resp)}")
    task_id = (resp.get("data") or {}).get("task_id")
    if not task_id:
        raise RuntimeError(f"提交成功但无 task_id: {resp}")
    return str(task_id)


def poll_result(task_id: str, ak: str, sk: str, token: str) -> dict:
    body = {
        "req_key": REQ_KEY,
        "task_id": task_id,
        "req_json": json.dumps({"return_url": True}, ensure_ascii=False),
    }
    last: dict = {}
    for attempt in range(1, POLL_MAX_ATTEMPTS + 1):
        resp = api_post(ACTION_QUERY, body, ak, sk, token)
        last = resp
        code = resp.get("code")
        data = resp.get("data") or {}
        status = data.get("status") or ""

        if status in ("not_found", "expired"):
            raise RuntimeError(f"任务 {status}: {summarize_error(resp)}")

        if status == "done":
            # done 时仍可能失败：看外层 code
            if code != 10000:
                raise RuntimeError(f"生成失败: {summarize_error(resp)}")
            return data

        if code not in (10000, None) and status not in ("in_queue", "generating", ""):
            # 部分中间态仍返回 10000；非预期 code 直接失败
            if status not in ("in_queue", "generating"):
                raise RuntimeError(f"查询异常: {summarize_error(resp)} status={status}")

        if attempt == 1 or attempt % 5 == 0:
            print(f"  轮询 {attempt}/{POLL_MAX_ATTEMPTS}: status={status or '...'}")
        time.sleep(POLL_INTERVAL_SEC)

    raise RuntimeError(
        f"轮询超时（{POLL_MAX_ATTEMPTS} 次）: last={summarize_error(last)}"
    )


def download_url(url: str) -> bytes:
    req = urllib.request.Request(url, method="GET")
    with urllib.request.urlopen(req, timeout=120) as resp:
        return resp.read()


def extract_image_bytes(data: dict) -> bytes:
    urls = data.get("image_urls") or []
    if isinstance(urls, list) and urls:
        if len(urls) > 1:
            print(
                f"警告: 返回 {len(urls)} 张图，仅取第一张（force_single 预期为 1）",
                file=sys.stderr,
            )
        return download_url(str(urls[0]))

    b64s = data.get("binary_data_base64") or []
    if isinstance(b64s, str) and b64s:
        b64s = [b64s]
    if isinstance(b64s, list) and b64s:
        if len(b64s) > 1:
            print(
                f"警告: 返回 {len(b64s)} 段 base64，仅取第一张",
                file=sys.stderr,
            )
        raw = str(b64s[0])
        if "," in raw and raw.strip().startswith("data:"):
            raw = raw.split(",", 1)[1]
        return base64.b64decode(raw)

    raise RuntimeError(f"结果无 image_urls / binary_data_base64: keys={list(data.keys())}")


def deliver_image(job_id: str, filename: str, content: bytes) -> Path:
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    DELIVER_DIR.mkdir(parents=True, exist_ok=True)
    out_path = OUT_DIR / filename
    deliver_path = DELIVER_DIR / filename
    out_path.write_bytes(content)
    shutil.copy2(out_path, deliver_path)
    print(f"[OK] {job_id} -> {deliver_path.relative_to(REPO_ROOT)} ({len(content)} bytes)")
    return deliver_path


def run_generation(
    *,
    job_id: str,
    prompt: str,
    width: Optional[int],
    height: Optional[int],
    size: Optional[int],
    force_single: bool,
    scale: int,
    min_ratio: Optional[float],
    max_ratio: Optional[float],
    binary_data_base64: Optional[List[str]],
    image_urls: Optional[List[str]],
    filename: str,
    ak: str,
    sk: str,
    token: str,
) -> Path:
    if width and height:
        validate_output_size(int(width), int(height))
    elif size is not None:
        validate_size_area(int(size))
    # 都不传时使用文档默认面积，无需本地校验

    n_in = 0
    if binary_data_base64:
        n_in = len(binary_data_base64)
    elif image_urls:
        n_in = len(image_urls)
    if n_in > MAX_REFS:
        raise ValueError(f"输入图最多 {MAX_REFS} 张，收到 {n_in}")
    if n_in + 1 > 15:
        raise ValueError(f"输入图 {n_in} + 输出 1 超过 15 上限")

    size_desc = (
        f"{width}x{height}"
        if width and height
        else (f"size={size}" if size is not None else "default_2K")
    )
    print(
        f"[>] submit {job_id} ({size_desc}, refs={n_in}, "
        f"force_single={force_single}, scale={scale})"
    )
    task_id = submit_task(
        prompt=prompt,
        width=width,
        height=height,
        size=size,
        force_single=force_single,
        scale=scale,
        min_ratio=min_ratio,
        max_ratio=max_ratio,
        binary_data_base64=binary_data_base64,
        image_urls=image_urls,
        ak=ak,
        sk=sk,
        token=token,
    )
    print(f"  task_id={task_id}")
    data = poll_result(task_id, ak, sk, token)
    content = extract_image_bytes(data)
    return deliver_image(job_id, filename, content)


def run_probe(ak: str, sk: str, token: str, with_ref: bool) -> int:
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    common = dict(
        force_single=True,
        scale=DEFAULT_SCALE,
        min_ratio=None,
        max_ratio=None,
        size=None,
        width=1328,
        height=1328,
        ak=ak,
        sk=sk,
        token=token,
    )
    if with_ref:
        if not PROBE_REF_PATH.is_file():
            raise SystemExit(f"缺少探针参考图: {PROBE_REF_PATH}")
        data = validate_local_ref(PROBE_REF_PATH)
        b64 = [base64.b64encode(data).decode("ascii")]
        print("探针模式: 带参考图 (--probe-ref)")
        run_generation(
            job_id="probe_ref",
            prompt=(
                "Keep the simple orange square composition from the reference. "
                "Hyper-casual ski game UI icon, soft 3D, clean edges, single image only."
            ),
            binary_data_base64=b64,
            image_urls=None,
            filename="probe_ref_out.png",
            **common,
        )
    else:
        print("探针模式: 纯文生图 (--probe)")
        run_generation(
            job_id="probe",
            prompt=(
                "A simple orange rounded square UI button for a hyper-casual ski game, "
                "white ski icon, soft 3D bevel, light snow background, single image only."
            ),
            binary_data_base64=None,
            image_urls=None,
            filename="probe_out.png",
            **common,
        )
    return 0


def run_batch(only: Optional[str], force: bool, ak: str, sk: str, token: str) -> int:
    catalog = load_catalog()
    style_prefix = load_style_prefix()
    manifest = load_manifest()
    items: dict = manifest.setdefault("items", {})

    jobs = catalog.get("jobs") or []
    failures = 0
    ran = 0
    skipped = 0

    for job in jobs:
        job_id = (job.get("id") or "").strip()
        if not job_id:
            print("警告: 跳过无 id 的 job", file=sys.stderr)
            continue
        if only and job_id != only:
            continue

        filename = (job.get("filename") or f"{job_id}.png").strip()

        try:
            width = int(job["width"]) if job.get("width") not in (None, "") else None
            height = int(job["height"]) if job.get("height") not in (None, "") else None
            size = (
                validate_size_area(job.get("size"))
                if job.get("size") not in (None, "")
                else None
            )
            if (width is None) ^ (height is None):
                raise ValueError("width/height 必须同时传入")
            force_single = bool(job.get("force_single", True))
            scale = normalize_scale(job.get("scale", DEFAULT_SCALE))
            min_ratio = (
                float(job["min_ratio"]) if job.get("min_ratio") not in (None, "") else None
            )
            max_ratio = (
                float(job["max_ratio"]) if job.get("max_ratio") not in (None, "") else None
            )

            prompt = build_prompt(job, style_prefix)
            b64_list, url_list, ref_hash_parts = parse_refs(job)
            content_hash = job_content_hash(job, prompt, ref_hash_parts)

            prev = items.get(job_id) or {}
            deliver_path = DELIVER_DIR / filename
            if (
                not force
                and prev.get("hash") == content_hash
                and deliver_path.is_file()
            ):
                print(f"跳过 {job_id}（增量未变）")
                skipped += 1
                continue

            # 默认宽高：catalog 未写且未写 size 时用文档推荐 2K 1:1
            use_w, use_h, use_size = width, height, size
            if use_w is None and use_h is None and use_size is None:
                use_w, use_h = 2048, 2048

            run_generation(
                job_id=job_id,
                prompt=prompt,
                width=use_w,
                height=use_h,
                size=use_size,
                force_single=force_single,
                scale=scale,
                min_ratio=min_ratio,
                max_ratio=max_ratio,
                binary_data_base64=b64_list,
                image_urls=url_list,
                filename=filename,
                ak=ak,
                sk=sk,
                token=token,
            )
            items[job_id] = {
                "hash": content_hash,
                "filename": filename,
                "updated_at": time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime()),
            }
            save_manifest(manifest)
            ran += 1
        except Exception as exc:
            failures += 1
            print(f"[FAIL] {job_id}: {exc}", file=sys.stderr)

    if only and ran == 0 and skipped == 0 and failures == 0:
        print(f"未找到 id={only} 的任务", file=sys.stderr)
        return 1

    print(f"完成: 生成 {ran}，跳过 {skipped}，失败 {failures}")
    return 1 if failures else 0


def main() -> int:
    parser = argparse.ArgumentParser(description="即梦图片 4.6 制作期 Pipeline")
    parser.add_argument("--probe", action="store_true", help="纯文生图探针")
    parser.add_argument("--probe-ref", action="store_true", help="带参考图探针")
    parser.add_argument("--only", type=str, default="", help="只跑指定 job id")
    parser.add_argument("--force", action="store_true", help="忽略增量缓存")
    args = parser.parse_args()

    load_dotenv_files()
    ak, sk, token = resolve_credentials()

    if args.probe and args.probe_ref:
        print("请二选一：--probe 或 --probe-ref", file=sys.stderr)
        return 2
    if args.probe:
        return run_probe(ak, sk, token, with_ref=False)
    if args.probe_ref:
        return run_probe(ak, sk, token, with_ref=True)
    return run_batch(args.only.strip() or None, args.force, ak, sk, token)


if __name__ == "__main__":
    try:
        raise SystemExit(main())
    except KeyboardInterrupt:
        print("\n已中断", file=sys.stderr)
        raise SystemExit(130)
