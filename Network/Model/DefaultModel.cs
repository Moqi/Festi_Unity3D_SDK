using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using MiniJSON;
using Festi.Event;

namespace Festi.Network.Model
{

	public class DefaultModel: Festi.Event.EventDispatcher
	{
		private string _host;
		private int    _port;
		private Socket _socket;

		public DefaultModel()
		{
			this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		public void Connect(string host, int port)
		{
			this._host = host;
			this._port = port;

			IPAddress ipAdress = Dns.GetHostAddresses(this._host)[0];
			IPEndPoint endPoint = new IPEndPoint(ipAdress, this._port);
			this._socket.BeginConnect(endPoint, OnConnect, null);
		} // end Connection

		public virtual void OnConnect(IAsyncResult data)
		{
			GameEvent gameEvent = new GameEvent(this);
			this.Dispatch("OnConnect", gameEvent);
		} // end OnConnect

	}

}