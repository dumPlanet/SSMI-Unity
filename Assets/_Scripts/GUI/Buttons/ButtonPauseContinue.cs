using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPauseContinue : MonoBehaviour {

	private InGameController _INGAMECONTROLLER;

	private AudioSource _ASOURCE;
	public AudioClip _SFXBTN;

	void Start () {
		_INGAMECONTROLLER = GameObject.Find ("Main Camera").GetComponent<InGameController> ();
		_ASOURCE = GetComponent<AudioSource> ();
		_ASOURCE.clip = _SFXBTN;
	}

	public void PauseGameEnd () {
		_INGAMECONTROLLER._pause = false;
		_ASOURCE.Play ();
	}
}
