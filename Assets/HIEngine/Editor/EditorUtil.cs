#region Copyright (c) 2017 HIEngine编辑器工具类, All rights reserved.

// HIEngine - Toolset and framework for Unity3D
// ===================================
// 
// Filename: EditorUtil.cs
// Date:     2017/06/21
// Author:  EamHI
// Email: 1509256451@qq.com
#endregion

using System.Collections.Generic;
using UnityEditor;

public class EditorUtil
{
    #region About Region宏管理相关工具
    /// <summary>
    /// 是否有指定宏呢
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns></returns>
    public static bool HasDefineSymbol(string symbol)
    {
        string symbolStrs =
           PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> symbols =
            new List<string>(symbolStrs.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries));
        return symbols.Contains(symbol);
    }

    /// <summary>
    /// 移除指定宏
    /// </summary>
    /// <param name="symbol"></param>
    public static void RemoveDefineSymbols(string symbol)
    {
        foreach (BuildTargetGroup target in System.Enum.GetValues(typeof(BuildTargetGroup)))
        {
            string symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            List<string> symbols =
                new List<string>(symbolStr.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries));
            if (symbols.Contains(symbol))
                symbols.Remove(symbol);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, string.Join(";", symbols.ToArray()));
        }
    }

    /// <summary>
    /// 添加指定宏（不重复）
    /// </summary>
    /// <param name="symbol"></param>
    public static void AddDefineSymbols(string symbol)
    {
        foreach (BuildTargetGroup target in System.Enum.GetValues(typeof(BuildTargetGroup)))
        {
            string symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            List<string> symbols =
                new List<string>(symbolStr.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries));
            if (!symbols.Contains(symbol))
            {
                symbols.Add(symbol);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(target, string.Join(";", symbols.ToArray()));
            }
        }
    }
    #endregion

}
