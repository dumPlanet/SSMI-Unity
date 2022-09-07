using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	PlayerShipController.cs
*/

public class TargetFrameCTRL : MonoBehaviour {

    [Header("Own Components")]
    public Texture _GREENSWITCH;

	[Header("Enemy as Target")]
    public GameObject _Enemy;

	[Header("PlayerCTRL")]
	private PlayerShipController _PSC;

    private void Start() {
		if (GameObject.Find("Player") != null) {
			_PSC = GameObject.Find ("Player").GetComponent<PlayerShipController> ();
		}
    }

    private void FixedUpdate() {

		gameObject.transform.position = new Vector3(_Enemy.transform.position.x, _Enemy.transform.position.x, _Enemy.transform.position.z);
        gameObject.transform.rotation = new Quaternion(-90f, transform.rotation.y, transform.rotation.z, 0);

		if (_Enemy.transform.position.z >= -17f) {
			gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", _GREENSWITCH);
		}
			
		if (_Enemy == null) {
            Destroy(gameObject, 0f);
        }

    }
}