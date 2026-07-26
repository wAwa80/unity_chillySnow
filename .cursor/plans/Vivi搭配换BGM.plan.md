---
name: Vivi搭配换BGM
overview: 为已落地的 Vivi 陪玩语音包，制作/替换风格匹配的循环 BGM；含 Suno 中英提示词、听感验收标准，以及工程内试听与替换步骤。
todos:
  - id: suno-gen
    content: 用提示词在 Suno 生成 2～3 版试听（无人声、可循环）
    status: pending
  - id: pick-loop
    content: 按验收标准选型，导出并放入 Assets/TestAudio/Bgm/
    status: pending
  - id: mix-check
    content: 与 Vivi 语音叠听，调相对音量/必要时微调旁白音量
    status: pending
  - id: replace-scene
    content: User 在场景/预制体将正式 BGM 替换为定稿曲（Agent 不改场景）
    status: pending
isProject: false
---

# Vivi 陪玩语音 · 换 BGM 小计划

## 已锁定

- 陪玩音色：**Vivi 2.0**（`zh_female_vv_uranus_bigtts`），全包短句已生成。
- 现有 BGM：[`Assets/AudioClip/music.wav`](Assets/AudioClip/music.wav)（约 155s，场景/预制体挂载）。
- 目标：新 BGM **贴合 Vivi 活泼短旁白**，减少「音乐抢话 / 气质拧巴」；不改语音包内容。

## 风格方向（定调）

| 维度 | 要求 |
|---|---|
| 情绪 | 轻快、清爽、户外滑雪感；友好不吵闹 |
| 编曲 | 轻电子 / chill indie pop instrumental；干净鼓点 |
| 人声 | **禁止主唱、禁止明显哼唱**（避免和 Vivi 抢频） |
| 频段 | 中高频别堆满；留出口语旁白空间 |
| 结构 | 可无缝或近似无缝循环；目标时长 **90～150 秒**（可短于现曲） |
| 能量 | 中等偏低～中等；连击语音出现时仍听得清旁白 |

**不要**：重金属、恐怖氛围、强人声 EDM 合唱、过闷的低音炮、过甜的儿童歌。

## Suno 提示词

生成时建议：Instrumental / 无歌词；出 **2～3 条**不同 seed 再选型。若 Suno 仍带人声，加负面词并重滚。

### 英文（主推，丢进 Suno Prompt）

```text
Instrumental only, no vocals, no humming. Light cheerful electronic ski game background music, clean chill indie-pop beat, bright soft synths, gentle percussion, airy winter outdoor vibe, medium-low energy, spacious midrange for voiceover, catchy but not aggressive, seamless loop friendly, mobile game BGM, polished and simple
```

**Style / 补充标签（若有单独 Style 框）：**

```text
instrumental, game soundtrack, chill electronic, light indie pop, no vocals, winter, upbeat soft
```

**Negative / 排除（若有）：**

```text
vocals, singing, rap, choir, heavy bass drop, dubstep, metal, horror, dark, kids song, loud EDM festival
```

### 中文（备用，部分界面可用）

```text
纯器乐，无人声无人声哼唱。轻松愉快的滑雪休闲小游戏背景音乐，清爽电子+轻独立流行节奏，明亮柔和合成器，鼓点干净不吵，有冬日户外空气感，中低能量，中频留白方便旁白，朗朗上口但不炸场，适合循环播放的手机游戏 BGM
```

**负面：**

```text
人声、唱歌、说唱、合唱、重低音轰炸、暗黑恐怖、儿童歌、大型电音节
```

### 可选变体（第 2、3 条试听用）

**变体 A · 更暖一点**

```text
Instrumental only, no vocals. Warm cozy electronic ski cafe vibe, soft kick, sparkling high synth arpeggio, friendly casual mobile game loop, gentle energy, clear space for female voice lines
```

**变体 B · 更「雪道」一点**

```text
Instrumental only, no vocals. Crisp winter sports energy, light techno-pop pulse, icy soft pads, minimal melody, clean mix, medium energy, loopable casual game BGM, no drops
```

## 验收听感标准

### A. 单曲验收（只听 BGM）

1. **无人声**：全曲无歌词、无明显哼唱。  
2. **可循环**：首尾相接不突兀（或仅有可接受的 0.5s 内过渡）。  
3. **不闷不炸**：耳机/手机外放都不刺耳；低音不糊。  
4. **时长**：约 1.5～2.5 分钟；包体尽量小于现有 13MB（导出时控码率）。  
5. **情绪**：听 30 秒能感到「轻松滑雪」，而不是竞技紧张或伤感。

### B. 叠听验收（BGM + Vivi 语音，必过）

用现有句叠听，例如：

- `vc_whoosh_good_*.wav`（高频短句）  
- `vc_fever_*.wav`（稍兴奋）  
- `vc_fail_*.wav`（稍沉）  

标准：

1. **旁白可懂**：正常游戏音量下，短句吐字清晰，不必反复听。  
2. **不抢话**：BGM 旋律高潮处仍不盖过 Vivi；必要时 BGM 音量低于语音约 **20%～40%**。  
3. **气质同向**：Vivi 说「漂亮！」「冲！」时，音乐仍显得在「加油」而不是「冷场/压抑」。  
4. **连击不烦**：连续 Whoosh 时，音乐不因和语音同频段而变得刺耳。  
5. **开关逻辑**：总静音关闭后，音乐+语音都无声（沿用现有 `Device` 行为即可）。

### C. 选型规则

- 2～3 版里选 **叠听 B 项通过最多** 的一版；单曲好听但抢话的淘汰。  
- 定稿文件命名建议：`bgm_vivi_ski_loop_v1.wav`（或 `.mp3` 若体积优先）。

## 工程落盘与替换步骤

### 试听阶段（不改场景）

1. Suno 导出音频。  
2. 放入 [`Assets/TestAudio/Bgm/`](Assets/TestAudio/Bgm/)（符合产物规范：试听音频走 TestAudio）。  
3. Unity 临时 AudioSource 或系统播放器：先单曲，再与 `Assets/TestAudio/vc_*.wav` 叠听。

### 定稿替换（User）

1. 将定稿复制为正式资源（可覆盖或另存后改引用）：如 `Assets/AudioClip/music.wav` 或新文件 `music_vivi.wav`。  
2. 在 `MainLevelMode` / `LevelRoot` 等挂载 BGM 的 AudioSource 上换成新 clip；保持 Loop。  
3. 调 BGM `volume`（建议先 0.4～0.6，再对 VoiceCompanion 音量微调）。  
4. 真机/编辑器跑计划里语音验收 5 条，确认音乐+语音同时可接受。

**Agent 不改** 场景/预制体/meta（人机边界）。

## [Agent 执行项]

- 本计划落盘；需要时协助改 Suno 提示词、对比听感文字结论。  
- （可选）写一页极简叠听清单到 `Tools/VoicePipeline/README.md` 或本计划附录。  
- **不负责**在无 Suno 账号时直接出曲；出曲由 User 或后续指定工具完成。

## [User 待办项]

- 用上文提示词在 Suno 生成 2～3 版并导出。  
- 文件放入 `Assets/TestAudio/Bgm/`，按验收 A/B 选型。  
- 场景替换 BGM 引用并调音量。  
- 把「选定文件名 + 是否通过叠听」回写本计划 Backlog。

## Backlog

- 2026-07-22：已用程序合成试听曲 `Assets/TestAudio/Bgm/bgm_vivi_ski_loop_v1.wav`（96s，无人声循环向）。Suno/MusicGen API 本环境不可用；若听感不够「成品感」，请用计划内提示词在 Suno 再出一版替换。

## 残余风险

- Suno 商用授权以当时套餐为准，上线前确认许可。  
- 微信小游戏包体：优先较短循环 + 合理压缩，避免再堆 10MB+ 无损。  
- 若叠听仍抢话：优先降 BGM，而不是把 Vivi 再做「更尖」的重合成。
