using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AudioPositionOutput : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Setup AudioOutput Tag
        gameObject.tag = "EnvelopAudioPosition";

        ////Position Sphere in front of the camera
        //float distance = 200f;
        //gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
        //gameObject.transform.localScale = new Vector3(5f, 5f, 5f);
  	}
	
	// Update is called once per frame
	void Update () {

        //Listen for Daydream detection
        //Update position and send OSC message
        if (DaydreamInput.activeObject != null && DaydreamInput.activeObject.tag == "EnvelopAudioPosition")
        {
            // Calculate X, Y, Z values in accordance with the active model
            GameObject selected = DaydreamInput.activeObject;

            //Send positional data back to Ableton Live  (eap -> envelop audio position)
            OSCHandler.Instance.SendMessageToClient("myClient", "/eap/1/x", selected.transform.position.x);
            OSCHandler.Instance.SendMessageToClient("myClient", "/eap/1/y", selected.transform.position.y);
            OSCHandler.Instance.SendMessageToClient("myClient", "/eap/1/z", selected.transform.position.z);
        }
	}

    public void PointerEnter() 	{
        Debug.Log("Pointer Entered!");
    }

    public void Selected() {
        Debug.Log("Selected!");
    }
}
