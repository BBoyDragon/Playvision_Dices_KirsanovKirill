

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Model;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

namespace Code.Server
{
    public class SignalRDiceSideInformationService: IDiceSideInformationService
    {
        private HubConnection connection;

        public SignalRDiceSideInformationService()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5004/SimpleDiceSixHub")
                .Build();
        }
        public async Task<List<DiceModel>> GetDiceInformation()
        {
            try
            {
                await connection.StartAsync();
                List<DiceModel> diceModels = await connection.InvokeAsync<List<DiceModel>>("GetDiceModels");
                await connection.StopAsync();
                return diceModels;

            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                return null;
            }
        }
    }
}