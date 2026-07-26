using UnityEngine;

namespace LevelMode
{
	/// <summary>
	/// 陪玩语音分槽目录：各情绪/事件对应一组本地 AudioClip。
	/// 在 Project 中 Create → ChillySnow → Voice Catalog 创建资产，再拖入 VoiceCompanion。
	/// </summary>
	[CreateAssetMenu(fileName = "VoiceCatalog", menuName = "ChillySnow/Voice Catalog", order = 10)]
	public class VoiceCatalog : ScriptableObject
	{
		[Header("开局问候")]
		public AudioClip[] greetingLevel;
		public AudioClip[] greetingEndless;

		[Header("擦树连击（对齐 Motivational 档位）")]
		public AudioClip[] whooshGood;
		public AudioClip[] whooshAwesome;
		public AudioClip[] whooshExquisite;
		public AudioClip[] whooshPerfect;

		[Header("高光 / 轻陪伴 / 结算")]
		public AudioClip[] fever;
		public AudioClip[] milestone;
		public AudioClip[] idleNudge;
		public AudioClip[] fail;
		public AudioClip[] clear;
		public AudioClip[] continueRun;

		/// <summary>
		/// 按槽位名取 clip 列表；未知槽位返回 null。
		/// </summary>
		public AudioClip[] GetClips(VoiceSlot slot)
		{
			switch (slot)
			{
			case VoiceSlot.GreetingLevel:
				return greetingLevel;
			case VoiceSlot.GreetingEndless:
				return greetingEndless;
			case VoiceSlot.WhooshGood:
				return whooshGood;
			case VoiceSlot.WhooshAwesome:
				return whooshAwesome;
			case VoiceSlot.WhooshExquisite:
				return whooshExquisite;
			case VoiceSlot.WhooshPerfect:
				return whooshPerfect;
			case VoiceSlot.Fever:
				return fever;
			case VoiceSlot.Milestone:
				return milestone;
			case VoiceSlot.IdleNudge:
				return idleNudge;
			case VoiceSlot.Fail:
				return fail;
			case VoiceSlot.Clear:
				return clear;
			case VoiceSlot.Continue:
				return continueRun;
			default:
				return null;
			}
		}
	}

	/// <summary>
	/// 与 Pipeline yaml 的 slot、Catalog 字段一一对应。
	/// </summary>
	public enum VoiceSlot
	{
		GreetingLevel,
		GreetingEndless,
		WhooshGood,
		WhooshAwesome,
		WhooshExquisite,
		WhooshPerfect,
		Fever,
		Milestone,
		IdleNudge,
		Fail,
		Clear,
		Continue
	}
}
