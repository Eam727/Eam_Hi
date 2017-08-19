using System;
using System.Collections.Generic;
using UnityEngine;

namespace HIEngine.AI.FSM
{
	public class DeadState : FSMState 
	{
        public override void Init()
        {
            stateId = FSMStateID.Dead;
        }

        public override void EnterState(BaseFSM fsm)
        {
            //播放死亡动画
            fsm.PlayeAnimation(fsm.animParams.Dead);
        }

        public override void ExitState(BaseFSM fsm)
        {
        }

        public override void Action(BaseFSM fsm)
        {
        }
    }
}
