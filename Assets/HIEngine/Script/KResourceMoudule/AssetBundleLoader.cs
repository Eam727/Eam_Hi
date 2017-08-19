using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HIEngine
{
    /// <summary>
    /// 加载模式，同步或异步
    /// </summary>
    public enum LoaderMode
    {
        Async,
        Sync,
    }

    public class AssetBundleLoader
    {


#if UNITY_5
        private static bool _hasPreloadAssetBundleManifest = false;
        private static AssetBundle _mainAssetBundle;
        private static AssetBundleManifest _assetBundleManifest;
        /// <summary>
        /// Unity5下，使用manifest进行AssetBundle的加载
        /// </summary>
        static void PreLoadManifest()
        {
            if (_hasPreloadAssetBundleManifest)
                return;

            
        }
#endif

        
    }
}
