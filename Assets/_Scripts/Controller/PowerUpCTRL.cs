using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCTRL : MonoBehaviour {

    public GameObject _PowerupExplo;

    void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Enemies") || other.CompareTag("Boundary")) {
            return;
        }

        if (other.tag == "Player")
        {
            GameObject.Instantiate(_PowerupExplo, transform.position, transform.rotation);
            Destroy(gameObject, 0f);
        }

    }
}
