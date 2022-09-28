using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Trippie
{
    public class ChatHub : Hub
    {
        private static string adminConnectionId;

        public void RegisterAdminInfo(string id, string role)
        {
            if (role == "admin")
            {
                adminConnectionId = Context.ConnectionId;
            }
        }

        public async Task CreateUserPrivateChat(string userId)
        {
            string groupname = userId;

            await Groups.Add(Context.ConnectionId, groupname);
            await Groups.Add(adminConnectionId, groupname);

            Clients.Group(groupname).notify(groupname);
        }

        public void SendPrivate(string groupname, string name, string message, string roleTest)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.Group(groupname).addPrivateMessage(groupname, name, message, roleTest);
        }
    }
}