using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InOptionsController : MonoBehaviour {

	public InputField _playername;
	public Slider _volume;
	private StayAlive _STAYALIVE;

	void Start () {
		_STAYALIVE = GameObject.Find ("StayAlive").GetComponent<StayAlive> ();

		_playername.text = _STAYALIVE._playerName;
		_volume.value = _STAYALIVE._volumeSetup;
	}

	void Update() {
		_STAYALIVE._volumeSetup = _volume.value;
	}

	public void VolumeSetup() {
		_volume.value = _STAYALIVE._volumeSetup;
		_STAYALIVE.WriteToFileOptions (_playername.text, _volume.value);
	}

	public void ApplyClickName() {
		_STAYALIVE._playerName = _playername.text;
		_STAYALIVE.WriteToFileOptions (_playername.text, _volume.value);
	}
}
