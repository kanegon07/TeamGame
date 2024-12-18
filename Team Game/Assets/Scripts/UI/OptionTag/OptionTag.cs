using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using R3.Triggers;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

[RequireComponent(typeof(ObservableEventTrigger))]
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
	private ObservableEventTrigger _eventTrigger = null;

	// 自分の上でマウスのボタンが押され、離されたときのメッセージ
	public Observable<Unit> OnPointerClickObservable => _eventTrigger.OnPointerClickAsObservable()
		.AsUnitObservable();

	private void Turn() {
		if (_isTurnedOn) {
			_optionPublisher.Publish(new OptionMessage(false));
		} else {
			_optionPublisher.Publish(new OptionMessage(true));
		}
	}

	private void OnTurn(bool flag) => _isTurnedOn = flag;

	private void Awake() {
		_eventTrigger = GetComponent<ObservableEventTrigger>();

		_optionStateSubscriber.Subscribe(x => OnTurn(x))
			.AddTo(this.GetCancellationTokenOnDestroy());

		OnPointerClickObservable.Subscribe(_ => Turn())
			.AddTo(this.GetCancellationTokenOnDestroy());
	}

	private void OnEnable() {
		OptionInputAction?.Enable();
	}

	private void Update() {
		if (OptionInputAction.WasPerformedThisFrame()) {
			Turn();
		}
	}

	private void OnDisable() {
		OptionInputAction?.Disable();
	}

	private void OnDestroy() {
		OptionInputAction?.Dispose();
	}
}