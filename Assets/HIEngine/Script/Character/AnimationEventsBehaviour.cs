using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HIEngine.Character
{
	public class AnimationEventsBehaviour : MonoBehaviour
	{
        private Animator anim = null;
        private CharacterAnimation chAnim;
        //private CharacterSkillSystem chSkillSys = null;
		public void Start()
        {
            anim = GetComponent<Animator>();
            chAnim = transform.parent.GetComponent<CharacterAnimation>();
            //chSkillSys = transform.parent.GetComponent<CharacterSkillSystem>();
        }

        #region 动画事件响应

        //取消当前动画，回到默认动画
        public void CancelAnim(string paramName)
        {
           // if (paramName.StartsWith("attack") && chAnim != null)
           //     chAnim.isAttack = false;
            anim.SetBool(paramName, false);
        }

        //播放攻击动画时，释放技能
        //public void OnMeleeAttack()
        //{
        //    chSkillSys.DeploySkill();
        //}

        #endregion
	}
}
