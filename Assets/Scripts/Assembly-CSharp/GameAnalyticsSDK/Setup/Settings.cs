using UnityEngine;
using System.Collections.Generic;

namespace GameAnalyticsSDK.Setup
{
	public class Settings : ScriptableObject
	{
		public enum InspectorStates
		{
			Account = 0,
			Basic = 1,
			Debugging = 2,
			Pref = 3,
		}

		public enum HelpTypes
		{
			None = 0,
			IncludeSystemSpecsHelp = 1,
			ProvideCustomUserID = 2,
		}

		public int TotalMessagesSubmitted;
		public int TotalMessagesFailed;
		public int DesignMessagesSubmitted;
		public int DesignMessagesFailed;
		public int QualityMessagesSubmitted;
		public int QualityMessagesFailed;
		public int ErrorMessagesSubmitted;
		public int ErrorMessagesFailed;
		public int BusinessMessagesSubmitted;
		public int BusinessMessagesFailed;
		public int UserMessagesSubmitted;
		public int UserMessagesFailed;
		public string CustomArea;
		[SerializeField]
		private List<string> gameKey;
		[SerializeField]
		private List<string> secretKey;
		[SerializeField]
		public List<string> Build;
		[SerializeField]
		public List<string> SelectedPlatformStudio;
		[SerializeField]
		public List<string> SelectedPlatformGame;
		[SerializeField]
		public List<int> SelectedPlatformGameID;
		[SerializeField]
		public List<int> SelectedStudio;
		[SerializeField]
		public List<int> SelectedGame;
		public string NewVersion;
		public string Changes;
		public bool SignUpOpen;
		public string StudioName;
		public string GameName;
		public string EmailGA;
		public bool IntroScreen;
		public bool InfoLogEditor;
		public bool InfoLogBuild;
		public bool VerboseLogBuild;
		public bool UseManualSessionHandling;
		public bool SendExampleGameDataToMyGame;
		public bool InternetConnectivity;
		public List<string> CustomDimensions01;
		public List<string> CustomDimensions02;
		public List<string> CustomDimensions03;
		public List<string> ResourceItemTypes;
		public List<string> ResourceCurrencies;
		public RuntimePlatform LastCreatedGamePlatform;
		public List<RuntimePlatform> Platforms;
		public InspectorStates CurrentInspectorState;
		public List<Settings.HelpTypes> ClosedHints;
		public bool DisplayHints;
		public Vector2 DisplayHintsScrollState;
		public Texture2D Logo;
		public Texture2D UpdateIcon;
		public Texture2D InfoIcon;
		public Texture2D DeleteIcon;
		public Texture2D GameIcon;
		public Texture2D HomeIcon;
		public Texture2D InstrumentIcon;
		public Texture2D QuestionIcon;
		public Texture2D UserIcon;
		public Texture2D AmazonIcon;
		public Texture2D GooglePlayIcon;
		public Texture2D iosIcon;
		public Texture2D macIcon;
		public Texture2D windowsPhoneIcon;
		public bool UsePlayerSettingsBuildNumber;
		public bool SubmitErrors;
		public int MaxErrorCount;
		public bool SubmitFpsAverage;
		public bool SubmitFpsCritical;
		public bool IncludeGooglePlay;
		public int FpsCriticalThreshold;
		public int FpsCirticalSubmitInterval;
		public List<bool> PlatformFoldOut;
		public bool CustomDimensions01FoldOut;
		public bool CustomDimensions02FoldOut;
		public bool CustomDimensions03FoldOut;
		public bool ResourceItemTypesFoldOut;
		public bool ResourceCurrenciesFoldOut;
	}
}
