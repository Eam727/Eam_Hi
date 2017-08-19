using UnityEngine;
using System.Collections;


namespace HIEngine.Character
{
    public class NPCStatus : CharacterStatus
    {
        public int npcId = 0;
        public string name = "";
        public Vector3 defaultPos = Vector3.zero;
        public string jobName = "";
        public string resourcePath = "";
        public int showScene = 0;

        /// <summary>角色动画组件</summary>
        public CharacterAnimation chAnim = null;
        public void Start()
        {
            chAnim = GetComponent<CharacterAnimation>();
        }

    }

}