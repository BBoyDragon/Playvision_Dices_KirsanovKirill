using System;
using Code.Configs;
using Code.Controller;
using Code.Server;
using UnityEngine;

namespace Code.View
{
    public class Bootstrap: MonoBehaviour
    {
        [SerializeField] private RollButtonView _rollButtonView;
        [SerializeField] private DicePositionConfig _dicePositionConfig;
        private IDiceRotationController _diceRotationController;
        public void Start()
        {
            _diceRotationController =
                new DiceRotationController(new OnlySixDiceSideinformationService(), _dicePositionConfig);
            _rollButtonView.OnButtonPressed += _diceRotationController.RollDices;
        }

        public void OnDestroy()
        {
            _rollButtonView.OnButtonPressed -= _diceRotationController.RollDices;
        }
    }
}