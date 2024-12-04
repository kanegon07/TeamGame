using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// �E�B���h�E�N���X
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class Window : MonoBehaviour {
	/// <summary>
	/// �\����Ԃ��ύX���ꂽ�Ƃ��ɔ��M���郁�b�Z�[�W
	/// </summary>
	public struct DisplayMessage : IEquatable<DisplayMessage> {
		// �l
		public bool Value;

		public DisplayMessage(bool value) {
			Value = value;
		}

		public readonly bool Equals(DisplayMessage other) => Value == other.Value;
	}

	/// <summary>
	/// �A�N�e�B�u���(=���͂��󂯕t���邩)���ύX���ꂽ�Ƃ��ɔ��M���郁�b�Z�[�W
	/// </summary>
	public struct ActivateMessage : IEquatable<ActivateMessage> {
		// �l
		public bool Value;

		public ActivateMessage(bool value) {
			Value = value;
		}

		public readonly bool Equals(ActivateMessage other) => Value == other.Value;
	}

	// ���ʔԍ�
	// �C�x���g�Ƃ̑Ή��͊e�V�[���ł̃C�x���g�Ǘ��N���X���Q�Ƃ̂���
	[SerializeField] private byte ID = 0;
	// �A�N�e�B�u��ԂɂȂ����Ƃ��A�ŏ��ɑI������{�^��
	[SerializeField] private CustomButton FirstSelected = null;

	// ���b�Z�[�W���M�̑���
	// �C���X�^���XID���L�[�Ƃ��邱�ƂŁA�u�ǂ̃E�B���h�E����̃��b�Z�[�W���v����ʂ���
	[Inject] private IPublisher<int, DisplayMessage> _displayPublisher = null;		// �\��or��\���̃��b�Z�[�W
	[Inject] private IPublisher<int, ActivateMessage> _activatePublisher = null;	// �A�N�e�B�u��ԕύX�̃��b�Z�[�W

	// ���b�Z�[�W��M�̑���
	[Inject] private ISubscriber<byte, DisplayMessage> _displaySubscriber = null;	// �\��or��\���̃��b�Z�[�W
	[Inject] private ISubscriber<byte, ActivateMessage> _activateSubscriber = null; // �A�N�e�B�u��ԕύX�̃��b�Z�[�W

	// ���݂̏�Ԃ�ۑ�����ReactiveProperty
	// �[�I�ɂ����΁A�l���ύX���ꂽ�Ƃ��Ƀ��b�Z�[�W�𔭐M����ϐ�
	private readonly ReactiveProperty<bool> _isDisplayedRP = new(false);	// �\�������ǂ���
	private readonly ReactiveProperty<bool> _isActiveRP = new(false);		// �A�N�e�B�u���(=���͂��󂯕t����)���ǂ���

	// �摜
	private Image _image = null;
	// �L�����o�X�O���[�v
	private CanvasGroup _canvasGroup = null;

	// ReactiveProperty���̂��̂��擾����v���p�e�B
	// ���N���X����̍w�ǎ��ɗp����
	public ReadOnlyReactiveProperty<bool> IsDisplayedRP => _isDisplayedRP;	// �\�������ǂ���
	public ReadOnlyReactiveProperty<bool> IsActiveRP => _isActiveRP;        // �A�N�e�B�u���(=���͂��󂯕t����)���ǂ���

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

	/// <summary>
	/// �\����Ԃ̕ύX���̏���
	/// ��M�������b�Z�[�W�𒆌p(?)����
	/// </summary>
	/// <param name="msg">��M�������b�Z�[�W</param>
	private void OnDisplay(DisplayMessage msg) {
		IsDisplayed = msg.Value;

		if (_image != null) {
			_image.enabled = msg.Value;
		}

		_displayPublisher.Publish(transform.GetInstanceID(), msg);
	}

	/// <summary>
	/// �A�N�e�B�u��Ԃ̕ύX���̏���
	/// ��M�������b�Z�[�W�𒆌p(?)����
	/// </summary>
	/// <param name="msg">��M�������b�Z�[�W/param>
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