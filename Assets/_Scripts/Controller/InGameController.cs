using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

/*
	StayAlive.cs
*/

public class InGameController : MonoBehaviour {
    [Header("General")]
    private int _score;
	private float _WAITGAMEOVER = 5f;
	public bool _gover = false;
	private bool _gridon = false;
	public bool _pause = false;
	private float _timescaler = 1f;

    [Header("Player")]
    public float _shield = 100f;
	public float _SHIELDMAX = 100f;

    [Header("Time Counting")]
    private float _isSecound = 0f;
	private float _isMinute = 0f;
	private float _isHour = 0f;
	private float _timeSubstract;

    [Header("Audio")]
    public AudioClip _BGMUSIC;
	public AudioClip _WAVEALAERT;
	public AudioClip _GAMEOVERSFX;
	private AudioSource _ASOURCEMUSIC;

    [Header("Canvas and Text")]
    public Text _TIMECOUNTER;
	public Text _GAMEOVER;
	public Canvas _INGAMECANVAS;
	public Canvas _PAUSECANVAS;
    public Text _ROCKTEXT;
    public Text _ROCKREADY;
	public Text _scoreText;

    [Header("Objects")]
    public PlayerShipController _PLAYERCTRLREF; //PlayerShip
	public GameObject[] _PLANETS; //Planets
	public GameObject[] _ENEMIES;
    public GameObject[] _POWERUP;
	public GameObject _GUIGRID; //HUD Distance
	private GameObject _Grid;
	StayAlive _STAYALIVE;

    [Header("Planet Spawn")]
    public float _STARTWAITPLANET;
	public float _SPAWNWAITPLANET;
	public float _SPAWNWAVEPLANET;
	private int _planetCounter;
	public Vector3 _spawnValuePlanet;
    private int iwave = 4;

    [Header("Enemy Spawn")]
    public float _STARTWAITENEMIES;
	public float _SPAWNWAITENEMIES;
	public float _SPAWNWAVEENEMIES;
	private int _enemiesCounter;
	public Vector3 _spawnValueEnemies;

    [Header("Camera Shake")]
    private Transform _ORIGINPOS;
	private Vector3 _originalpos;
	private float _SHAKEDECAY = 0.7f;
	private float _SHAKEINTENSE = 1.0f;
	public float _SHAKEDURATION = 0f;

    [Header("Red Flash")]
    public GameObject _REDFLASH;

    // Use this for initialization
    void Start () {
		if (!File.Exists (Application.persistentDataPath + "/save.dat")) {
			File.Create (Application.persistentDataPath + "/save.dat");
		}

        if (GameObject.Find("RedFlash")) {
            _REDFLASH = GameObject.Find("RedFlash");
            _REDFLASH.SetActive(false);
        }

        //other scripts
        _STAYALIVE = GameObject.Find ("StayAlive").GetComponent<StayAlive> ();

		//time
		_timeSubstract = 60f;

		//BG Music
		_ASOURCEMUSIC = gameObject.GetComponent<AudioSource> ();

		//Cam Shake
		_ORIGINPOS = GetComponent (typeof(Transform)) as Transform;
		_originalpos = _ORIGINPOS.localPosition;

		//Start Coroutines
		_planetCounter = _PLANETS.Length;
		_enemiesCounter = _ENEMIES.Length;
		StartCoroutine (SpawnPlanets ());
		StartCoroutine (WaveAsteroidsEnemies ());

		//Play Background Music
		_ASOURCEMUSIC.clip = _BGMUSIC;
		_ASOURCEMUSIC.Play ();
	} //end START

    // Update is called once per frame
    void Update() {
        //time count Value
        TimeCounter();

        //Pause game
        GamePause();

        //Status Texts
        SendMessage("UpdateLaserBar", _PLAYERCTRLREF._laserEnergy);
        _scoreText.text = "Score: " + _score.ToString();
        _ROCKTEXT.text = "Rockets (" + _PLAYERCTRLREF._rocketLoad.ToString() + " of " + _PLAYERCTRLREF._rocketLoadMax.ToString() + ")";

        if (_PLAYERCTRLREF._rocketnext < Time.time) {
            _ROCKREADY.text = "Rocket READY!";

        } else {

            _ROCKREADY.text = "Rocket Pending: " + Mathf.CeilToInt(_PLAYERCTRLREF._rocketnext).ToString();

        }

		if (_PLAYERCTRLREF._rocketLoad <= 0 && _PLAYERCTRLREF._rocketLoad <= 0)
			_ROCKREADY.text = "Rockets depleted!";

		if (_PLAYERCTRLREF._rockEne == _PLAYERCTRLREF._FIXNOENEMY) {
			_ROCKREADY.text = "No Enemy in reach!";
		} else {
			_ROCKREADY.text = "Rocket READY!";
		}

        if (_PLAYERCTRLREF._rLeft != null) {
            if (_PLAYERCTRLREF._RTCTL.enabled == true) {
				_ROCKREADY.text = "Rocket left in use!";
            }
        }

        if (_PLAYERCTRLREF._rRight != null) {
            if (_PLAYERCTRLREF._RTCTR.enabled == true) {
                _ROCKREADY.text = "Rocket right in use!";
            }
        }
        //Grid GUI 
        GridGUI ();

		//Shaking by Collision with Enemies
		ShakeCam ();

		//Gameover Handling
		if (_gover == true) {
			StartCoroutine (GameOver ());
		}	 
	} //end UPDATE

	//own Methods
	public void AddScore (int scoreValue) {
		_score += scoreValue;
	} //end AddScore
		
	public void ShiedControll(float shieldvalue) {
		_shield -= shieldvalue;
		SendMessage ("UpdateShieldbar", _shield);
	} //end Shield Controll

	private void GridGUI () {
		if (_gridon == false) {
			if (_Grid != null) {
				Destroy (_Grid, Time.time * .02f);
			}
			if (Input.GetButtonDown ("Fire2")) {
				_gridon = true;
			}
		} else {
			if (_Grid == null) {
				_Grid = Instantiate (_GUIGRID, new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0));
            }	
			if (Input.GetButtonDown ("Fire2")) {
				_gridon = false;
			}
		}
	} //end GridGui

	private void GamePause() {
		if (_pause == false) {

			_INGAMECANVAS.gameObject.SetActive (true);
			_PAUSECANVAS.gameObject.SetActive (false);
			_PLAYERCTRLREF.enabled = true;
			Time.timeScale = _timescaler;

			if (Input.GetButtonDown ("Cancel") == true && _pause == false) {
				_pause = true;
			}
		} else {

			_INGAMECANVAS.gameObject.SetActive (false);
			_PAUSECANVAS.gameObject.SetActive (true);
			_PLAYERCTRLREF.enabled = false;
				
			Time.timeScale = 0f;

			if (Input.GetButtonDown ("Cancel") == true && _pause == true) {
				_pause = false;
			}
		}
	} // end Pause Game

	private void TimeCounter () {
		//strings 
		string minNull = "0";
		string hourNull = "0";
		string secNull = "0";

		//current values of time
		_isSecound = Time.timeSinceLevelLoad; //true secounds
		float secound = _isSecound;

		//reset the value of secound every 60 sec to zero
		if (secound >= _timeSubstract) {
			secound = _isSecound - _timeSubstract;
			if (_isSecound >= _timeSubstract + 60f) { _timeSubstract += 60f; }
		}
			
		if (secound >= 60f) {
			_isMinute++;
		} else if (_isSecound >= 60f && _isMinute == 0f) {
			_isMinute = 1;	
		}

		if (_isMinute >= 60f) {
			_isHour++;
			_isMinute = 0;
		}

		if (_isHour >= 24f) {
			_gover = true;
		}

		_isMinute = Mathf.CeilToInt (_isMinute);
		_isHour = Mathf.CeilToInt (_isHour);
		secound = Mathf.CeilToInt (secound)-1;

		if (_isMinute >= 10f) {
			minNull = "";
		}

		if (_isHour >= 10f) {
			hourNull = "";
		}

		if (secound >= 10f) {
			secNull = "";
		}

		_TIMECOUNTER.text = _STAYALIVE._playerName + " survived " + hourNull + _isHour.ToString() + " h : " + minNull + _isMinute.ToString() + " min : " + secNull + secound + " s";

		if (_gover == true) {
			_isSecound = secound;
		}

	} // end TimeCounter

	private void ShakeCam() {
		_SHAKEINTENSE = 0.1f;
		_SHAKEDECAY = 0.02f;

		if (_SHAKEDURATION > 0f) {
			_ORIGINPOS.localPosition = _originalpos + Random.insideUnitSphere * _SHAKEINTENSE;
			_SHAKEDURATION -= _SHAKEDECAY;
		} else {
			if (_SHAKEDURATION <= 0f) {
				_SHAKEDURATION = 0f;
			}
			_ORIGINPOS.localPosition = _originalpos;
		}
	} //end ShakeCam

    public void PlayerCol(Collider other, GameObject _PEXPLOSION, int _shieldDeduction) {
        if (other.tag == "Player") {
            _REDFLASH.SetActive(true);
            _SHAKEDURATION = 0.4f;
            ShiedControll(_shieldDeduction);

            if (_shield < 0.0f || _shield <= 0.0f) {
                GameObject.Instantiate(_PEXPLOSION, other.transform.position, other.transform.rotation);
                _shield = 0f;
                Destroy(other.gameObject);
                _gover = true;
            }
        }
    } //end PlayerCollisions

    //Generate Power Up
    private void GeneratePowerup(GameObject PowerupTyp) {

        int _randomVal = Random.Range(1, 7);
         //powerup random generation
        if (_randomVal == 3) { //chance to get an powerup 1 to 6
            Vector3 _powerupSpawn = new Vector3(Random.Range(-_spawnValueEnemies.x, _spawnValueEnemies.x), _spawnValueEnemies.y, _spawnValueEnemies.z);
            Quaternion _powerupRotation = Quaternion.identity;
            GameObject.Instantiate(PowerupTyp, _powerupSpawn, _powerupRotation);
        }

    }

    //Coroutines
    IEnumerator GameOver () {
		if (_isHour >= 24f) {
			_GAMEOVER.text = "Don't waste your Time!";
			_score += 2400000;
		} else {
			_GAMEOVER.text = "GAME OVER";
		}

		_ASOURCEMUSIC.PlayOneShot (_GAMEOVERSFX);

		yield return new WaitForSeconds (_WAITGAMEOVER);

		if (_score > _STAYALIVE._scoreSaveAlive) {
			_STAYALIVE.WriteToFileScores (_score, Mathf.CeilToInt(_isSecound), Mathf.CeilToInt(_isMinute), Mathf.CeilToInt(_isHour));
		}

		SceneManager.LoadScene ("Menu");
	} //end GameOver

	IEnumerator SpawnPlanets() {
		//wait
		yield return new WaitForSeconds (_STARTWAITPLANET);

		//loop
		while (true) {
			for (int i = 0; i < _planetCounter; i++) {

				GameObject _planet = _PLANETS [Random.Range (0, _PLANETS.Length)];

				//calc start pos
				Vector3 _spawnPosition = new Vector3 (Random.Range(-_spawnValuePlanet.x, _spawnValuePlanet.x), _spawnValuePlanet.y, _spawnValuePlanet.z);
				Quaternion _spawnRotation = Quaternion.identity;

				//create planets
				GameObject.Instantiate (_planet, _spawnPosition, _spawnRotation);

				//wait
				yield return new WaitForSeconds (_SPAWNWAITPLANET);
			}

			//wait wave
			yield return new WaitForSeconds (_SPAWNWAVEPLANET);

            //Break? for a moment of stop

			//get off
			if (_gover == true) {
				break;
			}
		}
	} //end Spawn Planets

    /*
    
        Enemies Spawn Co-Routines Waves Remember the Array

        1) Asteroids

        2) Drone Formation

        3) Light Hunter

        4) Double Rammers

        5) Bomber

        6) ACS

        7) Bomber (not yet included)
         
    */

	IEnumerator WaveAsteroidsEnemies() {
		//wait
		yield return new WaitForSeconds (_STARTWAITENEMIES);
		
		//loop
		while (true) {
			for (int i = 0; i < iwave; i++) { 

				GameObject _enemy = _ENEMIES [Random.Range (0, 2)];

				//calc start pos
				Vector3 _spawnPosition = new Vector3 (Random.Range (-_spawnValueEnemies.x, _spawnValueEnemies.x), _spawnValueEnemies.y, _spawnValueEnemies.z);
				Quaternion _spawnRotation = Quaternion.identity;

				//create the Enemies
				GameObject.Instantiate (_enemy, _spawnPosition, _spawnRotation);

				//wait
				yield return new WaitForSeconds (_SPAWNWAITENEMIES);

                GeneratePowerup(_POWERUP[Random.Range(0, _POWERUP.Length)]);
			}

			//wait wave
			yield return new WaitForSeconds (_SPAWNWAVEENEMIES);
			_ASOURCEMUSIC.PlayOneShot (_WAVEALAERT);

			iwave++;

            //WaveSwitch
            if (iwave == 3) {
                StartCoroutine(WaveDronesEnemies());
                iwave = 0;
                break;
            }

			//Gameover?
			if (_gover == true) break;
		}
	} //end Spawn Asteroids

    IEnumerator WaveDronesEnemies() {

        yield return new WaitForSeconds(_STARTWAITENEMIES);

        //loop
        while (true) {
            for (int i = 0; i < iwave; i++) {

                GameObject _enemy = _ENEMIES[3];

                //calc start pos
                Vector3 _spawnPosition = new Vector3(Random.Range(-_spawnValueEnemies.x, _spawnValueEnemies.x), _spawnValueEnemies.y, _spawnValueEnemies.z);
                Quaternion _spawnRotation = Quaternion.identity;

                //create the Enemies
                GameObject.Instantiate(_enemy, _spawnPosition, _spawnRotation);

                //wait
                yield return new WaitForSeconds(_SPAWNWAITENEMIES);

                GeneratePowerup(_POWERUP[Random.Range(0, _POWERUP.Length)]);
            }

            //wait wave
            yield return new WaitForSeconds(_SPAWNWAVEENEMIES);
            _ASOURCEMUSIC.PlayOneShot(_WAVEALAERT);

            iwave++;

            //WaveSwitch
            if (iwave == 3) {
                StartCoroutine(WaveLightHuntersEnemies());
                iwave = 0;
                break;
            }

            //Gameover?
            if (_gover == true) break;
        }
    }

    IEnumerator WaveLightHuntersEnemies() {

        yield return new WaitForSeconds(_STARTWAITENEMIES);

        //loop
        while (true) {
            for (int i = 0; i < iwave; i++) {

                GameObject _enemy = _ENEMIES[4];

                //calc start pos
                Vector3 _spawnPosition = new Vector3(Random.Range(-_spawnValueEnemies.x, _spawnValueEnemies.x), _spawnValueEnemies.y, _spawnValueEnemies.z);
                Quaternion _spawnRotation = Quaternion.identity;

                //create the Enemies
                GameObject.Instantiate(_enemy, _spawnPosition, _spawnRotation);

                //wait
                yield return new WaitForSeconds(_SPAWNWAITENEMIES);

                GeneratePowerup(_POWERUP[Random.Range(0, _POWERUP.Length)]);
            }

            //wait wave
            yield return new WaitForSeconds(_SPAWNWAVEENEMIES);
            _ASOURCEMUSIC.PlayOneShot(_WAVEALAERT);

            iwave++;

            //WaveSwitch
            if (iwave == 3) {
                StartCoroutine(WaveDRammerEnemies());
                iwave = 0;
                break;
            }

            //Gameover?
            if (_gover == true) break;
        }
    }

    IEnumerator WaveDRammerEnemies() {

        yield return new WaitForSeconds(_STARTWAITENEMIES);

        //loop
        while (true) {
            for (int i = 0; i < iwave; i++) {

                GameObject _enemy = _ENEMIES[5];

                //calc start pos
                Vector3 _spawnPosition = new Vector3(Random.Range(-_spawnValueEnemies.x, _spawnValueEnemies.x), _spawnValueEnemies.y, _spawnValueEnemies.z);
                Quaternion _spawnRotation = Quaternion.identity;

                //create the Enemies
                GameObject.Instantiate(_enemy, _spawnPosition, _spawnRotation);

                //wait
                yield return new WaitForSeconds(_SPAWNWAITENEMIES);

                GeneratePowerup(_POWERUP[Random.Range(0, _POWERUP.Length)]);
            }

            //wait wave
            yield return new WaitForSeconds(_SPAWNWAVEENEMIES);
            _ASOURCEMUSIC.PlayOneShot(_WAVEALAERT);

            iwave++;

            //WaveSwitch
            if (iwave == 3) {
                StartCoroutine(WaveACSEnemies());
                iwave = 0;
                break;
            }

            //Gameover?
            if (_gover == true) break;
        }
    }

    IEnumerator WaveACSEnemies() {
        yield return new WaitForSeconds(_STARTWAITENEMIES);

        //loop
        while (true) {
            for (int i = 0; i < iwave; i++) {

                GameObject _enemy = _ENEMIES[6]; //if Bomber in Game set up

                //calc start pos
                Vector3 _spawnPosition = new Vector3(Random.Range(-_spawnValueEnemies.x, _spawnValueEnemies.x), _spawnValueEnemies.y, _spawnValueEnemies.z);
                Quaternion _spawnRotation = Quaternion.identity;

                //create the Enemies
                GameObject.Instantiate(_enemy, _spawnPosition, _spawnRotation);

                //wait
                yield return new WaitForSeconds(_SPAWNWAITENEMIES);

                GeneratePowerup(_POWERUP[Random.Range(0, _POWERUP.Length)]);
            }

            //wait wave
            yield return new WaitForSeconds(_SPAWNWAVEENEMIES);
            _ASOURCEMUSIC.PlayOneShot(_WAVEALAERT);

            iwave++;

            //WaveSwitch
            if (iwave == 3) {
                StartCoroutine(WaveAsteroidsEnemies());
                iwave = 0;
                break;
            }

            //Gameover?
            if (_gover == true) break;
        }
    }
}