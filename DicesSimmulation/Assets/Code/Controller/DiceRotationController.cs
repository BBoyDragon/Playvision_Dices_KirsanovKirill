using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Configs;
using Code.Controller;
using Code.Model;
using Code.Server;
using Code.View;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class DiceRotationController : IDiceRotationController
{
    private IDiceSideInformationService _diceSideInformationService;
    private DicePositionConfig _dicePositionConfig;
    private List<DiceView> _diceViews;

    public DiceRotationController(IDiceSideInformationService diceSideInformationService,
        DicePositionConfig dicePositionConfig)
    {
        _diceSideInformationService = diceSideInformationService;
        _dicePositionConfig = dicePositionConfig;
        _diceViews = new List<DiceView>();
    }


    public async void RollDices()
    {
        _diceViews.ForEach(dice=>GameObject.Destroy(dice.gameObject));
        _diceViews.Clear();
        List<DiceModel> values = await _diceSideInformationService.GetDiceInformation();
        _diceViews = _dicePositionConfig.Positions.Take(values.Count)
            .Select(position =>
                GameObject.Instantiate<DiceView>(_dicePositionConfig.DiceView, position, Quaternion.identity))
            .ToList();
        _diceViews.Zip(values, (view, model) => new { View = view, Model = model })
            .ToList()
            .ForEach(pair => pair.View.Roll(pair.Model));
    }
}