using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyDomeController : MonoBehaviour {

	private float _rotationSpeed;
	private Rigidbody _RB;

	public void SkyRotation (float _speed) {

		_rotationSpeed = _speed * 2;

	}

	void Start() {

		_RB = gameObject.GetComponent<Rigidbody> ();

	}

	// Update is called once per frame
	void Update () {


		//for ongoing rotaion when input.getaxis == Time.time * _rotationSpeed 
		_RB.rotation = Quaternion.Euler (new Vector3 (0f, _rotationSpeed, 0f));

	}

}
