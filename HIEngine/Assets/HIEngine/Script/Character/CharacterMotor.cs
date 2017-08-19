using UnityEngine;
using System.Collections;

namespace HIEngine.Character
{
    /// <summary>
    /// 角色马达，控制角色的行动
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMotor : MonoBehaviour
    {
        //移动速度
        public float speed = 5;
        //转向速度
        public float rotationSpeed = 0.3f;
        //角色控制器
        private CharacterController ch = null;
        private CharacterAnimation chAnimation = null;

        public void Start()
        {
            ch = GetComponent<CharacterController>();
            chAnimation = GetComponent<CharacterAnimation>();
        }
        //移动
        public void Movement(float horizontal, float vertical)
        {
            if (horizontal != 0 || vertical != 0)
            {
                chAnimation.PlayAnimation("run");
                Rotating(horizontal, vertical);
                var direct = new Vector3(transform.forward.x, -1, transform.forward.z);
                ch.Move(direct * speed * Time.deltaTime);
            }
            else
            {
                chAnimation.PlayAnimation("idle");
            }
        }
        //旋转
        public void Rotating(float horizontal, float vertical)
        {
            var targetDirection = new Vector3(horizontal, 0, vertical);
            Util.LookAtTarget(targetDirection, transform, rotationSpeed);
        }
    }
}
