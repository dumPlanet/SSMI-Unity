using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonStartGame : MonoBehaviour {

	public GameObject loadingScreen;
	public Slider slider;
	private AudioSource _ASOURCE;
	public AudioClip _SFXBTN;
	public GameObject _PROBAR;

	void Start () {
		_ASOURCE = GetComponent<AudioSource> ();
		_ASOURCE.clip = _SFXBTN;
	}

	public void LoadGame(int sceneIndex) {
		_PROBAR.gameObject.SetActive (true);
		StartCoroutine(LoadAsynchronously(sceneIndex));
	}

	IEnumerator LoadAsynchronously (int sceneIndex) {
		AsyncOperation operation = SceneManager.LoadSceneAsync (sceneIndex);
		_ASOURCE.Play ();

		loadingScreen.SetActive (true);

		while (!operation.isDone) {

			float progress = Mathf.Clamp01 (operation.progress / 0.9f);
			slider.value = progress;
			yield return null;
		}
	}
}