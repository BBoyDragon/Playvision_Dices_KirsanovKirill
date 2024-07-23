using Microsoft.AspNetCore.SignalR;

namespace DiceServer;

public class SimpleDiceSixHub: Hub
{
    public async Task<List<DiceModel>> GetDiceModels()
    {
        List<DiceModel> diceModels = new List<DiceModel>();
        diceModels.Add(new DiceModel(6));
        diceModels.Add(new DiceModel(6));
        await Clients.Caller.SendAsync("ReceiveDiceModels", diceModels);
        return diceModels;
    }
}