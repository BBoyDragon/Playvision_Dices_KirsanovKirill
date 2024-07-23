

using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Model;

namespace Code.Server
{
    public class SignalRDiceSideInformationService: IDiceSideInformationService
    {
        public Task<List<DiceModel>> GetDiceInformation()
        {
            throw new System.NotImplementedException();
        }
    }
}