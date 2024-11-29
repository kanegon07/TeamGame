using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class CustomButton : MonoBehaviour {
	public struct PressMessage { }
	public struct CancelMessage { }

	[SerializeField] private byte ID = 0;

	[SerializeField] private CustomButton SelectOnLeft = null;
	[SerializeField] private CustomButton SelectOnUp = null;
	[SerializeField] private CustomButton SelectOnRight = null;
	[SerializeField] private CustomButton SelectOnDown = null;

	[Inject] private IPublisher<byte, PressMessage> _pressPublisher = null;
	[Inject] private IPublisher<byte, CancelMessage> _cancelPublisher = null;

	[Inject] private ISubscriber<int, Window.DisplayMessage> _displaySubscrber = null;
	[Inject] private ISubscriber<int, Window.ActivateMessage> _activateSubscrber = null;

	private readonly ReactiveProperty<bool> _isDisplayedRP = new(false);
	private readonly ReactiveProperty<bool> _isActiveRP = new(false);

	private ObservableEventTrigger _eventTrigger = null;

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

	public Observable<Unit> OnSelectObservable => _eventTrigger.OnSelectAsObservable()
		.AsUnitObservable().Where(_ => IsActive);
	public Observable<Unit> OnDeselectObservable => _eventTrigger.OnDeselectAsObservable()
		.AsUnitObservable().Where(_ => IsActive);
	public Observable<Unit> OnSubmitObservable => _eventTrigger.OnSubmitAsObservable()
		.AsUnitObservable().Where(_ => IsActive);
	public Observable<Unit> OnCancelObservable => _eventTrigger.OnCancelAsObservable()
		.AsUnitObservable().Where(_ => IsActive);
	public Observable<AxisEventData> OnMoveObservable => _eventTrigger.OnMoveAsObservable()
		.Where(_ => IsActive);
	public Observable<Unit> OnPointerEnterObservable => _eventTrigger.OnPointerEnterAsObservable()
		.AsUnitObservable().Where(_ => IsActive);
	public Observable<Unit> OnPointerExitObservable => _eventTrigger.OnPointerExitAsObservable()
		.AsUnitObservable().Where(_ => IsActive);
	public Observable<Unit> OnOnPointerDownObservable => _eventTrigger.OnPointerDownAsObservable()
		.AsUnitObservable().Where(_ => IsActive);
	public Observable<Unit> OnPointerClickObservable => _eventTrigger.OnPointerClickAsObservable()
		.AsUnitObservable().Where(_ => IsActive);
	public Observable<Unit> OnPointerUpObservable => _eventTrigger.OnPointerUpAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	private void OnSelect() {
		if (EventSystem.current.gameObject.TryGetComponent(out SelectionHook hook)) {
			hook.PrevSelected = gameObject;
		}
	}

	private void OnPress() => _pressPublisher.Publish(ID, new PressMessage());

	private void OnCancel() => _cancelPublisher.Publish(ID, new CancelMessage());

	private void OnMove(MoveDirection direction) {
		switch (direction) {
			case MoveDirection.Left:
				if (SelectOnLeft != null && SelectOnLeft.IsActive) {
					EventSystem.current.SetSelectedGameObject(SelectOnLeft.gameObject);
				}
				break;

			case MoveDirection.Up:
				if (SelectOnUp != null && SelectOnUp.IsActive) {
					EventSystem.current.SetSelectedGameObject(SelectOnUp.gameObject);
				}
				break;

			case MoveDirection.Right:
				if (SelectOnRight != null && SelectOnRight.IsActive) {
					EventSystem.current.SetSelectedGameObject(SelectOnRight.gameObject);
				}
				break;

			case MoveDirection.Down:
				if (SelectOnDown != null && SelectOnDown.IsActive) {
					EventSystem.current.SetSelectedGameObject(SelectOnDown.gameObject);
				}
				break;

			default:
				break;
		}
	}

	private void OnPointerEnter() {
		EventSystem.current.SetSelectedGameObject(gameObject);
	}

	private void OnDisplay(bool value) => IsDisplayed = value;

	private void OnActivate(bool value) => IsActive = value;

	private void Awake() {
		_eventTrigger = GetComponent<ObservableEventTrigger>();

		OnSelectObservable.Subscribe(_ => OnSelect()).AddTo(this.GetCancellationTokenOnDestroy());

		OnSubmitObservable.Subscribe(_ => OnPress()).AddTo(this.GetCancellationTokenOnDestroy());

		OnCancelObservable.Subscribe(_ => OnCancel()).AddTo(this.GetCancellationTokenOnDestroy());

		OnMoveObservable.Subscribe(x => OnMove(x.moveDir)).AddTo(this.GetCancellationTokenOnDestroy());

		OnPointerEnterObservable.Subscribe(_ => OnPointerEnter()).AddTo(this.GetCancellationTokenOnDestroy());

		OnPointerClickObservable.Subscribe(_ => OnPress()).AddTo(this.GetCancellationTokenOnDestroy());

		_displaySubscrber.Subscribe(transform.parent.GetInstanceID(), x => OnDisplay(x.Value));

		_activateSubscrber.Subscribe(transform.parent.GetInstanceID(), x => OnActivate(x.Value));
	}
}