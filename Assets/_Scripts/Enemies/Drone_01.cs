using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	PlayerShipController.cs
	InGameController.cs
*/

public class Drone_01 : MonoBehaviour {

    [Header("Basic Drone Values")]
    private Rigidbody _RB;
    private float _SPEED = 40f;
    private int _hitpoints = 5;
    private Vector3 _sinking;
    private float _rotateX = 0f;
    private bool _isStop = false;
    private bool _isReached = false;

    [Header("Movement Controls")]
    private bool _down = false;
    private bool _dwnCtrl = true;
    private bool _rotate = false;
    private bool _rotCtrl = true;
	private float _distanceStop;

    [Header("Constant Values of Drone01")]
    const float _WAITONE = 3f;
    const float _WAITTWO = 3.5f;
    const int _scoreValue = 125;
    const int _scoreRocketVal = 5;

    [Header("Elements of Drone01")]
    //Main Engine Position
    public GameObject _EMSPAWNR;
    public GameObject _EMSPAWNL;
    //Sec Engine Position
    public GameObject _ESSPAWNL;
    public GameObject _ESSPAWNR;
    //Particles Systems for Engine Effects
    public GameObject _MEParticles; //Main Engine
    public GameObject _MSParticles; //Bottom Engine
    //Weapon Position and GameObject
	public GameObject _BULLETSPAWN;
    public GameObject _BULLET;
    //Explosion
	public GameObject _EXPLODRONE;

    [Header("In Scipt generated Parts of Drone01")]
    private GameObject _mainEngineL;
    private GameObject _mainEngineR;
    private GameObject _secEngineL;
    private GameObject _secEngineR;

    [Header("Other Elements")]
    public GameObject _PEXPLO;
    private InGameController _IGC;

    [Header("Target")]
    private GameObject _PLAYER = null;

	// Use this for initialization
	void Start () {
        _RB = gameObject.GetComponent<Rigidbody>();
        _RB.velocity = transform.forward * _SPEED;
        _PLAYER = GameObject.Find("Player"); //set off for development room
		_distanceStop = Random.Range(-12f, -10f);

		if (GameObject.Find ("Main Camera") != null) {
			_IGC = GameObject.Find ("Main Camera").GetComponent<InGameController> (); //set off for development room
		} else {
			Debug.Log ("Do not find Main Camera GameObject!");
		}

        //left main engine
		_mainEngineL = GameObject.Instantiate (_MEParticles, new Vector3(_EMSPAWNL.transform.position.x, _EMSPAWNL.transform.position.y, _EMSPAWNL.transform.position.z), Quaternion.identity);
		_mainEngineL.transform.SetParent (_EMSPAWNL.transform, true);

        //right main engine
		_mainEngineR = GameObject.Instantiate (_MEParticles, new Vector3(_EMSPAWNR.transform.position.x, _EMSPAWNR.transform.position.y, _EMSPAWNR.transform.position.z), Quaternion.identity);
		_mainEngineR.transform.SetParent (_EMSPAWNR.transform, true);

        //left secondary engine
		_secEngineL = GameObject.Instantiate (_MSParticles, new Vector3(_ESSPAWNL.transform.position.x, _ESSPAWNL.transform.position.y, _ESSPAWNL.transform.position.z), Quaternion.identity);
        _secEngineL.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
		_secEngineL.transform.SetParent (_ESSPAWNL.transform, true);

        //right secondary engine
		_secEngineR = GameObject.Instantiate (_MSParticles, new Vector3(_ESSPAWNR.transform.position.x, _ESSPAWNR.transform.position.y, _ESSPAWNR.transform.position.z), Quaternion.identity);
        _secEngineR.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        _secEngineR.transform.SetParent (_ESSPAWNR.transform, true);

        //set secoundary engine off at start
		_secEngineL.SetActive (false);
		_secEngineR.SetActive (false);
        
    } //end Start

	void Update() {
        //slow down before nav position
        if (_SPEED != 0f && gameObject.transform.position.z >= -100f) {
            _SPEED -= 0.5f;
        }

        //start coroutine at nav position 1
		if (gameObject.transform.position.z >= _distanceStop && _isReached == false) {

            _isReached = true;
            _isStop = true;
            _down = true;
            _rotate = true;

            _SPEED = 0f;

            if (_isStop && _isReached) {
                _mainEngineL.SetActive(false);
                _mainEngineR.SetActive(false);

                _RB.velocity = transform.forward * _SPEED;

                StartCoroutine(Drone());
                _isStop = false;
            }
        }

        //sinking + up
        if (_down) {
            if (_down && _dwnCtrl) {
                _sinking.y -= .2f;
            }

            if (_sinking.y == -1.5f || _sinking.y <= -1.5f) {
                _dwnCtrl = false;
            }

            if (!_dwnCtrl) {
                _secEngineL.SetActive(true);
                _secEngineR.SetActive(true);
                _sinking.y += .2f;
                if (_sinking.y >= 0f) {
                    _down = false;
                }
            }
        } else {
            _sinking.y = 0f;
        }

        //rotate up and down
        if (_rotate) {
            if (_rotate && _rotCtrl) {
                _rotateX -= .25f;
            }

            if (_rotateX == -12.5f) {
                _rotCtrl = false;
            }

            if (!_rotCtrl) {
                _rotateX += .25f;
                if (_rotateX >= 0f && _rotateX != 0f) {
                    _rotate = false;
                }
            }
        } else {
            _rotateX = 0f;
            _secEngineL.SetActive(false);
            _secEngineR.SetActive(false);
        }

        //value application
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, _sinking.y, gameObject.transform.position.z);
        gameObject.transform.rotation = new Quaternion(gameObject.transform.rotation.x, 180f, _rotateX, 0);
        //gameObject.transform.Rotate(Vector3.right * _rotateX, Space.Self);

        //hitpoint handling
        if (_hitpoints <= 0) {
            GameObject.Instantiate(_EXPLODRONE, gameObject.transform.position, gameObject.transform.rotation);
            _IGC.AddScore(_scoreValue);
            Destroy(gameObject, 0f);
        }

	} //end Update

    IEnumerator Drone() {
        //create bullet Spawn Object
        GameObject _bullet = GameObject.Instantiate(_BULLET, _BULLETSPAWN.transform.position, _BULLETSPAWN.transform.rotation);
        _bullet.transform.SetParent(_BULLETSPAWN.transform, true);

        yield return new WaitForSeconds(_WAITONE);

        _mainEngineL.SetActive (true);
		_mainEngineR.SetActive (true);

        yield return new WaitForSeconds(_WAITTWO);

        _SPEED = 40f;
		_RB.velocity = transform.forward * -_SPEED; //minus development room
        _isStop = false;
   
    } //end Coroutine

	void OnTriggerEnter(Collider other) {

		if (other.CompareTag("Boundary") || other.CompareTag("Enemies") || other.CompareTag("PowerUpRocket")) {
			return;
		}

        if (other.tag == "Player") {
            _hitpoints -= 5;
            _IGC._SHAKEDURATION = 0.1f;
            _IGC.PlayerCol(other, _PEXPLO, 15);
        }

        if (other.tag == "playerBolt") {
            _IGC.AddScore(_scoreValue);
            _hitpoints -= 1;
            _IGC._SHAKEDURATION = 0.1f;
            _IGC.PlayerCol(other, _PEXPLO, 15);
        }

        if (other.tag == "PlayerRocket") {
            _IGC.AddScore(_scoreRocketVal);
            _hitpoints -= 5;
            _IGC._SHAKEDURATION = 0.1f;
            _IGC.PlayerCol(other, _PEXPLO, 15);
        }
	} //end OnTriggerEnter
}