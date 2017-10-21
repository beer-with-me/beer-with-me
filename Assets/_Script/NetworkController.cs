using UnityEngine;
using System;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class NetworkController : MonoBehaviour {
	public GameController gameController;

	private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	private byte[] _recieveBuffer = new byte[2048];

	void Start () {
		StartConnection("61.216.17.151", 8787);
//		SendToServer(new byte[]{0xa5, 0x01, 0x20, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00});
	}

	void Update(){
//		if(Input.GetMouseButtonDown(0))
//			SendToServer(new byte[]{0xa5, 0x01, 0x22, 0x06, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00});
	}

	void OnApplicationQuit() {
		CloseConnection();
	}

	public void StartConnection(string IP, int Port) {
		try
		{
			_clientSocket.Connect(new IPEndPoint(IPAddress.Parse(IP), Port));
		}
		catch(SocketException ex)
		{
			Debug.Log(ex.Message);
		}
	}
	/// 
	/// 發送到 Server & 啟動接收
	/// 
	public void SendToServer(byte[] data) {
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

		string recvStr = Encoding.UTF8.GetString(recData, 0, recieved);
//		Debug.Log("recvStr: " + recvStr);
		string temp = "";
		foreach (byte b in recData) {
			temp += " ";
			temp += String.Format ("{0:X}", b);
		}
		Debug.Log (temp);

		_clientSocket.BeginReceive(_recieveBuffer,0,_recieveBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),null);
	}
	/// 
	/// 關閉 Socket 連線.
	/// 
	public void CloseConnection() {
		_clientSocket.Shutdown(SocketShutdown.Both);
		_clientSocket.Close();
	}
}