using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(MakeGuide))]
[ExecuteInEditMode] 
public class MakeGuideEditor : Editor {

    string filePath;

    int guideListLength;//步骤数
    MakeGuide guide = null;//对象

    int m_selectPage = 0; //选择的菜单
    string[] m_ButtonStr = new string[2] {"配置引导","读取配置"};

    void OnEnable()
    {
        //获取当前编辑自定义Inspector的对象
        guide = (MakeGuide)target;
        guideListLength = guide.guideList.Count;
    }

    public override void OnInspectorGUI()
    { 
        GUILayout.Space(10);

        //绘制标题
        GUI.skin.label.fontSize = 20;
        GUI.color = Color.yellow;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Guide Config");
        GUILayout.Space(5);

        GUI.color = Color.white;

        DrawConfigGuide();
    }

    /// <summary>
    /// 配置
    /// </summary>
    void DrawConfigGuide()
    {
        if (Event.current.type == EventType.DragExited)
        {
            UpdatePath();
        }

        GUI.skin.label.fontSize = 13;
        GUI.color = Color.grey;
        GUILayout.Label("Tip:引导id代码中读取文件末尾数字...");
        GUILayout.Space(10);

        GUI.color = Color.white;

        guide.guideName = EditorGUILayout.TextField("引导名称:",guide.guideName);

        guide.guideSource = (GUIDE_SOURCE)EditorGUILayout.EnumPopup("引导来源:", guide.guideSource);

        if (guide.guideSource == GUIDE_SOURCE.Lv)
        {
            guide.guideParam = EditorGUILayout.IntField("引导等级:", guide.guideParam);
        }
        else if (guide.guideSource == GUIDE_SOURCE.Mission)
        {
            guide.guideParam = EditorGUILayout.IntField("引导任务id:", guide.guideParam);
        }
        else
        {
            EditorGUILayout.HelpBox("请选择引导来源!", MessageType.Error);
            return;
        }

        EditorGUILayout.LabelField("-------------------------------------------------------------");
    

        guideListLength = EditorGUILayout.IntField("步骤个数：", guideListLength);
        if (guideListLength<=0)
        {
            EditorGUILayout.HelpBox("请设置本次引导步骤数!",MessageType.Error);
            return;
        }

        //调整引导数量
        AdjustGuideList();

        guide.foldOut = EditorGUILayout.Foldout(guide.foldOut, "步骤列表");

        if (guide.foldOut)
        {
            for (int i=0;i<guide.guideList.Count;i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20.0f);

                guide.guideList[i].foldOut = EditorGUILayout.Foldout(guide.guideList[i].foldOut, "Step " + (i + 1));

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-",GUILayout.MaxWidth(20.0f),GUILayout.MaxWidth(15.0f),GUILayout.MaxHeight(15.0f)))
                {
                    guide.guideList.RemoveAt(i);
                    guideListLength--;
                    if (guideListLength<=0)
                    {
                        EditorGUILayout.HelpBox("请设置本次引导步骤数!", MessageType.Error);
                        return;
                    }
                }
                if (GUILayout.Button("+", GUILayout.MaxWidth(20.0f), GUILayout.MaxWidth(15.0f), GUILayout.MaxHeight(15.0f)))
                {
                    guide.guideList.Insert(i+1,new GuideUI());
                    guideListLength++;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();

                if (guide.guideList[i].foldOut)
                {
                    GuideUI g = guide.guideList[i];
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(30.0f);
                    g.go = EditorGUILayout.ObjectField("引导物体",g.go,typeof(GameObject),true) as GameObject;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(30.0f);
                    g.hierarchyPath = EditorGUILayout.TextField("场景路径",g.hierarchyPath);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(30.0f);
                    g.guideType =(GUIDE_TYPE)EditorGUILayout.EnumPopup("需求动作",g.guideType);
                    EditorGUILayout.EndHorizontal();
                    if (g.guideType == GUIDE_TYPE.waitSeconds)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(30.0f);
                        g.typeNeedParam = EditorGUILayout.FloatField("等待秒数", g.typeNeedParam);
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(30.0f);
                    g.handDirection = (HAND_DIRECTION)EditorGUILayout.EnumPopup("手指方向",g.handDirection);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(30.0f);
                    g.wizardDirection = (WIZARD_DIRECTION)EditorGUILayout.EnumPopup("人物方向",g.wizardDirection);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(30.0f);
                    g.showMessage = EditorGUILayout.TextField("提示信息",g.showMessage);
                    EditorGUILayout.EndHorizontal();
                }
                GUILayout.Space(10.0f);
            }
        }
        //保存序列化到文件
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }

    //修改步骤个数后调整步骤列表信息
    void AdjustGuideList()
    {
        if (guide.guideList.Count== guideListLength)
        {
            return;
        }
        while (guide.guideList.Count > guideListLength)
        {
            guide.guideList.RemoveAt(guide.guideList.Count - 1);
        }
        while (guide.guideList.Count < guideListLength)
        {
            guide.guideList.Add(new GuideUI());
        }
    }

    string hierarchyPath;
    string GetHierarchyPath(Transform t, bool initPath = true)
    {
        if (initPath) hierarchyPath = "";
        hierarchyPath = t.name + hierarchyPath;
        if (t.parent.name != "Canvas")
        {
            Transform parent = t.parent;
            hierarchyPath = "/" + hierarchyPath;
            GetHierarchyPath(parent, false);
        }
        return hierarchyPath;
    }

    void UpdatePath()
    {
        MakeGuide makeGuide = (MakeGuide)target;
        List<GuideUI> guideList = makeGuide.guideList;
        foreach (GuideUI guideUI in guideList)
        {
            if (guideUI.go != null)
            {
                guideUI.hierarchyPath = GetHierarchyPath(guideUI.go.transform);
            }
        }
    }
}
