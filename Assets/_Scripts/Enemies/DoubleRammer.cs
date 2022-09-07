using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	InGameController.cs
*/

public class DoubleRammer : MonoBehaviour {

    [Header("Basic Values")]
    private Rigidbody _RB;
    public float _SPEED = 20f;
    private float _RotSpeed = 0f;
    private float _Thumble;
    private float _maxThumble = 2f;
    private float _minThumble = -2f;
    private int _hitpoints = 8;

    [Header("Thumble Helpers")]
    private bool _thumCTRLr = true;
    private bool _thumCTRLl = false;

    [Header("Constant Values of DoubleRammer")]
    const int _scoreValue = 125;
    const int _scoreRocketVal = 75;

    [Header("Extern")]
    public GameObject _PEXPLO;
    public GameObject _OWNEXPLO;
    private InGameController _IGC;

    // Use this for initialization
    void Start () {
        _RB = gameObject.GetComponent<Rigidbody>();
		_IGC = GameObject.Find ("Main Camera").GetComponent<InGameController> ();
        _RotSpeed = Random.Range(1f, 16f);
    }
	
	// Update is called once per frame
	void Update () {
        //permanent rotation
		if (_RotSpeed <= 360f) {
			_RotSpeed += 1f;
		} else {
			_RotSpeed = 0f;
		}

        //permanent thumble
        if (_thumCTRLr) {
            _Thumble += .1f;

            if (_Thumble >= _maxThumble) {
                _thumCTRLr = false;
                _thumCTRLl = true;
            }
        }

        if (_thumCTRLl) {
            _Thumble -= .1f;

            if (_Thumble <= _minThumble) {
                _thumCTRLl = false;
                _thumCTRLr = true;
            }
        }

		//application of values
		transform.rotation = Quaternion.identity;
		transform.Rotate (_Thumble, 90f, _RotSpeed, Space.Self);
        transform.Translate(0f, 0f, _SPEED * Time.deltaTime, Space.World);

        if (_hitpoints <= 0) {
            GameObject.Instantiate(_OWNEXPLO, gameObject.transform.position, gameObject.transform.rotation);
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