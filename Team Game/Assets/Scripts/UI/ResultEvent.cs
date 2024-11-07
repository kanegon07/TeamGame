using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ResultEvent : MonoBehaviour {
	private UnityAction _listener = null;

	private void TransitToTitle() {
		SceneManager.LoadScene("Title");
	}
	private void Awake() {
		_listener = new(TransitToTitle);
	}
	private void OnEnable() {
		EventManager.StartListening(0, _listener);
	}
	private void OnDisable() {
		EventManager.StopListening(0, _listener);
	}
}