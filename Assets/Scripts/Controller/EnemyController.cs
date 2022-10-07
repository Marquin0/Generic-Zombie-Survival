using Assets.Scripts.Systems.Entities;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Entity
{
    private BehaviorTree behaviorTree;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        behaviorTree = GetComponent<BehaviorTree>();
        behaviorTree.SetVariableValue("Target", PlayerController.Instance.gameObject);
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        animator.SetFloat("MovementSpeed", navMeshAgent.velocity.magnitude);
    }
}
