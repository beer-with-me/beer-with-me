using UnityEngine;
using System;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;

public enum Command{
	C2M_CREATE = 0x20,
	C2M_JOIN = 0x22,
	C2M_LINK_KEY = 0x30,
}

public class Pocket{
	public int version;
	public Command command;
	public int[] datas;

	public Pocket(){
		
	}

	public Pocket(int v, Command c, int[] d){
		version = v;
		command = c;
		datas = d;
	}

	public byte[] Convert(){
		byte[] ret = new byte[datas.Length * 2 + 5];
		ret[0] = 0xa5;
		ret[1] = System.Convert.ToByte(version);
		ret[2] = System.Convert.ToByte((int)command);
		ret[3] = System.Convert.ToByte((datas.Length * 2) % 256);
		ret[4] = System.Convert.ToByte((datas.Length * 2) / 256);
		for(int i=0;i<datas.Length;i++){
			ret [5 + i * 2] = System.Convert.ToByte ((datas[i]) % 256);
			ret [6 + i * 2] = System.Convert.ToByte ((datas[i]) / 256);
		}
		return ret;
	}
}

public class NetworkController : MonoBehaviour {
	private string serverIp = "61.216.17.151";
	private int serverPort = 8787;
	public GameController gameController;

	private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	private byte[] _recieveBuffer = new byte[2048];

	public Pocket now_Pocket;

	void Start () {
		StartConnection(serverIp, serverPort);
	}

	void OnApplicationQuit() {
		CloseConnection();
	}

	public void StartConnection(string IP, int Port) {
		try
		{
			_clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(IP), Port), new AsyncCallback(OnConnect), _clientSocket);
		}
		catch(SocketException ex)
		{
			Debug.Log(ex.Message);
		}
	}
	/// 
	/// 發送到 Server & 啟動接收
	/// 
	public void SendToServer(Pocket pocket) {
		byte[] data = pocket.Convert();
		try
		{
//			byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(sJson);
			byte[] byteArray = data;
			SendData(byteArray);
		}
		catch(SocketException ex)
		{
			Debug.LogWarning(ex.Message);
		}
		_clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),null);
	}
	/// 
	/// 發送封包到 Socket Server 
	/// 
	private void SendData(byte[] data)
	{
		SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
		socketAsyncData.SetBuffer(data,0,data.Length);
		_clientSocket.SendAsync(socketAsyncData);
	}
	private void OnConnect(IAsyncResult iar)
	{
		Debug.Log ("On Server Connected");
//		SendToServer(new byte[]{0xa5, 0x01, 0x20, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00});
	}

	/// 
	/// 接收封包.
	/// 
	private void ReceiveCallback(IAsyncResult AR)
	{
		int recieved = _clientSocket.EndReceive(AR);

		Debug.Log("ReceiveCallback - recieved: " + recieved + " bytes");

		if(recieved <= 0)
			return;

		byte[] recData = new byte[recieved];
		Buffer.BlockCopy(_recieveBuffer,0,recData,0,recieved);

		string temp = "";
		foreach (byte b in recData) {			
			temp += " ";
			temp += String.Format ("{0:X}", b);
		}
		Debug.Log (temp);
		AnalysisReceive (recData);
	}

	public void AnalysisReceive(Byte[] recData){
		Pocket pocket = new Pocket();
		pocket.version = recData [1];
		pocket.command = (Command)recData[2];
		int length = (recData[3] + recData[4] * 256) / 2;
		pocket.datas = new int[length];
		for (int i = 0; i < length; i++) {
			pocket.datas [i] = recData [5 + i * 2] + recData [6 + i * 2] * 256;
		}

		now_Pocket = pocket;
	}


	/// 
	/// 關閉 Socket 連線.
	/// 
	public void CloseConnection() {
		_clientSocket.Shutdown(SocketShutdown.Both);
		_clientSocket.Close();
	}
}