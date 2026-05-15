using System;

namespace EasyMobile
{
	[Serializable]
	public class Achievement : GameServiceItem
	{
		public Achievement(string name, string iosId, string androidId)
			: base(name, iosId, androidId)
		{
		}
	}
}
