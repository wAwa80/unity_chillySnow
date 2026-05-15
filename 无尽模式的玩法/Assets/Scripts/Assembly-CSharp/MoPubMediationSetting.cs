using System.Collections.Generic;

public class MoPubMediationSetting : Dictionary<string, object>
{
	public MoPubMediationSetting(string adVendor)
	{
		Add("adVendor", adVendor);
	}
}
