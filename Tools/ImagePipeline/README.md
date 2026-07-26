# ImagePipeline — 即梦图片生成 4.6（制作期）

用火山即梦 `jimeng_seedream46_cvtob` 批量出概念/对照图，落盘到 `Assets/TestImage/`。  
**游戏运行时不会调用本工具。**

官方文档：
- [即梦图片生成 4.6 接口](https://docs.volcengine.com/docs/85621/2275082?lang=zh)
- [公共参数 · Header 鉴权](https://docs.volcengine.com/docs/6369/67268?lang=zh)

## 密钥（User）

写入 `Tools/.env`（已被 gitignore，勿提交）：

```text
VOLC_ACCESS_KEY=你的AccessKeyId
VOLC_SECRET_KEY=你的SecretAccessKey
# 可选 STS：
# VOLC_SECURITY_TOKEN=
```

也接受别名 `VOLCENGINE_AK` / `VOLCENGINE_SK`。  
注意：这与语音 TTS 的 `VOLC_TTS_API_KEY` **不是同一套**。

鉴权走 **Header SigV4**（`Authorization` + `X-Date` + `X-Content-Sha256`），Query 只带 `Action` / `Version`。

## 依赖

```bash
pip install pyyaml
```

## 用法

```bash
cd Tools/ImagePipeline
python scripts/generate.py --probe          # 纯文生图探针
python scripts/generate.py --probe-ref      # 带本地参考图探针
python scripts/generate.py                  # 跑 catalog 全部任务（增量）
python scripts/generate.py --only concept_mode_btn_level
python scripts/generate.py --force          # 忽略增量强制重跑
```

产物：
- 中间：`Tools/ImagePipeline/out/`
- 交付：`Assets/TestImage/<filename>`

## 4.6 参数要点（与文档对齐）

| 项 | 值 |
|---|---|
| Endpoint | `https://visual.volcengineapi.com` |
| Submit / Query | `CVSync2AsyncSubmitTask` / `CVSync2AsyncGetResult` |
| Version | `2022-08-31` |
| Region / Service | `cn-north-1` / `cv` |
| `req_key` | **`jimeng_seedream46_cvtob`** |
| `scale` | **int [1, 100]**，默认 **50**（不是 4.0 的 0~1 浮点） |
| `force_single` | 建议 `true`（控延迟与费用） |
| 参考图 | `image_urls` 0~14 张；本地 path 以 `binary_data_base64` 提交 |
| 宽高 | 须同时传；面积 ∈ `[1024², 4096²]`；也可只传 `size` 面积 |
| 输出 | 轮询 `req_json.return_url=true`；返回 URL 为 **png**（24h） |

## catalog 示例

```yaml
- id: my_job
  filename: my_job.png
  width: 2048
  height: 2048
  force_single: true
  scale: 50
  prompt: "..."
  refs:
    - path: Assets/TestImage/_img_preview/slice_btn_level.png
```

规则：
- 参考图仅 JPEG/PNG，单张 ≤15MB，最多 **14** 张
- 分辨率 ≤4096；建议参考图 ≤6 张
- 本地参考图缺失 → warning 并降级纯文生图
- 勿在 prompt 写「生成 N 张 / 四宫格」

## 正式资源

`mock_*` / `concept_*` 仅对照。正式 Sprite 由 User 审图后拷到 `Assets/Texture2D/` 并挂场景。

## 故障排查

| 现象 | 处理 |
|------|------|
| `未找到 VOLC_ACCESS_KEY` | 在 `Tools/.env` 填写 AK/SK 后重试 |
| `HTTP 401` / `Access Denied` / `50400` | 未开通**即梦AI-图片生成4.6**，或子用户无视觉/`cv` 权限 |
| 提交成功但轮询超时 | 提高等待或看控制台配额/欠费 |
| 参考图相关报错 | 确认 JPEG/PNG、≤15MB、≤14 张、路径相对仓库根 |
