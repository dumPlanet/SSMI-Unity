using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	acs_base_main.cs
*/

public class acs_gun : MonoBehaviour {

	[Header("Basic Values")]
	public float _SPEED = 15f;
	public float _RotSpeed = .9f;
	private float _shotCounter;
	private float _shotCMax = 10f;
	private Quaternion _rotationAngle;

	[Header("Components")]
	public GameObject _acsBase;
	public GameObject _gun;
	private acs_base_main _acsScript;
	public GameObject _gunfire;

	[Header("Extern")]
	public GameObject _OWNEXPLO;
	private GameObject _Player;

	// Use this for initialization
	void Start () {
		_Player = GameObject.Find ("Player");
		_acsScript = _acsBase.GetComponent<acs_base_main> ();
	}

	// Update is called once per frame
	void Update () {

		_shotCounter += .1f;

		//look to the player
		_rotationAngle = Quaternion.LookRotation (new Vector3 (-_Player.transform.position.x, 0f, 0f) - transform.position, transform.forward);
		Debug.Log ("Rotation Angle: " + _rotationAngle.ToString());
		_rotationAngle.x = 0f;
		_rotationAngle.z = 0f;
		Quaternion _lastRotationTmp = gameObject.transform.rotation;
		Debug.Log ("Last Rotation Value: " + _lastRotationTmp.ToString());
		//transform.rotation = Quaternion.Slerp (_lastRotationTmp, _rotationAngle, _RotSpeed * Time.deltaTime);
		//transform.localRotation = Quaternion.Euler( _lastRotationTmp.eulerAngles.x, transform.localRotation.eulerAngles.y, _lastRotationTmp.z);
		transform.rotation = Quaternion.Lerp (_lastRotationTmp, _rotationAngle, _RotSpeed * Time.deltaTime);
		//transform.rotation = Quaternion.RotateTowards(_lastRotationTmp, _rotationAngle, _RotSpeed * Time.deltaTime); //no rotation very weak
		Debug.Log ("transform.rotation: " + transform.rotation.ToString());

		if (_shotCounter >= _shotCMax/* and rotation is done*/) {
			Debug.Log ("ACE gun shot");
			_shotCounter = 0f;
		}

		if (_acsScript._isAlive == false) {
			GameObject.Instantiate(_OWNEXPLO, gameObject.transform.position, gameObject.transform.rotation);
			Destroy(gameObject, 3f);
		}
	}
}
