using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ORTChatMVC.SignalRHubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        // List of connected Phone clients on server .. feel free to persist or use this list however.
        private static List<PhoneClient> PhoneClientList = new List<PhoneClient>();
        private static List<PcClient> PcClientList = new List<PcClient>();

        public override Task OnConnected()
        {
            PcClient pcClientToAdd = new PcClient()
            {
                ClientId = Context.ConnectionId,
                UserName = String.Empty
            };
            PcClientList.Add(pcClientToAdd);

            return Clients.All.addChatMessage(JsonConvert.SerializeObject(new { name = "Server", message = "A new user has joined the room" }));
        }

        public override Task OnDisconnected()
        {
            PcClient client = null;

            foreach (PcClient existingClient in PcClientList)
            {
                if (Context.ConnectionId == existingClient.ClientId)
                {
                    client = existingClient;
                    break;
                }
            }

            PcClientList.Remove(client);

            return Clients.All.addChatMessage(JsonConvert.SerializeObject(new { name = "Server", message = string.Format("{0} has left the room.", client.UserName) }));
        }

        // SignalR method call to add a new Phone client connection & join chatroom.
        public void JoinFromPhone(string phoneID, string chatUserName)
        {
            PhoneClient phoneClientToAdd = new PhoneClient()
            {
                PhoneClientId = phoneID,
                UserName = chatUserName
            };

            PhoneClientList.Add(phoneClientToAdd);

            // Guess what this does?
            // AddToGroup("ChatRoom A");
        }

        // Disconnect given Phone client & leave chatroom.
        public void Disconnect(string phoneID, string chatUserName)
        {
            PhoneClient client = null;

            foreach (PhoneClient existingClient in PhoneClientList)
            {
                if (phoneID == existingClient.PhoneClientId)
                {
                    client = existingClient;
                    break;
                }
            }

            // Cleanup & remove from chatroom.
            PhoneClientList.Remove(client);
            Clients.All.removeClient(Clients.Caller);

            // Pop out of group.
            // RemoveFromGroup("ChatRoom A");
        }

        // Get chatty.
        public void PushMessageToClients(string data)
        {
            var info = (dynamic)JsonConvert.DeserializeObject(data);

            foreach (PcClient existingClient in PcClientList)
            {
                if (Context.ConnectionId == existingClient.ClientId)
                {
                    if (String.IsNullOrEmpty(existingClient.UserName))
                    {
                        existingClient.UserName = info.name;
                    }
                    break;
                }
            }

            // Push to all connected clients.
            Clients.All.addChatMessage(data);

            // Guess what the next few lines do ...

            // Invoke a method on the calling client only.
            // Caller.addChatMessage(message);

            // Similar to above, the more verbose way.
            // Clients[Context.ConnectionId].addChatMessage(message);

            // Communicate to a Group.
            // Clients["ChatRoom A"].addChatMessage(message);
        }
    }
}