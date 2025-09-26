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

	public static Run GetDefault()
	{
		Run currentRun = Neuron.GetCurrentRun();
		if (currentRun != null && currentRun.success)
		{
			return new Run(Level.Get(), currentRun.score);
		}
		return new Run(Level.Get(), 0);
	}
}
