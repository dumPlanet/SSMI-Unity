using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	InGameController.cs
*/

public class BulletWeapon : MonoBehaviour {

    [Header("BulletExt")]
    public Rigidbody _RB;
    public float _SPEED = -75f;
    private float _maxLifeTime = 10f;
    private float _lifeTime;

    [Header("External Value")]
    private InGameController _IGC;
    public GameObject _PEXPLO;
    public GameObject _BulletSparkle;

    void Start() {
        _IGC = GameObject.Find("Main Camera").GetComponent<InGameController>();
    }

    private void Update() {

        _lifeTime += .01f ;

        if (_lifeTime >= _maxLifeTime) {
            GameObject.Instantiate(_BulletSparkle, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject, 0f);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemies") || other.CompareTag("PowerUpRocket")) {
            return;
        }

        if (other.tag == "Player") {
            _IGC._SHAKEDURATION = 0.025f;
            _IGC.PlayerCol(other, _PEXPLO, 5);
        }
    }
}