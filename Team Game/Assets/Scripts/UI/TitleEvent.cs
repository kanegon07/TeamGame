using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TitleEvent : MonoBehaviour {
	[SerializeField] private Window _title = null;
	[SerializeField] private Window _stageSelector = null;

	private UnityAction _listener = null;

	private void ActivateStageSelector() {
		_stageSelector.gameObject.SetActive(true);

		_title.ActivateOrDeactivate(false);
		_stageSelector.ActivateOrDeactivate(true);
	}
	private void TransitToStage1() {
		SceneManager.LoadScene("Stage1");
	}
	private void TransitToStage2() {
		SceneManager.LoadScene("Stage2");
	}
	private void TransitToStage3() {
		SceneManager.LoadScene("Stage3");
	}
	private void Awake() {
		_listener = new(ActivateStageSelector);
	}
	private void OnEnable() {
		EventManager.StartListening(0, _listener);
		EventManager.StartListening(1, TransitToStage1);
		EventManager.StartListening(2, TransitToStage2);
		EventManager.StartListening(3, TransitToStage3);
	}
	private void OnDisable() {
		EventManager.StopListening(0, _listener);
		EventManager.StopListening(1, TransitToStage1);
		EventManager.StopListening(2, TransitToStage2);
		EventManager.StopListening(3, TransitToStage3);
	}
}