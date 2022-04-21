using System;
using System.Collections;
using Sources.Runtime.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime
{
    [RequireComponent(typeof(Image))]
    public class CoolDownView : MonoBehaviour
    {
        [SerializeField]
        private Sprite _activeSprite;
        [SerializeField]
        private Sprite _coolDownSprite;
        [SerializeField]
        private Image _cooldownDark;
        
        private TextMeshProUGUI _timerText;
        private Image _image;
        private IAbility _ability;
        private float _countdown;

        private void Awake()
        {
            _timerText = GetComponentInChildren<TextMeshProUGUI>();
            _timerText.gameObject.SetActive(false);
            _image = GetComponent<Image>();
        }

        public void BindAbility(IAbility ability)
        {
            _ability = ability;
            _ability.Casted += () => StartCoroutine(StartCooldown());
        }

        private IEnumerator StartCooldown()
        {
            _countdown = _ability.CoolDown;
            _image.sprite = _coolDownSprite;
            _timerText.gameObject.SetActive(true);
            _cooldownDark.gameObject.SetActive(true);
            
            while (_countdown >= 0)
            {
                _cooldownDark.fillAmount = _countdown / _ability.CoolDown;
                _timerText.text = Mathf.Ceil(_countdown).ToString();
                _countdown -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _cooldownDark.gameObject.SetActive(false);
            _timerText.gameObject.SetActive(false);
            _image.sprite = _activeSprite;
        }
    }
}