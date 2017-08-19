using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 引导类型
/// </summary>
public enum GUIDE_TYPE
{
    undefined = 0,
    click = 1,           //点击
    protocol = 2,    //服务器通知
    waitSeconds = 3, //显示几秒
    arrival = 4, //到达
}

/// <summary>
/// 引导手指方向
/// </summary>
public enum HAND_DIRECTION
{
    top = 0,
    bottom = 1,
}

/// <summary>
/// 任务方向
/// </summary>
public enum WIZARD_DIRECTION
{
    None = 0,
    TopLeft = 1,
    TopRight = 2,
    BottomLeft = 3,
    BottomRight = 4,
}

[Serializable]
public class GuideUI
{
    public GameObject go;//要点击的物体
    public string hierarchyPath; //物体在层次面板路径

    public GUIDE_TYPE guideType = GUIDE_TYPE.click;
    public float typeNeedParam = 0.5f;

    public HAND_DIRECTION handDirection = HAND_DIRECTION.top;
    public WIZARD_DIRECTION wizardDirection = WIZARD_DIRECTION.BottomLeft;
    public string showMessage = string.Empty;//提示框显示信息

    public bool foldOut = false; //折叠
    public GuideUI() { }
    public GuideUI(GameObject go, string hierarchyPath)
    {
        this.go = go;
        this.hierarchyPath = hierarchyPath;
    }
}

/// <summary>
/// //引导种类
/// </summary>
public enum GUIDE_SOURCE
{
    None = 0,
    Lv = 1,
    Mission = 2,
}

/// <summary>
/// 引导配置类
/// </summary>
[Serializable]
public class MakeGuide : ScriptableObject {

    public bool foldOut = false;//折叠
    public string guideName = "";
    public int guideId = 0;

    public GUIDE_SOURCE guideSource = GUIDE_SOURCE.None; //引导种类
    public int guideParam = 0;//种类对应需要值,任务则为任务id，等级则为需要等级

    public List<GuideUI> guideList = new List<GuideUI>(); //该引导下的步骤

    //public void MyReset()
    //{
    //    name = "";
    //    foldOut = false;
    //    guideSource = GUIDE_SOURCE.None;
    //    guideParam = 0;
    //    guideList = new List<GuideUI>();
    //}
}
