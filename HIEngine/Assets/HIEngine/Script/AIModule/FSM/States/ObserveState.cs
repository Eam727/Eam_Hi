using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HIEngine.AI.FSM
{
    class ObserveState : FSMState
    {
        private Vector3 preRotation;
        public override void Action(BaseFSM fsm)
        {
            Util.LookAtTarget(fsm.targetPlayer.position - fsm.transform.position, fsm.transform, fsm.RotationSpeed);
        }

        public override void EnterState(BaseFSM fsm)
        {
            preRotation = fsm.targetPlayer.position - fsm.transform.position;
        }

        public override void ExitState(BaseFSM fsm)
        {
            Util.LookAtTarget(preRotation, fsm.transform, fsm.RotationSpeed);
        }

        public override void Init()
        {
            stateId = FSMStateID.Observe;
        }
    }
}
