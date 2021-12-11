using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    private AIStateMachine brain;
    private Animator animator;
    private NavMeshAgent agent;
    private Transform playerTransform;//todo Get
    private float changeMind, attackTimer;

    private bool playerIsNear => Vector3.Distance(transform.position, playerTransform.position) < 5;
    private bool withinAttackRange => Vector3.Distance(transform.position, playerTransform.position) < 1;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        brain = GetComponent<AIStateMachine>();
        brain.PushState(Idle, OnIdleEnter, OnIdleExit);
    }

    void OnIdleEnter() => agent.ResetPath();

    void Idle()
    {
        changeMind -= Time.deltaTime;
        if (playerIsNear)
        {
            brain.PushState(Chase, OnChaseEnter, OnChaseExit);
        }
        else if (changeMind <= 0)
        {
            brain.PushState(Wander, OnWanderEnter, OnWanderExit);
            changeMind = Random.Range(4, 10);
        }
    }

    void OnIdleExit() { }

    void OnChaseEnter() => animator.SetBool("Chase", true);

    void Chase()
    {
        agent.SetDestination(playerTransform.position);
        if (Vector3.Distance(transform.position, playerTransform.position) > 5.5f)
        {
            brain.PopState();
            brain.PushState(Idle, OnIdleEnter, OnIdleExit);
        }
        if (withinAttackRange)
        {
            brain.PushState(Attack, OnEnterAttack, null);
        }
    }

    void OnChaseExit() => animator.SetBool("Chase", false);

    void OnWanderEnter()
    {
        animator.SetBool("Chase", true);
        Vector3 wanderDirection = (Random.insideUnitSphere * 4f) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(wanderDirection, out navMeshHit, 3f, NavMesh.AllAreas);
        Vector3 destination = navMeshHit.position;
        agent.SetDestination(destination);
    }

    void Wander()
    {
        if (agent.remainingDistance <= .25f)
        {
            agent.ResetPath();
            brain.PushState(Idle, OnIdleEnter, OnIdleExit);
        }
        if (playerIsNear)
        {
            brain.PushState(Chase, OnChaseEnter, OnChaseExit);
        }
    }

    void OnWanderExit() => animator.SetBool("Chase", false);

    void OnEnterAttack() => agent.ResetPath();

    void Attack()
    {
        attackTimer -= Time.deltaTime;
        if (!withinAttackRange)
        {
            brain.PopState();
        }
        else if (attackTimer <= 0)
        {
            animator.SetTrigger("Attack");
            attackTimer = 2f;
        }
    }
}
