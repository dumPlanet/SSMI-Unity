using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shieldbar : MonoBehaviour {

	public Image Shieldbar_CURRENT;
	public Text ShieldPercentText;

	private void UpdateShieldbar(float _shieldvalue) {
		float _SHIELDMAX = 100f;
		float ratio = Mathf.Clamp01(_shieldvalue/_SHIELDMAX);
        Shieldbar_CURRENT.rectTransform.localScale = new Vector3 (ratio, 1, 1);
		ShieldPercentText.text = "Shield Energy " + (ratio * 100f).ToString() + "%" ;
	}
}