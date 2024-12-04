using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

/// <summary>
/// R3��MessagePipe��p�����A����̃{�^���N���X
/// </summary>
[RequireComponent(typeof(ObservableEventTrigger))]
public class CustomButton : MonoBehaviour {
	/// <summary>
	/// �{�^���������ꂽ�Ƃ��ɔ��M���郁�b�Z�[�W
	/// </summary>
	public struct PressMessage { }

	/// <summary>
	/// �L�����Z�����ꂽ�Ƃ��ɔ��M���郁�b�Z�[�W
	/// </summary>
	public struct CancelMessage { }

	// ���ʔԍ�
	// �C�x���g�Ƃ̑Ή��͊e�V�[���ł̃C�x���g�Ǘ��N���X���Q�Ƃ̂���
	[SerializeField] private byte ID = 0;

	// OnMove�ɂ���ăt�H�[�J�X��ύX����{�^��
	[SerializeField] private CustomButton SelectOnLeft = null;	// ��
	[SerializeField] private CustomButton SelectOnUp = null;	// ��
	[SerializeField] private CustomButton SelectOnRight = null;	// �E
	[SerializeField] private CustomButton SelectOnDown = null;	// ��

	// ���b�Z�[�W���M�̑���
	// ID���L�[�Ƃ��邱�ƂŁA�u�ǂ̃{�^������̃��b�Z�[�W���v����ʂ���
	[Inject] private IPublisher<byte, PressMessage> _pressPublisher = null;		// �{�^���������b�Z�[�W
	[Inject] private IPublisher<byte, CancelMessage> _cancelPublisher = null;	// �L�����Z�����b�Z�[�W

	// �E�B���h�E�N���X����̃��b�Z�[�W��M�̑���
	[Inject] private ISubscriber<int, Window.DisplayMessage> _displaySubscrber = null;		// �\��or��\���̃��b�Z�[�W
	[Inject] private ISubscriber<int, Window.ActivateMessage> _activateSubscrber = null;	// �A�N�e�B�u��ԕύX�̃��b�Z�[�W

	// ���݂̏�Ԃ�ۑ�����ReactiveProperty
	// �[�I�ɂ����΁A�l���ύX���ꂽ�Ƃ��Ƀ��b�Z�[�W�𔭐M����ϐ�
	private readonly ReactiveProperty<bool> _isDisplayedRP = new(false);	// �\�������ǂ���
	private readonly ReactiveProperty<bool> _isActiveRP = new(false);		// �A�N�e�B�u���(=���͂��󂯕t����)���ǂ���

	// EventSystem����̃��b�Z�[�W���󂯎�����
	private ObservableEventTrigger _eventTrigger = null;

	// ReactiveProperty���̂��̂��擾����v���p�e�B
	// ���N���X����̍w�ǎ��ɗp����
	public ReadOnlyReactiveProperty<bool> IsDisplayedRP => _isDisplayedRP;	// �\�������ǂ���
	public ReadOnlyReactiveProperty<bool> IsActiveRP => _isActiveRP;		// �A�N�e�B�u��Ԃ��ǂ���

	// ReactiveProperty�̒l���擾�E�X�V����v���p�e�B
	// �\�������ǂ���
	public bool IsDisplayed {
		get => _isDisplayedRP.Value;
		set => _isDisplayedRP.Value = value;
	}

	// �A�N�e�B�u��Ԃ��ǂ���
	public bool IsActive {
		get => _isActiveRP.Value;
		set => _isActiveRP.Value = value;
	}

	// EventSystem�ɑI�����ꂽ�Ƃ��̃��b�Z�[�W
	public Observable<Unit> OnSelectObservable => _eventTrigger.OnSelectAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// EventSystem����̑I�����������ꂽ�Ƃ��̃��b�Z�[�W
	public Observable<Unit> OnDeselectObservable => _eventTrigger.OnDeselectAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// �I�𒆂�Submit�{�^�������͂��ꂽ�Ƃ��̃��b�Z�[�W
	public Observable<Unit> OnSubmitObservable => _eventTrigger.OnSubmitAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// �I�𒆂�Cancel�{�^�������͂��ꂽ�Ƃ��̃��b�Z�[�W
	public Observable<Unit> OnCancelObservable => _eventTrigger.OnCancelAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// �I�𒆂�Move�{�^�������͂��ꂽ�Ƃ��̃��b�Z�[�W
	public Observable<AxisEventData> OnMoveObservable => _eventTrigger.OnMoveAsObservable()
		.Where(_ => IsActive);

	// �}�E�X�|�C���^�[���d�Ȃ����Ƃ��̃��b�Z�[�W
	public Observable<Unit> OnPointerEnterObservable => _eventTrigger.OnPointerEnterAsObservable()
		.AsUnitObservable().Where(_ => IsActive);
	
	// �}�E�X�|�C���^�\�����ꂽ�Ƃ��̃��b�Z�[�W
	public Observable<Unit> OnPointerExitObservable => _eventTrigger.OnPointerExitAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// �����̏�Ń}�E�X�̃{�^���������ꂽ�Ƃ��̃��b�Z�[�W
	public Observable<Unit> OnOnPointerDownObservable => _eventTrigger.OnPointerDownAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// �����̏�Ń}�E�X�̃{�^����������A�����ꂽ�Ƃ��̃��b�Z�[�W
	public Observable<Unit> OnPointerClickObservable => _eventTrigger.OnPointerClickAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// �����̏�Ń}�E�X�̃{�^����������A�Ⴄ�Ƃ���ŗ����ꂽ�Ƃ��̃��b�Z�[�W
	public Observable<Unit> OnPointerUpObservable => _eventTrigger.OnPointerUpAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	/// <summary>
	/// OnSelect���b�Z�[�W��M���̏���
	/// �I���t�b�N�N���X�Ɏ��g��o�^����
	/// </summary>
	private void OnSelect() {
		if (EventSystem.current.gameObject.TryGetComponent(out SelectionHook hook)) {
			hook.PrevSelected = gameObject;
		}
	}

	/// <summary>
	/// OnPress���b�Z�[�W��M���̏���
	/// PressMessage�𔭐M����
	/// </summary>
	private void OnPress() => _pressPublisher.Publish(ID, new PressMessage());

	/// <summary>
	/// OnCancel���b�Z�[�W��M���̏���
	/// CancelMessage�𔭐M����
	/// </summary>
	private void OnCancel() => _cancelPublisher.Publish(ID, new CancelMessage());

	/// <summary>
	/// OnMove���b�Z�[�W��M���̏���
	/// </summary>
	/// <param name="direction">���͂��ꂽ����</param>
	private void OnMove(MoveDirection direction) {
		switch (direction) {
			case MoveDirection.Left:	// ��
				if (SelectOnLeft != null && SelectOnLeft.IsActive) {
					// EventSystem��SelectOnLeft�̃I�u�W�F�N�g��I��������
					EventSystem.current.SetSelectedGameObject(SelectOnLeft.gameObject);
				}
				break;

			case MoveDirection.Up:		// ��
				if (SelectOnUp != null && SelectOnUp.IsActive) {
					// EventSystem��SelectOnUp�̃I�u�W�F�N�g��I��������
					EventSystem.current.SetSelectedGameObject(SelectOnUp.gameObject);
				}
				break;

			case MoveDirection.Right:	// �E
				if (SelectOnRight != null && SelectOnRight.IsActive) {
					// EventSystem��SelectOnRight�̃I�u�W�F�N�g��I��������
					EventSystem.current.SetSelectedGameObject(SelectOnRight.gameObject);
				}
				break;

			case MoveDirection.Down:	// ��
				if (SelectOnDown != null && SelectOnDown.IsActive) {
					// EventSystem��SelectOnDown�̃I�u�W�F�N�g��I��������
					EventSystem.current.SetSelectedGameObject(SelectOnDown.gameObject);
				}
				break;

			default:
				break;
		}
	}

	/// <summary>
	/// OnPointerEnter���b�Z�[�W��M���̏���
	/// </summary>
	private void OnPointerEnter() {
		// EventSystem�Ɏ��g�̃I�u�W�F�N�g��I��������
		EventSystem.current.SetSelectedGameObject(gameObject);
	}

	/// <summary>
	/// DisplayMessage��M���̏���
	/// ���g�̕\����Ԃ��ύX����
	/// </summary>
	/// <param name="value">�󂯎�����l</param>
	private void OnDisplay(bool value) => IsDisplayed = value;

	/// <summary>
	/// ActivateMessage��M���̏���
	/// ���g�̃A�N�e�B�u��Ԃ��ύX����
	/// </summary>
	/// <param name="value">�󂯎�����l</param>
	private void OnActivate(bool value) => IsActive = value;

	private void Awake() {
		_eventTrigger = GetComponent<ObservableEventTrigger>();

		OnSelectObservable.Subscribe(_ => OnSelect()).AddTo(this.GetCancellationTokenOnDestroy());

		OnSubmitObservable.Subscribe(_ => OnPress()).AddTo(this.GetCancellationTokenOnDestroy());

		OnCancelObservable.Subscribe(_ => OnCancel()).AddTo(this.GetCancellationTokenOnDestroy());

		OnMoveObservable.Subscribe(x => OnMove(x.moveDir)).AddTo(this.GetCancellationTokenOnDestroy());

		OnPointerEnterObservable.Subscribe(_ => OnPointerEnter()).AddTo(this.GetCancellationTokenOnDestroy());

		OnPointerClickObservable.Subscribe(_ => OnPress()).AddTo(this.GetCancellationTokenOnDestroy());

		_displaySubscrber.Subscribe(transform.parent.GetInstanceID(), x => OnDisplay(x.Value)).AddTo(this.GetCancellationTokenOnDestroy());

		_activateSubscrber.Subscribe(transform.parent.GetInstanceID(), x => OnActivate(x.Value)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}