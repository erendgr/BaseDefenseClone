﻿using Abstract;
using AIBrain;
using Datas.ValueObjects;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace States.Enemy
{
    public class MoveToTurret : EnemyBaseState
    {
        #region Self Variables

        #region Private Variables

        private EnemyAIBrain _manager;
        private NavMeshAgent _agent;
        private EnemyTypeData _data;
        private readonly string _player = "Player";
        private readonly string _soldier = "Soldier";

        #endregion

        #endregion

        public MoveToTurret(ref EnemyAIBrain manager, ref NavMeshAgent agent, ref EnemyTypeData data)
        {
            _manager = manager;
            _agent = agent;
            _data = data;
        }

        public override void EnterState()
        {
            _agent.speed = _data.MoveSpeed;
            _manager.AnimTriggerState(EnemyAnimState.Walk);
            _agent.SetDestination(_manager.TurretTarget.transform.position);
        }

        public override void UpdateState()
        {
            _manager.AnimBoolState(EnemyAnimState.BaseAttack, _data.AttackRange > _agent.remainingDistance);

            if (_manager.HealthCheck())
            {
                _manager.SwitchState(EnemyStates.EnemyDeath);
            }
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag(_player))
            {
                _manager.Target = other.transform.parent.gameObject;
                _manager.SwitchState(EnemyStates.ChaseToPlayer);
            }

            if (other.CompareTag(_soldier))
            {
                _manager.Target = other.transform.parent.gameObject;
                _manager.SwitchState(EnemyStates.ChaseToSoldier);
            }
        }

        public override void OnTriggerExitState(Collider other)
        {
        }
    }
}