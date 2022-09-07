using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {

	private float thumble = 1.5f;
    private float thPlan = 0.15f;
    
	void Start() {

        if (tag == "Enemies") {
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * thumble;
        } else if (tag == "PlanetsVFX") {
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * thPlan;
        } else {
            //move the game object to screen bottom (-) or up 
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * thumble;
        }

    }
}
