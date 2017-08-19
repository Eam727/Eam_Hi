using UnityEngine;
using System.Collections;
using UnityEditor;
using HIEngine;
using System.IO;

public class MakeGuideEditorWindow : EditorWindow {

    [MenuItem("HIEngine/MakeGuide/Create a guide")]
    static void CreateGuideAsset()
    {
        ScriptableObject guide = CreateInstance<MakeGuide>();

        if (!guide)
        {
            Debug.Log("guide not found!");
            return;
        }
        string path = Const.guideAssetsFolder;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string[] filePaths = Directory.GetFiles(path);
        int index = 0;
        if (filePaths==null || filePaths.Length==0)
        {
            index = 1;
        }
        else
        {
            index = filePaths.Length + 1;
        }
        foreach (string filePath in filePaths)
        {
            if (filePath.Contains(".meta"))
            {
                index--;
            }
        }
        path = string.Format("Assets/Packages/Guides/{0}.asset", (typeof(MakeGuide).ToString()+"_"+ index));
        
        // 生成自定义资源到指定路径  
        AssetDatabase.CreateAsset(guide, path);
    }
}
