using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Laserbar : MonoBehaviour {

	public Image LaserEnergyStatusBar_VG;
	public Text LaserPercentText;

	private void UpdateLaserBar(float laserValue) {
		float _LASERMAX = 50;
		float ratio = laserValue / _LASERMAX;
		LaserEnergyStatusBar_VG.rectTransform.localScale = new Vector3 (ratio, 1, 1);
		LaserPercentText.text = "Laser Energy: " + (ratio * 100).ToString () + "%";
	}

}
