using System;
using System.Collections.Generic;
using UnityEngine;

namespace HIEngine.AI.FSM
{
	public class PursuitState : FSMState 
	{
        public override void Init()
        {
            stateId = FSMStateID.Pursuit;
        }

        public override void EnterState(BaseFSM fsm)
        {
        }

        public override void ExitState(BaseFSM fsm)
        {
            //停止移动 
            fsm.StopMove();
        }

        public override void Action(BaseFSM fsm)
        {
            //播放跑动画
            if (fsm.targetPlayer != null && fsm.targetPlayer.gameObject != null)
            {
                fsm.PlayeAnimation(fsm.animParams.Run);
                fsm.MoveToTarget(fsm.targetPlayer.position,fsm.MoveSpeed,fsm.RotationSpeed);
            }
        }
    }
}
