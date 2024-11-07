using UnityEngine;

public class TitleEventTrigger : MonoBehaviour {
	[SerializeField] private CustomButton _start = null;
	[SerializeField] private CustomButton _stage1 = null;
	[SerializeField] private CustomButton _stage2 = null;
	[SerializeField] private CustomButton _stage3 = null;

	private void OnEnable() {
		_start.OnClickAsync += () => {
			EventManager.TriggerEvent(0);
		};

		_stage1.OnClickAsync += () => {
			EventManager.TriggerEvent(1);
		};

		_stage2.OnClickAsync += () => {
			EventManager.TriggerEvent(2);
		};

		_stage3.OnClickAsync += () => {
			EventManager.TriggerEvent(3);
		};
	}
}