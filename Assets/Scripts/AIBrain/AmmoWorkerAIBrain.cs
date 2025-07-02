using System.Collections;
using System.Collections.Generic;
using _ObjectPooling.Scripts.Enums;
using _ObjectPooling.Scripts.Signals;
using Abstract;
using Command;
using Command.Stack;
using Datas.UnityObjects;
using Datas.ValueObjects.AI;
using Enums;
using Signals;
using States.AmmoWorker;
using UnityEngine;
using UnityEngine.AI;

namespace AIBrain
{
    public class AmmoWorkerAIBrain : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GameObject stackHolder;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;

        #endregion

        #region Private Variables

        private AmmoWorkerAIData _data;
        private Vector3 _stackPosition;
        private Coroutine _waitForAmmoArea;
        private GameObject _turretAmmoArea;
        private GameObject _turretAmmoAreaParent;
        private List<GameObject> _stackList = new();
        private AddItemToStackCommand _addItemToStack;
        private WaitForSeconds _waitForSeconds = new(0.3f);
        private GetWorkerStackItemPosCommand _getAmmoStackItemPosition;
        
        private AnyState _anyState;
        private WaitToAmmoArea _waitToAmmoArea;
        private MoveToWareHouse _moveToWareHouse;
        private AmmoWorkerBaseState _currentState;
        private WaitForFullStack _waitForFullStack;
        private MoveToTurretAmmoArea _moveToTurretAmmoArea;
        
        private SwitchStateCommand _switchStateCommand;
        private Dictionary<AmmoWorkerStates, AmmoWorkerBaseState> _stateMap;

        #endregion
        
        #region Public Variables

        public GameObject Target;

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetTurretData();
            
            _getAmmoStackItemPosition = new GetWorkerStackItemPosCommand(ref _stackList, ref _data.WorkerStackData, ref stackHolder);
            _addItemToStack = new AddItemToStackCommand(ref _stackList, ref stackHolder);

            _stateMap = new Dictionary<AmmoWorkerStates, AmmoWorkerBaseState>();
            var brain = this;

            _stateMap[AmmoWorkerStates.MoveToWareHouse] = new MoveToWareHouse(ref brain, ref agent);
            _stateMap[AmmoWorkerStates.WaitForFullStack] = new WaitForFullStack(ref brain, ref agent);
            _stateMap[AmmoWorkerStates.MoveToTurretAmmoArea] = new MoveToTurretAmmoArea(ref brain, ref agent);
            _stateMap[AmmoWorkerStates.WaitToAmmoArea] = new WaitToAmmoArea(ref brain);
            _stateMap[AmmoWorkerStates.AnyState] = new AnyState(ref brain, ref agent);
            
            _switchStateCommand = new SwitchStateCommand(_stateMap);
        }

        private AmmoWorkerAIData GetTurretData() => Resources.Load<CD_AI>("Data/CD_AI").AmmoWorkerAIData;

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
            _currentState = _stateMap[AmmoWorkerStates.MoveToWareHouse];
            _currentState.EnterState();
        }

        private void SubscribeEvents()
        {
            WorkerSignals.Instance.onAmmoAreaFull += OnTurretAmmoAreaFull;
        }

        private void UnsubscribeEvents()
        {
            WorkerSignals.Instance.onAmmoAreaFull -= OnTurretAmmoAreaFull;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Event Methods

        private void OnTurretAmmoAreaFull(GameObject area)
        {
            if (area != _turretAmmoAreaParent) return;
            SwitchState(AmmoWorkerStates.AnyState);
            CoroutineCheck();
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            _currentState.OnTriggerEnterState(other);
        }

        public void SwitchState(AmmoWorkerStates newState)
        {
            var nextState = _switchStateCommand.Execute(newState);
            if (nextState == null) return;

            _currentState = nextState;
            _currentState.EnterState();
        }

        public void AnimTriggerState(WorkerAnimState animState)
        {
            animator.SetTrigger(animState.ToString());
        }

        private void CoroutineCheck()
        {
            if (_waitForAmmoArea == null) return;
            
            StackSignals.Instance.onDecreaseStackHolder?.Invoke(_turretAmmoArea);
            StopCoroutine(_waitForAmmoArea);
            _waitForAmmoArea = null;
        }

        public void TurretAmmoArea(GameObject turretAmmoArea)
        {
            _turretAmmoArea = turretAmmoArea;
            _turretAmmoAreaParent = _turretAmmoArea.transform.parent.gameObject;
        }

        public void InteractTurretAmmoArea()
        {
            _waitForAmmoArea = StartCoroutine(WaitForAmmoArea());
        }

        public void InteractWareHouseArea()
        {
            StartCoroutine(AddAmmoToStack());
        }

        public void Wait()
        {
            StartCoroutine(ZeroPointWait());
        }

        private IEnumerator ZeroPointWait()
        {
            if (_stackList.Count > 0)
            {
                Target = WorkerSignals.Instance.onGetTurretArea();

                if (Target == null) AnimTriggerState(WorkerAnimState.Idle);

                while (Target == null)
                {
                    yield return new WaitForSeconds(1f);
                    Target = WorkerSignals.Instance.onGetTurretArea();
                }

                SwitchState(AmmoWorkerStates.MoveToTurretAmmoArea);
            }
            else
            {
                yield return new WaitForSeconds(1f);
                SwitchState(AmmoWorkerStates.MoveToWareHouse);
            }
        }

        private IEnumerator AddAmmoToStack()
        {
            while (_stackList.Count < _data.WorkerStackData.Capacity)
            {
                _stackPosition = _getAmmoStackItemPosition.Execute(_stackPosition);
                var obj = PoolSignals.Instance.onDequeuePoolableGOWithTransform(PoolType.AmmoBox,
                    Target.transform);
                _addItemToStack.Execute(obj, _stackPosition);
                yield return _waitForSeconds;
            }

            SwitchState(AmmoWorkerStates.MoveToTurretAmmoArea);
        }

        private IEnumerator WaitForAmmoArea()
        {
            StackSignals.Instance.onInteractStackHolder?.Invoke(_turretAmmoArea, _stackList);
            while (_stackList.Count > 0)
            {
                yield return _waitForSeconds;
            }

            StackSignals.Instance.onDecreaseStackHolder?.Invoke(_turretAmmoArea);
            SwitchState(AmmoWorkerStates.MoveToWareHouse);
            _waitForAmmoArea = null;
        }
    }
}