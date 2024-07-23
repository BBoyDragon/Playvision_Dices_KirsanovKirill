using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Model;

namespace Code.Server
{
    public interface IDiceSideInformationService
    {
        public Task<List<DiceModel>> GetDiceInformation();
    }
}