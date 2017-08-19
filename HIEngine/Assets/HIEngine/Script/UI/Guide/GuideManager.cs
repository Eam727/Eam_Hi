using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace HIEngine
{
    public class GuideManager : MonoSingleton<GuideManager>
    {
        public List<MakeGuide> guideInfo = new List<MakeGuide>();
        private MakeGuide curGuide = null;
        private void Start()
        {
            Init();
           
        }
        void Init()
        {
            guideInfo.Clear();

            string path = Const.guideLoadPath;//Res文件夹位置Application.dataPath + "/Resources/Guides"; 引导资源读取文件夹
            if (path == null || path == string.Empty)
                return;

            string[] files= Directory.GetFiles(path);

            if (files == null || files.Length == 0)
                return;

            foreach (var file in files)
            {
                if (!file.Contains(".meta"))
                {
                    int index = file.IndexOf("Resources/");
                    string resPath = file.Substring(index+10);
                    resPath = resPath.Remove(resPath.IndexOf('.'));
                    ScriptableObject so = Resources.Load<ScriptableObject>(resPath);//移除后缀
                    if (so==null)
                    {
                        Log.Info("Res load null,path: "+resPath);
                    }
                    else
                    {
                        AddGuide(so as MakeGuide);
                        Log.Info("Load res guide success,res path: "+resPath);
                    }
                }
            }

            if (guideInfo.Count == 0)
            {
                return;
            }
        }
        
        void AddGuide(MakeGuide mg)
        {
            if (!guideInfo.Contains(mg))
            {
                guideInfo.Add(mg);
            }
        }

        void ReadAll()
        {
            foreach (var value in guideInfo)
            {
                Log.Error("name: "+value.guideName);
            }
        }
    }
}
