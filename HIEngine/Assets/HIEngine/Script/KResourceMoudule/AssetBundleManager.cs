//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace HIEngine
//{
//    public class AssetBundleManager : MonoBehaviour
//    {

//        private static AssetBundleManager instance;
//        public static AssetBundleManager Instance
//        {
//            get
//            {
//                if (instance == null)
//                    instance = new AssetBundleManager();
//                return instance;
//            }
//        }

//        public delegate void OnLoadAssetBundleFinish(AssetBundle assetBundle,string nameWithoutExtention);

//        public void Load(string assetBundleDir,string assetBundleName,OnLoadAssetBundleFinish callback)
//        {
//            StartCoroutine(LoadIEnumerator(assetBundleDir,assetBundleName,callback));
//        }

//        private IEnumerator LoadIEnumerator(string assetBundleDir,string assetBundleName,OnLoadAssetBundleFinish callback)
//        {
//            //首先加载总Manifest文件;    
//            WWW wwwAll = new WWW(assetBundleDir);
//            yield return wwwAll;
//            if (!string.IsNullOrEmpty(wwwAll.error))
//            {
//                Debug.LogError(wwwAll.error);
//            }
//            else
//            {
//                AssetBundle ab = wwwAll.assetBundle;
//                AssetBundleManifest manifest = (AssetBundleManifest)ab.LoadAsset("AssetBundleManifest");
//                ab.Unload(false);

//                //获取依赖文件列表;    
//                string[] depends = manifest.GetAllDependencies(assetBundleName);
//                AssetBundle[] dependsAssetBundle = new AssetBundle[depends.Length];

//                for (int index = 0; index < depends.Length; index++)
//                {
//                    //加载所有的依赖文件;    
//                    WWW dwww = WWW.LoadFromCacheOrDownload(assetBundleDir + depends[index], 0);
//                    yield return dwww;
//                    dependsAssetBundle[index] = dwww.assetBundle;
//                }

//                //加载我们需要的文件;    
//                WWW www = new WWW(assetBundleDir + assetBundleName);
//                yield return www;
//                if (!string.IsNullOrEmpty(www.error))
//                {
//                    Debug.LogError(www.error);
//                }
//                else
//                {
//                    string name = assetBundleName.Substring(0, assetBundleName.IndexOf("."));
//                    AssetBundle assetBundle = www.assetBundle;
//                    onLoadFinish(assetBundle, name);

//                    assetBundle.Unload(false);
//                }

//                //卸载依赖文件的包    
//                for (int index = 0; index < depends.Length; index++)
//                {
//                    dependsAssetBundle[index].Unload(false);
//                }
//            }  
//        }

//        public string GetString(AssetBundle assetBundle, string name)
//        {
//            return assetBundle.LoadAsset<TextAsset>(name).ToString();
//        }

//        public GameObject GetGameObject(AssetBundle assetBundle, string name)
//        {
//            return assetBundle.LoadAsset<GameObject>(name);
//        }  

//    }
//}
