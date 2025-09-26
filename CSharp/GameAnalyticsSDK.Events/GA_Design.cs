using System.Collections.Generic;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	public static class GA_Design
	{
		public static void NewEvent(string eventName, float eventValue, IDictionary<string, object> fields)
		{
			CreateNewEvent(eventName, eventValue, fields);
		}

		public static void NewEvent(string eventName, IDictionary<string, object> fields)
		{
			CreateNewEvent(eventName, null, fields);
		}

		private static void CreateNewEvent(string eventName, float? eventValue, IDictionary<string, object> fields)
		{
			if (eventValue.HasValue)
			{
				GA_Wrapper.AddDesignEvent(eventName, eventValue.Value, fields);
			}
			else
			{
				GA_Wrapper.AddDesignEvent(eventName, fields);
			}
		}
	}
}
