using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour {
	public GameObject laserPrefab;
	public Transform cameraRigTransform;
	public GameObject teleportReticlePrefab;
	public Transform headTransform;
	public Vector3 teleportReticleOffset;
	public LayerMask teleportArea;
	private GameObject laser;
	private Vector3 hitPoint;
	private bool canTeleport;
	private GameObject reticle;

	
	// Get reference to controller
	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device Controller {
		get {
			return SteamVR_Controller.Input((int)trackedObj.index);
		}
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();		
	}

	void Update() {
		// if player presses touchpad, send raycast
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
			RaycastHit hit;

			if (Physics.Raycast(trackedObj.transform.position, transform.TransformDirection(Vector3.forward), out hit, 100, teleportArea)) {
				// instantiates laser and reticle if not already existing
				if (!laser && !reticle) {
					laser = Instantiate(laserPrefab);
					reticle = Instantiate(teleportReticlePrefab);
				}
				hitPoint = hit.point;
				showLaser(hit);
				reticle.transform.position = hitPoint + teleportReticleOffset;
				canTeleport = true;
			 }
		}
		else if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && laser && reticle) {
			Destroy(laser);
			laser = null;
			Destroy(reticle);
			reticle = null;
		}

		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && canTeleport) {
			Teleport();
		}
	}

	private void showLaser(RaycastHit hit) {
		laser.transform.position = Vector3.Lerp(trackedObj.transform.position,
			hitPoint, 0.5f);
		laser.transform.LookAt(hitPoint);
		laser.transform.localScale = new Vector3(0.005f, 0.02f, hit.distance);
	}

	private void Teleport() {
		canTeleport = false;
		Vector3 difference = cameraRigTransform.position - headTransform.position;
		difference.y = 0;
		cameraRigTransform.position = hitPoint + difference;
	}
}
