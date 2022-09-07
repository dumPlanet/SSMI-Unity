using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	InGameController.cs
*/


public class PlayerShipController : MonoBehaviour {

    [Header("Player Movement")]
    private float _xMin = -1.5f;
    private float _xMax = 1.5f;
    private float _SPEED = 5f;

    [Header("Player Laser Setup")]
    public bool _shotReady = false;
    public float _laserEnergy = 50f;
    public float _laserEnergyMax = 50f;
    private float _nextlaser = 0.0f; //counts up
    private float _FIRERATE = 0.5f;
    private float _LASERRECHARGE = 2.5f; //sec
    public Transform _SHOTSPAWNL;
    public Transform _SHOTSPAWNR;
    public GameObject _SHOOT;

    [Header("Rockets Setup")]
    public GameObject _ROCKET;
    public GameObject _rLeft;
    public GameObject _rRight;
    public Transform _RSpawnL;
    public Transform _RSpawnR;
    public RocketCTRL _RTCTL;
    public RocketCTRL _RTCTR;
	public string _EnemyOutofReach = "Wait, Rockt System failure: rebooting!";

    [Header("Rocket Values")]
    public int _rocketLoad;
    public int _rocketLoadMax = 10;
    public int _rlId;
    public int _rrId;
    public string _rocketname_left;
    public string _rocketname_right;
    public float _rocketnext;
    private float _RFRATE = 10f;
    private int _rockEneID;

    [Header("Rocket Enemies")]
    public GameObject _rockEne;
    public GameObject _FIXNOENEMY;

    [Header("Objects and RB")]
    private Rigidbody _RB;
    public SkyDomeController _SKYDOME;
    private InGameController _IGC;

    [Header("Audio")]
    public AudioClip _RORELFX;
    private AudioSource _PSCAudio;

    void Start() {
        _FIXNOENEMY = GameObject.Find("rocketTargetEnemyNO");
        _IGC = GameObject.Find("Main Camera").GetComponent<InGameController>();
        _RB = gameObject.GetComponent<Rigidbody>();
        _PSCAudio = gameObject.GetComponent<AudioSource>();
        _rlId = 0;
        _rrId = 0;
        BoardRockets();
    } //end Start

    //Update Loop
    void Update() {
        //Weapons 
        laserOnline(); //is laser ready to shoot
        Invoke("laserRecharge", _LASERRECHARGE); //recharge by time

        //Search permanently for Enemies
        _rockEne = FindClostestEnemy();

        if (_IGC._gover == false) {
            //Rocket Firering
			if (Input.GetButtonDown("Fire3") && _rocketLoad > 0 && _rockEne != _FIXNOENEMY ) {
                _rocketnext = Time.deltaTime + _RFRATE;
				if (Time.time > _rocketnext) {
					RocketLunch ();
				} else {
					_EnemyOutofReach = "No enemy in reach. Please wait for enemies!";
				}
            }

            if (_rocketLoad == 0 && _rocketLoadMax > 0 && _rLeft == null && _rRight == null) {
                BoardRockets();
                _PSCAudio.PlayOneShot(_RORELFX);
            }

            //Fire Lasers
            if (this.gameObject != null) {
                if (Input.GetButton("Fire1") && Time.time > _nextlaser && _shotReady == true && _laserEnergy == 50f) {
                    _nextlaser = Time.time + _FIRERATE;
                    GameObject.Instantiate(_SHOOT, _SHOTSPAWNL.position, _SHOTSPAWNL.rotation, _SHOTSPAWNL);
                    GameObject.Instantiate(_SHOOT, _SHOTSPAWNR.position, _SHOTSPAWNR.rotation, _SHOTSPAWNR);
                    _laserEnergy = 0f;
                    _shotReady = false;
                }
            }
        }
    } //end Update

    // Update Physics
    void FixedUpdate() {
        //moving player
        float _moveHorizontal = Input.GetAxis("Horizontal"); 
        Vector3 _movement = new Vector3(-_moveHorizontal, 0.0f, 0.0f); 
        _RB.velocity = _movement * _SPEED;
        _RB.position = new Vector3(Mathf.Clamp(_RB.position.x, _xMin, _xMax), 0.0f, 0.0f);
        _RB.rotation = Quaternion.Euler(0.0f, 0.0f, -_RB.velocity.x * 5f);
        _SKYDOME.SkyRotation(_moveHorizontal);
    } //end fixedUpdate

    void OnTriggerEnter(Collider other) {
        if (other.tag == "PlayerRocket") {
            return;
        }

		//Powerup Laser Upgrade +1 >> Send to IngameController >> Send to Enemy Collison and reduce Enemy hitpoint value

        if (other.tag == "Powerup") {
            _rocketLoadMax += 10;
        } 
    } //end Collider

    public void laserOnline() {
        if (Time.time > _nextlaser) {
            if (_laserEnergy <= 0) {
                _shotReady = false;
            } else {
                _shotReady = true;
            }
        }
    } //end Laser Online

    public void laserRecharge () {
        if (_laserEnergy < _laserEnergyMax) {
            _laserEnergy += 1f;
            Mathf.CeilToInt(_laserEnergy);
        }
    } //end LaserRecharge

    private void BoardRockets () {
        //left rocket
        _rLeft = GameObject.Instantiate(_ROCKET, new Vector3(_RSpawnL.position.x, _RSpawnL.position.y, _RSpawnL.position.z), Quaternion.identity);
        _rlId = _rLeft.GetInstanceID();
        _rLeft.transform.SetParent(_RSpawnL, true);
        _rLeft.name = "rocketLeft" + _rlId.ToString();
        _rocketname_left = _rLeft.name;
        _RTCTL = _rLeft.GetComponent<RocketCTRL>();
        //right rocket
        _rRight = GameObject.Instantiate(_ROCKET, new Vector3(_RSpawnR.position.x, _RSpawnR.position.y, _RSpawnR.position.z), Quaternion.identity);
        _rrId = _rRight.GetInstanceID();
        _rRight.transform.SetParent(_RSpawnR, true);
        _rRight.name = "rocketRight" + _rrId.ToString();
        _rocketname_right = _rRight.name;
        _RTCTR = _rRight.GetComponent<RocketCTRL>();

        //getting rocket script component
        _RTCTR = _rRight.GetComponent<RocketCTRL>();
        _RTCTL = _rLeft.GetComponent<RocketCTRL>();

        //disable rocket script component
        _RTCTL.enabled = false;
        _RTCTR.enabled = false;

        //setup rocket load variables
        _rocketLoad = 2;
        _rocketLoadMax -= 2;
    }//end board rocket

    GameObject FindClostestEnemy() {

        GameObject _closest = null;

        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemies");
        Vector3 position = transform.position;

        foreach (GameObject go in gos) {
            Vector3 diff = go.transform.position - position;
            float currentDistance = diff.sqrMagnitude;
			if (currentDistance >= 300f) {
				_closest = go;
			} else {
				return _FIXNOENEMY;
			}
        }
			
		return _closest;

    }//end find closest enemy

    private void RocketLunch() {
		if (_rockEne != _FIXNOENEMY || _rockEne != null) {
			if (_rLeft != null && _rRight != null && _rocketLoad % 2 == 0) {
				_RTCTL.enabled = true;
				//change value of loaded rockets
				_rocketLoad -= 1;
			}

			if (_rLeft == null && _rRight != null && _rocketLoad % 2 != 0) {
				_RTCTR.enabled = true;
				//change value of loaded rockets
				_rocketLoad -= 1;
			}
		}

        //clearfix
        if (_rocketLoad <= -1) {
            _rocketLoad = 0;
        }

        if (_rocketLoadMax <= -1) {
            _rocketLoadMax = 0;
        }
    } //end rockets
}