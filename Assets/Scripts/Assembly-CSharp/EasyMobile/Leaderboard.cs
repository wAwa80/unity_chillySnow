using System;

namespace EasyMobile
{
	[Serializable]
	public class Leaderboard : GameServiceItem
	{
		public Leaderboard(string name, string iosId, string androidId)
			: base(name, iosId, androidId)
		{
		}
	}
}
