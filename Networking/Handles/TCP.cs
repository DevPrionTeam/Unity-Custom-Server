using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityDedicatedServer.Networking.Data;

namespace UnityDedicatedServer.Networking.Handles
{
    public class TCP
    {
        // Variaveis do socket.
        public TcpClient socket;
        private readonly int ID;

        // Configurações do buffer para stream de dados.
        public static int dataBufferSize = 4096;// MB
        private NetworkStream stream;
        private byte[] receiveBuffer;


        /// <summary>
        /// Construtor usado para guardar o ID do cliente conectado ao socket.
        /// </summary>
        /// <param name="_id"> ID do cliente conectado ao socket.</param>
        public TCP(int _id)
        {
            ID = _id;
        }

        /// <summary>
        /// Metodo responsavel por ditar o que acontece ao conectar o socket com o servidor.
        /// </summary>
        /// <param name="_socket"> Socket da conexao TCP</param>
        public void Connect(TcpClient _socket)
        {
            // Grava o socket da conexão tcp e os tamanhos do buffer de dados.
            socket = _socket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            // Preparando Stream de dados.
            stream = socket.GetStream();
            receiveBuffer = new byte[dataBufferSize];
            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);

            // Manda pacote de boas vindas.
            ServerSend._Packet0x01(ID, "Welcome to the server !");
        }

        /// <summary>
        /// Função resposavel por fazer o envio dos dados, do pacote, para o cliente.
        /// </summary>
        /// <param name="_packet">Pacote criado pelo handler. </param>
        public void SendData(Packet _packet)
        {
            try
            {
                // Verifica se o socket nao é nulo.
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ ERRO ] Client::TCP::SendData -> Erro ao enviar dados para o player {ID} via TCP ! :: {ex}");
            }
        }


        /// <summary>
        /// Metodo responsavel pelo controle de callback para retenção de dados.
        /// </summary>
        /// <param name="_result"> Resultado do callback.</param>
        private void ReceiveCallBack(IAsyncResult _result)
        {
            try
            {
                // Recebendo numero de bytes lidos do dado recebido pela stream.
                int _byteLenght = stream.EndRead(_result);

                if (_byteLenght <= 0)// Dados sem nenhum byte de tamanho [ bug ou hacker ]
                {
                    // FAZER : Desconectar cliente.
                    return;
                }

                // Criado byte de dados baseado no tamanho recebido.
                byte[] _data= new byte[_byteLenght];
                Array.Copy(receiveBuffer, _data, _byteLenght);

                // FAZER : Controle de dados.

                // Continua lendo dados da stream.
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ ERRO ] Client::TCP::ReceiveCallBack -> Erro ao receber dados da conexao TCP :: {ex}");
                // FAZER : Desconectar cliente.
            }
        }
    }
}
