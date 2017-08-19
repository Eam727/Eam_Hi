using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace HIEngine
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        private int curAsyncLoadingAssetsNum = 0; //目前异步加载中的资源数

        #region PreLoadAssetBundleManifest
        private static bool hasPreloadABManifest = false; //是否已经加载好总的AB依赖项
        private static AssetBundle _mainAssetBundle; //总的ab
        private static AssetBundleManifest _assetBundleManifest; //总的依赖项

        /// <summary>
        /// 提前同步加载总的AB依赖文件
        /// </summary>
        private void PreLoadAssetBundleManifest()
        {
            if (!hasPreloadABManifest)
            {
                string path = GetABAllPathFromABPath(Util.RuntimePlatformName); //依赖文件名称为平台名
                _mainAssetBundle = AssetBundle.LoadFromFile(path);
                _assetBundleManifest = _mainAssetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                hasPreloadABManifest = true;
            }
        }
        #endregion

        #region  AB资源加载(异步)
        /// <summary>
        /// 实例化AB资源(异步)
        /// </summary>
        public void InstantiateABAssetAsync(string name, System.Action<Object> instantiateEndCallback = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (instantiateEndCallback != null)
                    instantiateEndCallback(null);
                return;
            }
            System.Action<Object> callback = (prefab) =>
            {
                Object obj = Instantiate(prefab);
                if (obj != null)
                {
                    if (instantiateEndCallback != null)
                    {
                        instantiateEndCallback(obj);
                    }
                }
                else
                {
                    Log.Error("Load ab async1 failed,path:" + name);
                }
            };
            LoadAssetBundleAsync(name, callback);
        }

        /// <summary>
        /// 异步加载目标AB资源
        /// </summary>
        /// <param name="name">StreamingAssets文件夹下对应平台文件夹下文件相对路径</param>
        /// <param name="callback">加载完成回调</param>
        private void LoadAssetBundleAsync(string name, System.Action<Object> callback)
        {
            name = name + Const.AssetBundleExt; //".assetbundle"

            PreLoadAssetBundleManifest();

            System.Action<List<AssetBundle>> action = (depenceAssetBundles) =>
            {
                LoadResReturnAB(name, (assetBundle) =>
                {
                    Object obj = assetBundle.LoadAsset(GetObjNameFromABPath(name));//LoadAsset(name）,这个name没有后缀.assetbundle,eg:hound.prefab
                                                                              //卸载资源内存
                    assetBundle.Unload(false);
                    for (int i = 0; i < depenceAssetBundles.Count; i++)
                    {
                        depenceAssetBundles[i].Unload(false);
                    }
                    if (obj == null)
                        Log.Error("Load ab failed,path:"+name);
                    //加载目标资源完成的回调
                    if(callback!=null)
                    callback(obj);
                });

            };
            LoadDependenceAssets(name, action);
        }

        /// <summary>
        /// 异步加载目标资源的依赖资源
        /// </summary>
        private void LoadDependenceAssets(string targetAssetName,System.Action<List<AssetBundle>> action)
        {
            Log.Info("要加载的目标资源:" + targetAssetName);

            List<AssetBundle> depenceAssetBundles = new List<AssetBundle>();//用来存放加载出来的依赖资源的AssetBundle

            string[] dependences = _assetBundleManifest.GetAllDependencies(targetAssetName);
            Log.Info("依赖文件个数：" + dependences.Length);
            int length = dependences.Length;
            int finishedCount = 0;
            if (length == 0)
            {
                //没有依赖
                action(depenceAssetBundles);
            }
            else
            {
                //有依赖，加载所有依赖资源
                for (int i = 0; i < length; i++)
                {
                    string dependenceAssetName = dependences[i];
                    Log.Info("依赖资源{0}: " + dependenceAssetName, i);
                    //加载，加到assetpool
                    LoadResReturnAB(dependenceAssetName, (assetBundle) =>
                     {
                        // 这里不能loadasset,材质加载时贴图没加载，移动端会出现贴图丢失
                        //UnityEngine.Object obj = assetBundle.LoadAsset(assetName);
                        Log.Info("Load AB success,name:" + dependenceAssetName);

                         depenceAssetBundles.Add(assetBundle);
                         finishedCount++;
                         if (finishedCount == length)
                         {
                             Log.Info("All depence asset load success!");
                            //依赖都加载完了
                            action(depenceAssetBundles);
                         }
                     });
                }
            }
        }

        /// <summary>
        /// 异步加载总的依赖AssetBundleManifest(这里弃用，总的依赖在初始同步预加载)
        /// </summary>
        [System.Obsolete]
        private void LoadAssetBundleManifest(System.Action<AssetBundleManifest> action)
        {
            string manifestName = Util.RuntimePlatformName;//依赖文件名称为平台名
            if (!hasPreloadABManifest)
            {
                LoadResReturnAB(manifestName, (assetBundle) =>
                {
                    _mainAssetBundle = assetBundle; //缓存总的依赖资源
                    UnityEngine.Object obj = _mainAssetBundle.LoadAsset("AssetBundleManifest");
                    _assetBundleManifest = obj as AssetBundleManifest;
                    hasPreloadABManifest = true;
                    action(_assetBundleManifest);
                });
            }
            else
            {
                action(_assetBundleManifest);
            }
        }
        
        /// <summary>
        /// 根据AB资源相对路径加载AB并回调
        /// </summary>
        private void LoadResReturnAB(string name,System.Action<AssetBundle> callback)
        {
            //AB路径
            string path = GetABAllPathFromABPath(name);
            //Log.Info("加载：" + path);
            StartCoroutine(LoadResReturnABAsyncImpl(path, callback));    
        }

        /// <summary>
        /// 异步加载AB
        /// </summary>
        private IEnumerator LoadResReturnABAsyncImpl(string path, System.Action<AssetBundle> callback)
        {
            AssetBundleCreateRequest assetBundleRequest = AssetBundle.LoadFromFileAsync(path);
            yield return assetBundleRequest;
            if (assetBundleRequest.assetBundle != null)
            {
                Log.Info("异步加载完成："+path);
                callback(assetBundleRequest.assetBundle);
            }
            else
            {
                Log.Error("Load AB Async failed,name:"+ path);
            }
        }
        #endregion

        #region AB资源加载(同步)
        /// <summary>
        /// 同步加载AB资源
        /// </summary>
        /// <returns></returns>
        private Object LoadAssetBundle(string name)
        {
            name = name + Const.AssetBundleExt; //".assetbundle"
            string path = GetABAllPathFromABPath(name);
            PreLoadAssetBundleManifest();
            AssetBundle ab = AssetBundle.LoadFromFile(path);
            if (ab!=null)
            {
                Object prefab = ab.LoadAsset(GetObjNameFromABPath(path));
                ab.Unload(false);
                return prefab;
            }
            Log.Error("Load asset bundle failed,path:"+name);
            return null;
        }
        /// <summary>
        /// 实例化AB资源(同步)
        /// </summary>
        public Object InstantiateABAsset(string name)
        {
            Object prefab = LoadAssetBundle(name);
            if (prefab!=null)
            {
                return Instantiate(prefab);
            }
            return null;
        }
        #endregion

        #region Resources目录资源加载(异步)
        /// <summary>
        /// 加载Resources目录下资源
        /// </summary>
        /// <param name="_relPath">Resources目录相对路径</param>
        /// <param name="_loadEndCallback">加载完回调</param>
        /// <param name="_type">物体创建类型</param>
        private void LoadAssetFromResFolderAsync(string _relPath, System.Action<Object> _loadEndCallback = null, System.Type _type = null)
        {
            if (string.IsNullOrEmpty(_relPath))
            {
                if (_loadEndCallback !=null)
                {
                    _loadEndCallback(null);
                }
                return;
            }
            StartCoroutine(LoadAssetFromResFolderAsyncImpl(_relPath, _loadEndCallback, _type));    
        }
        /// <summary>
        /// 异步加载Resources目录下资源
        /// </summary>
        private IEnumerator LoadAssetFromResFolderAsyncImpl(string _relPath, System.Action<Object> _loadEndCallback = null, System.Type _type = null)
        {
            if (string.IsNullOrEmpty(_relPath))
            {
                if (_loadEndCallback !=null)
                {
                    _loadEndCallback(null);
                    yield break;
                }
            }

            ++curAsyncLoadingAssetsNum;//异步计数
            ResourceRequest request = (null == _type) ? Resources.LoadAsync(_relPath) : Resources.LoadAsync(_relPath, _type);
            yield return request;
            --curAsyncLoadingAssetsNum;

            if (request.asset ==null)
            {
                Log.Error("Can not load asset async: {0} from resources folder!");
            }
            if (_loadEndCallback != null)
            {
                _loadEndCallback(request.asset);
            }
        }

        /// <summary>
        /// 实例化Resources目录下资源(异步)
        /// </summary>
        public void InstantiateAssetFromResFolderAsync(string _relPath, System.Action<Object> _instantiateEndCallback = null, System.Type _type = null)
        {
            if (string.IsNullOrEmpty(_relPath))
            {
                if (_instantiateEndCallback != null)
                    _instantiateEndCallback(null);
                return;
            }
            System.Action<Object> callback = (Object prefab) =>
            {
                if (prefab!=null)
                {
                    Object obj = Instantiate(prefab);
                    if (_instantiateEndCallback != null)
                    {
                        _instantiateEndCallback(obj);
                    }
                }
                else
                {
                    Log.Error("Load res asset failed,path:"+_relPath);
                }
            };
            LoadAssetFromResFolderAsync(_relPath,callback,_type);
        }
        #endregion

        #region Resources目录资源加载(同步)
        /// <summary>
        /// 实例化Resources目录资源
        /// </summary>
        public Object InstantiateAssetFromResFolder(string _relPath, System.Type _type = null)
        {
            if (string.IsNullOrEmpty(_relPath))
                return null;
            Object obj = LoadAssetFromResFolder(_relPath, _type);
            if (obj!=null)
            {
                //Resources.UnloadAsset(obj);
                return Instantiate(obj);
            }
            else
            {
                Log.Error("Load res asset failed,path:"+_relPath);
                return null;
            }
        }
        /// <summary>
        /// 加载Resources目录资源
        /// </summary>
        /// <param name="_relPath"></param>
        /// <param name="_type"></param>
        /// <returns></returns>
        private Object LoadAssetFromResFolder(string _relPath,System.Type _type=null)
        {
            if (string.IsNullOrEmpty(_relPath))
                return null;
            Object asset = _type ==null? Resources.Load(_relPath):Resources.Load(_relPath,_type);
            return asset;
        }
        #endregion

        #region 功能函数
        /// <summary>
        /// 根据AB路径取得对应物体名字
        /// </summary>
        private string GetObjNameFromABPath(string abPath)
        {
            int index = abPath.LastIndexOf("/");
            string assetName = abPath.Substring(index + 1);
            assetName = assetName.Replace(Const.AssetBundleExt, "");
            return assetName;
        }
        /// <summary>
        /// 根据AB相对路径获取绝对路径
        /// </summary>
        private string GetABAllPathFromABPath(string jPath)
        {
            string path = Util.DataPath + "/AssetBundles/" + Util.RuntimePlatformName + "/" + jPath;
            return path;
        }
        #endregion
    }
}
