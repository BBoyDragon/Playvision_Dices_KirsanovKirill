using System.Collections.Generic;
using System.Linq;
using Code.View;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "DicePositionConfig", menuName = "DicePositionConfig", order = 1)]
    public class DicePositionConfig: ScriptableObject
    {
        [SerializeField]
        private DiceView diceView;

        [SerializeField] private List<Vector3> positions;

        public DiceView DiceView => diceView;

        public List<Vector3> Positions => positions;
    }
}