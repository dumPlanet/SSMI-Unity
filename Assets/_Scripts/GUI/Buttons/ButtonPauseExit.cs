using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonPauseExit : MonoBehaviour {

	private AudioSource _ASOURCE;
	public AudioClip _SFXBTN;

	void Start () {
		_ASOURCE = GetComponent<AudioSource> ();
		_ASOURCE.clip = _SFXBTN;
	}

	// Update is called once per frame
	public void ExitToMainMenu () {
		SceneManager.LoadSceneAsync (0);
        Debug.Log("Game Quit!");
		_ASOURCE.Play ();
	}
}
