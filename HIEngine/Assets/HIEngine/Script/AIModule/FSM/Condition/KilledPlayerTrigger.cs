using HIEngine.Character;
using System;
using System.Collections.Generic;

namespace HIEngine.AI.FSM
{
    public class KilledPlayerTrigger : FSMTrigger
    {
        public override void Init()
        {
            triggerId = FSMTriggerID.KilledPlayer;
        }

        protected override bool Evaluate(BaseFSM fsm)
        {
            if (fsm.targetPlayer == null) return false;
            //玩家被打死        
            if (fsm.targetPlayer.GetComponent<CharacterStatus>().curHP <= 0)
                return true;
            return false;
        }
    }
}
