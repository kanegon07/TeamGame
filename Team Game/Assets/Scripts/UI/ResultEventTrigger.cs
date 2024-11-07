using UnityEngine;

public class ResultEventTrigger : MonoBehaviour {
	[SerializeField] private CustomButton _returnToTitle = null;

	private void OnEnable() {
		_returnToTitle.OnClickAsync += () => {
			EventManager.TriggerEvent(0);
		};
	}
}