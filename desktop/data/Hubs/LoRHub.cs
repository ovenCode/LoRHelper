using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace data.Hubs
{
    public class LoRHub : Hub
    {
        public async Task UpdateLayoutAsync(string value)
        {
            //await parkingiController.PutParkingSpot(Guid.Parse(value));
            Console.WriteLine("Sending a layout update");
            await Clients.All.SendAsync("LayoutUpdate", value);
        }
    }
}
