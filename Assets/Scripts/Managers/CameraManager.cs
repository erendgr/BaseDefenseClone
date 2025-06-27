using System;
using Signals;
using Unity.Cinemachine;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _camera;

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onSetPlayerPosition += OnSetCameraTarget;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onSetPlayerPosition -= OnSetCameraTarget;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void OnSetCameraTarget(Transform playerTransform)
        {
            _camera.Follow = playerTransform;
        }
    }
}