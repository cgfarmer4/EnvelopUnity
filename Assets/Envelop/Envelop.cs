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
    private static int NUM_CHANNELS = 24;

    //Model Objects
    GameObject EnvelopModel;
    Midway EnvelopVenue;
    ArrayList columns;
    GameObject[] inputs = new GameObject[NUM_INPUTS];
    GameObject[] outputChannels = new GameObject[NUM_CHANNELS];

	// Use this for initialization
	void Start () {
        EnvelopModel = new GameObject("Envelop");
        EnvelopVenue = new Midway();
        EnvelopVenue.Create();
        gameObject.AddComponent<oscControl>();

        ColumnModels();
        ChannelModels();
        ChannelLevelModels();
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

        if(address == "bundle") { // Position 
            foreach(OSCMessage message in pckt.Data) {

                //Debug.Log(message.Address);

                if(message.Address.Substring(1,6) == "source") {
                    String[] packetSplit = message.Address.Split(delimiterChars);
                    int inputNumber = Int32.Parse(packetSplit[2]);

                    //Debug.Log("0::" + message.Data[0]);
                    //Debug.Log("1::" + message.Data[1]);
                    //Debug.Log("2::" + message.Data[2]);

                    float positionX = Midway.cx + float.Parse(message.Data[0].ToString()) * Midway.xRange / 2;
                    float positionY = Midway.cy + float.Parse((string)message.Data[2].ToString()) * Midway.yRange / 2;
                    float positionZ = Midway.cz + float.Parse((string)message.Data[1].ToString()) * -Midway.zRange / 2;

                    inputs[inputNumber - 1].transform.position = new Vector3(positionX, positionY, positionZ);

                }
            }
        }
        else { // Output Levels
            char[] delimiters = { '/' };
            String[] splitAddress = pckt.Address.Split(delimiters);

            if (splitAddress[1] == "envelop"){
                int channel = Int32.Parse(splitAddress[3].Substring(2));
                outputChannels[channel - 1].transform.localScale = new Vector3(100.0f, 100.0f, 100.0f) * (float)pckt.Data[0];
			}

        }


        if (address == "message")
        {
            OSCMessage message = (OSCMessage)pckt.Data[0];
			Debug.Log(message);
        }
	}

	void InputModels() {
        for (int x = 0; x < NUM_INPUTS; x++) {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale += new Vector3(RADIUS, RADIUS, RADIUS);
            sphere.transform.parent = EnvelopModel.transform;
            inputs[x] = sphere;
            sphere.name = "Input" + x;
        }
    }

    void ColumnModels() {
        int numColumns = 0;
        columns = new ArrayList();
        foreach(Vector3 position in EnvelopVenue.COLUMN_POSITIONS) {
            GameObject cylinder;
			cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			cylinder.transform.localScale += new Vector3(RADIUS, HEIGHT / 2, RADIUS);
            cylinder.transform.position = new Vector3(position.x, HEIGHT / 2, position.y);

            float theta = (float) Math.Atan2(cylinder.transform.position.x, cylinder.transform.position.y) - (float)Math.PI / 2;
            cylinder.transform.Rotate(0, theta * (180 / (float)Math.PI), 0, Space.Self);
            cylinder.transform.parent = EnvelopModel.transform;
            cylinder.name = "Column" + numColumns;
            numColumns++;
            columns.Add(cylinder);
        }
    }

    void ChannelModels() {
        int speakerNum = 0;

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
            channelBox1.name = "Speaker" + speakerNum;
            speakerNum++;
            //channelBox1.transform.Rotate(-SPEAKER_ANGLE, -columnDegrees, 0, Space.Self);

			GameObject channelBox2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            channelBox2.transform.parent = EnvelopModel.transform;
			channelBox2.transform.localScale = new Vector3(21 * INCHES, 16 * INCHES, 15 * INCHES);
			channelBox2.transform.position = new Vector3(column.transform.position.x, 6 * FEET, column.transform.position.z);
            channelBox2.transform.LookAt(new Vector3(0, HEIGHT / 2, 0));
			channelBox2.name = "Speaker" + speakerNum;
			speakerNum++;

			GameObject channelBox3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            channelBox3.transform.parent = EnvelopModel.transform;
			channelBox3.transform.localScale = new Vector3(21 * INCHES, 16 * INCHES, 15 * INCHES);
			channelBox3.transform.position = new Vector3(column.transform.position.x, 11 * FEET, column.transform.position.z);
            channelBox3.transform.LookAt(Vector3.zero);
            channelBox3.transform.Rotate(-SPEAKER_ANGLE, 0, 0, Space.Self);
            channelBox3.name = "Speaker" + speakerNum;
			speakerNum++;
            //channelBox3.transform.Rotate(-SPEAKER_ANGLE, -columnDegrees, 0, Space.Self);
        }
    }

    void SubModels() {
        int numSubs = 0;

        foreach(Vector3 subPosition in EnvelopVenue.SUB_POSITIONS) {
            GameObject subBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
            subBox.transform.parent = EnvelopModel.transform;
            subBox.transform.localScale = new Vector3(29 * INCHES, 20 * INCHES, 29 * INCHES);
			subBox.transform.position = new Vector3(subPosition.x, 10 * INCHES, subPosition.y);
            subBox.transform.LookAt(Vector3.zero);
            subBox.name = "Sub" + numSubs;
            numSubs++;
        }
    }

    void ChannelLevelModels() {
        int boxPosition = 1;
        int currentColumn = 0;

        for (int i = 0; i < NUM_CHANNELS; i++) {
            GameObject cube;
            boxPosition++;

            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = EnvelopModel.transform;
            cube.transform.localScale += new Vector3(RADIUS, RADIUS, RADIUS);
            cube.name = "Channel" + i;

            if(i != 0 && i % 3 == 0) {
               currentColumn++;
               boxPosition = 1;
            }

            GameObject column = (GameObject)columns[currentColumn];
            cube.transform.position = new Vector3(column.transform.position.x + 20 * boxPosition, column.transform.position.y + 20 * boxPosition, column.transform.position.z + 20 * boxPosition);
            outputChannels[i] = cube;
        }
    }
}
