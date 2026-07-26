# AI_CONTEXT — Chilly Snow / 微信小游戏

给 Agent 的项目速览。改 UI / 玩法 / 性能前先读本文件；细则以 `.cursor/rules/` 与 `.cursor/plans/` 为准。

---

## 1. 产品与目标

| 项 | 内容 |
|----|------|
| 类型 | 休闲滑雪 / 闪避障碍（hyper-casual） |
| 平台 | 微信小游戏（团结引擎导出） |
| 核心循环 | 局外等待 → Tap 开局 → 下滑闪避 → 死亡续命 / 通关 → 回局外 |
| 双模式 | **关卡**（有终点）↔ **无尽**（无限距离）；单场景宿主，局外单按钮切换 |
| 体验目标 | **首屏 30 秒**：看懂怎么玩、当前模式、点哪开始 |
| 性能目标 | 首包尽量 **&lt; 20MB**；低端机目标 **60fps**（保底可玩帧率优先于特效） |

---

## 2. 技术栈（以仓库实况为准）

| 层 | 选用 | 备注 |
|----|------|------|
| 引擎 | **Unity 2022.3.62t4 / 团结 1.8.2** | `ProjectSettings/ProjectVersion.txt` |
| 微信 | `com.qq.weixin.minigame` | `Packages/manifest.json` |
| UI | **UGUI**（含 TextMesh Pro） | Canvas 驱动 HUD / Continue / Settings |
| HUD 过渡 | `UIHudTransition`：协程 + AnimationCurve | **未使用 DOTween**；勿默认新增 Tween 库 |
| 骨骼 | **当前仓库无 Spine 包** | 勿按 Spine 工作流假设资源 |
| 脚本主目录 | `Assets/Scripts/Assembly-CSharp/` | 命名空间多见 `LevelMode` |
| 无尽遗留资源 | `Assets/EndlessRes/` | 合并后勿以 `MainEndlessMode` 为入口 |
| 目录实况 | 无统一 `Assets/UI`、`Assets/Spine` | 审计/设计须按实路径，禁止套用通用模板目录 |

---

## 3. 场景与架构锚点

- **唯一运行场景（目标）**：`MainLevelMode`（Build 只留此游戏场景）。
- **模式状态**：`GameMode`（Level / Endless；持久化；对局中禁止切换）。
- **HUD 过渡**：`UIHudTransition`（`HUD_Level` / `HUD_Endless` CanvasGroup；先退后进；`UserToggleMode`）。
- **玩法宿主**：`Neuron` / `Skier` / `PineGenerator` / `FinishLine` 等；场景根不要双挂 Player+Skier。
- **共用 UI（常驻 Canvas）**：`SettingsBar`、`Continue`、`Tutorial`、`Motivational`、`Score` 等；模式入口建议在 Settings 旁或与 SKINS 成对的局外底栏（以生效计划为准）。

计划权威：`.cursor/plans/双模式场景合并_*.plan.md`（及关联无尽整合 / 切图审查计划）。

---

## 4. UI / 视觉约定（摘要）

- 气质：极简 hyper-casual；浅底、高饱和功能色（橙设置/模式、绿 SKINS/静音、青进度）。
- 成对底栏钮（MODE / SKINS）语言：圆角方、轻 3D 底边、白图标 + 短标签；模式状态用互斥图标或底条短文案。
- AI 效果图 / 试切图 → `Assets/TestImage/`；**`mock_*` 仅对照，不进正式 Sprite 白名单**。
- 正式切图外围须透明；坏体积 / 非透明底禁用（见切图计划白名单）。

### 相关规则

| 场景 | 规则 |
|------|------|
| UI 改版设计（先方案后代码） | `.cursor/rules/game-ui-director.mdc` |
| UI Audit 只读诊断 | `.cursor/rules/ui-audit.mdc` |
| 概念图 / Midjourney 流水线 | `.cursor/plans/ui_art_pipeline_midjourney.plan.md` |

---

## 5. AI 美术分工（防「漂亮的垃圾」）

```text
Cursor 理解 Unity（玩法/HUD/层级/性能）
    → 写出 UI 需求与资源规格
    → Tools/ImagePipeline（即梦 4.0）或可选 Midjourney 概念探索
    → 图入 Assets/TestImage/
    → Agent 脚本 + User 挂 Prefab
```

- **制作期出图主路径**：`Tools/ImagePipeline`（即梦 `jimeng_seedream46_cvtob` / 图片生成 4.6，密钥 `Tools/.env` 的 `VOLC_ACCESS_KEY` / `VOLC_SECRET_KEY`）→ `Assets/TestImage/`。  
- **Midjourney / 外绘**：可选概念探索，**不是**本项目 UI 设计师。  
- **不要**让外绘直接「设计 Unity 游戏 UI」；它不懂循环、Atlas、九宫、微信包体。  
- 系列化后再考虑 Flux + LoRA；Unity MCP 为未来增强，不阻塞当前。

Audit 优先级默认：`P0 首屏30秒 → P1 核心点击 → P2 反馈 → P3 商业入口 → P4 动画 → P5 性能`。

---

## 6. 人机边界（强制）

| 角色 | 职责 |
|------|------|
| **Agent** | C# 逻辑、接口、`SerializeField`、终端可验证项；中文注释 |
| **User** | 场景 / Prefab / 锚点 / OnClick / Sprite 拖拽 / 动画帧；外绘出图拷贝 |

计划与交付必须拆：`[Agent 执行项]` / `[User 待办项]`。  
交接注释：`// TODO: [User Action] ...`  
规则：`.cursor/rules/human-ai-boundary.mdc`。

---

## 7. 产物路径

| 类型 | 目录 |
|------|------|
| 计划 / 技术方案 | `.cursor/plans/` |
| AI 图片 / 效果图 / 概念图 | `Assets/TestImage/` |
| 图片制作期 Pipeline | `Tools/ImagePipeline/` → 交付 `Assets/TestImage/` |
| AI / Pipeline 音频 | `Assets/TestAudio/`（`Tools/VoicePipeline/`） |

规则：`.cursor/rules/artifact-location.mdc`。

---

## 8. Agent 工作备忘

1. 改模式 / HUD 前核对 `GameMode`、`UIHudTransition` 与现行 `.plan.md`；图纸冲突时停机移交，禁止擅自改计划。
2. UI 任务：先 Audit 或 Director 方案，再实现；禁止「先全面美化」。
3. 性能：合批、减 Overdraw、防止 `TestImage` 误入包、局内隐藏无关 Canvas。
4. 涉及视觉：逻辑验证通过后交 User 真机/编辑器确认。
5. 本文件只作速览；包体/帧率若与发行配置冲突，以发行与计划为准并回写此处。
