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
        // Lista de clientes conectados en el servidor
        private static List<Client> clientList = new List<Client>();

        public override Task OnConnected()
        {
            //Al conectarse un nuevo cliente, guardamos el ConnectionId como su Id
            Client clientToAdd = new Client()
            {
                ClientId = Context.ConnectionId,
                UserName = String.Empty
            };

            //Nos fijamos si ya existe en la lista de clientes, si no, lo agregamos
            if (clientList.Any(c => c.ClientId == clientToAdd.ClientId))
            {
                return null;
            }

            clientList.Add(clientToAdd);

            //Le enviamos a todos los clientes, excepto del que viene la conexión, que un nuevo usuario se ha unido a la sala
            return Clients.AllExcept(Context.ConnectionId).addChatMessage(JsonConvert.SerializeObject(new { name = "Server", message = "A new user has joined the room" + (Context.QueryString[0] == "webSockets" ? " from a PC" : " from a PHONE") }));
        }

        public override Task OnDisconnected()
        {
            Client client = null;

            //Al desconectarse un cliente, lo buscamos en la lista
            foreach (Client existingClient in clientList)
            {
                if (Context.ConnectionId == existingClient.ClientId)
                {
                    client = existingClient;
                    break;
                }
            }

            //Validamos si no existe
            if (client == null)
            {
                return null;
            }

            //Lo eliminamos de la lista
            clientList.Remove(client);
            Clients.All.removeClient(Clients.Caller);

            //Enviamos un mensaje a todos los clientes, excepto del que viene la desconexión, que un usuario ha abandonado la sala
            return Clients.AllExcept(Context.ConnectionId).addChatMessage(JsonConvert.SerializeObject(new { name = "Server", message = string.Format("{0} has left the room.", !String.IsNullOrEmpty(client.UserName) ? "'" + client.UserName + "'" : "An user") }));
        }

        //Desconexión desde un dispositivo móvil
        public void DisconnectFromPhone(string chatUserName)
        {
            Client client = null;

            //Al desconectarse un cliente, lo buscamos en la lista
            foreach (Client existingClient in clientList)
            {
                if (Context.ConnectionId == existingClient.ClientId)
                {
                    client = existingClient;
                    break;
                }
            }

            //Lo eliminamos de la lista
            clientList.Remove(client);
            Clients.All.removeClient(Clients.Caller);

            //Enviamos un mensaje a todos los clientes, excepto del que viene la desconexión, que un usuario ha abandonado la sala
            Clients.AllExcept(Context.ConnectionId).addChatMessage(JsonConvert.SerializeObject(new { name = "Server", message = string.Format("{0} has left the room.", chatUserName) }));
        }

        public void PushMessageFromPhone(string chatUserName, string message)
        {
            //Enviamos el mensaje recibido desde el dispositivo móvil a todos los clientes en la sala
            Clients.All.addChatMessage(JsonConvert.SerializeObject(new { name = chatUserName, message = message }));
        }

        public void SomeoneIsTyping(string data)
        {
            //Deserializamos los datos recibidos cuando un cliente comienza a tipear
            var info = (dynamic)JsonConvert.DeserializeObject(data);
            //Agregamos el ConnectionId para luego poder saber qué elemento del DOM eliminar cuando deja de escribir
            info.connectionId = Context.ConnectionId;

            //Enviamos a todos los clientes el mensaje de que alguien se encuentra tipeando
            Clients.AllExcept(Context.ConnectionId).showIsTyping(JsonConvert.SerializeObject(info));
        }

        public void FinishTyping()
        {
            //Al finalizar el tipeo, enviamos a todos los clientes el mensaje de que el usuario con el ConnectionId del Context dejó de tipear
            Clients.AllExcept(Context.ConnectionId).hideIsTyping(JsonConvert.SerializeObject(new { connectionId = Context.ConnectionId }));
        }

        public void PushMessageToClients(string data)
        {
            //Deserializamos los datos recibidos cuando un cliente envia un mensaje
            var info = (dynamic)JsonConvert.DeserializeObject(data);

            //Buscamos el cliente en la lista, y le agregamos el nombre si no lo tiene (primero se conecta a la sala y luego ingresa el nombre)
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

            //Enviamos el mensaje recibido a todos los clientes en la sala
            Clients.All.addChatMessage(data);
        }
    }
}