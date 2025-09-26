using UnityEngine;

public struct Sampler
{
	public delegate float ValueMethod();

	private readonly ValueMethod GetValue;

	private readonly float leftKey;

	private readonly float rightKey;

	private readonly float leftValue;

	private readonly float rightValue;

	private readonly int _count;

	private readonly float[] _levels;

	private readonly float[] _coefs;

	private readonly float[] _constants;

	public Sampler(ValueMethod GetValue, params float[] values)
	{
		this.GetValue = GetValue;
		leftKey = Mathf.RoundToInt(values[0]);
		leftValue = values[1];
		rightKey = leftKey;
		rightValue = leftValue;
		_count = values.Length / 2 - 1;
		_levels = new float[_count];
		_coefs = new float[_count];
		_constants = new float[_count];
		for (int i = 0; i < _count; i++)
		{
			rightKey = Mathf.RoundToInt(values[i * 2 + 2]);
			rightValue = values[i * 2 + 3];
			_levels[i] = rightKey;
			float num = (rightValue - leftValue) / (rightKey - leftKey);
			_coefs[i] = num;
			_constants[i] = leftValue - leftKey * num;
			leftKey = rightKey;
			leftValue = rightValue;
		}
		leftKey = Mathf.RoundToInt(values[0]);
		leftValue = values[1];
	}

	public float Sample()
	{
		float num = GetValue();
		if (num < leftKey)
		{
			return leftValue;
		}
		for (int i = 0; i < _count; i++)
		{
			if (num < _levels[i])
			{
				return num * _coefs[i] + _constants[i];
			}
		}
		return rightValue;
	}
}
