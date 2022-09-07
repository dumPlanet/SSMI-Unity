using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraResizer : MonoBehaviour {
    public float horizontalResolution = 1920;

    void OnGUI() {
        float currentAspect = (float)Screen.width / (float)Screen.height;
        //Camera.main.fieldOfView = horizontalResolution * currentAspect / 60;
        Camera.main.fieldOfView = 60f;
    }
}