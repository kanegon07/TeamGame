using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionTag : CustomButton {
	[SerializeField] private InputAction OptionInputAction = null;

	private void Update() {
		if (OptionInputAction.WasPerformedThisFrame()) {
			OnPress();
		}
	}

	private void OnDestroy() {
		OptionInputAction.Dispose();
	}

	protected override void Awake() {
		base.Awake();

		IsActiveRP.Subscribe(x => {
			if (x) {
				OptionInputAction.Enable();
			} else {
				OptionInputAction.Disable();
			}
		}).AddTo(this.GetCancellationTokenOnDestroy());
	}
}