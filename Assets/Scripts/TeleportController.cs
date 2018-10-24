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
	private Transform laserTransform;
	private Vector3 hitPoint;
	private bool canTeleport;
	private GameObject reticle;
	private Transform teleportReticleTransform;

	
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

	void Start() {
		laser = Instantiate(laserPrefab);
		laserTransform = laser.transform;
		reticle = Instantiate(teleportReticlePrefab);
		teleportReticleTransform = reticle.transform;
	}

	void Update() {
		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) {
			RaycastHit hit;

			if (Physics.Raycast(trackedObj.transform.position,
				transform.forward, out hit, 100, teleportArea)) {
					hitPoint = hit.point;
					showLaser(hit);
					reticle.SetActive(true);
					teleportReticleTransform.position = hitPoint + teleportReticleOffset;
					canTeleport = true;
			 }
		}
		else {
			laser.SetActive(false);
			reticle.SetActive(false);
		}
		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && canTeleport) {
			Teleport();
		}
	}

	private void showLaser(RaycastHit hit) {
		laser.SetActive(true);
		laserTransform.position = Vector3.Lerp(trackedObj.transform.position,
			hitPoint, 0.5f);
		laserTransform.LookAt(hitPoint);
		laserTransform.localScale = new Vector3(laserTransform.localScale.x,
			laserTransform.localScale.y, hit.distance);
	}

	private void Teleport() {
		canTeleport = false;
		reticle.SetActive(false);
		Vector3 difference = cameraRigTransform.position - headTransform.position;
		difference.y = 0;
		cameraRigTransform.position = hitPoint + difference;
	}
}
