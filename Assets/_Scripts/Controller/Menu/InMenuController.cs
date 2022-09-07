using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InMenuController : MonoBehaviour {

	//Text Elements
	public Text _TITLE;
	public Text _SUBTITLE;
	public Text _VERINF;
	public Text _TEXTOBJLASTSCORE;

	//formatin time output
	public string _minNull = "0";
	public string _hourNull = "0";
	public string _secNull = "0";

	//StayAlive
	private StayAlive _STAYALIVE;

	//Sound
	public AudioClip _ACLIPS;
	private AudioSource _ASOURCE;

	// Use this for initialization
	void Start () {
		_STAYALIVE = GameObject.Find ("StayAlive").GetComponent<StayAlive> ();

		_STAYALIVE.ReadFromFileOptions ();
		_STAYALIVE.ReadFromFileScores ();

		if (_STAYALIVE._timeMinute >= 10f) _minNull = "";
		if (_STAYALIVE._timeHour >= 10f) _hourNull = "";
		if (_STAYALIVE._timeSecound >= 10f) _secNull = "";

		string _scoreText = _hourNull + _STAYALIVE._timeHour.ToString () + " : " + _minNull + _STAYALIVE._timeMinute.ToString () + " : " + _secNull + _STAYALIVE._timeSecound.ToString ();

		if (_STAYALIVE._scoreSaveAlive < 1) {
			_TEXTOBJLASTSCORE.text = "There was no Defender Data found!";
		} else {
			_TEXTOBJLASTSCORE.text = "Last Best Score: " + _STAYALIVE._scoreSaveAlive.ToString () + " | " + _STAYALIVE._playerName + " |  survived: " + _scoreText;
		}

		_ASOURCE = gameObject.GetComponent<AudioSource> ();
		_ASOURCE.clip = _ACLIPS;
		_ASOURCE.Play ();
	} //end Start
	
	// Update is called once per frame
	void Update () {

		if (Time.timeScale == 0f) {
			Time.timeScale = 1f;
		}
		
	} //End Update
}
