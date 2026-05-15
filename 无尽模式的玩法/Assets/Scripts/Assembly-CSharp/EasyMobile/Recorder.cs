using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Moments;
using UnityEngine;
using MinAttribute = UnityEngine.MinAttribute;

namespace EasyMobile
{
	[AddComponentMenu("Easy Mobile/Recorder")]
	[RequireComponent(typeof(Camera))]
	[DisallowMultipleComponent]
	public sealed class Recorder : MonoBehaviour
	{
		public enum RecorderState
		{
			Stopped = 1,
			Recording
		}

		[SerializeField]
		private bool _autoHeight = true;

		[SerializeField]
		[Min(8f)]
		private int _width = 320;

		[SerializeField]
		[Min(8f)]
		private int _height = 480;

		[SerializeField]
		[Range(1f, 30f)]
		private int _framePerSecond = 15;

		[SerializeField]
		[Range(0.1f, 30f)]
		private float _length = 3f;

		[SerializeField]
		private RecorderState _state = RecorderState.Stopped;

		private Camera _targetCamera;

		private int maxFrameCount;

		private float pastTime;

		private float timePerFrame;

		private Queue<RenderTexture> recordedFrames;

		//private ReflectionUtils<Recorder> reflectionUtils;

		public bool AutoHeight => _autoHeight;

		public int Width => _width;

		public int Height => _height;

		public int FramePerSecond => _framePerSecond;

		public float Length => _length;

		public RecorderState State => _state;

		public Camera TargetCamera
		{
			get
			{
				if (_targetCamera == null)
				{
					_targetCamera = GetComponent<Camera>();
				}
				return _targetCamera;
			}
		}

		public static int CalculateAutoHeight(int width, Camera targetCam)
		{
			return Mathf.RoundToInt((float)width / targetCam.aspect);
		}

		public static float EstimateMemoryUse(int width, int height, int fps, float length)
		{
			float num = (float)fps * length;
			num *= (float)(width * height * 4);
			return num / 1048576f;
		}

		public void Setup(bool autoHeight, int width, int height, int fps, float length)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Recorder), "x");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(Recorder), "x");
			ParameterExpression parameterExpression3 = Expression.Parameter(typeof(Recorder), "x");
			ParameterExpression parameterExpression4 = Expression.Parameter(typeof(Recorder), "x");
			if (_state == RecorderState.Recording)
			{
				Debug.LogWarning("Attempting to init the recorder while a recording is in process.");
				return;
			}
			FlushMemory();
			_autoHeight = autoHeight;
			//reflectionUtils.ConstrainMin(Expression.Lambda<Func<Recorder, int>>(Expression.Field(parameterExpression, FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[1] { parameterExpression }), width);
			//if (!_autoHeight)
			//{
			//	reflectionUtils.ConstrainMin(Expression.Lambda<Func<Recorder, int>>(Expression.Field(parameterExpression2, FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[1] { parameterExpression2 }), height);
			//}
			//reflectionUtils.ConstrainRange(Expression.Lambda<Func<Recorder, int>>(Expression.Field(parameterExpression3, FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[1] { parameterExpression3 }), fps);
			//reflectionUtils.ConstrainRange(Expression.Lambda<Func<Recorder, float>>(Expression.Field(parameterExpression4, FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[1] { parameterExpression4 }), length);
			Init();
		}

		public void Record()
		{
			_state = RecorderState.Recording;
		}

		public AnimatedClip Stop()
		{
			_state = RecorderState.Stopped;
			if (recordedFrames.Count == 0)
			{
				Debug.LogWarning("Nothing recorded, an empty clip will be returned.");
			}
			AnimatedClip result = new AnimatedClip(_width, _height, _framePerSecond, recordedFrames.ToArray());
			recordedFrames.Clear();
			return result;
		}

		public bool IsRecording()
		{
			return _state == RecorderState.Recording;
		}

		private void Awake()
		{
			//reflectionUtils = new ReflectionUtils<Recorder>(this);
			recordedFrames = new Queue<RenderTexture>();
			Init();
		}

		private void OnDestroy()
		{
			_state = RecorderState.Stopped;
			FlushMemory();
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (_state != RecorderState.Recording)
			{
				Graphics.Blit(source, destination);
				return;
			}
			pastTime += Time.unscaledDeltaTime;
			if (pastTime >= timePerFrame)
			{
				pastTime -= timePerFrame;
				RenderTexture renderTexture = null;
				if (recordedFrames.Count >= maxFrameCount)
				{
					renderTexture = recordedFrames.Dequeue();
				}
				if (renderTexture == null)
				{
					renderTexture = new RenderTexture(_width, _height, 0, RenderTextureFormat.ARGB32);
					renderTexture.wrapMode = TextureWrapMode.Clamp;
					renderTexture.filterMode = FilterMode.Bilinear;
					renderTexture.anisoLevel = 0;
				}
				else
				{
					renderTexture.DiscardContents();
				}
				Graphics.Blit(source, renderTexture);
				recordedFrames.Enqueue(renderTexture);
			}
			Graphics.Blit(source, destination);
		}

		private void Init()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Recorder), "x");
			maxFrameCount = Mathf.RoundToInt(_length * (float)_framePerSecond);
			timePerFrame = 1f / (float)_framePerSecond;
			pastTime = 0f;
			if (_autoHeight)
			{
				//reflectionUtils.ConstrainMin(Expression.Lambda<Func<Recorder, int>>(Expression.Field(parameterExpression, FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[1] { parameterExpression }), CalculateAutoHeight(_width, TargetCamera));
			}
		}

		private void FlushMemory()
		{
			if (recordedFrames == null)
			{
				return;
			}
			foreach (RenderTexture recordedFrame in recordedFrames)
			{
				recordedFrame.Release();
				Flush(recordedFrame);
			}
			recordedFrames.Clear();
		}

		private void Flush(UnityEngine.Object obj)
		{
			UnityEngine.Object.Destroy(obj);
		}
	}
}
