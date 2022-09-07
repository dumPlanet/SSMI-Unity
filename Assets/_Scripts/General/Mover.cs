using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    private float _speed;
    private float _speedEnemies;
    private float _speedPowerup;
    private float _scale;

    // Update is called once per frame
    void Start () {

        _speed = Random.Range(3f, 9f) * 5f;
        _speedEnemies = Random.Range(11f, 16f) * 5f;
        _speedPowerup = Random.Range(10f, 21f);
        _scale = Random.Range(2f, 5f);

        if (tag == "Enemies") {
            GetComponent<Rigidbody>().velocity = transform.forward * _speedEnemies;
        }

        if (tag == "Powerup") {
            GetComponent<Rigidbody>().velocity = transform.forward * _speedPowerup;
        }

        if (tag == "PlanetsVFX") {
            GetComponent<Transform>().localScale = new Vector3(_scale, _scale, _scale);
            GetComponent<Rigidbody>().velocity = transform.forward * _speed;
        }
	}
}