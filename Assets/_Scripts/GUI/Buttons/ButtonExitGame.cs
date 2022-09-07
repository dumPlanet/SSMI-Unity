using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExitGame : MonoBehaviour {

	private AudioSource _ASOURCE;
	public AudioClip _SFXBTN;

	void Start () {
		_ASOURCE = GetComponent<AudioSource> ();
		_ASOURCE.clip = _SFXBTN;
	}

	public void ExitGame() {
		_ASOURCE.Play ();
		Application.Quit ();
	}
}
