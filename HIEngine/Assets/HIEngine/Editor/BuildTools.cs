#region Copyright (c) 2017 HIEngine打包工具,All rights reserved.

// HIEngine - Toolset and framework for Unity3D
// ===================================
// 
// Filename: BuildTools.cs
// Date:     2017/04/18
// Author:  Eam

#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace HIEngine.Editor
{
    public partial class BuildTools
    {

#if UNITY_5
        /// <summary>
        /// Unity 5新AssetBundle系统，需要为打包的AssetBundle配置名称
        /// (需要打包的资源在这里设置)
        /// 直接将HIEngine配置的Packages目录整个自动配置名称，因为这个目录本来就是整个导出
        /// </summary>
        [MenuItem("HIEngine/AssetBundle/Make AB Names from [Packages]",false,101)]
        public static void MakeABNames()
        {
            Dictionary<Object,List<Object>> depens = new Dictionary<Object, List<Object>>();

            var dir = "Assets/"+ Const.BuildAssetFromPath + "/";

            //清除不需要打包的资源的ab名
            foreach(var assetGuid in AssetDatabase.FindAssets("")) //从Project面板找含有""字段的物体，即遍历Project面板所有物体,返回GUID(全局唯一标识符)数组
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);//将GUID(全局唯一标识符)转换为对应的资源路径;
                //所有的路径都是相对于工程目录文件。例如” Assets/MyTextures/hello.png”
                var assetImport = AssetImporter.GetAtPath(assetPath);
                var bundleName = assetImport.assetBundleName;
                if (string.IsNullOrEmpty(bundleName))
                    continue;
                if (!assetPath.StartsWith(dir)) //Packages目录之外的资源不打包，去除AB名
                    assetImport.assetBundleName = null;
            }


            //设置要打包的资源的ab名
            foreach(var filepath in Directory.GetFiles(dir,"*.*",SearchOption.AllDirectories))
            {
                if (filepath.EndsWith(".meta")) continue;

                var assetObject = AssetDatabase.LoadMainAssetAtPath(filepath);
                if (assetObject == null)
                {
                    Log.Error("Can't load {0}", filepath);
                    continue;
                }

                var importer = AssetImporter.GetAtPath(filepath);
                if(importer==null)
                {
                    Log.Error("Not found:{0}",filepath);
                    continue;
                }

                var bundleName = filepath.Substring(dir.Length,filepath.Length-dir.Length);
                importer.assetBundleName = bundleName + Const.AssetBundleExt;
            }

            Log.Info("Make all asset names successfully!--Eam");
        }

        [MenuItem("HIEngine/AssetBundle/Remove All [Packages] AB's Name", false, 102)]
        public static void RemoveABNames()
        {
            var dir = "Assets/" + Const.BuildAssetFromPath + "/";

            //清除不需要打包的资源的ab名
            foreach (var assetGuid in AssetDatabase.FindAssets("")) //从Project面板找含有""字段的物体，即遍历Project面板所有物体,返回GUID(全局唯一标识符)数组
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);//将GUID(全局唯一标识符)转换为对应的资源路径;
                //所有的路径都是相对于工程目录文件。例如” Assets/MyTextures/hello.png”
                var assetImport = AssetImporter.GetAtPath(assetPath);
                var bundleName = assetImport.assetBundleName;
                if (string.IsNullOrEmpty(bundleName))
                    continue;
                if (!assetPath.StartsWith(dir)) //Packages目录之外的资源不打包，去除AB名
                    assetImport.assetBundleName = null;
            }

            Log.Info("Remove all package asset names successfully!--Eam");
        }

        /// <summary>
        /// 打包所有平台所有资源
        /// 
        /// (打包不同平台资源时必须在设置中切换到对应平台)
        /// </summary>
        [MenuItem("HIEngine/AssetBundle/Build All to All Platforms", false, 104)]
        public static void BuildAllAssetBundlesToAllPlatforms()
        {
            var platforms = new List<BuildTarget>()
            {           
                BuildTarget.Android,
                BuildTarget.StandaloneWindows,
                //windows系统不能打ios资源
                //BuildTarget.iOS,
                // BuildTarget.StandaloneOSXIntel,
            };

            // 打包列出的所有平台
            var currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            platforms.Remove(currentBuildTarget);
            //打包当前平台资源
            BuildAllAssetBundlesToSelectedPlatform(currentBuildTarget);

            //切换其他平台,打包其他平台资源
            foreach (var platform in platforms)
            {
                if (EditorUserBuildSettings.SwitchActiveBuildTarget(platform))
                    BuildAllAssetBundlesToSelectedPlatform(platform);
                else
                    Log.Error("Cannot switch platform to:" + GetPlatformName(platform));
            }

            Log.Info("All Platforms' assets are built!--Eam");

            // 打包完恢复选择的平台;比较慢,所以可以选择是否恢复,可以不恢复项目包
            if (Const.RecoverPlatAfterBuildAB)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(currentBuildTarget);
                Log.Info("Recover platform after building AB,cur plat: <color=yellow>{0}!</color>", GetPlatformName(EditorUserBuildSettings.activeBuildTarget));
            }
            else
                Log.Info("No need recover platform,so pay attention,current platform is <color=yellow>{0}!</color>", GetPlatformName(EditorUserBuildSettings.activeBuildTarget));
        }

        [MenuItem("HIEngine/AssetBundle/Build All to Windows", false, 102)]
        public static void BuildAllAssetBundlesToWindows()
        {
            BuildAllAssetBundlesToSelectedPlatform(BuildTarget.StandaloneWindows);
        }

        [MenuItem("HIEngine/AssetBundle/Build All to Android", false, 103)]
        public static void BuildAllAssetBundlesToAndroid()
        {
            BuildAllAssetBundlesToSelectedPlatform(BuildTarget.Android);
        }

        //Windows不能打IOS资源
        //[MenuItem("HIEngine/AssetBundle/Build All to IOS")]
        //public static void BuildAllAssetBundlesToIOS()
        //{
        //    BuildAllAssetBundlesToSelectedPlatform(BuildTarget.iOS);
        //}

        /// <summary>
        /// 打包当前激活平台所有资源
        /// </summary>
        [MenuItem("HIEngine/AssetBundle/Build All to Current ActiveBuildTarget Platform %&b", false, 105)]
        public static void BuildAllAssetBundlesToCurActivePlatform()
        {
            BuildAllAssetBundlesToSelectedPlatform(EditorUserBuildSettings.activeBuildTarget);
        }

        /// <summary>
        /// 打包选定平台所有资源
        /// </summary>
        public static void BuildAllAssetBundlesToSelectedPlatform(BuildTarget targetPlat)
        {
            if (EditorApplication.isPlaying)
            {
                Log.Error("Cannot build in playing mode!Please stop!");
                return;
            }
            //MakeABNames();
            var outputPath = GetExportPath(targetPlat);
            Log.Info("Asset bundle start,build to:{0}", outputPath);
            if (targetPlat!= EditorUserBuildSettings.activeBuildTarget)  //选择打包平台和当前平台不一致，先切换
            {
                bool canSwitch= EditorUserBuildSettings.SwitchActiveBuildTarget(targetPlat);
                if (!canSwitch)
                {
                    Log.Error("Cant switch the current platform to " + targetPlat.ToString());
                    return;
                }
                else
                {
                    Log.Info("BuildAllAssetBundlesToSelectedPlatform,Switch the platform to <color=yellow>{0}</color> first!",targetPlat.ToString());
                }
            }
            //使用LZ4压缩格式进行打包
            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.ChunkBasedCompression, targetPlat);
            AssetDatabase.Refresh();
            Log.Info("Build assetbundles end,plat:<color=yellow>{0}</color>!", targetPlat.ToString());
        }

#endif

        #region 打包功能
        /// <summary>
        /// 根据打包平台获取ab放置路径
        /// </summary>
        public static string GetExportPath(BuildTarget platform)
        {
            string basePath = Path.GetFullPath(Const.BuildAssetToPath);
            if(File.Exists(basePath)){
                BuildTools.ShowDialog("路径配置错误："+basePath);
                throw new System.Exception("路径配置错误");
            }
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            string path = null;
            string platformName = GetPlatformName(platform);
            path = basePath + "/" + platformName + "/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// 根据打包平台获取平台名称
        /// </summary>
        public static string GetPlatformName(BuildTarget platform)
        {
            switch (platform)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "IOS";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                    return "OSX";
                default:
                    return "Others";
            }
        }

        public static bool ShowDialog(string msg,string title="提示",string button="确定")
        {
            return EditorUtility.DisplayDialog(title,msg,button);
        }

        public static void ShowDialogSelection(string msg,System.Action yesCallback)
        {
            if (EditorUtility.DisplayDialog("确定吗?",msg,"是!","否!"))
            {
                if (yesCallback != null)
                    yesCallback();
            }
        }

        #endregion

        /// <summary>
        /// 检查如果有依赖，报出
        /// 检查prefab中存在prefab依赖，进行打散！
        /// </summary>
        public static void CheckAndLogDependencies(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return;

            // 输出依赖
            var depSb = new StringBuilder();
            var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object));
            var depsArray = EditorUtility.CollectDependencies(new[] { asset });
            if (depsArray != null && depsArray.Length > 0)
            {
                if (depsArray.Length == 1 && depsArray[0] == asset)
                {
                    // 自己依赖自己的忽略掉
                }
                else
                {
                    foreach (var depAsset in depsArray)
                    {
                        if (depAsset != asset)
                        {
                            var depAssetPath = AssetDatabase.GetAssetPath(depAsset);
                            depSb.AppendLine(string.Format("Path:<color=green>{0}</color> -->Name: {1} <color=green>Type:<{2}></color>", depAssetPath, depAsset.name, depAsset.GetType()));
                        }                      
                    }
                    Log.Error("[BuildAssetBundle]Asset:<color=grey> {0} </color>has dependencies: \n \n {1}", assetPath, depSb.ToString());
                }
            }
        }

        [MenuItem("Assets/Print Asset Dependencies", false, 100)]
        public static void MenuCheckAndLogDependencies()
        {
            var obj = Selection.activeObject;
            if (obj == null)
            {
                Log.Error("No selection object");
                return;
            }
            var assetPath = AssetDatabase.GetAssetPath(obj);
            BuildTools.CheckAndLogDependencies(assetPath);
        }

        public static void SyncPackage2Resource()
        {
            
        }
        
        public static void SyncResource2Package()
        {
            double startTime = EditorApplication.timeSinceStartup;
            string syncedDir = string.Empty;
            Object[] selections = Selection.GetFiltered(typeof(Object),SelectionMode.DeepAssets);

            for (int i=0;i<selections.Length;i++)
            {
                Object selectObj = selections[i];
                string selectObjPath = AssetDatabase.GetAssetPath(selectObj);
                string lastSyncPath = string.Empty;
                selectObjPath = selectObjPath.Replace("\\", "/");

                if (selectObjPath.Contains("Assets/Packages"))
                {
                    Debug.LogError("不能从Packages向自身拷贝资源.");
                    return;
                }
                EditorUtility.DisplayProgressBar("同步资源进度条(" + i + "/" + selections.Length + ")", selectObjPath, i * 1.0f / selections.Length);

                //
                //
            }
        }

    }

}