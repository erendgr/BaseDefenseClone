using System;
using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables
        
        [SerializeField] private bool isReadyForTouch, isFirstTimeTouchTaken;
        [SerializeField] private FloatingJoystick joystick;

        #endregion

        #region Private Variables

        private bool _isTouching;
        private float _currentVelocity; //ref type
        private Vector2? _mousePosition; //ref type
        private Vector3 _moveVector; //ref type
        private GameStates _inputStates = GameStates.Idle;

        #endregion
        
        #endregion
        
        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onInputTaken += OnEnableInput;
            InputSignals.Instance.onInputReleased += OnDisableInput;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            //CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
            //LevelSignals.Instance.onNextLevel += OnNextLevel;
        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onInputTaken -= OnEnableInput;
            InputSignals.Instance.onInputReleased -= OnDisableInput;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            // CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            // LevelSignals.Instance.onNextLevel += OnNextLevel;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Awake()
        {
            
        }

        private void Start()
        {
            JoystickInput();
        }

        private void Update()
        {
            if (!isReadyForTouch) return;
            
            switch (_inputStates)
            {
                case GameStates.Idle:
                    JoystickInput();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        #region Event Methods
        
        private void OnEnableInput()
        {
            isReadyForTouch = true;
        }
        
        private void OnDisableInput()
        {
            isReadyForTouch = false;
        }
        
        private void OnPlay()
        {
            isReadyForTouch = true;
        }
        
        private void OnChangeGameState()
        {
            _inputStates = GameStates.Idle;
        }
        
        private void OnReset()
        {
            _isTouching = false;
            isReadyForTouch = false;
            isFirstTimeTouchTaken = false;
        }

        // private void OnNextLevel() 
        // {
        //     _isTouching = false;
        //     isReadyForTouch = false;
        //     isFirstTimeTouchTaken = false;
        // }

        #endregion
        
        #region InputUpdateMethods
        
        private void JoystickInput()
        {
            _moveVector.x = joystick.Horizontal;
            _moveVector.z = joystick.Vertical;
                        
            InputSignals.Instance.onJoystickDragged?.Invoke(new IdleInputParams()
            {
                ValueX = _moveVector.x,
                ValueZ = _moveVector.z
            });
        }

        #endregion
    }
}