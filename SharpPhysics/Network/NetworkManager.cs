using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace SharpPhysics.Network
{
	internal class NetworkManager
	{
		// client vars
		public static Socket CSocket;
		public static IPEndPoint EndPnt;
		public static char[] extraData;
		public static Packet lastPacketSent;

		// both
		public static string phase;

		// server vars
		public static char[][] extraDataReceived;
		public static Packet[] lastPacket;
		internal static byte[][] lastBytes;
		public static IPEndPoint SrvrEndPnt;
		public static Socket[] SSocket;

		public static async void InitClient(string ipAdress, int portInt)
		{
			phase = "Getting IP";
			EndPnt = new(IPAddress.Parse(ipAdress), portInt);

			phase = "Initializing socket";
			CSocket = new(
				EndPnt.AddressFamily,
				SocketType.Stream,
				ProtocolType.Tcp);

			phase = "Awaiting server response";
			await CSocket.ConnectAsync(EndPnt);

			phase = "Sending server join message";
			SendPacketClient(PacketType.Join, true);

			phase = "Awaiting server response";
			InitListener(13);


		}

		public static void SendPacketClient(PacketType type, object? data) =>
			new Thread(() => { SendPacket(type, data, CSocket); })
			{
				Name = "Network Thread"
			}.Start();

		internal static void SendPacket(PacketType type, object? data, Socket skt)
		{
			Packet pkt = new();
			pkt.extraData = extraData;
			pkt.PacketType = (int)PacketType.Join;
			pkt.packetData = true;
			byte[] pktData;
			unsafe
			{
				pkt.extraDataSize = (short)(sizeof(char) * extraData.Length);
				pkt.packetDataSize = (short)Marshal.SizeOf(data);

				Packet* ptr = &pkt;
				pktData = new byte[pkt.extraDataSize + pkt.packetDataSize + (sizeof(short) * 2) + sizeof(byte)];
				byte* curPtr = (byte*)ptr;
				for (int i = 0; i < pktData.Length; i++)
				{
					pktData[i] = *curPtr;
				}
			}

			lastPacketSent = pkt;

			skt.SendAsync(pktData);
		}

		public static async Task<Packet> GetPacket(Socket socket, int socketIdx)
		{
			lastBytes[socketIdx] = new byte[2048];
			await socket.ReceiveAsync(lastBytes[socketIdx]);
			unsafe
			{
				Packet* ptr = (Packet*)lastBytes[socketIdx][0];
				return *ptr;
			}
		}

		public static void InitListener(int port)
		{
			phase = "Initializing server";
			SrvrEndPnt = new(IPAddress.Parse("127.0.0.1"), port);

			InitSSocket(port, EndPnt);

			phase = "Initializing listeners";
			Thread thrd = new Thread(InitNormalListenerStarter);
			thrd.Name = "Server listening starter";
			thrd.Start();
		}

		internal static async void InitSSocket(int port, IPEndPoint endPnt)
		{
			Socket[] sockets = new Socket[SSocket.Length + 1];

			SSocket[SSocket.Length + 1] = new(SrvrEndPnt.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

			phase = "Binding endpoints at index " + SSocket.Length + 1;
			SSocket[SSocket.Length + 1].Bind(SrvrEndPnt);
			SSocket[SSocket.Length + 1].Listen(100);
		}

		internal static async void InitNormalListenerStarter()
		{
			while (true)
			{
				InitSSocket(13, new(IPAddress.Parse("127.0.0.1"), 13));
				await SSocket[SSocket.Length].AcceptAsync();
				new Thread(() => { InitNormalListener(SSocket.Length); }) { Name = "Server Thread" }.Start();
			}
		}

		internal static async void InitNormalListener(int socketIdx)
		{
			while (true)
			{
				await GetPacket(SSocket[socketIdx], socketIdx);
			}
		}

		public static void DisableServer()
		{

		}

		public static void DisableClient()
		{

		}
	}
}
