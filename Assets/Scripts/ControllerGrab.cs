using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrab : MonoBehaviour {
	private SteamVR_TrackedObject trackedObj;
	// Object that controller is colliding with
	private GameObject collidingObj;
	// Object that player is currently grabbing
	private GameObject objInHand;

	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input((int)trackedObj.index);}
	}
	
	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	// Check if colliding object exists and has a rigid body
	private void SetCollidingObject(Collider other) {
		if (collidingObj || !other.GetComponent<Rigidbody>()) {
			return;
		}
		collidingObj = other.gameObject;
	}

	// Sets up object as potential grab target
	public void OnTriggerEnter(Collider other) {
		SetCollidingObject(other);
	}
	// Object is still grab target when controller hovers
	public void OnTriggerStay(Collider other) {
		SetCollidingObject(other);
	}
	// Remove target once controller leaves
	public void OnTriggerExit(Collider other) {
		if (!collidingObj) {
			return;
		}
		collidingObj = null;
	}

	// Creates a joint between the controller and object
	public void GrabObject() {
		objInHand = collidingObj;
		collidingObj = null;

		var joint = AddFixedJoint();
		joint.connectedBody = objInHand.GetComponent<Rigidbody>();
	}

	// Creates a fixed joint on the controller
	public FixedJoint AddFixedJoint() {
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	// Destroys joint
	private void ReleaseObject() {
		if (GetComponent<FixedJoint>()) {
			return;
		}
		// Destroys the joint, freeing the object
		GetComponent<FixedJoint>().connectedBody = null;
		Destroy(GetComponent<FixedJoint>());

		// Changes velcity of object (throwing) 
		objInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
		objInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
		objInHand = null;
	}

	// Update is called once per frame
	void Update () {
		// Grabs object when trigger is pressed down
		if (Controller.GetHairTriggerDown()) {
			if (collidingObj) {
				GrabObject();
			}
		}
		// Drops object when trigger is released
		if (Controller.GetHairTriggerUp()) {
			if (objInHand) {
				ReleaseObject();
			}
		}
	}
}
