using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStationPosition : MonoBehaviour {
	Transform pos;


	// Use this for initialization
	void Start () {
		pos = GetComponent<Transform>();
		Debug.Log(pos);
	}
}
