using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStationPosition : MonoBehaviour {
	Transform transform;


	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform>();
		Debug.Log(transform.position);
	}
}
