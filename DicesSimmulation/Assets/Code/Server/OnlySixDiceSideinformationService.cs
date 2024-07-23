using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Model;

namespace Code.Server
{
    public class OnlySixDiceSideinformationService: IDiceSideInformationService
    {
        public Task<List<DiceModel>> GetDiceInformation()
        {
            List<DiceModel> diceModels = new List<DiceModel>();
            diceModels.Add(new DiceModel(6));
            diceModels.Add(new DiceModel(6));
            Task.Delay(100000);
            return Task.FromResult(diceModels);
        }
    }
}