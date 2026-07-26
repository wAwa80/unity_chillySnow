namespace LevelMode
{
	public sealed class Run
	{
		public int level;

		public int score;

		public bool isBest;

		public bool usedSecondChance;

		public bool success;

		public Run(int level, int score)
		{
			this.level = level;
			this.score = score;
			isBest = false;
			usedSecondChance = false;
			success = false;
		}

		/// <summary>
		/// 新开一局（Refresh 后首滑 / 菜单开滑）：本关分数从 0 起算。
		/// 续命由 Finger 传入 GetCurrentRun()，不走此方法。
		/// 原版在 success 时继承 score，会导致通关进下一关分数叠加上一关（虚高）。
		/// </summary>
		public static Run GetDefault()
		{
			return new Run(Level.Get(), 0);
		}
	}
}
