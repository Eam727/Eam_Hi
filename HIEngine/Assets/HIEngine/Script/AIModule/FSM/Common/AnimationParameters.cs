using System;
using System.Collections.Generic;
using UnityEngine;

namespace HIEngine.AI.FSM
{
    /// <summary>为动画状态机提供的参数</summary>
    [Serializable]
	public class AnimationParameters
	{
        public string Idle = "idle";
        public string Dead = "dead";
        public string Run = "run";
        public string FightIdle = "fightIdle";
        public string Attack = "attack";
        public string Walk = "walk";
	}
}
