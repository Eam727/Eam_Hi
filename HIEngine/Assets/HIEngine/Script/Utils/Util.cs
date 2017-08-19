#region Copyright (c) 2017 HIEngine工具类, All rights reserved.

// HIEngine - Toolset and framework for Unity3D
// ===================================
// 
// Filename: Util.cs
// Date:     2017/04/17
// Author:  EamHI
// Email: 1509256451@qq.com

#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using HIEngine;

public class Util
    {
        #region About data数据相关工具
        public static int Int(object o)
        {
            return Convert.ToInt32(o);
        }

        public static float Float(object o)
        {
            return (float)Math.Round(Convert.ToSingle(o), 2);
        }

        public static long Long(object o)
        {
            return Convert.ToInt64(o);
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        public static string Encode(string message)
        {
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        public static string Decode(string message)
        {
            byte[] bytes = Convert.FromBase64String(message);
            return Encoding.GetEncoding("utf-8").GetString(bytes);
        }

        /// <summary>
        /// 判断是否全为数字(字符)
        /// </summary>
        public static bool IsNumeric(string str)
        {
            if (str == null || str.Length == 0) return false;
            for (int i = 0; i < str.Length; i++)
            {
                if (!Char.IsNumber(str[i])) { return false; }
            }
            return true;
        }

        /// <summary>
        /// 判断是否全为数字(正则表达式)
        /// </summary>
        public static bool IsNumber(string str)
        {
            Regex regex = new Regex("^[0-9]$");
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 判断是否全为中文(正则表达式)
        /// </summary>
        public static bool IsChinese(string str)
        {
            Regex regex = new Regex("^[\u4E00-\u9FA5]+$");
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 非法字符检测(正则表达式)
        /// 起名
        /// </summary>
        public static Regex m_ValidRegex = null;
        public static bool ContainsInvalidWords(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            if (m_ValidRegex == null)
            {
                m_ValidRegex = new Regex(Const.ValidRegex);
            }
            if (!m_ValidRegex.IsMatch(str))
                return true;
            return false;
        }

        /// <summary>
        /// 非法字符检测(正则表达式)
        /// 自定义正则
        /// </summary>
        public static bool ContainsInvalidWords(string str, Regex reg)
        {
            if (reg == null)
            {
                return ContainsInvalidWords(str);
            }
            if (string.IsNullOrEmpty(str))
                return false;
            if (!reg.IsMatch(str))
                return true;
            return false;
        }

        /// <summary>
        /// 敏感词检测(正则表达式)
        /// (聊天、起名等)
        /// </summary>
        public static Regex m_SensitiveRegex = null;
        public static bool ContainsSensitiveWords(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            if (m_SensitiveRegex == null)
            {
                m_SensitiveRegex = new Regex(Const.SensitiveRegex);
            }
            if (!m_SensitiveRegex.IsMatch(str))
                return true;
            return false;
        }
        #endregion

        #region About Time时间相关工具
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        public static long GetCurTimeStamp()
        {
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 获取目标时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTargetTimeStamp(DateTime dt)
        {
            TimeSpan ts = new TimeSpan(dt.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 时间戳转为相应时间
        /// </summary>
        private DateTime GetTargetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));  //时间戳起始点转为目前时区
            long lTime = long.Parse(timeStamp + "0000000");//转为long类型  
            TimeSpan toNow = new TimeSpan(lTime); //时间间隔
            return dtStart.Add(toNow); //加上时间间隔得到目标时间
        }

        ///// <summary>
        ///// 延迟帧调用函数
        ///// </summary>
        ///// <param name="delayCount"></param>
        ///// <param name="callback"></param>
        //public void PlayFuncDelayFrame(int delayCount,System.Action callback)
        //{
           
        //}
        //IEnumerator IEnumPlayFuncDelay(int delayCount,System.Action func=null)
        //{
        //    yield return new WaitForEndOfFrame();
        //    while (delayCount-- >0)
        //    {
        //        yield return new WaitForEndOfFrame();
        //    }
        //    if (func != null)
        //    {
        //        func();
        //    }
        //}
        #endregion

        #region About String字符串处理工具

        /// <summary>
        /// 截断字符串变成列表
        /// </summary>
        public static List<T> Split<T>(string str,params char[] args)
        {
            if(args.Length==0)
            {
                args = new[] { '|' }; //默认
            }
            var retList = new List<T>();
            if(!string.IsNullOrEmpty(str))
            {
                string[] strs = str.Split(args);
                for (int i = 0; i < strs.Length;i++ )
                {
                    string trimS = strs[i].Trim();
                    if(!string.IsNullOrEmpty(trimS)){
                        T val = (T)Convert.ChangeType(trimS,typeof(T));
                        if(val!=null){
                            retList.Add(val);
                        }
                    }
                }
            }
            return retList;
        }


        #endregion

        #region About Component物体组件相关工具
        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// childPath子物体路径
        /// </summary>
        public static T Get<T>(GameObject go, string childPath) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.transform.Find(childPath);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Transform版
        /// childPath子物体路径
        /// </summary>
        public static T Get<T>(Transform go, string childPath) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.Find(childPath);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Component版
        /// childPath子物体路径
        /// </summary>
        public static T Get<T>(Component go, string childPath) where T : Component
        {
            return go.transform.Find(childPath).GetComponent<T>();
        }

        /// <summary>
        /// 添加组件(GameObject版)
        /// destroyOwned:是否销毁已有的(默认不销毁)
        /// </summary>
        public static T Add<T>(GameObject go, bool destroyOwned = false) where T : Component
        {
            if (go != null)
            {
                if (destroyOwned)
                {
                    T[] ts = go.GetComponents<T>();
                    for (int i = 0; i < ts.Length; i++)
                    {
                        if (ts[i] != null) GameObject.Destroy(ts[i]);
                    }
                }
                return go.gameObject.AddComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 添加组件(Transform版)
        /// destroyOwned:是否销毁已有的(默认不销毁)
        /// </summary>
        public static T Add<T>(Transform go, bool destroyOwned = false) where T : Component
        {
            return Add<T>(go.gameObject, destroyOwned);
        }

        /// <summary>
        /// 路径查找字物体(Transform)
        /// </summary>
        public static GameObject Child(Transform go, string childPath)
        {
            Transform tran = go.Find(childPath);
            if (tran == null)
                return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 路径查找子物体(GameObject)
        /// </summary>
        public static GameObject Child(GameObject go, string childPath)
        {
            return Child(go.transform, childPath);
        }

        /// <summary>
        /// 名称查找子物体(Transform)
        /// [危险：需要子与孙物体名称不同]
        /// </summary>
        public static GameObject DeepChild(Transform go,string childName)
        {
            Transform result = null;
            result = go.Find(childName);
            if (result==null)
            {
                for (int i=0;i<go.childCount;i++)
                {
                    result = DeepChild(go.GetChild(i).transform, childName).transform;
                    if (result != null)
                        return result.gameObject;
                }
            }
            return result.gameObject;
        }

        /// <summary>
        /// 名称查找子物体(GameObject)
        /// [危险：需要子与孙物体名称均不同]
        /// </summary>
        public static GameObject DeepChild(GameObject go,string childName)
        {
            return DeepChild(go.transform,childName);
        }

        /// <summary>
        /// 清除子节点(Transform)
        /// </summary>
        /// <param name="go">父物体</param>
        /// <param name="immediate">是否立即(建议false)</param>
        /// <param name="avoidChildName">排除在外不销毁子物体名</param>
        public static void ClearChild(Transform go, bool immediate = false, string[] avoidChildName = null)
        {
            if (go.childCount == 0) return;
            for (int i = 0; i < go.childCount; i++)
            {
                Transform child = go.GetChild(i);
                bool flag = false; //是否被排除
                if (avoidChildName != null)
                {
                    for (int j = 0; j < avoidChildName.Length; j++)
                    {
                        if (String.Equals(avoidChildName[i], child.gameObject.name))
                            flag = true;
                    }
                }
                if (!flag)
                {
                    if (immediate)
                        GameObject.DestroyImmediate(child.gameObject);
                    else
                        GameObject.Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// 清除子节点(GameObject)
        /// </summary>
        public static void ClearChild(GameObject go, bool immediate = false, string[] avoidChildName = null)
        {
            ClearChild(go.transform, immediate, avoidChildName);
        }

        /// <summary>
        /// 隐藏子节点(Transform)
        /// </summary>
        /// <param name="go">父物体</param>
        /// <param name="avoidChildName">排除在外不销毁子物体名</param>
        public static void HideChild(Transform go, string[] avoidChildName = null)
        {
            if (go.childCount == 0) return;
            for (int i = 0; i < go.childCount; i++)
            {
                Transform child = go.GetChild(i);
                bool flag = false; //是否被排除
                if (avoidChildName != null)
                {
                    for (int j = 0; j < avoidChildName.Length; j++)
                    {
                        if (String.Equals(avoidChildName[i], child.gameObject.name))
                            flag = true;
                    }
                }
                if (!flag)
                    child.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 隐藏子节点(GameObject)
        /// </summary>
        public static void HideChild(GameObject go, string[] avoidChildName = null)
        {
            HideChild(go.transform, avoidChildName);
        }

        /// <summary>
        /// 设置层
        /// </summary>
        public static void SetLayer(GameObject go,int layer,bool includeChild = true)
        {
            go.layer = layer;
            if (!includeChild) return;
            for (int i = 0; i < go.transform.childCount;i++ )
            {
                var child = go.transform.GetChild(i);
                SetLayer(child.gameObject, layer, includeChild);
            }
        }

    /// <summary>
    /// 添加子节点
    /// </summary>
    public static void AddChildToTarget(Transform target,Transform child)
        {
            child.parent = target;
            child.localScale = Vector3.one;
            child.localPosition = Vector3.zero;
            child.localEulerAngles = Vector3.zero;
            SetLayer(child.gameObject,target.gameObject.layer);
        }

        /// <summary>
        /// 面向目标方向
        /// </summary>
        /// <param name="targetDirection">目标方向</param>
        /// <param name="transform">需要转向的对象</param>
        /// <param name="rotationSpeed">转向速度</param>
        public static void LookAtTarget(Vector3 targetDirection, Transform transform, float rotationSpeed)
        {
            if (targetDirection != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);
            }
        }
        #endregion

        #region About State项目状态变量相关
        /// <summary>
        /// 网络是否可用
        /// </summary>
        public static bool IsNetAvailable
        {
            get
            {
                return Application.internetReachability != NetworkReachability.NotReachable;
            }
        }

        /// <summary>
        /// 是否是无线
        /// </summary>
        public static bool IsWifi
        {
            get
            {
                return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
            }
        }

        /// <summary>
        /// 是否为手机端
        /// </summary>
        public static bool IsMobile()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 是否为Debug版本
        /// </summary>
        public static bool IsDebug()
        {
            return Debug.isDebugBuild || Const.IsDebug;
        }
        #endregion

        #region About System系统相关工具

        /// <summary>
        /// 获取设备名称
        /// </summary>
        public static string GetDeviceName()
        {
            return SystemInfo.deviceName;
        }


        #endregion

        #region About Project项目相关工具
        /// <summary>
        /// 平台对应名称，用于索引读取AB的文件夹
        /// </summary>
        public static string RuntimePlatformName
        {
            get{
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsPlayer:
                    case RuntimePlatform.WindowsEditor:
                        return "Windows";
                    case RuntimePlatform.Android:
                        return "Android";
                    case RuntimePlatform.IPhonePlayer:
                        return "IOS";
                    case RuntimePlatform.OSXPlayer:
                    case RuntimePlatform.OSXEditor:
                        return "OSX";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 项目可写路径
        /// </summary>
        public static string WritablePath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                    case RuntimePlatform.IPhonePlayer:
                    case RuntimePlatform.OSXDashboardPlayer:
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                        return (Application.persistentDataPath + '/' + Const.ProjectName);
                    default:
                        return ("C:/" + Const.ProjectName + "Cache");
                }
            }
        }
    /// <summary>
    /// 各平台AB资源存放目录
    /// </summary>
    public static string DataPath
    {
        get
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return "jar:file://" + Application.dataPath+"!/assets";
                case RuntimePlatform.IPhonePlayer:
                    return Application.dataPath + "/Raw";
                default:
                    return Application.dataPath + "/StreamingAssets";
            }
        }
    }
        #endregion

        #region About File文件处理相关工具

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sPath">源文件夹</param>
        /// <param name="dPath">目标文件夹</param>
        public static void CopyFolder(string sPath, string dPath)
        {
            if (!Directory.Exists(dPath))
            {
                Directory.CreateDirectory(dPath);
            }

            DirectoryInfo sDir = new DirectoryInfo(sPath);
            FileInfo[] fileArray = sDir.GetFiles();
            foreach (FileInfo file in fileArray)
            {
                if (file.Extension != ".meta")
                    file.CopyTo(dPath + "/" + file.Name, true);
            }

            DirectoryInfo[] subDirArray = sDir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirArray)
            {
                CopyFolder(subDir.FullName, dPath + "/" + subDir.Name);
            }
        }

        #endregion

        
    }

