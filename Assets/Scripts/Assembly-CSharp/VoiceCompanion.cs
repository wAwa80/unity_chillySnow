using System.Collections;
using UnityEngine;

namespace LevelMode
{
	/// <summary>
	/// 本地陪玩语音：订阅 Neuron 事件，按优先级/冷却播放 Catalog 中的短句。
	/// V1：事件旁白 + 轻陪伴（问候/里程碑/冷场/续命）；不联网。
	/// </summary>
	public class VoiceCompanion : Singleton<VoiceCompanion>
	{
		private const float WhooshCooldown = 1.2f;
		private const float IdleNudgeDelay = 8f;
		private const float FailDelay = 0.2f;
		private const float ClearDelay = 0.15f;
		private const float GreetingDelay = 0.3f;
		private const float ContinueDelay = 0.1f;
		private const int EndlessMilestoneMeters = 50;

		// TODO: [User Action] 请在编辑器 Inspector 中拖拽赋值 VoiceCatalog（Create → ChillySnow → Voice Catalog）
		[SerializeField]
		private VoiceCatalog catalog;

		// TODO: [User Action] 请在编辑器中挂专用 AudioSource（建议与本组件同节点），并拖拽赋值；勿与滑行/死亡 SFX 共用
		[SerializeField]
		private AudioSource audioSource;

		private int playingPriority = -1;
		private float whooshLastPlayTime = -999f;
		private int lastWhooshClipIndex = -1;
		private VoiceSlot lastWhooshSlot = VoiceSlot.WhooshGood;

		private bool greetedThisRun;
		private bool hadWhooshThisRun;
		private bool idleNudgedThisRun;
		private bool milestoneHalfDone;
		private int nextEndlessMilestone = EndlessMilestoneMeters;
		private int metersThisRun;
		private float runStartTime;
		private bool runActive;

		private Coroutine delayedPlay;

		/// <summary>
		/// Fever 进入边沿由 Skier 调用；不依赖 Neuron 枚举扩展。
		/// </summary>
		public static void NotifyFeverEntered()
		{
			if (i != null)
			{
				i.TryPlay(VoiceSlot.Fever, 70, 0f);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			if (audioSource == null)
			{
				audioSource = GetComponent<AudioSource>();
			}
		}

		private void Update()
		{
			// 当前句播完后释放优先级，允许同级/低级在冷却后再次触发
			if (playingPriority >= 0 && audioSource != null && !audioSource.isPlaying)
			{
				playingPriority = -1;
			}

			if (!runActive || idleNudgedThisRun || hadWhooshThisRun)
			{
				return;
			}
			if (Time.time - runStartTime >= IdleNudgeDelay)
			{
				idleNudgedThisRun = true;
				TryPlay(VoiceSlot.IdleNudge, 10, 0f);
			}
		}

		protected override void OnStartRun(Run run)
		{
			ResetRunState();
			runActive = true;
			runStartTime = Time.time;
			VoiceSlot greeting = GameMode.IsEndless ? VoiceSlot.GreetingEndless : VoiceSlot.GreetingLevel;
			if (!greetedThisRun)
			{
				greetedThisRun = true;
				TryPlay(greeting, 30, GreetingDelay);
			}
		}

		protected override void OnWhoosh(int points)
		{
			hadWhooshThisRun = true;
			int combo = Pine.GetWhooshCombo();
			if (combo <= 1)
			{
				return;
			}

			VoiceSlot slot;
			int priority;
			if (combo > 6)
			{
				slot = VoiceSlot.WhooshPerfect;
				priority = 55;
			}
			else if (combo > 3)
			{
				slot = VoiceSlot.WhooshExquisite;
				priority = 50;
			}
			else if (combo > 2)
			{
				slot = VoiceSlot.WhooshAwesome;
				priority = 45;
			}
			else
			{
				slot = VoiceSlot.WhooshGood;
				priority = 40;
			}

			// 同档冷却；跨档升级允许立即抢播（优先级更高时由门控处理）
			if (slot == lastWhooshSlot && Time.time - whooshLastPlayTime < WhooshCooldown)
			{
				return;
			}

			if (TryPlay(slot, priority, 0f))
			{
				whooshLastPlayTime = Time.time;
				lastWhooshSlot = slot;
			}
		}

		protected override void OnMeterPlusOne()
		{
			metersThisRun++;
			if (GameMode.IsEndless)
			{
				if (metersThisRun >= nextEndlessMilestone)
				{
					nextEndlessMilestone += EndlessMilestoneMeters;
					TryPlay(VoiceSlot.Milestone, 20, 0f);
				}
				return;
			}

			// 关卡：首次过半程播一次里程碑
			if (milestoneHalfDone)
			{
				return;
			}
			float total = FinishLine.GetDistance();
			if (total < 0.01f)
			{
				return;
			}
			float progress = Mathf.Clamp01((0f - Skier.GetY()) / total);
			if (progress >= 0.5f)
			{
				milestoneHalfDone = true;
				TryPlay(VoiceSlot.Milestone, 20, 0f);
			}
		}

		protected override void OnEndRun()
		{
			runActive = false;
			CancelDelayedPlay();
			Run run = Neuron.GetCurrentRun();
			if (run != null && run.success)
			{
				// 通关语音仅关卡模式
				if (GameMode.IsLevel)
				{
					TryPlay(VoiceSlot.Clear, 100, ClearDelay);
				}
			}
			else
			{
				TryPlay(VoiceSlot.Fail, 100, FailDelay);
			}
		}

		protected override void OnContinue()
		{
			runActive = true;
			runStartTime = Time.time;
			TryPlay(VoiceSlot.Continue, 80, ContinueDelay);
		}

		protected override void OnRefresh()
		{
			runActive = false;
			CancelDelayedPlay();
			StopVoice();
			ResetRunState();
		}

		private void ResetRunState()
		{
			greetedThisRun = false;
			hadWhooshThisRun = false;
			idleNudgedThisRun = false;
			milestoneHalfDone = false;
			nextEndlessMilestone = EndlessMilestoneMeters;
			metersThisRun = 0;
			whooshLastPlayTime = -999f;
			lastWhooshClipIndex = -1;
		}

		/// <summary>
		/// 门控：鼓励开关、静音、Catalog、优先级打断；可选延迟开播。
		/// </summary>
		private bool TryPlay(VoiceSlot slot, int priority, float delay)
		{
			if (!CanPlay())
			{
				return false;
			}
			if (catalog == null || audioSource == null)
			{
				return false;
			}
			AudioClip[] clips = catalog.GetClips(slot);
			if (clips == null || clips.Length == 0)
			{
				return false;
			}

			// 仅更高优先级可打断；正在播同级/更高级则丢弃
			if (playingPriority >= 0 && audioSource.isPlaying && priority <= playingPriority)
			{
				return false;
			}

			AudioClip clip = PickClip(slot, clips);
			if (clip == null)
			{
				return false;
			}

			CancelDelayedPlay();
			if (delay <= 0f)
			{
				PlayNow(clip, priority);
			}
			else
			{
				delayedPlay = StartCoroutine(PlayAfterDelay(clip, priority, delay));
			}
			return true;
		}

		private bool CanPlay()
		{
			if (!MotivationalButton.motivationalOn)
			{
				return false;
			}
			if (!Device.IsSoundOn())
			{
				return false;
			}
			return true;
		}

		private AudioClip PickClip(VoiceSlot slot, AudioClip[] clips)
		{
			if (clips.Length == 1)
			{
				return clips[0];
			}
			// Whoosh 同槽避免连续同一句
			int index = Random.Range(0, clips.Length);
			if (IsWhooshSlot(slot) && clips.Length > 1 && index == lastWhooshClipIndex)
			{
				index = (index + 1) % clips.Length;
			}
			if (IsWhooshSlot(slot))
			{
				lastWhooshClipIndex = index;
			}
			return clips[index];
		}

		private static bool IsWhooshSlot(VoiceSlot slot)
		{
			return slot == VoiceSlot.WhooshGood
				|| slot == VoiceSlot.WhooshAwesome
				|| slot == VoiceSlot.WhooshExquisite
				|| slot == VoiceSlot.WhooshPerfect;
		}

		private IEnumerator PlayAfterDelay(AudioClip clip, int priority, float delay)
		{
			yield return new WaitForSeconds(delay);
			delayedPlay = null;
			// Fail/Clear 在 EndRun 后仍需播放；其余延迟句若局已结束则取消
			if (!CanPlay())
			{
				yield break;
			}
			if (priority < 100 && !runActive && !Neuron.IsPlaying())
			{
				yield break;
			}
			PlayNow(clip, priority);
		}

		private void PlayNow(AudioClip clip, int priority)
		{
			if (audioSource == null || clip == null)
			{
				return;
			}
			audioSource.Stop();
			audioSource.clip = clip;
			audioSource.Play();
			playingPriority = priority;
		}

		private void StopVoice()
		{
			if (audioSource != null && audioSource.isPlaying)
			{
				audioSource.Stop();
			}
			playingPriority = -1;
		}

		private void CancelDelayedPlay()
		{
			if (delayedPlay != null)
			{
				StopCoroutine(delayedPlay);
				delayedPlay = null;
			}
		}
	}
}
