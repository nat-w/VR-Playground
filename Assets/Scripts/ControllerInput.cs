using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}
	
	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	// Update is called once per frame
	void Update () {
		// This is the position of the finger on the touch pad
		if (Controller.GetAxis() != Vector2.zero) {
			Debug.Log(Controller.GetAxis());
		}

		// This is how you get input from the trigger
		if (Controller.GetHairTriggerDown()) {
			Debug.Log("Trigger Press");
		}
		if (Controller.GetHairTriggerUp()) {
			Debug.Log("Trigger Release");
		}

		// This is how you get input from the grip button
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
			Debug.Log("Grip Button Press");
		}
		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip)) {
			Debug.Log("Grip Button Press");
		}
	}
}
