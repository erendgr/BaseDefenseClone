using System.Collections.Generic;
using _StateMachine;
using Abstract;
using Enums;
using Signals;
using States.Hostage;
using UnityEngine;
using UnityEngine.AI;

namespace AIBrain
{
    public class HostageAIBrain : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public GameObject Target;

        #endregion

        #region SerializField Variables

        [SerializeField] private Animator animator;
        [SerializeField] private float checkTimer;
        [SerializeField] private NavMeshAgent agent;

        #endregion

        #region Private Variables

        private float _timer;
        private HostageBaseStates _currentState;
        private readonly string _speed = "Speed";
        private StateMachine<HostageStates, HostageBaseStates> _stateMachine;

        #endregion

        #endregion
        
        private void Awake()
        {
            var brain = this;
            
            var stateMap = new Dictionary<HostageStates, HostageBaseStates>
            {
                [HostageStates.Terrified] = new HostageTerrifiedState(ref brain),
                [HostageStates.FollowPlayer] = new HostageFollowState(ref brain, ref agent),
                [HostageStates.MoveToBarrack] = new MoveToBarrack(ref brain, ref agent),
            };
            _stateMachine = new StateMachine<HostageStates, HostageBaseStates>(stateMap);
        }

        #region Event Subscription

        private void OnEnable()
        {
            _currentState = _stateMachine.Switch(HostageStates.Terrified);
            _currentState.EnterState();
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            IdleSignals.Instance.onPlayerEntrySoldierArea += OnPlayerEntrySoldierArea;
        }

        private void UnsubscribeEvents()
        {
            IdleSignals.Instance.onPlayerEntrySoldierArea -= OnPlayerEntrySoldierArea;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Event Methods

        private void OnPlayerEntrySoldierArea(GameObject hostage)
        {
            if (hostage == gameObject)
            {
                SwitchState(HostageStates.MoveToBarrack);
            }
        }

        #endregion

        private void Update()
        {
            _currentState.UpdateState();
        }

        private void OnTriggerEnter(Collider other)
        {
            _currentState.OnTriggerEnterState(other);
        }

        public void SwitchState(HostageStates state)
        {
            _currentState = _stateMachine.Switch(state);
            _currentState.EnterState();
        }

        public void AnimTriggerState(HostageAnimState animState)
        {
            animator.SetTrigger(animState.ToString());
        }

        public void AnimBoolState(HostageAnimState animState, bool isFollow)
        {
            animator.SetBool(animState.ToString(), isFollow);
        }

        public void AnimFloatState(float speed)
        {
            animator.SetFloat(_speed, speed);
        }
    }
}