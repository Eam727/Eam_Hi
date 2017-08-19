using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HIEngine.Character
{
    public class CharacterStatus : MonoBehaviour
    {
        public int curHP = 100;
        public int maxHP = 100;
        public int curSP = 100;
        public int maxSP = 100;

        public int damage = 100;
        public float attackSpeed = 5.0f;
        public float attackDistance = 2;

        public Transform HitFxPos;

        public void Start()
        {
            HitFxPos = Util.Child(transform,"HitFxPos").transform;
        }
    }
}
