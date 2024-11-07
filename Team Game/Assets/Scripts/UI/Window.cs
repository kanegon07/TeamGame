using UnityEngine;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour {
	[SerializeField] private CustomButton _firstSelected = null;

	private CanvasGroup _canvasGroup = null;

	public void ResetSelectedObject() {
		EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
	}
	public void ActivateOrDeactivate(bool flag) {
		_canvasGroup.interactable = flag;
		if (flag) {
			ResetSelectedObject();
			// UIController.CurrentWindow = this;
		}
	}
	private void OnEnable() {
		if (_canvasGroup == null) {
			_canvasGroup = GetComponent<CanvasGroup>();
		}

		ResetSelectedObject();
	}
	private void Start() {
		ResetSelectedObject();
	}
}