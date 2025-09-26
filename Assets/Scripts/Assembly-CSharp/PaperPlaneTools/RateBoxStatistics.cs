using System;
using UnityEngine;

namespace PaperPlaneTools
{
	[Serializable]
	public class RateBoxStatistics
	{
		[SerializeField]
		private int sessionsCount;
		[SerializeField]
		private int customEventCount;
		[SerializeField]
		private int appInstallAt;
		[SerializeField]
		private int appLaunchAt;
		[SerializeField]
		private int dialogShownAt;
		[SerializeField]
		private bool dialogIsRejected;
		[SerializeField]
		private bool dialogIsRated;
		[SerializeField]
		private string applicationVersion;
	}
}
