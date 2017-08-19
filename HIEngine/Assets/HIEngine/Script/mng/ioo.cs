using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HIEngine
{
    /// <summary>
    /// 管理器管理类
    /// </summary>
    public abstract class ioo
    {
        /// <summary>
        /// 资源管理器
        /// </summary>
        public static ResourceManager resourceManager
        {
            get
            {
                return ResourceManager.Instance;
            }
        }

        /// <summary>
        /// 面板管理器
        /// </summary>
        public static PanelManager panelManager
        {
            get
            {
                return PanelManager.Instance;
            }
        }
    }
}
