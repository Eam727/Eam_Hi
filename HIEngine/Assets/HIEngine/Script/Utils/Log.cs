#region About Log项目日志相关工具
///当前全局log的输出规则如下:
///Debug模式下可以输出所有级别log,非Debug模式下：
///1, 非Mobile平台可以输出所有log;2, Mobile版本只能输出LogError
///Tips:Mobile平台如果需要开启所有log:1, 编译时候开启DevelopBuild选项. 2.Const.cs中IsDebug设为true
#endregion

using UnityEngine;
namespace HIEngine
{
    public class Log : MonoBehaviour
    {
        /// <summary>
        /// 打印普通日志
        /// </summary>
        public static void Info(string str)
        {
            if (!Util.IsMobile() || Util.IsDebug())
                Debug.Log(str);
        }
        public static void Info(string str, params object[] args)
        {
            if (args != null)
                str = string.Format(str, args);
            Info(str);
        }

        public static void Warn(string str)
        {
            if (!Util.IsMobile() || Util.IsDebug())
                Debug.LogWarning(str);
        }
        public static void Warn(string str, params object[] args)
        {
            if (args != null)
                str = string.Format(str, args);
            Warn(str);
        }

        public static void Error(string str)
        {
            Debug.LogError(str);
        }
        public static void Error(string str, params object[] args)
        {
            if (args != null)
                str = string.Format(str, args);
            Error(str);
        }

        public static void Console(string str, int level = 0,bool showInConsole = true)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if(Const.enableDebugDetail)
            {
                if(showInConsole)
                    Debug.Log(str);
                HConsole.Info(str, level);
            }
#endif
        }
    }
}
