using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace data.Hubs
{
    public class LoRHub : Hub
    {
        public async Task UpdateLayout(string value)
        {
            //await parkingiController.PutParkingSpot(Guid.Parse(value));
            Console.WriteLine("Sending a layout update");
            await Clients.All.SendAsync("LayoutUpdate", value);
        }
    }
}
