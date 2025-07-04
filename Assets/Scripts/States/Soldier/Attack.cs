﻿using Abstract;
using AIBrain;
using Datas.ValueObjects.AI;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace States.Soldier
{
    public class Attack : SoldierBaseStates
    {
        #region Self Variables

        #region Private Variables

        private SoldierAIWorker _manager;
        private NavMeshAgent _agent;
        private SoldierAIData _data;

        #endregion

        #endregion

        public Attack(ref SoldierAIWorker manager, ref NavMeshAgent agent, ref SoldierAIData data)
        {
            _manager = manager;
            _agent = agent;
            _data = data;
        }

        public override void EnterState()
        {
            _agent.ResetPath();
            _manager.IsAttack(true);
            _manager.AnimTriggerState(SoldierAnimState.AttackStart);
        }

        public override void UpdateState()
        {
            _manager.AnimSetFloat(_agent.velocity.magnitude);
            if (_manager.HealthCheck())
            {
                _manager.SwitchState(SoldierStates.Dead);
            }

            if (_agent.remainingDistance > _data.AttackRange)
            {
                _manager.AnimTriggerState(SoldierAnimState.Any);
                _manager.SwitchState(SoldierStates.MoveToEnemy);
            }
            else
            {
                LookTarget();
            }
        }

        private void LookTarget()
        {
            var direct = _manager.Target.transform.position - _manager.transform.position;
            var lookRotation = Quaternion.LookRotation(direct, Vector3.up);
            _manager.transform.rotation = Quaternion.Slerp(_manager.transform.rotation,
                lookRotation, 0.1f);
        }
    }
}