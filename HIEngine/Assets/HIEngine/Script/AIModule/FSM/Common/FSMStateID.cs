using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIEngine.AI.FSM
{
    public enum FSMStateID
    {
        Pursuit,        //追逐	
        Dead,           //死亡	
        Attacking,  //攻击	
        Patrolling,     //巡逻	
        Wander,         //徘徊	
        Idle,            //待机	
        Default,        //默认	
        Arrival,        //到达	
        Observe,    //观察(NPC用)
        None,           //无

    }
}
