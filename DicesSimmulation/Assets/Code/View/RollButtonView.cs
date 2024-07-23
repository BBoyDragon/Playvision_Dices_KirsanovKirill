using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.View
{
    public class RollButtonView: MonoBehaviour
    {
        [SerializeField] private Button _rollButton;
        public event Action OnButtonPressed;

        private void Start()
        {
            _rollButton.onClick.AddListener(Press);
        }

        private void OnDestroy()
        {
            _rollButton.onClick.RemoveAllListeners();
        }

        private void Press()
        {
            OnButtonPressed?.Invoke();
        }
    }
}