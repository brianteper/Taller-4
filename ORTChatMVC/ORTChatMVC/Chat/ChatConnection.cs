﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ORTChatMVC.Chat;
using Microsoft.AspNet.SignalR;

namespace ORTChatMVC.Chat
{
    //public class ChatConnection : PersistentConnection
    //{
    //    private static Dictionary<string, string> _clients = new Dictionary<string, string>();

    //    protected override Task OnConnected(IRequest request, string connectionId)
    //    {
    //        _clients.Add(connectionId, string.Empty);
    //        ChatData chatData = new ChatData("Server", "A new user has joined the room.");
    //        return Connection.Broadcast(chatData);
    //    }

    //    protected override Task OnReceived(IRequest request, string connectionId, string data)
    //    {
    //        ChatData chatData = JsonConvert.DeserializeObject<ChatData>(data);
    //        _clients[connectionId] = chatData.Name;
    //        return Connection.Broadcast(chatData);
    //    }

    //    protected override Task OnDisconnected(IRequest request, string connectionId)
    //    {
    //        string name = _clients[connectionId];
    //        ChatData chatData = new ChatData("Server", string.Format("{0} has left the room.", name));
    //        _clients.Remove(connectionId);
    //        return Connection.Broadcast(chatData);
    //    }
    //}
}