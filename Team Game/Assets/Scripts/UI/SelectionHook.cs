using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.SceneManagement;

public class SelectionHook : MonoBehaviour {
	public GameObject PrevSelected { get; set; } = null;

	private IEnumerator RestrictSelection() {
		while (true) {
			yield return new WaitUntil(
				() => EventSystem.current.currentSelectedGameObject == null
			);

			if (PrevSelected == null) {
				continue;
			}

			EventSystem.current.SetSelectedGameObject(PrevSelected);
		}
	}

	private void ResetSelection(Scene _) => PrevSelected = null;

	private void Start() {
		StartCoroutine(RestrictSelection());

		SceneManager.sceneUnloaded += ResetSelection;
	}
}