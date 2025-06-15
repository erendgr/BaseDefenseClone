using System.Collections.Generic;
using Controllers.UI;
using Enums;
using Keys;
using Signals;
using TMPro;
using UnityEngine;


namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private List<GameObject> panels;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI gemText;
        
        #endregion

        #region Private Variables

        private UIPanelController _uiPanelController;
        private ScoreDataParams _scoreData;
        
        #endregion
        
        #endregion
        
        private void Awake()
        {
            _uiPanelController = new UIPanelController();
        }
        
        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            UISignals.Instance.onOpenPanel += OnOpenPanel;
            UISignals.Instance.onClosePanel += OnClosePanel;
            ScoreSignals.Instance.onSetScoreToUI += OnSetScoreText;
        }
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            UISignals.Instance.onOpenPanel -= OnOpenPanel;
            UISignals.Instance.onClosePanel -= OnClosePanel;
            ScoreSignals.Instance.onSetScoreToUI -= OnSetScoreText;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Event Methods

        private void OnPlay()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.Start);
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.Idle);
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.Score);
            
        }

        private void OnOpenPanel(UIPanels panelType)
        {
            _uiPanelController.OpenPanel(panelType, panels);
        }

        private void OnClosePanel(UIPanels panelType)
        {
            _uiPanelController.ClosePanel(panelType, panels);

        }

        private void OnSetScoreText()
        {
            _scoreData = ScoreSignals.Instance.onGetScoreData();
            moneyText.text = _scoreData.MoneyScore.ToString();
            gemText.text = _scoreData.GemScore.ToString();
        }

        #endregion

        public void Play()
        {
            CoreGameSignals.Instance.onPlay?.Invoke();
        }
    }
}