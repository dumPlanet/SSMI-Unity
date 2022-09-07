using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCircleSpawn : MonoBehaviour {

    [Header("Drone Bullet Extention")]
    public GameObject _BULLET;
    public GameObject _BulletSparkle;

    [Header("Drone Bullet Main")]
    const float _SPEED = 75f;
    private float _WaitTime = 3f;

    private void Update() {
        StartCoroutine(FireBullets());

        if (gameObject.transform.position.y != 1.25f) {

            if (gameObject.transform.position.y < 0f) gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        }
    }

    private void SpawnBulletsInCircle() {
        for (var i = 0; i < 10; i++) {
            float _angle = i * Mathf.PI * 2 / 10f;
            Vector3 _pos = new Vector3(Mathf.Cos(_angle), 0f, Mathf.Sin(_angle)) * 36f * Time.deltaTime;
            GameObject _BulletExt;
            Rigidbody _RBExt;
            _BulletExt = Instantiate(_BULLET, gameObject.transform.position, Quaternion.identity);
            _BulletExt.name = "BulletExt" + i.ToString();
            _RBExt = _BulletExt.GetComponent<Rigidbody>();
            _RBExt.AddForce(_pos * _SPEED, ForceMode.Acceleration);
        }
    }

    IEnumerator FireBullets() {

        yield return new WaitForSeconds(_WaitTime);

        SpawnBulletsInCircle();

        GameObject.Instantiate(_BulletSparkle, gameObject.transform.position, Quaternion.identity);

        Destroy(gameObject, 0f);

    }

}