using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonCredits : MonoBehaviour {

	private AudioSource _ASOURCE;
	public AudioClip _SFXBTN;

	void Start () {
		_ASOURCE = GetComponent<AudioSource> ();
		_ASOURCE.clip = _SFXBTN;
	}

	// Update is called once per frame
	public void CreditsMenu () {
		SceneManager.LoadSceneAsync (2);
		_ASOURCE.Play ();
	}
}
