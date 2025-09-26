using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using IronSourceJSON;
using UnityEngine;

public class IronSourceEvents : MonoBehaviour
{
	private const string ERROR_CODE = "error_code";

	private const string ERROR_DESCRIPTION = "error_description";

	private const string INSTANCE_ID_KEY = "instanceId";

	private const string PLACEMENT_KEY = "placement";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<IronSourceError> m__onRewardedVideoAdShowFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onRewardedVideoAdOpenedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onRewardedVideoAdClosedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onRewardedVideoAdStartedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onRewardedVideoAdEndedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<IronSourcePlacement> m__onRewardedVideoAdRewardedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<IronSourcePlacement> m__onRewardedVideoAdClickedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m__onRewardedVideoAvailabilityChangedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, bool> m__onRewardedVideoAvailabilityChangedDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m__onRewardedVideoAdOpenedDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m__onRewardedVideoAdClosedDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, IronSourcePlacement> m__onRewardedVideoAdRewardedDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, IronSourceError> m__onRewardedVideoAdShowFailedDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, IronSourcePlacement> m__onRewardedVideoAdClickedDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onInterstitialAdReadyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<IronSourceError> m__onInterstitialAdLoadFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onInterstitialAdOpenedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onInterstitialAdClosedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onInterstitialAdShowSucceededEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<IronSourceError> m__onInterstitialAdShowFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onInterstitialAdClickedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m__onInterstitialAdReadyDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, IronSourceError> m__onInterstitialAdLoadFailedDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m__onInterstitialAdOpenedDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m__onInterstitialAdClosedDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m__onInterstitialAdShowSucceededDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, IronSourceError> m__onInterstitialAdShowFailedDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m__onInterstitialAdClickedDemandOnlyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onInterstitialAdRewardedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onOfferwallOpenedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<IronSourceError> m__onOfferwallShowFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onOfferwallClosedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<IronSourceError> m__onGetOfferwallCreditsFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<Dictionary<string, object>> m__onOfferwallAdCreditedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m__onOfferwallAvailableEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onBannerAdLoadedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<IronSourceError> m__onBannerAdLoadFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onBannerAdClickedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onBannerAdScreenPresentedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onBannerAdScreenDismissedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m__onBannerAdLeftApplicationEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m__onSegmentReceivedEvent;

	private static event Action<IronSourceError> _onRewardedVideoAdShowFailedEvent
	{
		add
		{
			Action<IronSourceError> action = IronSourceEvents.m__onRewardedVideoAdShowFailedEvent;
			Action<IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdShowFailedEvent, (Action<IronSourceError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<IronSourceError> action = IronSourceEvents.m__onRewardedVideoAdShowFailedEvent;
			Action<IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdShowFailedEvent, (Action<IronSourceError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<IronSourceError> onRewardedVideoAdShowFailedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdShowFailedEvent == null || !IronSourceEvents._onRewardedVideoAdShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdShowFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdShowFailedEvent -= value;
			}
		}
	}

	private static event Action _onRewardedVideoAdOpenedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onRewardedVideoAdOpenedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdOpenedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onRewardedVideoAdOpenedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdOpenedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onRewardedVideoAdOpenedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdOpenedEvent == null || !IronSourceEvents._onRewardedVideoAdOpenedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdOpenedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdOpenedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdOpenedEvent -= value;
			}
		}
	}

	private static event Action _onRewardedVideoAdClosedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onRewardedVideoAdClosedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdClosedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onRewardedVideoAdClosedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdClosedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onRewardedVideoAdClosedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdClosedEvent == null || !IronSourceEvents._onRewardedVideoAdClosedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClosedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdClosedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClosedEvent -= value;
			}
		}
	}

	private static event Action _onRewardedVideoAdStartedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onRewardedVideoAdStartedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdStartedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onRewardedVideoAdStartedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdStartedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onRewardedVideoAdStartedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdStartedEvent == null || !IronSourceEvents._onRewardedVideoAdStartedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdStartedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdStartedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdStartedEvent -= value;
			}
		}
	}

	private static event Action _onRewardedVideoAdEndedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onRewardedVideoAdEndedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdEndedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onRewardedVideoAdEndedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdEndedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onRewardedVideoAdEndedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdEndedEvent == null || !IronSourceEvents._onRewardedVideoAdEndedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdEndedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdEndedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdEndedEvent -= value;
			}
		}
	}

	private static event Action<IronSourcePlacement> _onRewardedVideoAdRewardedEvent
	{
		add
		{
			Action<IronSourcePlacement> action = IronSourceEvents.m__onRewardedVideoAdRewardedEvent;
			Action<IronSourcePlacement> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdRewardedEvent, (Action<IronSourcePlacement>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<IronSourcePlacement> action = IronSourceEvents.m__onRewardedVideoAdRewardedEvent;
			Action<IronSourcePlacement> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdRewardedEvent, (Action<IronSourcePlacement>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<IronSourcePlacement> onRewardedVideoAdRewardedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdRewardedEvent == null || !IronSourceEvents._onRewardedVideoAdRewardedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdRewardedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdRewardedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdRewardedEvent -= value;
			}
		}
	}

	private static event Action<IronSourcePlacement> _onRewardedVideoAdClickedEvent
	{
		add
		{
			Action<IronSourcePlacement> action = IronSourceEvents.m__onRewardedVideoAdClickedEvent;
			Action<IronSourcePlacement> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdClickedEvent, (Action<IronSourcePlacement>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<IronSourcePlacement> action = IronSourceEvents.m__onRewardedVideoAdClickedEvent;
			Action<IronSourcePlacement> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdClickedEvent, (Action<IronSourcePlacement>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<IronSourcePlacement> onRewardedVideoAdClickedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdClickedEvent == null || !IronSourceEvents._onRewardedVideoAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClickedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClickedEvent -= value;
			}
		}
	}

	private static event Action<bool> _onRewardedVideoAvailabilityChangedEvent
	{
		add
		{
			Action<bool> action = IronSourceEvents.m__onRewardedVideoAvailabilityChangedEvent;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAvailabilityChangedEvent, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = IronSourceEvents.m__onRewardedVideoAvailabilityChangedEvent;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAvailabilityChangedEvent, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> onRewardedVideoAvailabilityChangedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAvailabilityChangedEvent == null || !IronSourceEvents._onRewardedVideoAvailabilityChangedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAvailabilityChangedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAvailabilityChangedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAvailabilityChangedEvent -= value;
			}
		}
	}

	private static event Action<string, bool> _onRewardedVideoAvailabilityChangedDemandOnlyEvent
	{
		add
		{
			Action<string, bool> action = IronSourceEvents.m__onRewardedVideoAvailabilityChangedDemandOnlyEvent;
			Action<string, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAvailabilityChangedDemandOnlyEvent, (Action<string, bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, bool> action = IronSourceEvents.m__onRewardedVideoAvailabilityChangedDemandOnlyEvent;
			Action<string, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAvailabilityChangedDemandOnlyEvent, (Action<string, bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, bool> onRewardedVideoAvailabilityChangedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAvailabilityChangedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAvailabilityChangedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAvailabilityChangedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAvailabilityChangedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAvailabilityChangedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onRewardedVideoAdOpenedDemandOnlyEvent
	{
		add
		{
			Action<string> action = IronSourceEvents.m__onRewardedVideoAdOpenedDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdOpenedDemandOnlyEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = IronSourceEvents.m__onRewardedVideoAdOpenedDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdOpenedDemandOnlyEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onRewardedVideoAdOpenedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdOpenedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdOpenedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onRewardedVideoAdClosedDemandOnlyEvent
	{
		add
		{
			Action<string> action = IronSourceEvents.m__onRewardedVideoAdClosedDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdClosedDemandOnlyEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = IronSourceEvents.m__onRewardedVideoAdClosedDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdClosedDemandOnlyEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onRewardedVideoAdClosedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClosedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClosedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string, IronSourcePlacement> _onRewardedVideoAdRewardedDemandOnlyEvent
	{
		add
		{
			Action<string, IronSourcePlacement> action = IronSourceEvents.m__onRewardedVideoAdRewardedDemandOnlyEvent;
			Action<string, IronSourcePlacement> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdRewardedDemandOnlyEvent, (Action<string, IronSourcePlacement>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, IronSourcePlacement> action = IronSourceEvents.m__onRewardedVideoAdRewardedDemandOnlyEvent;
			Action<string, IronSourcePlacement> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdRewardedDemandOnlyEvent, (Action<string, IronSourcePlacement>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, IronSourcePlacement> onRewardedVideoAdRewardedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdRewardedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdRewardedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string, IronSourceError> _onRewardedVideoAdShowFailedDemandOnlyEvent
	{
		add
		{
			Action<string, IronSourceError> action = IronSourceEvents.m__onRewardedVideoAdShowFailedDemandOnlyEvent;
			Action<string, IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdShowFailedDemandOnlyEvent, (Action<string, IronSourceError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, IronSourceError> action = IronSourceEvents.m__onRewardedVideoAdShowFailedDemandOnlyEvent;
			Action<string, IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdShowFailedDemandOnlyEvent, (Action<string, IronSourceError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, IronSourceError> onRewardedVideoAdShowFailedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdShowFailedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdShowFailedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string, IronSourcePlacement> _onRewardedVideoAdClickedDemandOnlyEvent
	{
		add
		{
			Action<string, IronSourcePlacement> action = IronSourceEvents.m__onRewardedVideoAdClickedDemandOnlyEvent;
			Action<string, IronSourcePlacement> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdClickedDemandOnlyEvent, (Action<string, IronSourcePlacement>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, IronSourcePlacement> action = IronSourceEvents.m__onRewardedVideoAdClickedDemandOnlyEvent;
			Action<string, IronSourcePlacement> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onRewardedVideoAdClickedDemandOnlyEvent, (Action<string, IronSourcePlacement>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, IronSourcePlacement> onRewardedVideoAdClickedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClickedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClickedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action _onInterstitialAdReadyEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onInterstitialAdReadyEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdReadyEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onInterstitialAdReadyEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdReadyEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onInterstitialAdReadyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdReadyEvent == null || !IronSourceEvents._onInterstitialAdReadyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdReadyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdReadyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdReadyEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onInterstitialAdLoadFailedEvent
	{
		add
		{
			Action<IronSourceError> action = IronSourceEvents.m__onInterstitialAdLoadFailedEvent;
			Action<IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdLoadFailedEvent, (Action<IronSourceError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<IronSourceError> action = IronSourceEvents.m__onInterstitialAdLoadFailedEvent;
			Action<IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdLoadFailedEvent, (Action<IronSourceError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<IronSourceError> onInterstitialAdLoadFailedEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdLoadFailedEvent == null || !IronSourceEvents._onInterstitialAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdLoadFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdLoadFailedEvent -= value;
			}
		}
	}

	private static event Action _onInterstitialAdOpenedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onInterstitialAdOpenedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdOpenedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onInterstitialAdOpenedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdOpenedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onInterstitialAdOpenedEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdOpenedEvent == null || !IronSourceEvents._onInterstitialAdOpenedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdOpenedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdOpenedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdOpenedEvent -= value;
			}
		}
	}

	private static event Action _onInterstitialAdClosedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onInterstitialAdClosedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdClosedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onInterstitialAdClosedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdClosedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onInterstitialAdClosedEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdClosedEvent == null || !IronSourceEvents._onInterstitialAdClosedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClosedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdClosedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClosedEvent -= value;
			}
		}
	}

	private static event Action _onInterstitialAdShowSucceededEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onInterstitialAdShowSucceededEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdShowSucceededEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onInterstitialAdShowSucceededEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdShowSucceededEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onInterstitialAdShowSucceededEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdShowSucceededEvent == null || !IronSourceEvents._onInterstitialAdShowSucceededEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowSucceededEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdShowSucceededEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowSucceededEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onInterstitialAdShowFailedEvent
	{
		add
		{
			Action<IronSourceError> action = IronSourceEvents.m__onInterstitialAdShowFailedEvent;
			Action<IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdShowFailedEvent, (Action<IronSourceError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<IronSourceError> action = IronSourceEvents.m__onInterstitialAdShowFailedEvent;
			Action<IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdShowFailedEvent, (Action<IronSourceError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<IronSourceError> onInterstitialAdShowFailedEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdShowFailedEvent == null || !IronSourceEvents._onInterstitialAdShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowFailedEvent -= value;
			}
		}
	}

	private static event Action _onInterstitialAdClickedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onInterstitialAdClickedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdClickedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onInterstitialAdClickedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdClickedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onInterstitialAdClickedEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdClickedEvent == null || !IronSourceEvents._onInterstitialAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClickedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClickedEvent -= value;
			}
		}
	}

	private static event Action<string> _onInterstitialAdReadyDemandOnlyEvent
	{
		add
		{
			Action<string> action = IronSourceEvents.m__onInterstitialAdReadyDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdReadyDemandOnlyEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = IronSourceEvents.m__onInterstitialAdReadyDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdReadyDemandOnlyEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onInterstitialAdReadyDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdReadyDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdReadyDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string, IronSourceError> _onInterstitialAdLoadFailedDemandOnlyEvent
	{
		add
		{
			Action<string, IronSourceError> action = IronSourceEvents.m__onInterstitialAdLoadFailedDemandOnlyEvent;
			Action<string, IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdLoadFailedDemandOnlyEvent, (Action<string, IronSourceError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, IronSourceError> action = IronSourceEvents.m__onInterstitialAdLoadFailedDemandOnlyEvent;
			Action<string, IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdLoadFailedDemandOnlyEvent, (Action<string, IronSourceError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, IronSourceError> onInterstitialAdLoadFailedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdLoadFailedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdLoadFailedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onInterstitialAdOpenedDemandOnlyEvent
	{
		add
		{
			Action<string> action = IronSourceEvents.m__onInterstitialAdOpenedDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdOpenedDemandOnlyEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = IronSourceEvents.m__onInterstitialAdOpenedDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdOpenedDemandOnlyEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onInterstitialAdOpenedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdOpenedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdOpenedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onInterstitialAdClosedDemandOnlyEvent
	{
		add
		{
			Action<string> action = IronSourceEvents.m__onInterstitialAdClosedDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdClosedDemandOnlyEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = IronSourceEvents.m__onInterstitialAdClosedDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdClosedDemandOnlyEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onInterstitialAdClosedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClosedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClosedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onInterstitialAdShowSucceededDemandOnlyEvent
	{
		add
		{
			Action<string> action = IronSourceEvents.m__onInterstitialAdShowSucceededDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdShowSucceededDemandOnlyEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = IronSourceEvents.m__onInterstitialAdShowSucceededDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdShowSucceededDemandOnlyEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onInterstitialAdShowSucceededDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdShowSucceededDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdShowSucceededDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowSucceededDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdShowSucceededDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowSucceededDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string, IronSourceError> _onInterstitialAdShowFailedDemandOnlyEvent
	{
		add
		{
			Action<string, IronSourceError> action = IronSourceEvents.m__onInterstitialAdShowFailedDemandOnlyEvent;
			Action<string, IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdShowFailedDemandOnlyEvent, (Action<string, IronSourceError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, IronSourceError> action = IronSourceEvents.m__onInterstitialAdShowFailedDemandOnlyEvent;
			Action<string, IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdShowFailedDemandOnlyEvent, (Action<string, IronSourceError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, IronSourceError> onInterstitialAdShowFailedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdShowFailedDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdShowFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowFailedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdShowFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowFailedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onInterstitialAdClickedDemandOnlyEvent
	{
		add
		{
			Action<string> action = IronSourceEvents.m__onInterstitialAdClickedDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdClickedDemandOnlyEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = IronSourceEvents.m__onInterstitialAdClickedDemandOnlyEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdClickedDemandOnlyEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onInterstitialAdClickedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClickedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClickedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action _onInterstitialAdRewardedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onInterstitialAdRewardedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdRewardedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onInterstitialAdRewardedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onInterstitialAdRewardedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onInterstitialAdRewardedEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdRewardedEvent == null || !IronSourceEvents._onInterstitialAdRewardedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdRewardedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdRewardedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdRewardedEvent -= value;
			}
		}
	}

	private static event Action _onOfferwallOpenedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onOfferwallOpenedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onOfferwallOpenedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onOfferwallOpenedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onOfferwallOpenedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onOfferwallOpenedEvent
	{
		add
		{
			if (IronSourceEvents._onOfferwallOpenedEvent == null || !IronSourceEvents._onOfferwallOpenedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallOpenedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onOfferwallOpenedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallOpenedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onOfferwallShowFailedEvent
	{
		add
		{
			Action<IronSourceError> action = IronSourceEvents.m__onOfferwallShowFailedEvent;
			Action<IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onOfferwallShowFailedEvent, (Action<IronSourceError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<IronSourceError> action = IronSourceEvents.m__onOfferwallShowFailedEvent;
			Action<IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onOfferwallShowFailedEvent, (Action<IronSourceError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<IronSourceError> onOfferwallShowFailedEvent
	{
		add
		{
			if (IronSourceEvents._onOfferwallShowFailedEvent == null || !IronSourceEvents._onOfferwallShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallShowFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onOfferwallShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallShowFailedEvent -= value;
			}
		}
	}

	private static event Action _onOfferwallClosedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onOfferwallClosedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onOfferwallClosedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onOfferwallClosedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onOfferwallClosedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onOfferwallClosedEvent
	{
		add
		{
			if (IronSourceEvents._onOfferwallClosedEvent == null || !IronSourceEvents._onOfferwallClosedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallClosedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onOfferwallClosedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallClosedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onGetOfferwallCreditsFailedEvent
	{
		add
		{
			Action<IronSourceError> action = IronSourceEvents.m__onGetOfferwallCreditsFailedEvent;
			Action<IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onGetOfferwallCreditsFailedEvent, (Action<IronSourceError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<IronSourceError> action = IronSourceEvents.m__onGetOfferwallCreditsFailedEvent;
			Action<IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onGetOfferwallCreditsFailedEvent, (Action<IronSourceError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<IronSourceError> onGetOfferwallCreditsFailedEvent
	{
		add
		{
			if (IronSourceEvents._onGetOfferwallCreditsFailedEvent == null || !IronSourceEvents._onGetOfferwallCreditsFailedEvent.GetInvocationList().Contains(value))
			{
				_onGetOfferwallCreditsFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onGetOfferwallCreditsFailedEvent.GetInvocationList().Contains(value))
			{
				_onGetOfferwallCreditsFailedEvent -= value;
			}
		}
	}

	private static event Action<Dictionary<string, object>> _onOfferwallAdCreditedEvent
	{
		add
		{
			Action<Dictionary<string, object>> action = IronSourceEvents.m__onOfferwallAdCreditedEvent;
			Action<Dictionary<string, object>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onOfferwallAdCreditedEvent, (Action<Dictionary<string, object>>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<Dictionary<string, object>> action = IronSourceEvents.m__onOfferwallAdCreditedEvent;
			Action<Dictionary<string, object>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onOfferwallAdCreditedEvent, (Action<Dictionary<string, object>>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<Dictionary<string, object>> onOfferwallAdCreditedEvent
	{
		add
		{
			if (IronSourceEvents._onOfferwallAdCreditedEvent == null || !IronSourceEvents._onOfferwallAdCreditedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallAdCreditedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onOfferwallAdCreditedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallAdCreditedEvent -= value;
			}
		}
	}

	private static event Action<bool> _onOfferwallAvailableEvent
	{
		add
		{
			Action<bool> action = IronSourceEvents.m__onOfferwallAvailableEvent;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onOfferwallAvailableEvent, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = IronSourceEvents.m__onOfferwallAvailableEvent;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onOfferwallAvailableEvent, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> onOfferwallAvailableEvent
	{
		add
		{
			if (IronSourceEvents._onOfferwallAvailableEvent == null || !IronSourceEvents._onOfferwallAvailableEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallAvailableEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onOfferwallAvailableEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallAvailableEvent -= value;
			}
		}
	}

	private static event Action _onBannerAdLoadedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onBannerAdLoadedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onBannerAdLoadedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onBannerAdLoadedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onBannerAdLoadedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onBannerAdLoadedEvent
	{
		add
		{
			if (IronSourceEvents._onBannerAdLoadedEvent == null || !IronSourceEvents._onBannerAdLoadedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdLoadedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onBannerAdLoadedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdLoadedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onBannerAdLoadFailedEvent
	{
		add
		{
			Action<IronSourceError> action = IronSourceEvents.m__onBannerAdLoadFailedEvent;
			Action<IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onBannerAdLoadFailedEvent, (Action<IronSourceError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<IronSourceError> action = IronSourceEvents.m__onBannerAdLoadFailedEvent;
			Action<IronSourceError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onBannerAdLoadFailedEvent, (Action<IronSourceError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<IronSourceError> onBannerAdLoadFailedEvent
	{
		add
		{
			if (IronSourceEvents._onBannerAdLoadFailedEvent == null || !IronSourceEvents._onBannerAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdLoadFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onBannerAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdLoadFailedEvent -= value;
			}
		}
	}

	private static event Action _onBannerAdClickedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onBannerAdClickedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onBannerAdClickedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onBannerAdClickedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onBannerAdClickedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onBannerAdClickedEvent
	{
		add
		{
			if (IronSourceEvents._onBannerAdClickedEvent == null || !IronSourceEvents._onBannerAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdClickedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onBannerAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdClickedEvent -= value;
			}
		}
	}

	private static event Action _onBannerAdScreenPresentedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onBannerAdScreenPresentedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onBannerAdScreenPresentedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onBannerAdScreenPresentedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onBannerAdScreenPresentedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onBannerAdScreenPresentedEvent
	{
		add
		{
			if (IronSourceEvents._onBannerAdScreenPresentedEvent == null || !IronSourceEvents._onBannerAdScreenPresentedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdScreenPresentedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onBannerAdScreenPresentedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdScreenPresentedEvent -= value;
			}
		}
	}

	private static event Action _onBannerAdScreenDismissedEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onBannerAdScreenDismissedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onBannerAdScreenDismissedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onBannerAdScreenDismissedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onBannerAdScreenDismissedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onBannerAdScreenDismissedEvent
	{
		add
		{
			if (IronSourceEvents._onBannerAdScreenDismissedEvent == null || !IronSourceEvents._onBannerAdScreenDismissedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdScreenDismissedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onBannerAdScreenDismissedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdScreenDismissedEvent -= value;
			}
		}
	}

	private static event Action _onBannerAdLeftApplicationEvent
	{
		add
		{
			Action action = IronSourceEvents.m__onBannerAdLeftApplicationEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onBannerAdLeftApplicationEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = IronSourceEvents.m__onBannerAdLeftApplicationEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onBannerAdLeftApplicationEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action onBannerAdLeftApplicationEvent
	{
		add
		{
			if (IronSourceEvents._onBannerAdLeftApplicationEvent == null || !IronSourceEvents._onBannerAdLeftApplicationEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdLeftApplicationEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onBannerAdLeftApplicationEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdLeftApplicationEvent -= value;
			}
		}
	}

	private static event Action<string> _onSegmentReceivedEvent
	{
		add
		{
			Action<string> action = IronSourceEvents.m__onSegmentReceivedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onSegmentReceivedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = IronSourceEvents.m__onSegmentReceivedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref IronSourceEvents.m__onSegmentReceivedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onSegmentReceivedEvent
	{
		add
		{
			if (IronSourceEvents._onSegmentReceivedEvent == null || !IronSourceEvents._onSegmentReceivedEvent.GetInvocationList().Contains(value))
			{
				_onSegmentReceivedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onSegmentReceivedEvent.GetInvocationList().Contains(value))
			{
				_onSegmentReceivedEvent -= value;
			}
		}
	}

	private void Awake()
	{
		base.gameObject.name = "IronSourceEvents";
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void onRewardedVideoAdShowFailed(string description)
	{
		if (IronSourceEvents._onRewardedVideoAdShowFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onRewardedVideoAdShowFailedEvent(errorFromErrorObject);
		}
	}

	public void onRewardedVideoAdOpened(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdOpenedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdOpenedEvent();
		}
	}

	public void onRewardedVideoAdClosed(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdClosedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdClosedEvent();
		}
	}

	public void onRewardedVideoAdStarted(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdStartedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdStartedEvent();
		}
	}

	public void onRewardedVideoAdEnded(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdEndedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdEndedEvent();
		}
	}

	public void onRewardedVideoAdRewarded(string description)
	{
		if (IronSourceEvents._onRewardedVideoAdRewardedEvent != null)
		{
			IronSourcePlacement placementFromObject = getPlacementFromObject(description);
			IronSourceEvents._onRewardedVideoAdRewardedEvent(placementFromObject);
		}
	}

	public void onRewardedVideoAdClicked(string description)
	{
		if (IronSourceEvents._onRewardedVideoAdClickedEvent != null)
		{
			IronSourcePlacement placementFromObject = getPlacementFromObject(description);
			IronSourceEvents._onRewardedVideoAdClickedEvent(placementFromObject);
		}
	}

	public void onRewardedVideoAvailabilityChanged(string stringAvailable)
	{
		bool obj = ((stringAvailable == "true") ? true : false);
		if (IronSourceEvents._onRewardedVideoAvailabilityChangedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAvailabilityChangedEvent(obj);
		}
	}

	public void onRewardedVideoAvailabilityChangedDemandOnly(string args)
	{
		if (IronSourceEvents._onRewardedVideoAvailabilityChangedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			bool arg = ((list[1].ToString().ToLower() == "true") ? true : false);
			string arg2 = list[0].ToString();
			IronSourceEvents._onRewardedVideoAvailabilityChangedDemandOnlyEvent(arg2, arg);
		}
	}

	public void onRewardedVideoAdOpenedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent(instanceId);
		}
	}

	public void onRewardedVideoAdClosedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent(instanceId);
		}
	}

	public void onRewardedVideoAdRewardedDemandOnly(string args)
	{
		if (IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			string arg = list[0].ToString();
			IronSourcePlacement placementFromObject = getPlacementFromObject(list[1]);
			IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent(arg, placementFromObject);
		}
	}

	public void onRewardedVideoAdShowFailedDemandOnly(string args)
	{
		if (IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[1]);
			string arg = list[0].ToString();
			IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent(arg, errorFromErrorObject);
		}
	}

	public void onRewardedVideoAdClickedDemandOnly(string args)
	{
		if (IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			string arg = list[0].ToString();
			IronSourcePlacement placementFromObject = getPlacementFromObject(list[1]);
			IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent(arg, placementFromObject);
		}
	}

	public void onInterstitialAdReady()
	{
		if (IronSourceEvents._onInterstitialAdReadyEvent != null)
		{
			IronSourceEvents._onInterstitialAdReadyEvent();
		}
	}

	public void onInterstitialAdLoadFailed(string description)
	{
		if (IronSourceEvents._onInterstitialAdLoadFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onInterstitialAdLoadFailedEvent(errorFromErrorObject);
		}
	}

	public void onInterstitialAdOpened(string empty)
	{
		if (IronSourceEvents._onInterstitialAdOpenedEvent != null)
		{
			IronSourceEvents._onInterstitialAdOpenedEvent();
		}
	}

	public void onInterstitialAdClosed(string empty)
	{
		if (IronSourceEvents._onInterstitialAdClosedEvent != null)
		{
			IronSourceEvents._onInterstitialAdClosedEvent();
		}
	}

	public void onInterstitialAdShowSucceeded(string empty)
	{
		if (IronSourceEvents._onInterstitialAdShowSucceededEvent != null)
		{
			IronSourceEvents._onInterstitialAdShowSucceededEvent();
		}
	}

	public void onInterstitialAdShowFailed(string description)
	{
		if (IronSourceEvents._onInterstitialAdShowFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onInterstitialAdShowFailedEvent(errorFromErrorObject);
		}
	}

	public void onInterstitialAdClicked(string empty)
	{
		if (IronSourceEvents._onInterstitialAdClickedEvent != null)
		{
			IronSourceEvents._onInterstitialAdClickedEvent();
		}
	}

	public void onInterstitialAdReadyDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdLoadFailedDemandOnly(string args)
	{
		if (IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[1]);
			string arg = list[0].ToString();
			IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent(arg, errorFromErrorObject);
		}
	}

	public void onInterstitialAdOpenedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdClosedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdShowSucceededDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdShowSucceededDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdShowSucceededDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdShowFailedDemandOnly(string args)
	{
		if (IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[1]);
			string arg = list[0].ToString();
			IronSourceEvents._onInterstitialAdShowFailedDemandOnlyEvent(arg, errorFromErrorObject);
		}
	}

	public void onInterstitialAdClickedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdRewarded(string empty)
	{
		if (IronSourceEvents._onInterstitialAdRewardedEvent != null)
		{
			IronSourceEvents._onInterstitialAdRewardedEvent();
		}
	}

	public void onOfferwallOpened(string empty)
	{
		if (IronSourceEvents._onOfferwallOpenedEvent != null)
		{
			IronSourceEvents._onOfferwallOpenedEvent();
		}
	}

	public void onOfferwallShowFailed(string description)
	{
		if (IronSourceEvents._onOfferwallShowFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onOfferwallShowFailedEvent(errorFromErrorObject);
		}
	}

	public void onOfferwallClosed(string empty)
	{
		if (IronSourceEvents._onOfferwallClosedEvent != null)
		{
			IronSourceEvents._onOfferwallClosedEvent();
		}
	}

	public void onGetOfferwallCreditsFailed(string description)
	{
		if (IronSourceEvents._onGetOfferwallCreditsFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onGetOfferwallCreditsFailedEvent(errorFromErrorObject);
		}
	}

	public void onOfferwallAdCredited(string json)
	{
		if (IronSourceEvents._onOfferwallAdCreditedEvent != null)
		{
			IronSourceEvents._onOfferwallAdCreditedEvent(Json.Deserialize(json) as Dictionary<string, object>);
		}
	}

	public void onOfferwallAvailable(string stringAvailable)
	{
		bool obj = ((stringAvailable == "true") ? true : false);
		if (IronSourceEvents._onOfferwallAvailableEvent != null)
		{
			IronSourceEvents._onOfferwallAvailableEvent(obj);
		}
	}

	public void onBannerAdLoaded()
	{
		if (IronSourceEvents._onBannerAdLoadedEvent != null)
		{
			IronSourceEvents._onBannerAdLoadedEvent();
		}
	}

	public void onBannerAdLoadFailed(string description)
	{
		if (IronSourceEvents._onBannerAdLoadFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onBannerAdLoadFailedEvent(errorFromErrorObject);
		}
	}

	public void onBannerAdClicked()
	{
		if (IronSourceEvents._onBannerAdClickedEvent != null)
		{
			IronSourceEvents._onBannerAdClickedEvent();
		}
	}

	public void onBannerAdScreenPresented()
	{
		if (IronSourceEvents._onBannerAdScreenPresentedEvent != null)
		{
			IronSourceEvents._onBannerAdScreenPresentedEvent();
		}
	}

	public void onBannerAdScreenDismissed()
	{
		if (IronSourceEvents._onBannerAdScreenDismissedEvent != null)
		{
			IronSourceEvents._onBannerAdScreenDismissedEvent();
		}
	}

	public void onBannerAdLeftApplication()
	{
		if (IronSourceEvents._onBannerAdLeftApplicationEvent != null)
		{
			IronSourceEvents._onBannerAdLeftApplicationEvent();
		}
	}

	public void onSegmentReceived(string segmentName)
	{
		if (IronSourceEvents._onSegmentReceivedEvent != null)
		{
			IronSourceEvents._onSegmentReceivedEvent(segmentName);
		}
	}

	private IronSourceError getErrorFromErrorObject(object descriptionObject)
	{
		Dictionary<string, object> dictionary = null;
		if (descriptionObject is IDictionary)
		{
			dictionary = descriptionObject as Dictionary<string, object>;
		}
		else if (descriptionObject is string && !string.IsNullOrEmpty(descriptionObject.ToString()))
		{
			dictionary = Json.Deserialize(descriptionObject.ToString()) as Dictionary<string, object>;
		}
		IronSourceError result = new IronSourceError(-1, string.Empty);
		if (dictionary != null && dictionary.Count > 0)
		{
			int errorCode = Convert.ToInt32(dictionary["error_code"].ToString());
			string errorDescription = dictionary["error_description"].ToString();
			result = new IronSourceError(errorCode, errorDescription);
		}
		return result;
	}

	private IronSourcePlacement getPlacementFromObject(object placementObject)
	{
		Dictionary<string, object> dictionary = null;
		if (placementObject is IDictionary)
		{
			dictionary = placementObject as Dictionary<string, object>;
		}
		else if (placementObject is string)
		{
			dictionary = Json.Deserialize(placementObject.ToString()) as Dictionary<string, object>;
		}
		IronSourcePlacement result = null;
		if (dictionary != null && dictionary.Count > 0)
		{
			int rewardAmount = Convert.ToInt32(dictionary["placement_reward_amount"].ToString());
			string rewardName = dictionary["placement_reward_name"].ToString();
			string placementName = dictionary["placement_name"].ToString();
			result = new IronSourcePlacement(placementName, rewardName, rewardAmount);
		}
		return result;
	}
}
