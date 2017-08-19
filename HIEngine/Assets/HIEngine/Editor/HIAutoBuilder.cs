using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HIEngine.Editor
{
    public class HIAutoBuilder
    {
        private static string GetProjectName()
        {
            if(!string.IsNullOrEmpty(Const.ProjectName))
                return Const.ProjectName;
            string[] s = Application.dataPath.Split('/');
            return s[s.Length - 2];
        }

        private static string[] GetScenePaths()
        {
            string[] scenes = new string[EditorBuildSettings.scenes.Length];

            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = EditorBuildSettings.scenes[i].path;
            }

            return scenes;
        }



        [MenuItem("HIEngine/Open PC PersistentDataPath Folder")]
        public static void OpenPersistentDataPath()
        {
            System.Diagnostics.Process.Start("");
        }
    }
}
