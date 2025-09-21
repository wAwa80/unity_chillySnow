using UnityEngine;

namespace EasyMobile
{
	public class Recorder : MonoBehaviour
	{
		public enum RecorderState
		{
			Stopped = 1,
			Recording = 2,
		}

		[SerializeField]
		private bool _autoHeight;
		[SerializeField]
		private int _width;
		[SerializeField]
		private int _height;
		[SerializeField]
		private int _framePerSecond;
		[SerializeField]
		private float _length;
		[SerializeField]
		private RecorderState _state;
	}
}
