using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFlashCTRL : MonoBehaviour {

    private Animation _ANIMATION;
    private Animator _ANIMATOR;

     void Start() {
        _ANIMATION = GetComponent<Animation>();
        _ANIMATOR = GetComponent<Animator>();          
     }

    private void Update() {
        if (_ANIMATOR.GetCurrentAnimatorStateInfo(0).IsName("AnimEnd")) {
            gameObject.SetActive(false);
        }
    }
}
