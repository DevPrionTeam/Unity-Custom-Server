using System;

namespace UnityDedicatedServer.Networking.Data
{
    /// <summary>
    /// Classe responsavel pelo controlew de pacotes enviados do servidor.
    /// </summary>
    public class ServerSend
    {
        /// <summary>
        /// Manda um pacote para um cliente especifico.
        /// </summary>
        /// <param name="_toClient">Cliente a ser mandado o pacote.</param>
        /// <param name="_packet">Pacote a ser enviado.</param>
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        /// <summary>
        /// Manda um pacote para todos os players no servidor.
        /// </summary>
        /// <param name="_packet">Pacote a ser enviado</param>
        private static void SendTCPDataToALL(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i < Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        /// <summary>
        /// Manda um pacote para todos os players, exceto para o player escolhido.
        /// </summary>
        /// <param name="_exceptSocket"></param>
        /// <param name="_packet"></param>
        private static void SendTCPDataToALL(int _exceptSocket,Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i < Server.MaxPlayers; i++)
            {
                if( i != _exceptSocket) Server.clients[i].tcp.SendData(_packet);
            }
        }


        #region Packets que o servidor manda pro cliente [...]
        public static void _Packet0x01(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.pWelcome))
            { 
                _packet.Write(_msg);
                _packet.Write(_toClient);

                // Encaminha o pacote para o socket TCP
                SendTCPData(_toClient, _packet);
            }
        }// Welcome packet sender.
        #endregion
    }
}
