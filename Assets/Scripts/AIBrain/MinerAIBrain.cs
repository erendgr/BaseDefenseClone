using System.Collections;
using System.Collections.Generic;
using _StateMachine;
using Abstract;
using Enums;
using Signals;
using States.Miners;
using UnityEngine;
using UnityEngine.AI;

namespace AIBrain
{
    public class MinerAIBrain : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public GameObject GemAreaHolder;
        public GameObject Target;

        #endregion

        #region SerializField Variables

        [SerializeField] private Animator animator;
        [SerializeField] private GameObject pickaxe;
        [SerializeField] private GameObject diamond;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private NavMeshObstacle obstacle;
        
        #endregion

        #region Private Variables

        private MinerBaseState _currentState;
        private StateMachine<MinerStates, MinerBaseState> _stateMachine;

        #endregion

        #endregion

        private void Awake()
        {
            var brain = this;

            var stateMap = new Dictionary<MinerStates, MinerBaseState>
            {
                [MinerStates.MoveToMine] = new MoveToMineState(ref brain, ref agent, ref obstacle),
                [MinerStates.DigMine] = new DigMineState(ref brain),
                [MinerStates.MoveToGemHolder] = new MoveToGemHolder(ref brain, ref agent),
            };
            _stateMachine = new StateMachine<MinerStates, MinerBaseState>(stateMap);
        }

        private void OnEnable()
        {
            Target = IdleSignals.Instance.onGetMineGameObject();
            GemAreaHolder = IdleSignals.Instance.onGetGemAreaHolder();
            _currentState = _stateMachine.Switch(MinerStates.MoveToMine);
            _currentState.EnterState();
        }

        private void OnTriggerEnter(Collider other)
        {
            _currentState.OnTriggerEnterState( other);
        }

        public void SwitchState(MinerStates state)
        {
            _currentState = _stateMachine.Switch(state);
            _currentState.EnterState();
        }

        public void GemHolderWaiter()
        {
            StartCoroutine(GemHoldWait());
        }

        public void DigWaiter()
        {
            StartCoroutine(DigWait());
        }

        private IEnumerator GemHoldWait()
        {
            agent.isStopped = true;
            DiamondController(false);
            IdleSignals.Instance.onAddGemToGemHolder?.Invoke(transform);
            yield return new WaitForSeconds(0.3f);
            agent.isStopped = false;
            SwitchState(MinerStates.MoveToMine);
        }

        private IEnumerator DigWait()
        {
            AnimState(MinerAnimState.Dig);
            yield return new WaitForSeconds(5f);
            PickaxeController(false);
            obstacle.enabled = false;
            yield return new WaitForSeconds(0.1f);
            agent.enabled = true;
            SwitchState(MinerStates.MoveToGemHolder);
        }


        public void PickaxeController(bool isOn)
        {
            pickaxe.SetActive(isOn);
        }

        public void DiamondController(bool isOn)
        {
            diamond.SetActive(isOn);
        }

        public void AnimState(MinerAnimState animState)
        {
            animator.SetTrigger(animState.ToString());
        }
    }
}