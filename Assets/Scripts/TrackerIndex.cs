using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerIndex : MonoBehaviour {
	public int device;
	private SteamVR_TrackedObject script;
	// Use this for initialization
	void Start () {
		script = GetComponent<SteamVR_TrackedObject>();

		switch (device)
		{
			case 3:
				script.index = SteamVR_TrackedObject.EIndex.Device3;
				break;
			case 4:
				script.index = SteamVR_TrackedObject.EIndex.Device4;
				break;
			case 5:
				script.index = SteamVR_TrackedObject.EIndex.Device5;
				break;
			default:
				break;
		}
	}
}
