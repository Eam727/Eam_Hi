#region Copyright (c) 2017 HIEngine常量类, All rights reserved.

// HIEngine - Toolset and framework for Unity3D
// ===================================
// 
// Filename: Const.cs
// Date:     2017/04/17
// Author:  EamHI
// Email: 1509256451@qq.com

#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Const
{
    public static bool IsDebug = true; //是否是Debug模式(可以打印log日志,由是否DebugBuild或者这个变量决定)
    public static bool enableDebugDetail = false; //是否允许在游戏内部在界面上显示日志细节


    public static string guideAssetsFolder = Application.dataPath + "/Packages/Guides"; //引导文件夹生成
    public static string guideLoadPath = Application.dataPath + "/Resources/Guides"; //引导资源读取文件夹

    public static string ProjectName = "EamHI";
    public static string BuildAssetFromPath = "Packages";
    public static string BuildAssetToPath = "Assets/StreamingAssets/AssetBundles";

    public static bool RecoverPlatAfterBuildAB = true; //控制编辑器打包多平台资源后是否切回原平台

    public static string AssetBundleExt = ".assetbundle"; //打ab后缀名

    public static string WindowsManifestPath = Application.streamingAssetsPath + "AssetBundles/Windows/Windows";
    public static string AndroidManifestPath = Application.streamingAssetsPath + "AssetBundles/Android/Android";


    public static string ValidRegex = "^[-·a-zA-Z0-9\\u4e00-\\u9fa5]+$";     //非法词过滤(合法词只包含 - · 大小写字母 数字 汉字)
    public static string SensitiveRegex = "鳄鱼|宝宝";                                   //敏感词过滤(含其中任一项都敏感)



}