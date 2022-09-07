using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLaserMove : MonoBehaviour {

	public float _SPEEDLASER;

	// Use this for initialization
	void Start () {

		GetComponent<Rigidbody>().velocity = transform.forward * _SPEEDLASER;

	}

	void Update() {
		if (gameObject.transform.position.z <= -17f)
			Destroy (gameObject, 0f);
	}
}
