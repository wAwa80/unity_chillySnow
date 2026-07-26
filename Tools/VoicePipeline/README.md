# ChillySnow 陪玩语音制作期 Pipeline

运行时游戏**不**调用本目录；仅用于预生成 wav，再导入 Unity。

## 环境变量 / 本地密钥

推荐把密钥写在（二选一，已被 gitignore）：

- `Tools/.env`（你当前使用的路径）
- 或 `Tools/VoicePipeline/.env`

```text
VOLC_TTS_API_KEY=（必填，新版控制台 API Key）
VOLC_TTS_SPEAKER=zh_female_vv_uranus_bigtts
VOLC_TTS_RESOURCE_ID=seed-tts-2.0
```

`synthesize.py` 启动时会自动加载上述 `.env`；也可用本机环境变量（已有值不会被覆盖）。**禁止提交真实 Key。**

说明：控制台若展示模型名 `seed-audio-1.0`，批量合成对 Vivi（`*_uranus_bigtts`）通常仍用 Resource-Id `seed-tts-2.0`。以 `--probe` 为准。

依赖：`pip install pyyaml`

## 命令

```bash
cd Tools/VoicePipeline

# 探针（确认 Key + 音色）
python scripts/synthesize.py --probe

# 按 catalog/voice_lines.yaml 增量合成，并拷贝到 Assets/TestAudio/
python scripts/synthesize.py

# 只重做一句
python scripts/synthesize.py --only whoosh_good_01
```

## 产物

| 路径 | 说明 |
|---|---|
| `out/*.wav` | 中间输出 |
| `manifests/generated.json` | 增量 hash |
| `Assets/TestAudio/vc_*.wav` | 最终交付（User 拖进 VoiceCatalog） |

## User 挂载（Unity）

1. Create → ChillySnow → Voice Catalog  
2. 场景建空物体，挂 `VoiceCompanion` + `AudioSource`，拖 Catalog  
3. 将 `Assets/TestAudio/` 下各 wav 按槽位填入 Catalog  
4. 按计划验收 5 条手测  

密钥不要提交到 git。
