using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

	public float lifeTime;

	void Start() {

		//destroy explosion Instance after playing
		Destroy (gameObject, lifeTime);

	}
}
