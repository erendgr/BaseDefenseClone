﻿using Datas.ValueObjects.Player;
using Keys;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private PlayerManager manager;

        #endregion

        #region Private Variables

        private PlayerMovementData _movementData;
        private bool _isReadyToMove, _isReadyToPlay, _lockTarget;
        private float _inputValueX;
        private float _inputValueZ;
        private GameObject _target;
        private Vector3 _directCache;

        #endregion

        #endregion

        public void SetMovementData(PlayerMovementData dataMovementData)
        {
            _movementData = dataMovementData;
        }

        public void UpdateInputValue(IdleInputParams inputParams)
        {
            _inputValueX = inputParams.ValueX;
            _inputValueZ = inputParams.ValueZ;
        }

        public void UpdateTurretInputValue(IdleInputParams inputParams)
        {
            if (inputParams.ValueZ <= -0.6f)
            {
                manager.PlayerOutTurret();
            }
        }

        public void IsLockTarget(bool lockTarget)
        {
            _lockTarget = lockTarget;
            if (lockTarget)
            {
                _target = AttackSignals.Instance.onGetPlayerTarget();
            }
        }

        public void IsReadyToPlay(bool state)
        {
            _isReadyToPlay = state;
        }

        private void FixedUpdate()
        {
            if (_isReadyToPlay)
            {
                Move();
            }
            else
            {
                Stop();
            }
        }

        private void Move()
        {
            IdleMove();
        }

        private void IdleMove()
        {
            var velocity = rigidbody.linearVelocity;
            velocity = new Vector3(_inputValueX * _movementData.PlayerJoystickSpeed, velocity.y,
                _inputValueZ * _movementData.PlayerJoystickSpeed);
            rigidbody.linearVelocity = velocity;
            if (!_lockTarget)
            {
                _directCache = new Vector3(velocity.x, 0, velocity.z);
                if (_directCache == Vector3.zero) return;
                Rotate();
            }
            else
            {
                LockTarget();
            }
        }

        private void Rotate()
        {
            var direct = Quaternion.LookRotation(_directCache);
            transform.GetChild(0).transform.rotation = direct;
        }

        private void LockTarget()
        {
            var direct = _target.transform.position - transform.GetChild(0).transform.position;
            transform.GetChild(0).transform.rotation = Quaternion.Slerp(transform.GetChild(0).transform.rotation,
                Quaternion.LookRotation(direct), 0.2f);
        }

        private void Stop()
        {
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        public void OnReset()
        {
            Stop();
            _isReadyToPlay = false;
            _isReadyToMove = false;
        }
    }
}