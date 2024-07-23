using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.View
{
    public class RollButtonView: MonoBehaviour
    {
        [SerializeField] private Button _rollButton;
        public event Func<Task> OnButtonPressed;

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