using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;

public class oscControl : MonoBehaviour
{
	private OSCServer myServer;

	public string outIP = "127.0.0.1";
	public int outPort = 9999;
	public int inPort = 57121;

	// Buffer size of the application (stores 100 messages from different servers)
	public int bufferSize = 100;

	// Script initialization
	void Start()
	{
		// init OSC
		OSCHandler.Instance.Init();

		// Initialize OSC clients (transmitters)
		OSCHandler.Instance.CreateClient("myClient", IPAddress.Parse(outIP), outPort);

		// Initialize OSC servers (listeners)
		myServer = OSCHandler.Instance.CreateServer("myServer", inPort);
		// Set buffer size (bytes) of the server (default 1024)
		myServer.ReceiveBufferSize = 1024;
		// Set the sleeping time of the thread (default 10)
		myServer.SleepMilliseconds = 10;

	}

	
	void Update()
	{		
		// Send random number to the client
		//float randVal = UnityEngine.Random.Range(0f, 0.7f);
		//OSCHandler.Instance.SendMessageToClient("myClient", "/1/fader1", randVal);

	}

}