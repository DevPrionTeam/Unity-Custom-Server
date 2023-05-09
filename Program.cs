using System;
using UnityDedicatedServer.Networking;

namespace UnityDedicatedServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Nome da janela
            Console.Title = "Unity Game Dedicated Server !";

            // Inicializando o servidor.
            Server.Start(50, "127.0.0.1", 26950);

            Console.ReadKey();
        }
    }
}