using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using R3.Triggers;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

// R3��MessagePipe��p�����A����̃{�^���N���X
[RequireComponent(typeof(ObservableEventTrigger))]
public class CustomButton : MonoBehaviour {
	// �L�����Z�����b�Z�[�W
	public struct CancelMessage { }

	// �eID�̑Ή��ɂ��Ă͊e�C�x���g�N���X���Q�Ƃ̂���
	[SerializeField] private int EventID = -1;	// �������ɌĂԃC�x���g��ID
	[SerializeField] private int WindowID = -1;	// ���g��R�Â���E�B���h�E��ID

	// OnMove�ɂ���ăt�H�[�J�X��ύX����{�^��
	[SerializeField] private CustomButton SelectOnLeft = null;	// ��
	[SerializeField] private CustomButton SelectOnUp = null;	// ��
	[SerializeField] private CustomButton SelectOnRight = null;	// �E
	[SerializeField] private CustomButton SelectOnDown = null;  // ��

	// ���b�Z�[�W���M�̑���
	[Inject] private readonly IPublisher<int> _eventPublisher = null;					// �C�x���g

	[Inject] private readonly IPublisher<int, CancelMessage> _cancelPublisher = null;	// �L�����Z��

	// ���b�Z�[�W��M�̑���
	[Inject] private readonly ISubscriber<int, Window.StateMessage> _stateSubscriber = null;

	// ���݂̏�Ԃ�ۑ�����ReactiveProperty
	// �[�I�ɂ����΁A�l���ύX���ꂽ�Ƃ��Ƀ��b�Z�[�W�𔭐M����ϐ�
	private readonly ReactiveProperty<bool> _isDisplayedRP = new(false);	// �\�����
	private readonly ReactiveProperty<bool> _isActiveRP = new(false);		// �A�N�e�B�u���(=���͎�t�̐���)

	// EventSystem����̃��b�Z�[�W���󂯎�����
	private ObservableEventTrigger _eventTrigger = null;

	public Action OnReceiveCallback = null;

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
		set => _isActiveRP.Value = _isDisplayedRP.Value && value;
	}

	// EventSystem����̃��b�Z�[�W
	public Observable<Unit> OnSelectObservable => _eventTrigger.OnSelectAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// �I��

	public Observable<Unit> OnDeselectObservable => _eventTrigger.OnDeselectAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// �I������

	public Observable<Unit> OnSubmitObservable => _eventTrigger.OnSubmitAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// ����

	public Observable<Unit> OnCancelObservable => _eventTrigger.OnCancelAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// �L�����Z��

	public Observable<AxisEventData> OnMoveObservable => _eventTrigger.OnMoveAsObservable()
		.Where(_ => IsActive);						// �ړ�

	public Observable<Unit> OnPointerEnterObservable => _eventTrigger.OnPointerEnterAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// �}�E�X�I�[�o�[
	
	public Observable<Unit> OnPointerExitObservable => _eventTrigger.OnPointerExitAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// �}�E�X�I�[�o�[����

	public Observable<Unit> OnOnPointerDownObservable => _eventTrigger.OnPointerDownAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// �{�^������

	public Observable<Unit> OnPointerClickObservable => _eventTrigger.OnPointerClickAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// �{�^������->�}�E�X�I�[�o�[��Ԃŉ���

	// �����̏�Ń}�E�X�̃{�^����������A�Ⴄ�Ƃ���ŗ����ꂽ�Ƃ��̃��b�Z�[�W
	public Observable<Unit> OnPointerUpObservable => _eventTrigger.OnPointerUpAsObservable()
		.AsUnitObservable().Where(_ => IsActive);   // �{�^������->�}�E�X�I�[�o�[�łȂ���Ԃŉ���

	/// <summary>
	/// OnPress���b�Z�[�W��M���̏���
	/// PressMessage�𔭐M����
	/// </summary>
	protected void OnPress() {
		OnReceiveCallback?.Invoke();
		_eventPublisher.Publish(EventID);
	}

	/// <summary>
	/// OnCancel���b�Z�[�W��M���̏���
	/// CancelMessage�𔭐M����
	/// </summary>
	private void OnCancel() => _cancelPublisher.Publish(WindowID, new CancelMessage());

	/// <summary>
	/// OnMove���b�Z�[�W��M���̏���
	/// </summary>
	/// <param name="direction">���͂��ꂽ����</param>
	private void OnMove(MoveDirection direction) {
		switch (direction) {
			case MoveDirection.Left:	// ��
				if (SelectOnLeft != null && SelectOnLeft.IsActive) {
					// ���̃I�u�W�F�N�g��I��������
					EventSystem.current.SetSelectedGameObject(SelectOnLeft.gameObject);
				}
				break;

			case MoveDirection.Up:		// ��
				if (SelectOnUp != null && SelectOnUp.IsActive) {
					// ��̃I�u�W�F�N�g��I��������
					EventSystem.current.SetSelectedGameObject(SelectOnUp.gameObject);
				}
				break;

			case MoveDirection.Right:	// �E
				if (SelectOnRight != null && SelectOnRight.IsActive) {
					// �E�̃I�u�W�F�N�g��I��������
					EventSystem.current.SetSelectedGameObject(SelectOnRight.gameObject);
				}
				break;

			case MoveDirection.Down:	// ��
				if (SelectOnDown != null && SelectOnDown.IsActive) {
					// ���̃I�u�W�F�N�g��I��������
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
		// ���g�̃I�u�W�F�N�g��I��������
		EventSystem.current.SetSelectedGameObject(gameObject);
	}

	private void OnChangeState(Window.StateMessage.State state) {
		switch (state) {
			case Window.StateMessage.State.Hiding:
				IsDisplayed = false;
				IsActive = false;
				break;

			case Window.StateMessage.State.Inactive:
				IsDisplayed = true;
				IsActive = false;
				break;

			case Window.StateMessage.State.Active:
				IsDisplayed = true;
				IsActive = true;
				break;

			default:
				break;
		}
	}

	virtual protected void Awake() {
		// �K�v�ȃR���|�[�l���g���L���b�V��
		_eventTrigger = GetComponent<ObservableEventTrigger>();

		// ���b�Z�[�W��M���̏�����ݒ�
		OnSubmitObservable.Subscribe(_ => OnPress()).AddTo(this.GetCancellationTokenOnDestroy());

		OnCancelObservable.Subscribe(_ => OnCancel()).AddTo(this.GetCancellationTokenOnDestroy());

		OnMoveObservable.Subscribe(x => OnMove(x.moveDir)).AddTo(this.GetCancellationTokenOnDestroy());

		OnPointerEnterObservable.Subscribe(_ => OnPointerEnter()).AddTo(this.GetCancellationTokenOnDestroy());

		OnPointerClickObservable.Subscribe(_ => OnPress()).AddTo(this.GetCancellationTokenOnDestroy());

		_stateSubscriber.Subscribe(WindowID, x => OnChangeState(x.Value)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}