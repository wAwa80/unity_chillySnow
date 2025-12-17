using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Logger
{
    public static bool EnableCSharpDebugLog = true;
//# if DEBUG_LOG || UNITY_EDITOR
//#else
//    public static bool EnableCSharpDebugLog = false;
//#endif
    public static void Log(object message)
    {
        if (EnableCSharpDebugLog)
        {
            Debug.Log(message);
        }
    }
    public static void Log(object msg, string color)
    {
        if (EnableCSharpDebugLog)
        {
                Debug.Log($"<color={color}>{msg}</color>");
        }
    }
    public static void LogFormat(string format, params object[] args)
    {
        if (EnableCSharpDebugLog)
        {
            Debug.LogFormat(format, args);
        }
    }
    public static void LogWarning(object message)
    {
        if (EnableCSharpDebugLog)
        {
            Debug.LogWarning(message);
        }
    }
    public static void LogWarningFormat(string format, params object[] args)
    {
        if (EnableCSharpDebugLog)
        {
            Debug.LogWarningFormat(format, args);
        }
    }
    public static void LogError(object message)
    {
        Debug.LogError(message);
    }
    public static void LogErrorFormat(string format, params object[] args)
    {
        Debug.LogErrorFormat(format, args);
    }


    private const float BYTES_2_MB = 1f / (1024 * 1024);
    public static string GetDisplaySize(long downloadSize)
    {
        if (downloadSize >= 1024 * 1024)
        {
            return $"{downloadSize * BYTES_2_MB:f2}MB";
        }
        if (downloadSize >= 1024)
        {
            return $"{downloadSize / 1024:f2}KB";
        }
        return $"{downloadSize:f2}B";
    }

    private static readonly Stopwatch _watch = Stopwatch.StartNew();
    //private static long _monoUsedSize = 0;
    private static string _testName;
    private static string _msgId;
    private static string _step;
    private static long _elapsedMilliseconds;

    public static void StartRecord(string name, int id, int step)
    {
        _testName = name;
        _msgId = id.ToString();
        _step = step.ToString();
        _watch.Restart();
        //_monoUsedSize = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong();
    }
    public static void EndRecord()
    {
        _watch.Stop();
        //string size = GetDisplaySize(UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong() - _monoUsedSize);
        _elapsedMilliseconds = _watch.ElapsedMilliseconds;
        Log($"方法:{_testName}    用时:{_elapsedMilliseconds}ms", "yellow");
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("msgId", _msgId);
        data.Add("step", _step);
        data.Add("_elapsedMilliseconds", _elapsedMilliseconds.ToString());
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        AppsFlyerManager.Instance.TrackAFEvent("httpUseTime", data);
#endif
    }
}
