using UnityEditor;
using UnityEngine;

[InitializeOnLoad] //运行Unity时运行编辑器脚本
internal class HIUIRootAssetEditorInitializer
{
    static HIUIRootAssetEditorInitializer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
    }
    
    private static void HierarchyItemCB(int instanceid,Rect selectionrect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
        if (obj!=null)
        {
            if (obj.GetComponent<Camera>() !=null)
            {
                Rect r = new Rect(selectionrect);
                r.x = r.width - 80;
                r.width = 80;
                r.y += 2; //稍作偏移
                var style = new GUIStyle();
                style.normal.textColor = Color.yellow;
                style.hover.textColor = Color.green;
                GUI.Label(r, "[HICamera]", style); //绘制label

            }
        }
    }
}

public class HIUIAssetEditor
{

	
}
