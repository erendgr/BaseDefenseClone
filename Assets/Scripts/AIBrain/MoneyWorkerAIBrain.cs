using System.Collections.Generic;
using _StateMachine;
using Abstract;
using Command.Stack;
using Datas.UnityObjects;
using Datas.ValueObjects.AI;
using Enums;
using Signals;
using States.MoneyWorker;
using UnityEngine;
using UnityEngine.AI;

namespace AIBrain
{
    public class MoneyWorkerAIBrain : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables

        [SerializeField] private GameObject stackHolder;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;

        #endregion

        #region Private Variables

        private MoneyWorkerData _data;
        private Vector3 _stackPosition;
        private readonly string _speed = "Speed";
        private List<GameObject> _stackList = new();
        private AddMoneyStackToScore _addMoneyStackToScore;
        private AddItemToStackCommand _objToWorkerStackCommand;
        private GetWorkerStackItemPosCommand _moneyGetWorkerStackItemPosCommand;

        private MoveToBase _moveToBase;
        private MoveToWait _moveToWait;
        private MoneyWorkerBaseState _currentState;
        private MoveToRemoveStack _moveToRemoveStack;
        private MoveToMoneyPosition _moveToMoneyPosition;
        private StateMachine<MoneyWorkerStates, MoneyWorkerBaseState> _stateMachine;

        #endregion
        
        #region Public Variables

        public GameObject Target;
        public GameObject Base;

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetTurretData();
            var brain = this;
            _moneyGetWorkerStackItemPosCommand = new GetWorkerStackItemPosCommand(ref _stackList, ref _data.WorkerStackData, ref stackHolder);
            _objToWorkerStackCommand = new AddItemToStackCommand(ref _stackList, ref stackHolder);
            _addMoneyStackToScore = new AddMoneyStackToScore(ref _stackList);
            
            var stateMap = new Dictionary<MoneyWorkerStates, MoneyWorkerBaseState>
            {
                [MoneyWorkerStates.MoveToBase] = new MoveToBase(ref brain, ref agent),
                [MoneyWorkerStates.MoveToMoneyPosition] = new MoveToMoneyPosition(ref brain, ref agent),
                [MoneyWorkerStates.MoveToWait] = new MoveToWait(ref brain, ref agent),
                [MoneyWorkerStates.MoveToRemoveStackState] = new MoveToRemoveStack(ref brain, ref agent),
            };
            _stateMachine = new StateMachine<MoneyWorkerStates, MoneyWorkerBaseState>(stateMap);
        }
        
        private MoneyWorkerData GetTurretData() => Resources.Load<CD_AI>("Data/CD_AI").MoneyWorkerData;

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
            Base = WorkerSignals.Instance.onGetBaseCenter();
            _currentState = _stateMachine.Switch(MoneyWorkerStates.MoveToBase);
            _currentState.EnterState();
        }

        private void SubscribeEvents()
        {
            WorkerSignals.Instance.onChangeDestination += OnChangeDestination;
        }

        private void UnsubscribeEvents()
        {
            WorkerSignals.Instance.onChangeDestination -= OnChangeDestination;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Event Methods

        private void OnChangeDestination()
        {
            if (!IsFullStack())
            {
                SwitchState(MoneyWorkerStates.MoveToRemoveStackState);
            }
            else
            {
                Target = WorkerSignals.Instance.onGetMoneyGameObject();
                SwitchState(Target == null 
                    ? MoneyWorkerStates.MoveToWait 
                    : MoneyWorkerStates.MoveToMoneyPosition);
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

        public void SwitchState(MoneyWorkerStates state)
        {
            _currentState = _stateMachine.Switch(state);
            _currentState.EnterState();
        }

        public void AnimFloatState(float speed)
        {
            animator.SetFloat(_speed, speed);
        }
        
        public bool IsFullStack()
        {
            return _stackList.Count < _data.WorkerStackData.Capacity;
        }
        
        public void InteractMoney(GameObject money)
        {
            if (_stackList.Count >= _data.WorkerStackData.Capacity) return; // switch state
            money.GetComponent<BoxCollider>().enabled = false;
            _stackPosition = _moneyGetWorkerStackItemPosCommand.Execute(_stackPosition);
            _objToWorkerStackCommand.Execute(money,_stackPosition);
            WorkerSignals.Instance.onRemoveMoneyFromList?.Invoke(money);
        }
        
        public void InteractBarrierArea()
        {
            _addMoneyStackToScore.Execute();
        }
    }
}