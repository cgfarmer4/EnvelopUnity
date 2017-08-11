using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaydreamInput : MonoBehaviour
{
    public LineRenderer laser;
    private GameObject activeObject;
    private static float RayDistance = 200f;

    // Use this for initialization
    void Start()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laser.SetPositions(initLaserPositions);
        laser.startWidth = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        //Controller direction
        Quaternion ori = GvrControllerInput.Orientation;
        gameObject.transform.localRotation = ori;
        Vector3 vector = ori * Vector3.forward;
        ShootLaserFromTargetPosition(transform.position, vector, RayDistance);

        //Beginning of click
        if (GvrControllerInput.ClickButtonDown) {
            DetectHit(transform.position, vector, RayDistance);
        }

        //Holding click
        if(GvrControllerInput.ClickButton) {

            //Move object
            if(activeObject != null) {
                MoveActiveObject();
            }
        }

        //Click finished
        if (GvrControllerInput.ClickButtonUp) {
            DeactivateActiveObject();
        }
    }

    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Vector3 endPosition = targetPosition + (length * direction);
        laser.SetPosition(0, targetPosition);
        laser.SetPosition(1, endPosition);
    }

    void MoveActiveObject() {
        if (activeObject.tag != "EnvelopAudioInput") return;

        //EnvelopAudioInput Move
		Quaternion ori = GvrControllerInput.Orientation;		
		Vector3 vector = ori * Vector3.forward;

		float moveDistance = RayDistance;
		Vector2 touchPos = GvrControllerInput.TouchPos;

        //Use the touch position for the other axis moving
		if (touchPos.y > 0.0f && touchPos.y < 1.0f)
		{
			moveDistance = RayDistance - (RayDistance * touchPos.y);
		}		

		Vector3 endPosition = transform.position + (moveDistance * vector);
		activeObject.transform.position = endPosition;
    }

	RaycastHit DetectHit(Vector3 targetPosition, Vector3 direction, float length)
	{
		Ray ray = new Ray(targetPosition, direction);
		RaycastHit raycastHit;

		if (Physics.Raycast(ray, out raycastHit, length))
		{
			GameObject raycastHitTarget = raycastHit.transform.gameObject;
			ActivateObject(raycastHitTarget);
		}

		return raycastHit;
	}

    void ActivateObject(GameObject raycastHitTarget) {
        activeObject = raycastHitTarget;
		Color orange = new Color(0, 0, 255f);
        Color blue = new Color(0, 0, 255f);
        Color red = new Color(255f, 0, 0);
        Color green = new Color(0, 255f, 0);
        Color redGreen = new Color(255f, 255f, 0);

        Renderer inputMesh = raycastHitTarget.GetComponent<Renderer>();

		switch (raycastHitTarget.tag)
		{
			case "Column":				
				inputMesh.material.color = blue;
				break;

			case "Speaker":				
				inputMesh.material.color = blue;
				break;

			case "Sub":				
				inputMesh.material.color = red;
				break;

			case "EnvelopAudioInput":
				inputMesh.material.color = green;
				break;

			case "EnvelopChannelLevel":				
				inputMesh.material.color = redGreen;
				break;
		}
    }

    void DeactivateActiveObject() {
		Color white = new Color(255f, 255f, 255f);
		Renderer targetRenderer = activeObject.GetComponent<Renderer>();
		targetRenderer.material.color = white;
        activeObject = null;
    }
   
}
