using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	PlayerShipController.cs
*/

public class RocketCTRL : MonoBehaviour {

    [Header("Rocket Values")]
    private PlayerShipController _PSC;
    private Rigidbody _RB;
    private float _SPEEDROC = .02f;
    public GameObject _EXPLOSION;
    private float _selfDestruction;
    private float _maxLifeTime = 5f;
	private float _distance;
	private Vector3 _currentPos;

    [Header("Enemy Target")]
    public GameObject _nextEnemy;
	private Transform _enemyOfRocket;
	public GameObject _TARGETFRAME;

    [Header("Trajectory")]
    public float _gravity;

    [Header("Audio")]
    public AudioClip _FIREDFX;
    public AudioClip _NOENEFX;
    public AudioClip _ENELOGFX;
    private AudioSource _SFXROCKET;

    private void Start() {

        if (GameObject.Find("Player") != null) {
            _PSC = GameObject.Find("Player").GetComponent<PlayerShipController>();
        }

        _EXPLOSION.SetActive(false);
        _nextEnemy = _PSC._rockEne;
		_enemyOfRocket = _nextEnemy.transform;
        _RB = GetComponent<Rigidbody>();
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        _RB.transform.GetChild(0).gameObject.SetActive(true); //engine particle system
        _SFXROCKET = gameObject.GetComponent<AudioSource>();
        _SFXROCKET.PlayOneShot(_FIREDFX);

		//Create TargetFrame for the current Enemy
		if (_nextEnemy != null && _nextEnemy != _PSC._FIXNOENEMY) {
			GameObject _targetframe = GameObject.Instantiate (_TARGETFRAME, _nextEnemy.transform.position, _nextEnemy.transform.rotation);
			int _tfID = _targetframe.GetInstanceID ();
			_targetframe.transform.SetParent (_nextEnemy.transform, true);
			_targetframe.name = "Targetframe" + _tfID.ToString ();
			_targetframe.GetComponentInParent<TargetFrameCTRL> ()._Enemy = _nextEnemy;
		} 
	}

    private void FixedUpdate() {
        if (_nextEnemy != null) {
            //set Movement active
            _RB.constraints = RigidbodyConstraints.None;

            //fly to
            _RB.velocity = new Vector3(_nextEnemy.transform.position.x, _nextEnemy.transform.position.y, _nextEnemy.transform.position.z) * _SPEEDROC * Time.time;

			//count up for selfdistruction
            _selfDestruction += 0.1f;
			Debug.Log ("Rocket Controller: Selfdestruction Value count: " + _selfDestruction.ToString());

			//current position in game room
			_currentPos = BallisticVel(_enemyOfRocket); 

            //Destroy Condition for Rocket
            if (_selfDestruction > _maxLifeTime || _nextEnemy == null) {
                rocketDestroy();
            }
        }

    }

	Vector3 BallisticVel(Transform target) { //without arc trajectory :: http://luminaryapps.com/blog/arcing-projectiles-in-unity/index.html
		Vector3 targetDir = target.position - transform.position;
		float step = _SPEEDROC * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0f);
		Debug.DrawRay (transform.position, newDir, Color.red);
		transform.rotation = Quaternion.LookRotation (newDir);
		return Vector3.MoveTowards (transform.position, target.position, step);
	}

    private void rocketDestroy() {
        _EXPLOSION.SetActive(true);
        GameObject.Instantiate(_EXPLOSION, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject, .1f);
    }
}