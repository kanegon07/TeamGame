using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class OptionTag : MonoBehaviour {
	public struct OptionMessage : IEquatable<OptionMessage> {
		public bool Value;

		public OptionMessage(bool value) {
			Value = value;
		}

		public readonly bool Equals(OptionMessage other) => Value == other.Value;
	}

	[SerializeField] private InputAction OptionInputAction = null;

	[Inject] private IPublisher<OptionMessage> _optionPublisher = null;

	[Inject] private ISubscriber<bool> _optionStateSubscriber = null;

	private bool _isTurnedOn = false;

	private void OnTurn(bool flag) => _isTurnedOn = flag;

	private void Awake() {
		_optionStateSubscriber.Subscribe(x => OnTurn(x)).AddTo(this.GetCancellationTokenOnDestroy());
	}

	private void OnEnable() {
		OptionInputAction?.Enable();
	}

	private void Update() {
		if (OptionInputAction.WasPerformedThisFrame()) {
			if (_isTurnedOn) {
				_optionPublisher.Publish(new OptionMessage(false));
			} else {
				_optionPublisher.Publish(new OptionMessage(true));
			}
		}
	}

	private void OnDisable() {
		OptionInputAction?.Disable();
	}

	private void OnDestroy() {
		OptionInputAction?.Dispose();
	}
}