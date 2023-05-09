using System;
using System.Collections.Generic;
using System.Linq;
using UnityDedicatedServer.Networking.Handles;
using System.Net;
using System.Net.Sockets;

namespace UnityDedicatedServer.Networking
{
    public class Server
    {
        // Configurações do servidor.
        public static int MaxPlayers { get; private set; }
        public static string Ip { get; private set; }
        public static int Port { get; private set; }
        private static TcpListener tcpListener;
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();


        /// <summary>
        /// Metodo responsavel por iniciar o servidor.
        /// </summary>
        /// <param name="_maxPlayers"> Numero maximo de players</param>
        /// <param name="_ip">IP no qual o servidor vai rodar</param>
        /// <param name="_port">Porta que o servidor esta rodando nesse IP</param>
        public static void Start(int _maxPlayers, string _ip,int _port)
        {
            // Iniciando configurações
            MaxPlayers = _maxPlayers;
            Ip = _ip;
            Port = _port;

            // Inicializando dicionario.
            InitializeServerData();

            // Inicia conexao TCP
            tcpListener = new TcpListener(IPAddress.Parse(Ip), Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectionCallBack), null);// CallBack

            // Log
            Console.WriteLine($"[ Aviso ] Server::Start() -> Servidor iniciado em : {Ip}:{Port} !");
        }


        /// <summary>
        /// Metodo responsavel pelo controle de callback da conexao TCP.
        /// </summary>
        /// <param name="_result"> Resultado do callback.</param>
        private static void TCPConnectionCallBack(IAsyncResult _result)
        { 
            // Pega o cliente e continua aceitando novas conexões.
            TcpClient _client = tcpListener.EndAcceptTcpClient( _result );
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectionCallBack), null);
            Console.WriteLine($"[ AVISO ] Server::TCPConnectionCallBack -> Tentativa de conexão recebida de {_client.Client.RemoteEndPoint} ...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                // Checa se não tem ninguem usando aquele ID
                // Caso nao tenha, Grava o cliente atual no ID verificado.
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    Console.WriteLine($"[ AVISO ] Server::TCPConnectionCallBack -> Conexão aceita para cliente {_client.Client.RemoteEndPoint} !");
                    return;
                }
            }

            // Log
            Console.WriteLine($"[ AVISO ] Server::TCPConnectionCallBack -> Falha na conexao do IP : {_client.Client.RemoteEndPoint}, servidor esta cheio !");
        }

        /// <summary>
        /// Dicionario responsavel pelos clientes do servidor e seus dados.
        /// </summary>
        private static void InitializeServerData()
        {
            // Preenchendo o dicionario e mandando o ID do cliente pro controle TCP.
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
        }
    }
}
