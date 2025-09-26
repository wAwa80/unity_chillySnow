using System;
using System.Runtime.InteropServices;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeScore : BaseReferenceHolder
	{
		private const ulong MinusOne = 18446744073709551615uL;

		internal NativeScore(IntPtr selfPtr)
			: base(selfPtr)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.Score.Score_Dispose(SelfPtr());
		}

		internal ulong GetDate()
		{
			return 18446744073709551615uL;
		}

		internal string GetMetadata()
		{
			return PInvokeUtilities.OutParamsToString((byte[] out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Score.Score_Metadata(SelfPtr(), out_string, out_size));
		}

		internal ulong GetRank()
		{
			return GooglePlayGames.Native.Cwrapper.Score.Score_Rank(SelfPtr());
		}

		internal ulong GetValue()
		{
			return GooglePlayGames.Native.Cwrapper.Score.Score_Value(SelfPtr());
		}

		internal PlayGamesScore AsScore(string leaderboardId, string selfPlayerId)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			ulong num = GetDate();
			if (num == 18446744073709551615uL)
			{
				num = 0uL;
			}
			DateTime date = dateTime.AddMilliseconds(num);
			return new PlayGamesScore(date, leaderboardId, GetRank(), selfPlayerId, GetValue(), GetMetadata());
		}

		internal static NativeScore FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeScore(pointer);
		}
	}
}
