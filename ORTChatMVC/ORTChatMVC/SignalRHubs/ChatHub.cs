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
    public class ChatHub : Hub
    {
        // List of connected Phone clients on server .. feel free to persist or use this list however.
        private static List<Client> clientList = new List<Client>();

        public override Task OnConnected()
        {
            Client clientToAdd = new Client()
            {
                ClientId = Context.ConnectionId,
                UserName = String.Empty
            };

            if (clientList.Any(c => c.ClientId == clientToAdd.ClientId))
            {
                return null;
            }

            clientList.Add(clientToAdd);
            return Clients.AllExcept(Context.ConnectionId).addChatMessage(JsonConvert.SerializeObject(new { name = "Server", message = "A new user has joined the room" + (Context.QueryString[0] == "webSockets" ? " from a PC" : " from a PHONE") }));
        }

        public override Task OnDisconnected()
        {
            Client client = null;

            foreach (Client existingClient in clientList)
            {
                if (Context.ConnectionId == existingClient.ClientId)
                {
                    client = existingClient;
                    break;
                }
            }

            clientList.Remove(client);

            if (client == null)
            {
                return null;
            }

            return Clients.AllExcept(Context.ConnectionId).addChatMessage(JsonConvert.SerializeObject(new { name = "Server", message = string.Format("{0} has left the room.", !String.IsNullOrEmpty(client.UserName) ? "'" + client.UserName + "'" : "An user") }));
        }

        // SignalR method call to add a new Phone client connection & join chatroom.
        //public void JoinFromPhone(string chatUserName)
        //{
        //    Client clientToAdd = new Client()
        //    {
        //        ClientId = Context.ConnectionId,
        //        UserName = chatUserName
        //    };

        //    clientList.Add(clientToAdd);

        //    Clients.AllExcept(Context.ConnectionId).addChatMessage(JsonConvert.SerializeObject(new { name = "Server", message = String.Format("{0} has joined the room from a phone", chatUserName) }));

        //    // Guess what this does?
        //    // AddToGroup("ChatRoom A");
        //}

        // Disconnect given Phone client & leave chatroom.
        public void DisconnectFromPhone(string chatUserName)
        {
            Client client = null;

            foreach (Client existingClient in clientList)
            {
                if (Context.ConnectionId == existingClient.ClientId)
                {
                    client = existingClient;
                    break;
                }
            }

            // Cleanup & remove from chatroom.
            clientList.Remove(client);
            Clients.All.removeClient(Clients.Caller);

            Clients.AllExcept(Context.ConnectionId).addChatMessage(JsonConvert.SerializeObject(new { name = "Server", message = string.Format("{0} has left the room.", chatUserName) }));
            // Pop out of group.
            // RemoveFromGroup("ChatRoom A");
        }

        public void PushMessageFromPhone(string chatUserName, string message)
        {
            // Push to all connected clients.
            Clients.All.addChatMessage(JsonConvert.SerializeObject(new { name = chatUserName, message = message }));
        }

        public void SomeoneIsTyping(string data)
        {
            var info = (dynamic)JsonConvert.DeserializeObject(data);
            info.connectionId = Context.ConnectionId;

            Clients.AllExcept(Context.ConnectionId).showIsTyping(JsonConvert.SerializeObject(info));
        }

        public void FinishTyping()
        {
            Clients.AllExcept(Context.ConnectionId).hideIsTyping(JsonConvert.SerializeObject(new { connectionId = Context.ConnectionId }));
        }

        // Get chatty.  
        public void PushMessageToClients(string data)
        {
            var info = (dynamic)JsonConvert.DeserializeObject(data);

            foreach (Client existingClient in clientList)
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