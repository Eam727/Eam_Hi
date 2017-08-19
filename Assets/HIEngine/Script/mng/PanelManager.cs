using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HIEngine
{
    public class PanelManager : MonoSingleton<PanelManager>
    {
        /// <summary>
        /// 新建UI的父物体
        /// </summary>
        private Transform m_Parent = null;
        private Transform Parent
        {
            get
            {
                if (null == m_Parent)
                {
                    GameObject go = GameObject.FindWithTag("GuiCamera");
                    if (go != null)
                        m_Parent = go.transform;
                    return m_Parent;
                }

                return m_Parent;
            }
        }
        /// <summary>
        /// 同步加载AB面板
        /// </summary>
        /// <param name="szPanelPathFileName"></param>
        /// <param name="endCallback"></param>
        /// <param name="needAnchor"></param>
        public void CreateABPanel(string szPanelPathFileName, System.Action<Object> endCallback = null,bool needAnchor = true)
        {
            Object obj = ioo.resourceManager.InstantiateABAsset(szPanelPathFileName);
            if (obj == null)
                return;
            string objName = string.Empty;
            string[] splits = szPanelPathFileName.Split('/');
            string[] splits2 = splits[splits.Length - 1].Split('.');
            objName = splits2[0];

            SetupPanel(objName, obj, needAnchor);
            if (endCallback != null)
            {
                endCallback(obj);
            }
        }
        /// <summary>
        /// 异步创建AB面板
        /// </summary>
        /// <param name="szPanelPathFileName">AB资源相对路径</param>
        /// <param name="createEndCallback"></param>
        /// <param name="needWaitBox">是否需要等待界面</param>
        /// <param name="needAnchor">是否需要对齐锚点</param>
        public void CreateABPanelAsync(string szPanelPathFileName, System.Action<Object> createEndCallback = null,bool needWaitBox=true,bool needAnchor =true)
        {
            System.Action<Object> instantiateEndCallback =(Object _instantiatedAsset) =>
            {
                if (needWaitBox)
                {

                }
                if (_instantiatedAsset == null)
                    return;

                string objName = string.Empty;
                string[] splits = szPanelPathFileName.Split('/');
                string[] splits2 = splits[splits.Length - 1].Split('.');
                objName = splits2[0];

                SetupPanel(objName, _instantiatedAsset, needAnchor);
                if (createEndCallback!=null)
                {
                    createEndCallback(_instantiatedAsset);
                }
            };
            ioo.resourceManager.InstantiateABAssetAsync(szPanelPathFileName, instantiateEndCallback);
        }

        /// <summary>
        /// 创建Resources目录下资源
        /// </summary>
        //public Object CreatePanelFromResFolder(string szPanelPathFileName, System.Action DestroyedCallback = null)
        //{
        //    return null;
        //}


        private void SetupPanel(string szPanelName, Object PanelObj, bool Anchor = true)
        {
            GameObject PanelGameObject = PanelObj as GameObject;
            if (null == PanelGameObject)
                return;

             PanelGameObject.name = szPanelName;
            Util.SetLayer(PanelGameObject, SortingLayer.NameToID("UI"));
            PanelGameObject.transform.parent = Parent;
            PanelGameObject.transform.localScale = Vector3.one;
            PanelGameObject.transform.localPosition = Vector3.zero;

            if (Anchor)
            {
                UIPanel UIPanelScript = PanelGameObject.GetComponent<UIPanel>();
                if (null != UIPanelScript)
                    UIPanelScript.SetAnchor(Parent.gameObject, 0, 0, 0, 0);
            }
         }
    }
}
