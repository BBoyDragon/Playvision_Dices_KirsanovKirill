using System;
using System.Threading.Tasks;
using Code.Configs;
using Code.Controller;
using Code.Server;
using UnityEngine;
using SignalRDiceSideInformationService = Code.Server.SignalRDiceSideInformationService;

namespace Code.View
{
    public class Bootstrap: MonoBehaviour
    {
        [SerializeField] private RollButtonView _rollButtonView;
        [SerializeField] private RollButtonView _rollButtonServerView;
        
        [SerializeField] private DicePositionConfig _dicePositionConfig;
        
        private IDiceRotationController _diceRotationController;
        private IDiceRotationController _diceRotationServerController;
        
        private Func<Task> handler;
        private Func<Task> serverHandler;
        public void Start()
        {
            _diceRotationController =
                new DiceRotationController(new OnlySixDiceSideinformationService(), _dicePositionConfig);
            _diceRotationServerController =
                new DiceRotationController(new SignalRDiceSideInformationService(), _dicePositionConfig);
            
            handler = async () => await _diceRotationController.RollDices();
            _rollButtonView.OnButtonPressed += handler;
            
            serverHandler = async () => await _diceRotationServerController.RollDices();
            _rollButtonServerView.OnButtonPressed += serverHandler;
        }

        public void OnDestroy()
        {
            _rollButtonView.OnButtonPressed -= handler;
            _rollButtonServerView.OnButtonPressed -= serverHandler;
        }
    }
}