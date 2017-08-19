using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;


namespace HIEngine.Character
{
    [RequireComponent(typeof(CharacterMotor))]
    public class CharacterInputController : MonoBehaviour
    {
        private CharacterMotor motor;
       
        public float doubleHitInterval = 1.5f;
        private bool isDoubleHit = false;
        private DateTime lastClickTime;

        private CharacterAnimation chAnimation;
        //private CharacterSkillSystem chSystem ;

        void OnEnable()
        {
            //EasyJoystick.On_JoystickMove += On_JoystickMove;
            //EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
            //EasyButton.On_ButtonPress += On_ButtonPress;
            //EasyButton.On_ButtonUp += On_ButtonUp;
            //EasyButton.On_ButtonDown += On_ButtonDown;
        }

        void Start()
        {
            motor = GetComponent<CharacterMotor>();
            //chSystem = GetComponent<CharacterSkillSystem>();
            chAnimation = GetComponent<CharacterAnimation>();
        }



        void Update()
        {
            //motor.Movement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }


//        void OnDisable()
//        {
//            UnRegistClick();
//        }

//        void OnDestroy()
//        {
//            UnRegistClick();
//        }

//        private void UnRegistClick()
//        {
//            EasyJoystick.On_JoystickMove -= On_JoystickMove;
//            EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
////            EasyButton.On_ButtonPress -= On_ButtonPress;
////            EasyButton.On_ButtonUp -= On_ButtonUp;
////            EasyButton.On_ButtonDown -= On_ButtonDown;
//        }

//        void On_JoystickMove(MovingJoystick move)
//        {
//            if (!chAnimation.isAttack)
//                motor.Movement(move.joystickAxis.x, move.joystickAxis.y);
//        }

//        void On_JoystickMoveEnd(MovingJoystick move)
//        {
//            if (!chAnimation.isAttack)
//                motor.Movement(move.joystickAxis.x, move.joystickAxis.y);
//        }

//        void On_ButtonPress(string buttonName)
//        {
//            if (buttonName == "BaseSkill")
//                On_ButtonDown(buttonName);
//        }

//        public void On_ButtonDown(string buttonName)
//        {  
//            switch (buttonName)
//            {
//                case "Skill1":
//                    chSystem.AttackUseSkill(1, false);
//                    break;
//                case "Skill2":
//                    chSystem.AttackUseSkill(2, false);
//                    break;
//                case "BaseSkill":
//                    var span = (DateTime.Now - lastClickTime).TotalSeconds;
//                    if (span <1f)
//                        return;
//                    isDoubleHit = span < doubleHitInterval;
//                    lastClickTime = DateTime.Now;
//                    chSystem.AttackUseSkill(3, isDoubleHit);
//                    break;
//            }  
           
           
//        }

//        void On_ButtonUp(string buttonName)
//        {
//            isDoubleHit = false;
//        }
    }
}