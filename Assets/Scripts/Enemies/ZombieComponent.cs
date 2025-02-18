﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ZombieStateMachine))]

public class ZombieComponent : MonoBehaviour
{
    public float ZombieDamage => Damage;

    [SerializeField] private float Damage;

    public NavMeshAgent ZombieNavMesh { get; private set; }
    public Animator ZombieAnimator { get; private set; }
    public GameObject FollowTarget;
    public ZombieStateMachine StateMachine { get; private set; }

    [SerializeField] private bool _Debug;

    private void Awake()
    {
        ZombieNavMesh = GetComponent<NavMeshAgent>();
        ZombieAnimator = GetComponent<Animator>();
        StateMachine = GetComponent<ZombieStateMachine>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_Debug)
        {
            Initialize(FollowTarget);
        }
    }

    public void Initialize(GameObject followTarget)
    {
        FollowTarget = followTarget;

        ZombieIdleState idleState = new ZombieIdleState(this, StateMachine);
        StateMachine.AddState(ZombieStateType.Idle, idleState);

        ZombieFollowState followState = new ZombieFollowState(FollowTarget, this, StateMachine);
        StateMachine.AddState(ZombieStateType.Follow, followState);

        ZombieAttackState attackState = new ZombieAttackState(followTarget, this, StateMachine);
        StateMachine.AddState(ZombieStateType.Attack, attackState);

        ZombieDeathState deathState = new ZombieDeathState(this, StateMachine);
        StateMachine.AddState(ZombieStateType.Dead, deathState);

        StateMachine.Initialize(ZombieStateType.Follow);
    }
}
