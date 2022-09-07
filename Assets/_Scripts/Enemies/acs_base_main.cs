using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	InGameController.cs
*/

public class acs_base_main : MonoBehaviour {

	[Header("Basic Values")]
	public float _SPEED = 15f;
	private int _hitpoints = 50;
	public bool _isAlive = true;

	[Header("Constant Values of ACS Base")]
	const int _scoreValue = 500;
	const int _scoreRocketVal = 250;

	[Header("Extern")]
	public GameObject _PEXPLO;
	public GameObject _OWNEXPLO;
	private InGameController _IGC;

	// Use this for initialization
	void Start () {
		_IGC = GameObject.Find ("Main Camera").GetComponent<InGameController> ();
	}

	// Update is called once per frame
	void Update () {

		//transform.Translate(0f, 0f, _SPEED * Time.deltaTime, Space.World);

		if (_hitpoints <= 0 && _isAlive == true) {
			GameObject.Instantiate(_OWNEXPLO, gameObject.transform.position, gameObject.transform.rotation);
			_isAlive = false;
			Destroy(gameObject, 0f);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Boundary") || other.CompareTag("Enemies") || other.CompareTag("PowerUpRocket"))
		{
			return;
		}

		if (other.tag == "Player")
		{
			_hitpoints -= 8;
			_IGC._SHAKEDURATION = 0.4f;
			_IGC.PlayerCol(other, _PEXPLO, 15);
		}

		if (other.tag == "playerBolt")
		{
			_IGC.AddScore(_scoreValue);
			_hitpoints -= 1;
			_IGC._SHAKEDURATION = 0.5f;
			_IGC.PlayerCol(other, _PEXPLO, 15);
		}

		if (other.tag == "PlayerRocket")
		{
			_IGC.AddScore(_scoreRocketVal);
			_hitpoints -= 5;
			_IGC._SHAKEDURATION = 0.6f;
			_IGC.PlayerCol(other, _PEXPLO, 15);
		}
	}
}
