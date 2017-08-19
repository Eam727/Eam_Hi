using UnityEngine;

namespace HIEngine.AI.FSM
{
    /// <summary>离开视野范围 </summary>
	public class LosePlayerTrigger : FSMTrigger
	{
        public override void Init()
        {
            this.triggerId = FSMTriggerID.LosePlayer;
        }

        protected override bool Evaluate(BaseFSM fsm)
        {
            if (fsm.targetPlayer == null) return false;
              
            //玩家离开视野范围
            if (Vector3.Distance(fsm.transform.position, fsm.targetPlayer.transform.position) > fsm.sightDistance )
                return true;

            return false;
        }
    }
}
