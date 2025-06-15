using System.Collections;
using Enums;
using Managers;
using TMPro;
using UnityEngine;

namespace Controllers.Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private TextMeshPro healthText;
        [SerializeField] private GameObject healthBar;
        [SerializeField] private GameObject healthBarStatus;

        #endregion

        #region Private Variables

        private float _health;
        private float _currentHealth;
        private Coroutine _healthReload;

        #endregion

        #endregion

        public void GetHealth(int health)
        {
            _health = health;
            _currentHealth = health;
            SetHealthText();
            SetHealthBar();
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth > 0)
            {
                SetHealthText();
                SetHealthBar();
            }
            else
            {
                healthBar.SetActive(false);
                manager.SetPlayerState(PlayerStates.Death);
            }
        }

        public void PlayerInBase(bool isInBase)
        {
            if (isInBase)
            {
                _healthReload = StartCoroutine(HealthReload());
            }
            else
            {
                healthBar.SetActive(true);
                SetHealthText();
                if (_healthReload == null) return;
                StopCoroutine(_healthReload);
                _healthReload = null;
            }
        }

        private IEnumerator HealthReload()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);
            while (_currentHealth < _health)
            {
                _currentHealth++;
                SetHealthText();
                SetHealthBar();
                yield return wait;
            }

            healthBar.SetActive(false);
            _healthReload = null;
        }

        private void SetHealthText()
        {
            var scale = (int)((_currentHealth / _health) * 100);
            healthText.SetText(scale.ToString());
        }

        private void SetHealthBar()
        {
            var scale = _currentHealth / _health;
            healthBarStatus.transform.localScale = new Vector3(scale, 1, 1);
        }
    }
}