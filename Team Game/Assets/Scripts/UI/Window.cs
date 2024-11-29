using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(CanvasGroup))]
public class Window : MonoBehaviour {
	public struct DisplayMessage : IEquatable<DisplayMessage> {
		public bool Value;

		public DisplayMessage(bool value) {
			Value = value;
		}

		public readonly bool Equals(DisplayMessage other) => Value == other.Value;
	}

	public struct ActivateMessage : IEquatable<ActivateMessage> {
		public bool Value;

		public ActivateMessage(bool value) {
			Value = value;
		}

		public readonly bool Equals(ActivateMessage other) => Value == other.Value;
	}

	[SerializeField] private byte ID = 0;
	[SerializeField] private CustomButton FirstSelected = null;

	[Inject] private IPublisher<int, DisplayMessage> _displayPublisher = null;
	[Inject] private IPublisher<int, ActivateMessage> _activatePublisher = null;

	[Inject] private ISubscriber<byte, DisplayMessage> _displaySubscriber = null;
	[Inject] private ISubscriber<byte, ActivateMessage> _activateSubscriber = null;

	private readonly ReactiveProperty<bool> _isDisplayedRP = new(false);
	private readonly ReactiveProperty<bool> _isActiveRP = new(false);

	private Image _image = null;
	private CanvasGroup _canvasGroup = null;

	public ReadOnlyReactiveProperty<bool> IsDisplayedRP => _isDisplayedRP;
	public ReadOnlyReactiveProperty<bool> IsActiveRP => _isActiveRP;

	public bool IsDisplayed {
		get => _isDisplayedRP.Value;
		set => _isDisplayedRP.Value = value;
	}

	public bool IsActive {
		get => _isActiveRP.Value;
		set => _isActiveRP.Value = value;
	}

	private void OnDisplay(DisplayMessage msg) {
		IsDisplayed = msg.Value;

		if (_image != null) {
			_image.enabled = msg.Value;
		}

		_displayPublisher.Publish(transform.GetInstanceID(), msg);
	}

	private void OnActivate(ActivateMessage msg) {
		IsActive = msg.Value;

		_canvasGroup.interactable = msg.Value;

		_activatePublisher.Publish(transform.GetInstanceID(), msg);

		if (msg.Value) {
			if (FirstSelected == null) {
				EventSystem.current.SetSelectedGameObject(null);
			} else {
				EventSystem.current.SetSelectedGameObject(FirstSelected.gameObject);
			}
		}
	}

	private void Awake() {
		_image = GetComponent<Image>();
		_canvasGroup = GetComponent<CanvasGroup>();

		_displaySubscriber.Subscribe(ID, x => OnDisplay(x)).AddTo(this.GetCancellationTokenOnDestroy());

		_activateSubscriber.Subscribe(ID, x => OnActivate(x)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}