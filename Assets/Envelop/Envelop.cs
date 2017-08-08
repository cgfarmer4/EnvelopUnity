using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityOSC;

public class Envelop : MonoBehaviour {

    //Constants
	private static float INCHES = 1.0f;
	private static float FEET = 12.0f * INCHES;
	private static float SPEAKER_ANGLE = 22.0f;
	private static float RADIUS = 20.0f * INCHES;
	private static float HEIGHT = 12.0f * FEET;
    private static int NUM_INPUTS = 2;

    //Model Objects
    GameObject EnvelopModel;
    Midway EnvelopVenue;
    ArrayList columns;
    GameObject[] inputs = new GameObject[NUM_INPUTS];

	// Use this for initialization
	void Start () {
        EnvelopModel = new GameObject("Envelop");
        EnvelopVenue = new Midway();
        EnvelopVenue.Create();
        gameObject.AddComponent<oscControl>();

        ColumnModels();
        ChannelModels();
        SubModels();
        InputModels();
    }
	
	// Update is called once per frame
	void Update () {

		// Reads all the messages received between the previous update and this one
		// Read received messages
		for (var i = 0; i < OSCHandler.Instance.packets.Count; i++)
		{
			// Process OSC
			receivedOSC(OSCHandler.Instance.packets[i]);
			// Remove them once they have been read.
			OSCHandler.Instance.packets.Remove(OSCHandler.Instance.packets[i]);
			i--;
		}

		//GameObject input = (GameObject)inputs[0];
		//input.transform.position = Input.mousePosition;
		
	}

	// Process OSC message
	private void receivedOSC(OSCPacket pckt)
	{
		if (pckt == null) { Debug.Log("Empty packet"); return; }

        char[] delimiterChars = { '/' };

		// Address
		string address = pckt.Address.Substring(1);

        if(address == "bundle") {
            foreach(OSCMessage message in pckt.Data) {

                Debug.Log(message.Address);

                if(message.Address.Substring(1,6) == "source") {
                    String[] packetSplit = message.Address.Split(delimiterChars);
                    int inputNumber = Int32.Parse(packetSplit[2]);

                    Debug.Log("0::" + message.Data[0]);
                    Debug.Log("1::" + message.Data[1]);
                    Debug.Log("2::" + message.Data[2]);

                    float positionX = Midway.cx + float.Parse(message.Data[0].ToString()) * Midway.xRange / 2;
                    float positionY = Midway.cy + float.Parse((string)message.Data[2].ToString()) * Midway.yRange / 2;
                    float positionZ = Midway.cz + float.Parse((string)message.Data[1].ToString()) * -Midway.zRange / 2;

                    inputs[inputNumber - 1].transform.position = new Vector3(positionX, positionY, positionZ);

                }
            }
        }
	}

	void InputModels() {
        for (int x = 0; x < NUM_INPUTS; x++) {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale += new Vector3(RADIUS, RADIUS, RADIUS);
            sphere.transform.parent = EnvelopModel.transform;
            inputs[x] = sphere;
        }
    }

    void ColumnModels() {
        columns = new ArrayList();
        foreach(Vector3 position in EnvelopVenue.COLUMN_POSITIONS) {
            GameObject cylinder;
			cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			cylinder.transform.localScale += new Vector3(RADIUS, HEIGHT / 2, RADIUS);
            cylinder.transform.position = new Vector3(position.x, HEIGHT / 2, position.y);

            float theta = (float) Math.Atan2(cylinder.transform.position.x, cylinder.transform.position.y) - (float)Math.PI / 2;
            cylinder.transform.Rotate(0, theta * (180 / (float)Math.PI), 0, Space.Self);
            cylinder.transform.parent = EnvelopModel.transform;
            columns.Add(cylinder);
        }
    }

    void ChannelModels() {
        foreach(GameObject column in columns) {
            //RADIANS
            //float columnTheta = (float)Math.Atan2(column.transform.position.y, column.transform.position.x) - (float)Math.PI / 2;

            //CONVERT RADIANS to DEGRESS
            //float columnDegrees = columnTheta * (180 / (float)Math.PI);

			GameObject channelBox1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            channelBox1.transform.parent = EnvelopModel.transform;
            channelBox1.transform.localScale = new Vector3(21 * INCHES, 16 * INCHES, 15 * INCHES);
            channelBox1.transform.position = new Vector3(column.transform.position.x, 1 * FEET, column.transform.position.z);
            channelBox1.transform.LookAt(Vector3.zero);
            channelBox1.transform.Rotate(-SPEAKER_ANGLE, 0, 0, Space.Self);
            //channelBox1.transform.Rotate(-SPEAKER_ANGLE, -columnDegrees, 0, Space.Self);

			GameObject channelBox2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            channelBox2.transform.parent = EnvelopModel.transform;
			channelBox2.transform.localScale = new Vector3(21 * INCHES, 16 * INCHES, 15 * INCHES);
			channelBox2.transform.position = new Vector3(column.transform.position.x, 6 * FEET, column.transform.position.z);
            channelBox2.transform.LookAt(new Vector3(0, HEIGHT / 2, 0));


			GameObject channelBox3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            channelBox3.transform.parent = EnvelopModel.transform;
			channelBox3.transform.localScale = new Vector3(21 * INCHES, 16 * INCHES, 15 * INCHES);
			channelBox3.transform.position = new Vector3(column.transform.position.x, 11 * FEET, column.transform.position.z);
            channelBox3.transform.LookAt(Vector3.zero);
            channelBox3.transform.Rotate(-SPEAKER_ANGLE, 0, 0, Space.Self);
            //channelBox3.transform.Rotate(-SPEAKER_ANGLE, -columnDegrees, 0, Space.Self);
        }
    }

    void SubModels() {
        foreach(Vector3 subPosition in EnvelopVenue.SUB_POSITIONS) {
            GameObject subBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
            subBox.transform.parent = EnvelopModel.transform;
            subBox.transform.localScale = new Vector3(29 * INCHES, 20 * INCHES, 29 * INCHES);
			subBox.transform.position = new Vector3(subPosition.x, 10 * INCHES, subPosition.y);
            subBox.transform.LookAt(Vector3.zero);
        }
    }
}
