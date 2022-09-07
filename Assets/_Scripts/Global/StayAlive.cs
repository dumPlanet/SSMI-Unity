using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[Serializable]
public class OptionData {
	public string name;
	public float svol;
}

[Serializable]
public class ScoreData {
	public int score;
	public float sec;
	public float min;
	public float hr;
}

//Game Object
public class StayAlive : MonoBehaviour {

	//trans values
	public int _scoreSaveAlive;
	public float _timeHour;
	public float _timeMinute;
	public float _timeSecound;

	//Audio
	public AudioMixer _AMIX;

	//options Data
	public string _playerName = "Defender";
	public float _volumeSetup = 0.5f;

	//reference save on itself so that only one Instance of StayAlive exists
	private static StayAlive _StayAliveExists;

	void Awake() {
		if (_StayAliveExists == null) {
			_StayAliveExists = this;
			DontDestroyOnLoad (this); 
		} else {
			Destroy (gameObject);
		}
	}

	void Start () {
		if (!File.Exists(Application.persistentDataPath + "/option.dat")) {
			File.Create (Application.persistentDataPath + "/option.dat");
		}
	}
		
	//by every frame
	void Update () {
		//_AMIX.GetFloat ("Attenuation", _volumeSetup);
		AudioListener.volume = _volumeSetup;
	}

	//own
	public void WriteToFileOptions(string name, float volume) {
		if (!File.Exists (Application.persistentDataPath + "/option.dat")) {
			File.Create (Application.persistentDataPath + "/option.dat");
		} 
			BinaryFormatter _optionsWrite = new BinaryFormatter ();
			FileStream _OPTPATH = File.Open (Application.persistentDataPath + "/option.dat", FileMode.Open);

			OptionData _data = new OptionData ();
			_data.name = name;
			_data.svol = volume;

			_optionsWrite.Serialize (_OPTPATH, _data);
			_OPTPATH.Close ();
	} //end write options data

	public void ReadFromFileOptions() {
		if (File.Exists (Application.persistentDataPath + "/option.dat")) {

			FileInfo path = new System.IO.FileInfo (Application.persistentDataPath + "/option.dat");
			long fileLength = path.Length;
			long filemin = 0L;

			if (fileLength > filemin) {
				BinaryFormatter _optionsRead = new BinaryFormatter ();
				FileStream _OPTPATH = File.Open (Application.persistentDataPath + "/option.dat", FileMode.Open);

				OptionData _data = (OptionData)_optionsRead.Deserialize (_OPTPATH);
				_playerName = _data.name;
				_volumeSetup = _data.svol;

				_OPTPATH.Close ();
			}
		}
	} //end read options data

	public void WriteToFileScores(int score, float sec, float min, float hr) {
		if (!File.Exists (Application.persistentDataPath + "/save.dat")) {
			File.Create (Application.persistentDataPath + "/save.dat");
		}

		//Files
		BinaryFormatter _optionsWrite = new BinaryFormatter ();
		FileStream _OPTPATH = File.Open (Application.persistentDataPath + "/save.dat", FileMode.Open);

		ScoreData _data = new ScoreData ();
		_data.score = score;
		_data.sec = sec;
		_data.min = min;
		_data.hr = hr;

		_optionsWrite.Serialize (_OPTPATH, _data);
		_OPTPATH.Close ();
	} //end write Scores

	public void ReadFromFileScores() {
		if (File.Exists (Application.persistentDataPath + "/save.dat")) {

			FileInfo path = new System.IO.FileInfo (Application.persistentDataPath + "/save.dat");
			long fileLength = path.Length;
			long filemin = 0L;

			if (fileLength > filemin) {
				BinaryFormatter _optionsRead = new BinaryFormatter ();
				FileStream _OPTPATH = File.Open (Application.persistentDataPath + "/save.dat", FileMode.Open);

				ScoreData _data = (ScoreData)_optionsRead.Deserialize (_OPTPATH);
				this._scoreSaveAlive = _data.score; //works
				this._timeSecound = _data.sec;
				this._timeMinute = _data.min; 
				this._timeHour = _data.hr; 

				_OPTPATH.Close ();
			}
		}
	} //end read scores
}
