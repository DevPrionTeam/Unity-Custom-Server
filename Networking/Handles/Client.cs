using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace UnityDedicatedServer.Networking.Handles
{
    public class Client
    {
        // Variaveis do cliente.
        public int ID;
        public TCP tcp;


        /// <summary>
        /// Construtor responsavel por receber e atribuir o ID do cliente ao socket.
        /// </summary>
        /// <param name="_clientID">ID do cliente.</param>
        public Client(int _clientID)
        {
            ID = _clientID;
            tcp = new TCP(ID);
        }
    }
}
