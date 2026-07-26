# UI × AI 美术流水线（概念 ≠ 落地）

> 本文件是**工作流约定**，不是双模式功能施工单。落盘：`.cursor/plans/`。  
> 关联：[`AI_CONTEXT.md`](../../AI_CONTEXT.md)、[`game-ui-director.mdc`](../rules/game-ui-director.mdc)、[`ui-audit.mdc`](../rules/ui-audit.mdc)。

---

## 0. 核心结论

| 错误做法 | 正确做法 |
|----------|----------|
| Midjourney 直接「设计你的 Unity UI」 | Midjourney = **概念美术供应商**（探索气质 / Banner / Mood） |
| 先出漂亮图再硬塞进项目 | **先理解游戏上下文** → 需求文档 → 概念探索 → 切图/落地 |
| 生成图当正式 Sprite | `mock_*` / 概念图仅对照；正式资源走白名单 + 透明底 |

**一句话**：GPT/Cursor 强在「知道为什么这样画」；Midjourney 强在「画得像」。顺序不可反。

---

## 1. 目标架构

```text
                 Cursor Agent（游戏理解层）
                         |
         ---------------------------------
         |                               |
   读 Unity 实况                      写需求 / Audit
   GameMode / HUD / Canvas            尺寸·状态·九宫·动效
         |                               |
         v                               v
   UI 设计决策                     概念 Prompt（可选）
                                         |
                                         v
                          Tools/ImagePipeline（即梦 4.0，主路径）
                          或 Midjourney / Flux（可选人工探索）
                                         |
                                         v
                              Assets/TestImage/ 对照
                                         |
                                         v
                    Agent 脚本 + User 挂 Prefab/Sprite
```

当前阶段：**主路径接即梦 API 4.6**（`Tools/ImagePipeline`，`req_key=jimeng_seedream46_cvtob`，密钥 `VOLC_ACCESS_KEY` / `VOLC_SECRET_KEY`）。Midjourney 保留为可选人工工具；Cursor 仍可产出可粘贴 Prompt。

---

## 2. 角色分工

| 角色 | 负责 | 不负责 |
|------|------|--------|
| **UI Audit Agent**（`ui-audit.mdc`） | 诊断首屏/层级/商业/实现风险 | 改代码、直接出最终切图 |
| **UI Director**（`game-ui-director.mdc`） | 改版方案、Prefab 建议、资源清单、落地拆分 | 越权改场景 meta |
| **Midjourney 等** | 高级原画、Moodboard、活动 Banner、气质探索 | 懂玩法循环、Atlas、九宫、微信包体 |
| **User** | 编辑器挂载、真机确认、Style Bible 定稿 | — |

---

## 3. 推荐阶段

### 第一阶段（现在 · 本仓库）

```text
Cursor + 项目规则
  → UI Audit / 改版设计 / C# 落地
Midjourney（可选）
  → 首页概念稿、活动 Banner、风格探索
落盘
  → Assets/TestImage/
```

### 第二阶段（多款小游戏 / 系列化时再做）

```text
收集已通过资源 → 训练 LoRA（Flux/ComfyUI）
  → Prompt + my_game_style_lora 出一致新图
  → 再进 TestImage / 正式白名单
```

Unity MCP（读 Hierarchy/截图）可作为未来增强；**未成熟前不阻塞**现有人工截图 + 规则上下文。

---

## 4. 让外绘「接近本游戏」的三种手段

1. **图片上下文（最简）**：主界面截图 + 现有 Icon/角色 → Prompt 写 Keep palette / characters / theme，Improve hierarchy。仍不懂 Unity。  
2. **Visual Style Bible（推荐沉淀）**：固定色板、按钮语言（橙方 3D / 绿 SKINS）、禁用项；所有 AI 共用。可放 `Assets/TestImage/style_bible/` 或计划附录。  
3. **Flux + LoRA（系列化）**：用本包通过资源训练风格，再生成新按钮/Banner。

---

## 5. Cursor 应产出的「给 Midjourney 的输入」模板

由 Director / Audit 生成，禁止只有形容词：

```text
[Concept only — not final Unity sprite]

Game: Chilly Snow, hyper-casual ski, WeChat minigame
Keep: light cream bg, orange/green/teal accents, simple white icons,
      rounded square buttons with light 3D bottom edge
Layout intent: <局外底栏 / 顶栏 / CTA 描述>
Must not change: <玩法可读性、Tap 主路径、勿加大商城抢焦点>
Deliverable: mood/concept board, NOT production nine-slice assets
```

配套资源规格仍由 GPT 写清，例如：

```text
SwitchModeBtn:
  相对尺寸: ≈ Settings 直径 × 1.8（或与 SKINS 成对）
  状态: normal / pressed / disabled(in-run)
  两态图标: level | endless
  正式图: 透明底；mock_* 禁止进包
```

---

## 6. 标准工作顺序（防「漂亮的垃圾」）

1. **Audit**（`ui-audit`）→ 问题与优先级  
2. **Design**（`game-ui-director`）→ 布局 + 资源清单 +（可选）概念 Prompt  
3. **Concept**（Midjourney 等）→ 图入 `Assets/TestImage/`  
4. **Implement** → Agent 脚本；User Prefab/Sprite；动画用现有方案  
5. **Verify** → 真机 / 低端机；包体与 Overdraw  

错误顺序：`Midjourney 直接设计 UI → 硬塞 Unity`。

---

## 7. `[Agent 执行项]` / `[User 待办项]`（本流水线文档本身）

### Agent
- 维护本约定与规则交叉链接；按用户请求跑 Audit / 出 Prompt / 写规格  
- 不把概念图当正式资源引用进逻辑  

### User
- 在 Midjourney / 其它工具出图并拷入 `Assets/TestImage/`  
- 定稿 Style Bible；编辑器挂正式 Sprite  
- 需要时再启动 LoRA / ComfyUI 流水线  

---

## 8. 残余说明

- 仓库**无**标准 `Assets/UI`、`Assets/Spine` 目录时，文档与 Prompt 必须按实况路径写。  
- 商业工具与 MCP 生态变化快；本文件只锁「理解优先于出图」原则，具体工具可替换。
