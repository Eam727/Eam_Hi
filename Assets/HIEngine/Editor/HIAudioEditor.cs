using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HIEngine.Editor
{
    public class HIAudioEditor : EditorWindow
    {
        HIAudioEditor()
        {
            titleContent = new GUIContent("Audio Manager");
        }

        [MenuItem("Tool/Audio Manager")]
        static void ShowWindow()
        {
            GetWindow(typeof(HIAudioEditor));
        }



        void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.Space(10);
            GUI.skin.label.fontSize = 20;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Audio Manager");

            GUILayout.Space(10);
            if(Selection.activeGameObject !=null)
            {
                this.RemoveNotification();
            }
            else
            {
                this.ShowNotification(new GUIContent("Please select a gameobject!"));
            }
            

            GUILayout.EndVertical();
        }

        void OnInspectorUpdate()
        {
            //Debug.Log("窗口面板的更新");
            //这里开启窗口的重绘，不然窗口信息不会刷新
            this.Repaint();
        }

    }
}
