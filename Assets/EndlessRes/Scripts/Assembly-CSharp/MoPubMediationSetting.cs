using System.Collections.Generic;


namespace EndlessMode
{
	public class MoPubMediationSetting : Dictionary<string, object>
	{
		public MoPubMediationSetting(string adVendor)
		{
			Add("adVendor", adVendor);
		}
	}
}
