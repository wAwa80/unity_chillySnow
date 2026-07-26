---
name: 首页UI视觉方向
overview: 汇总 Chilly Snow 微信小游戏首页/首屏30秒 UI 的 Audit、视觉方向探索、参考图、提示词、决策轨迹与下一步。随每轮选型推进更新「进度」与「决策日志」。
todos:
  - id: round0-audit
    content: Round 0：项目理解 + UI Audit（已完成）
    status: completed
  - id: round1-style
    content: Round 1：视觉方向探索（Midjourney 4组参考图已评；待重跑非 moodboard 独立探索）
    status: in_progress
  - id: round1-choose
    content: Round 1 选型落盘：主方向=1组DNA+4组场景；商业=3组；氛围=2组
    status: completed
  - id: round1-rerun
    content: Round 1 修正：按 A/B/C 三方向各出独立探索图（禁止 mood board 拼板）
    status: pending
  - id: round2-home
    content: Round 2：首页 UI 概念图（强 TAP TO PLAY + MODE/SKINS 成对）
    status: pending
  - id: round3-spec
    content: Round 3：资源拆解规格（尺寸/状态/九宫/透明底）
    status: pending
  - id: round4-assets
    content: Round 4：正式切图生产（即梦/PS；mock 不进包）
    status: pending
  - id: round5-unity
    content: Round 5：Unity 落地（Agent 脚本 + User 挂 Prefab）
    status: pending
isProject: true
---

# Chilly Snow 首页 UI 视觉方向计划（Living Doc）

> **用途**：把「首页/首屏30秒」相关的 Audit、概念探索、参考图、提示词、选型决策收成一份可续写计划。  
> **落盘**：`.cursor/plans/首页UI视觉方向_chillysnow.plan.md`  
> **关联**：[`AI_CONTEXT.md`](../../AI_CONTEXT.md)、[`game-ui-director.mdc`](../rules/game-ui-director.mdc)、[`ui-audit.mdc`](../rules/ui-audit.mdc)、[`ui_art_pipeline_midjourney.plan.md`](ui_art_pipeline_midjourney.plan.md)、[`即梦图片pipeline_b237be8f.plan.md`](即梦图片pipeline_b237be8f.plan.md)、[`note.md`](../rules/note.md)  
> **维护规则**：每推进一步必须更新 §1 进度、§6 决策日志、§7 当前主方向；新图入 `Tools/ReferenceImage/` 或 `Assets/TestImage/` 后回写 §4。

---

## 1. 当前进度（2026-07-24）

| 阶段 | 状态 | 摘要 |
|------|------|------|
| Round 0 项目理解 + UI Audit | **完成** | 只读审计：主 CTA 弱、MODE 钮 `scale=3` 抢焦点、需 MODE/SKINS 成对、TAP TO PLAY 强化 |
| Round 1 视觉方向探索 | **进行中** | Midjourney 出 4 组参考图（`Tools/ReferenceImage/`）；总监已评；主方向=1组DNA+4组场景 |
| Round 2 首页 UI 概念 | **进行中** | v3 四张已生成：`mock_home_first30_v3_ball_*.png`（小球角色；待选型） |
| Round 3～5 | 未开始 | 规格 → 切图 → Unity |

**当前主方向（已选，可微调）**：

```text
主视觉 DNA：第1组（奶油白雪 / 柔和蓝灰 / 橙角色 / 白圆角 UI）
首页场景感：第4组（雪坡动线 + 可点按钮语言）
商业/皮肤储备：第3组（装备化、Q 角色、道具图标）
氛围参考：第2组（温柔雪山，不作主 UI）

一句话：
「1组的干净高级感」+「4组的玩法路径感」+「3组的皮肤商业化」；
避免纯 App/冥想软件感；强化 TAP TO PLAY、动作感、奖励感。
```

---

## 2. 总流程（固定顺序）

来源：[`note.md`](../rules/note.md) + 流水线计划。

```text
Round 0：项目理解（Cursor + GPT）
        ↓
Round 1：视觉方向探索（Midjourney / 可选即梦）
        ↓  选型落盘
Round 2：UI首页设计（Midjourney / 即梦）
        ↓
Round 3：资源拆解（GPT / Director）
        ↓
Round 4：资产生产（即梦 Pipeline / PS / Flux）
        ↓
Round 5：Unity落地（Cursor Agent 脚本 + User 挂 Prefab）
```

**禁止顺序**：Midjourney 直接当 UI 设计师 → 硬塞场景。

**工具分工**：

| 工具 | 角色 |
|------|------|
| Cursor + GPT | 理解项目、Audit、规格、Prompt、决策记录 |
| Midjourney | Round 1～2 概念探索（人工） |
| 即梦 `Tools/ImagePipeline` | 批量概念/切图主路径（制作期 API） |
| Unity | User 挂载正式 Sprite；Agent 只改 C# |

**产物路径**：

| 类型 | 目录 |
|------|------|
| 视觉探索参考图（分组命名） | `Tools/ReferenceImage/` |
| 概念/效果对照图 | `Assets/TestImage/`（`mock_*` / `concept_*`） |
| 正式 Sprite | `Assets/Texture2D/`（白名单，User 拷贝） |
| 提示词草稿 | `.cursor/rules/note.md`（流程备忘） |
| 本计划 | `.cursor/plans/首页UI视觉方向_chillysnow.plan.md` |

---

## 3. Round 0：项目理解与 UI Audit（摘要）

### 3.1 产品锚点

- 休闲滑雪 hyper-casual；微信小游戏；团结引擎 + UGUI  
- 局外 Tap 开局；双模式 Level ↔ Endless；单按钮 `SwitchModeBtn`  
- 目标：**第一次打开 30 秒**内看懂「点哪玩 / 当前模式」

### 3.2 Audit 关键问题（P0～P1）

1. **主 CTA 弱**：无稳定 `TAP TO PLAY` 视觉锚点，依赖全屏 `FingerPage` 点击。  
2. **MODE 钮过重**：`SwitchModeBtn` `128×128` + `localScale=3`，通关页抢 `Level Complete`。  
3. **风格分裂**：顶栏小圆钮 vs 内容区巨型橙方钮。  
4. **引导偏关卡**：`Tutorial` 仅 Level=1；无尽无说明。  
5. **资源风险**：`Assets/TestImage` 大图多；`mock_*` 禁止进正式包。

### 3.3 Audit 后首页设计共识（未写代码）

```text
Top：Settings | 模式 HUD | NoAds/Sound
Center：角色/小球 + TAP TO PLAY（最强）
Bottom：MODE（左，橙）+ SKINS（右，绿）成对
MODE：切换箭头 + MODE 标签 + 底条「关卡✓/无尽✓」
禁止：MODE 塞进设置区变隐形；禁止领奖台复杂图标做主符号
```

---

## 4. 参考图资产台账

### 4.1 Round 1 视觉探索（`Tools/ReferenceImage/`）

命名约定：`{组}-{序号}.png`；组1仅一张拼板记为 `1.png`。

| 文件 | 组 | 总监解读 |
|------|----|----------|
| `1.png` | **第1组** | Art Direction Mood Board：环境 / 角色 / UI语言 / 整体质感。DNA 最完整，但偏 App UI |
| `2-1.png`～`2-4.png` | **第2组** | 柔和雪山氛围、毛绒/静谧场景；壁纸感强，UI 系统弱 |
| `3-1.png`～`3-4.png` | **第3组** | Q 角色、装备/滑雪板/护目镜、奖励图标；商业化与皮肤扩展最强 |
| `4-1.png`～`4-4.png` | **第4组** | 雪坡动线、场景内嵌按钮、低多边形玩法感；首屏30秒理解最强 |

**评分结论（已选）**：

| 维度 | 排名 | 选用 |
|------|------|------|
| 首屏 30 秒体验 | 4 > 1 > 3 > 2 | 首页场景偏 4 |
| 商业化 | 3 > 1 > 4 > 2 | 皮肤/商店偏 3 |
| Unity 成本最低 | 1 > 2 > 4 > 3 | DNA/UI 规范偏 1 |
| 长期皮肤扩展 | 3 > 1 > 4 > 2 | 装备语言偏 3 |

### 4.2 早期 UI 对照（`Assets/TestImage/`）

| 文件 | 用途 | 决策 |
|------|------|------|
| `mock_hud_mode_btn_level.png` / `_endless.png` | MODE 底栏成对风格（好看） | **保留为 MODE 视觉参考** |
| `mock_mode_btn_settings_*.png` | 设置旁中等钮 + 领奖台/∞ | **否决为主方案**（偏丑、偏设置区） |
| `mock_hud_level_mode.png` / `mock_hud_endless_mode.png` | 双模式 HUD 对照 | 布局参考 |
| `mode_switch_in_settings_bar.png` | 设置条旁入口 | 计划曾推荐；视觉评审后降级 |
| `btn_mode_status_*` | 无尽/关卡状态钮试作 | 仅历史；需透明底修正经验已沉淀 |
| `mock_home_first30_*.png` / `concept_mode_btn_*.png` | 后续首页/按钮概念 | 对照用 |

### 4.3 正式切图白名单（已有，勿与 mock 混用）

`Assets/Texture2D/`：`btn_mode_level.png`、`btn_mode_endless.png`、`btn_mode_toggle.png`、`settings.png`、`noads.png`、`soundOn/Off.png` 等。正式挂载只认此类路径。

---

## 5. 提示词库（已用 / 待用）

> 权威草稿也在 [`note.md`](../rules/note.md)；本计划为归档 + 版本说明。

### 5.1 Round 1 原始 Prompt（已用 · 产出 4 组参考）

**问题**：含 `professional game art direction mood board` → Midjourney 倾向出**一张拼板**，不是多张独立探索。

```text
Concept exploration only, not UI layout.

Create a visual style exploration board for a premium casual mobile ski game called "Chilly Snow".
...
Generate a professional game art direction mood board.

--ar 16:9
--style raw
--v 7
```

完整正文见 `note.md` Round 1 段。

**教训**：

- Discord `/imagine` 默认 2×2 Grid；U1～U4 才是单张放大。  
- Prompt 禁写 `mood board` / `presentation board` /「生成 N 张拼图」若目标是独立方案。  
- 多方案 = 多条 Prompt 或多次任务，不要指望一句话出四套方向。

### 5.2 Round 1 修正 Prompt（待跑 · A/B/C）

#### Round 1-A：Apple Arcade 极简

```text
Concept exploration only.

Create a visual identity exploration for "Chilly Snow",
a premium casual mobile ski game.

Focus only on:
environment, character, UI material language.

Do not create a UI screen.
Do not create a presentation board.

Style direction:
Apple Arcade inspired, minimal, clean, peaceful, premium, soft winter atmosphere.

Explore:
- snowy mountain environment
- simple pine trees
- cute minimal skier character
- rounded soft UI materials
- button shape language
- icon style

Color palette: cream white, soft blue, warm orange accent.

Commercial mobile game art direction.

--ar 16:9
--style raw
--v 7
```

#### Round 1-B：腾讯休闲（在 A 上改 Style）

```text
Style direction:
cute casual mobile game, more playful, friendly, bright, rewarding, toy-like UI
```

#### Round 1-C：Premium Hyper Casual（在 A 上改 Style）

```text
Style direction:
premium hyper casual, minimal but addictive, strong gameplay focus, clean modern UI
```

落盘建议：`Tools/ReferenceImage/r1a-*.png`、`r1b-*.png`、`r1c-*.png`（跑完后更新 §4）。

### 5.3 Round 2 首页 UI Prompt（已写 · 未跑）

见 `note.md` Round 2 段（`--ar 9:16`）。核心约束：

- 真可玩界面，非海报/闪屏  
- TOP Settings / HUD / NoAds；CENTER TAP TO PLAY；BOTTOM MODE + SKINS  
- Keep Chilly Snow 色板；Avoid RPG / 重粒子 / 商城抢焦点  

### 5.4 Round 2 强化版 Prompt 族（待选型后用）

基于聊天结论「第一组 DNA 保留 + 强化游戏感」，建议三套首页变体：

| ID | 方向 | 用途 |
|----|------|------|
| Home-A | Apple Arcade + 游戏 CTA | 主候选：干净 + 强 TAP |
| Home-B | 腾讯休闲 | 点击率 / 奖励感 / 商业 |
| Home-C | Premium Hyper Casual | 最贴近现网极简落地 |

### 5.5 即梦 API 首页 v3（已写入 catalog · 待跑）

文件：[`Tools/ImagePipeline/catalog/image_jobs.yaml`](../../Tools/ImagePipeline/catalog/image_jobs.yaml)

| Job id | 交付文件 | 要点 |
|--------|----------|------|
| `mock_home_first30_v3_ball_level` | `Assets/TestImage/mock_home_first30_v3_ball_level.png` | 关卡 HUD + 小球 + 疏朗底栏（主推） |
| `mock_home_first30_v3_ball_spacious` | `..._spacious.png` | 更空、更强调 TAP |
| `mock_home_first30_v3_ball_endless` | `..._endless.png` | 无尽 Best HUD |
| `mock_home_first30_v3_ball_classic_pair` | `..._classic_pair.png` | 贴近现网 MODE/SKINS 方钮语言 |

相对 g4 的硬约束：
1. **角色=青绿色小球**，禁止人物滑雪者  
2. 顶栏只保留设置+去广告，禁止六宫格玻璃图标  
3. TAP 最强；MODE/SKINS 成对方钮且更小，底边留白  

跑法：

```bash
cd Tools/ImagePipeline
python scripts/generate.py --force --only mock_home_first30_v3_ball_level
python scripts/generate.py --force --only mock_home_first30_v3_ball_spacious
python scripts/generate.py --force --only mock_home_first30_v3_ball_endless
python scripts/generate.py --force --only mock_home_first30_v3_ball_classic_pair
```

### 5.5 增长对比 Prompt（`note.md` 顶部）

用于三套首页方案对比（焦点 / 理解速度 / 情绪 / 商业风险 / Unity 成本），**不要**在未出图前由 AI 直接定生死。

### 5.6 基于参考图改 Prompt 的方法（给 User）

```text
1. 选参考：Tools/ReferenceImage/某组 1～2 张上传 Midjourney 作 image prompt
2. 写 Keep：从参考抽 DNA（色、材质、角色轮廓）
3. 写 Improve：首屏层级 / TAP / MODE-SKINS / 游戏感
4. 写 Must not：mood board、App dashboard、假英文、换皮 RPG
5. 写 Deliverable：single home screen OR single style exploration（二选一，勿混）
6. 参数：首页 --ar 9:16；探索 --ar 16:9；--style raw --v 7
```

---

## 6. 决策日志（按时间）

| 日期 | 步骤 | 选择 | 否决 / 备注 |
|------|------|------|-------------|
| 较早 | MODE 钮试作 | 橙方 + 底条「无尽✓/关卡✓」 | 透明底问题已修；尺寸曾过大 |
| 较早 | 设置旁中等钮 + 领奖台 | 对照 `mock_mode_btn_settings_*` | **视觉否决**；不如 `mock_hud_mode_btn_*` |
| 较早 | MODE 布局 | 底栏与 SKINS 成对（方案 A） | 设置条旁改为备选 |
| 2026-07-24 | UI Audit | 只读报告；不改代码 | P0：TAP + MODE 权重 |
| 2026-07-24 | 首页设计稿 | 中央 CTA + 底栏 MODE/SKINS | 未写代码 |
| 2026-07-24 | Round 1 出图 | 4 组参考入 `Tools/ReferenceImage/` | Prompt 含 mood board → 组1成拼板 |
| 2026-07-24 | Round 1 选型 | **主=1组DNA+4组场景；商=3；氛=2** | 不全盘换皮 3 组 |
| 2026-07-24 | 对组1评价 | DNA 保留，但要加强游戏感 | 下一轮禁 mood board；跑 A/B/C |
| 2026-07-24 | 即梦 g4 首页 | `mock_home_first30_g4_layout.png` | 底栏拥挤、主次不清、误生成人物滑雪者 |
| 2026-07-24 | 首页 v3 Prompt | 角色强制青绿色小球；顶栏仅设置+去广告；TAP 最强；MODE/SKINS 成对方钮且更小 | 写入 `image_jobs.yaml`；注意即梦 prompt+style≤800 字 |
| 2026-07-24 | 首页 v3 出图 | 四张 OK：`mock_home_first30_v3_ball_{level,spacious,endless,classic_pair}.png` | 小球已对；底栏仍偏大；下一轮再压 MODE/SKINS 尺寸 |

---

## 7. 当前主方向（冻结草案）

### Visual DNA（来自第1组，升级游戏感）

```text
环境：奶油白雪、柔和蓝灰阴影、极简松树
角色：Q版、橙色滑雪服、圆润比例、建议动态滑姿
UI：白色圆角软 3D、蓝色辅助 icon、橙色主 CTA
情绪：干净高级 + 可点可滑（不是天气/冥想 App）
```

### 首页信息架构（来自 Audit + 设计稿）

```text
TOP     Settings | HUD(Level进度 or Endless Best) | NoAds/Sound
CENTER  滑雪角色/小球 + TAP TO PLAY（最强）
BOTTOM  MODE（橙） | SKINS（绿）
MODE    切换箭头 + MODE + 底条 关卡✓/无尽✓
```

### 需要刻意增加的「游戏感」

- TAP TO PLAY 重量感（橙、大触控、轻呼吸）  
- 雪道/雪痕/动态姿态  
- 小奖励视觉：皇冠 / Best（勿抢主 CTA）  
- MODE/SKINS 成对、可点、不压通关文案  

---

## 8. 下一步 Checklist

### User

- [ ] Round 1-A / 1-B / 1-C 各跑 Midjourney，放大 U1～U4，拷入 `Tools/ReferenceImage/`（建议 `r1a-1.png`…）  
- [ ] 从 A/B/C 各选 1～2 张「像 Chilly Snow 且有游戏感」  
- [ ] 选定后更新本文件 §1 / §6，再开 Round 2 首页（可用参考图作 image prompt）  
- [ ] 正式切图进 `Assets/Texture2D/`；mock 仅留 `Assets/TestImage/`  

### Agent（本阶段）

- [x] 汇总本计划  
- [ ] Round 1 重跑结果到位后：写 Home-A/B/C 最终 Prompt（带 Keep 参考文件名）  
- [ ] Round 2 出图后：出资源规格表 + Prefab 节点建议（仍不擅自改场景）  
- [ ] Round 5：仅在用户确认方案后改 C#  

---

## 9. Unity 落地备忘（未实施）

```text
Canvas
├── HUD_Level / HUD_Endless
├── TopUtilityBar（Settings / Premium）
├── HomeRoot（TapToPlayText / TutorialHints）
├── BottomActionRoot
│   ├── SwitchModeRoot → SwitchModeBtn（Level/Endless 图互斥）
│   └── SkinsButton
└── Continue / LevelPassed
```

- 禁止 `SwitchModeBtn.localScale=3`；用正确 SizeDelta。  
- 动画：协程曲线；勿默认 DOTween。  
- `[Agent]` 脚本显隐 / SerializeField；`[User]` 拖引用、Sprite、OnClick。  

---

## 10. 更新本文件的模板（每推进一步复制一行）

```markdown
| YYYY-MM-DD | Round X | 选择：… | 否决：… | 新图路径：… |
```

并同步：

1. §1 进度表状态  
2. §4 资产台账新文件  
3. 若 Prompt 有变，追加 §5 版本号（v2、v3…）  
