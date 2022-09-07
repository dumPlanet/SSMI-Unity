using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enginePlayerDestroy : MonoBehaviour {

	private InGameController _ingameController;

	void Start () {
		GameObject _ingameControllerOBJ = GameObject.FindWithTag ("MainCamera");

		if (_ingameControllerOBJ != null) {
			_ingameController = _ingameControllerOBJ.GetComponent<InGameController> ();
		}

		if (_ingameControllerOBJ == null) {
			Debug.Log ("Cannot find 'InGameController' script.");
		}
	}

	// Update is called once per frame
	void Update () {
		if (_ingameController._gover == true) {
			Destroy(gameObject, 0.2f);
		}	
	}
}