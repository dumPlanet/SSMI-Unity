using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid01 : MonoBehaviour {

    [Header("Basic Astroid01 Values")]
    private Rigidbody _RB;
    private int _hitpoints = 3;

    [Header("Constant Values of Astroid01")]
    const int _scoreValue = 75;
    const float _SPEED = 45f;
    const int _scoreRocketVal = 15;

    [Header("Other Elements")]
    public GameObject _PEXPLO;
    public GameObject _EXPLOASTROID;
    private InGameController _IGC;

    // Use this for initialization
    void Start() {
        _RB = gameObject.GetComponent<Rigidbody>();
        _RB.velocity = transform.forward * _SPEED;

        if (GameObject.Find("Main Camera") != null) {
            _IGC = GameObject.Find("Main Camera").GetComponent<InGameController>();
        } else {
            Debug.Log("Do not find Main Camera GameObject!");
        }
    } //end Start

    private void Update() {
        //Debug.Log("Asteroid 01 Hitpoints: " + _hitpoints);
        //Debug.Log("Asteroid 01 Position.z: " + gameObject.transform.position.z.ToString());

        if (_hitpoints <= 0) {
            GameObject.Instantiate(_EXPLOASTROID, gameObject.transform.position, gameObject.transform.rotation);
            _IGC.AddScore(_scoreValue);
            Destroy(gameObject, 0f);
        }
    } //end Update

    void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Boundary") || other.CompareTag("Enemies") || other.CompareTag("PowerUpRocket")) {
            return;
        }

        if (other.tag == "Player") {
            _hitpoints -= 3;
            _IGC._SHAKEDURATION = 0.4f;
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
            _IGC._SHAKEDURATION = 0.21f;
            _IGC.PlayerCol(other, _PEXPLO, 15);
        }
    } //end OnTriggerEnter
}
