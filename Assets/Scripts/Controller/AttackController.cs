using Assets.Scripts.Systems.Modifier;
using Assets.Scripts.Utils;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class AttackController : MonoBehaviour
    {
        private Animator animator;
        private BehaviorTree behaviorTree;
        private PlayerController target;
        private AnimationEventHandler animationEventHandler;

        private bool attacking;
        private string finishEventName;

        public Weapon[] Weapons;
        public float Range = 2f;
        public float AttackAngle = 120;
        public Modifier modifier;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            behaviorTree = GetComponentInChildren<BehaviorTree>();
            animationEventHandler = GetComponentInChildren<AnimationEventHandler>();

            animationEventHandler.Event += AnimationEventHandler_Event;
        }

        private void AnimationEventHandler_Event(string animation)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < Range && transform.WithinSight(target.transform, AttackAngle))
            {
                target.Handle(modifier);
            }
        }

        private void Start()
        {
            target = PlayerController.Instance;
        }

        private void FixedUpdate()
        {
            if (!attacking)
            {
                return;
            }

            AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorState.IsName("Walking"))
            {
                attacking = false;
                behaviorTree.SendEvent(finishEventName);
            }
        }

        public void Attack(string finishEventName)
        {
            attacking = true;
            this.finishEventName = finishEventName;
            animator.SetTrigger("Attack");
        }
    }
}
