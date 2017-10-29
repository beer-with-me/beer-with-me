﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

public enum Data_Type{
	Byte,
	Short,
	Unsigned_Short,
	Four_Byte_Float,
}

public enum C2M_Command{
	Unsigned = 0x00,
	C2M_CREATE = 0x20,
	C2M_JOIN = 0x22,
	C2M_LINK_KEY = 0x30,
	C2M_START0 = 0x40,
	C2M_CROSS = 0x60,
	C2M_BEER_STOP = 0x61,
	C2M_PING = 0x70,
	C2M_EXIT = 0x80,
}

public enum M2C_Command{
	Unsigned = 0x00,
	M2C_CREATE = 0x20,
	M2C_JOIN = 0x22,
	M2C_LINK = 0x40,
	M2C_WAIT_FIRE = 0x52,
	M2C_CROSS = 0x60,
	M2C_SCORE = 0x68,
	M2C_EXIT = 0x80,
}

public class Pocket{
	public int version;
	public C2M_Command C2M_command;
	public M2C_Command M2C_command;
	public int[] datas;
	public byte[] b_datas;

	public Pocket(){
		
	}

	// 輸入數值，並自動建立bytes[]
	public Pocket(int v, C2M_Command c, int[] d){
		version = v;
		C2M_command = c;
		datas = d;
		b_datas = Generate_b_datas();
	}

	public Pocket(byte[] b_d){
		b_datas = b_d;
		version = b_d [1];
		M2C_command = (M2C_Command)b_d[2];
		Data_Type[] slices = Get_M2C_Pocket_Slices_Sizes ();
		datas = new int[slices.Length];
		int pointer = 5;
		for(int i=0;i<slices.Length;i++){
			int num = 0; 
			switch (slices [i]) {
			case Data_Type.Byte:
				num = b_d [pointer++];
				datas [i] = (num >= 128) ? num - 256 : num;
				break;
			case Data_Type.Short:
				num =  b_d [pointer++];
				num += b_d [pointer++] * 256;
				datas [i] = (num >= 32768) ? num - 65536 : num;
				break;
			case Data_Type.Unsigned_Short:
				num =  b_d [pointer++];
				num += b_d [pointer++] * 256;
				datas [i] = num;
				break;
			case Data_Type.Four_Byte_Float:
				break;
			default:
				break;
			}
		}

	}

	public byte[] Generate_b_datas(){
		Data_Type[] slices = Get_C2M_Pocket_Slices_Sizes ();

		if (slices.Length != datas.Length) {
			Debug.Log ("封包長度或參數數量錯誤\n" + C2M_command.ToString() + " ");
			return null;
		}

		int data_length = Get_Bytes_Amount (slices);
		byte[] ret = new byte[5 + data_length];
		ret[0] = 0xa5;
		ret[1] = System.Convert.ToByte(version);
		ret[2] = System.Convert.ToByte((int)C2M_command);
		ret[3] = System.Convert.ToByte(data_length % 256);
		ret[4] = System.Convert.ToByte(data_length / 256);
		int pointer = 5;
		for(int i=0;i<slices.Length;i++){
			int num = 0; 
			switch (slices [i]) {
			case Data_Type.Byte:
				num = datas [i] + ((datas [i] > 0) ? 0 : 256);
				ret [pointer++] = System.Convert.ToByte (num);
				break;
			case Data_Type.Short:
				num = datas [i] + ((datas [i] > 0) ? 0 : 65536);
				ret [pointer++] = System.Convert.ToByte (num % 256);
				ret [pointer++] = System.Convert.ToByte (num / 256);
				break;
			case Data_Type.Unsigned_Short:
				num = datas [i];
				ret [pointer++] = System.Convert.ToByte (num % 256);
				ret [pointer++] = System.Convert.ToByte (num / 256);
				break;
			case Data_Type.Four_Byte_Float:
				break;
			default:
				break;
			}
		}
		return ret;
	}

	public int Get_Bytes_Amount(Data_Type[] slices){
		int ret = 0;
		foreach (Data_Type slice in slices) {
			switch (slice) {
			case Data_Type.Byte:			ret += 1;	break;
			case Data_Type.Short:			ret += 2;	break;
			case Data_Type.Unsigned_Short:	ret += 2;	break;
			case Data_Type.Four_Byte_Float:	ret += 4;	break;
			}
		}
		return ret;
	}

	Data_Type[] Get_C2M_Pocket_Slices_Sizes(){
		switch (C2M_command) {
		case C2M_Command.C2M_CREATE:	return new Data_Type[2]{ Data_Type.Short, Data_Type.Short };
		case C2M_Command.C2M_JOIN:		return new Data_Type[3]{ Data_Type.Unsigned_Short, Data_Type.Short, Data_Type.Short };
		case C2M_Command.C2M_LINK_KEY:	return new Data_Type[1]{ Data_Type.Byte };
		case C2M_Command.C2M_START0:	return new Data_Type[0]{};
		case C2M_Command.C2M_CROSS:		return new Data_Type[6]{ Data_Type.Byte, Data_Type.Short, Data_Type.Four_Byte_Float, Data_Type.Four_Byte_Float, Data_Type.Four_Byte_Float, Data_Type.Four_Byte_Float };
		case C2M_Command.C2M_BEER_STOP:	return new Data_Type[1]{ Data_Type.Four_Byte_Float };
		case C2M_Command.C2M_PING:		return new Data_Type[0]{};
		case C2M_Command.C2M_EXIT:		return new Data_Type[0]{};
		default:
			break;
		}
		return null;
	}

	Data_Type[] Get_M2C_Pocket_Slices_Sizes(){
		switch (M2C_command) {
		case M2C_Command.M2C_CREATE:	return new Data_Type[1]{ Data_Type.Unsigned_Short };
		case M2C_Command.M2C_JOIN:		return new Data_Type[1]{ Data_Type.Byte };
		case M2C_Command.M2C_LINK:		return new Data_Type[2]{ Data_Type.Byte, Data_Type.Byte };
		case M2C_Command.M2C_WAIT_FIRE:	return new Data_Type[3]{ Data_Type.Byte, Data_Type.Short, Data_Type.Short };
		case M2C_Command.M2C_CROSS:		return new Data_Type[6]{ Data_Type.Byte, Data_Type.Short, Data_Type.Four_Byte_Float, Data_Type.Four_Byte_Float, Data_Type.Four_Byte_Float, Data_Type.Four_Byte_Float };
		case M2C_Command.M2C_SCORE:		return new Data_Type[2]{ Data_Type.Four_Byte_Float, Data_Type.Four_Byte_Float };
		case M2C_Command.M2C_EXIT:		return new Data_Type[0]{};
		default:
			break;
		}
		return null;
	}

	public void Print(string s){
		if (datas == null || b_datas == null) {
			Debug.Log ("datas == null || b_datas == null");
		}

		string d_string = "";
		foreach (int d in datas)d_string += d.ToString () + ",";
		string b_d_string = "";
		foreach (byte b_d in b_datas)b_d_string += String.Format ("{0:X2}", b_d) + " ";
		Debug.Log(s + "\nversion: " + version.ToString() + "\nC2M: " + C2M_command + "\nM2C: " + M2C_command + "\ndatas: " + d_string + "\nb_datas: " + b_d_string);
	}
}

public class NetworkController : MonoBehaviour {
	private string serverIp = "61.216.17.151";
	private int serverPort = 8787;
	public GameController gameController;

	private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	private byte[] _recieveBuffer = new byte[2048];
	[HideInInspector] public bool is_Connect;
	public bool hide_ping_msg;

	public Pocket now_Pocket;

	void Start () {
		is_Connect = false;
		StartConnection(serverIp, serverPort);
		StartCoroutine (Start_Ping ());
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
		if(pocket.C2M_command != C2M_Command.C2M_PING || !hide_ping_msg)pocket.Print ("SEND");
		byte[] data = pocket.b_datas;
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
		is_Connect = true;
//		SendToServer(new byte[]{0xa5, 0x01, 0x20, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00});
	}

	IEnumerator Start_Ping(){
		while (true) {
			if (is_Connect) SendToServer (new Pocket(gameController.version, C2M_Command.C2M_PING, new int[0]{}));
			yield return new WaitForSeconds (1);
		}
	}

	/// 
	/// 接收封包.
	/// 
	private void ReceiveCallback(IAsyncResult AR)
	{
		int recieved = _clientSocket.EndReceive(AR);
		Debug.Log ("received " + recieved.ToString () + " bytes");
		if(recieved <= 0)
			return;

		byte[] recData = new byte[recieved];
		Buffer.BlockCopy(_recieveBuffer,0,recData,0,recieved);

		AnalysisReceive (recData);
	}

	public void AnalysisReceive(Byte[] recData){
		now_Pocket = new Pocket(recData);
		now_Pocket.Print ("RECEIVED");
	}


	/// 
	/// 關閉 Socket 連線.
	/// 
	public void CloseConnection() {
		_clientSocket.Shutdown(SocketShutdown.Both);
		_clientSocket.Close();
	}
}