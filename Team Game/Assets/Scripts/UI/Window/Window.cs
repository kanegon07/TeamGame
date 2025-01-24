using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

// �E�B���h�E�N���X
public class Window : MonoBehaviour {
	public struct StateMessage : IEquatable<StateMessage> {
		public enum State {
			Hiding,
			Inactive,
			Active
		}

		public State Value;

		public StateMessage(State value) {
			Value = value;
		}

		public readonly bool Equals(StateMessage other) => Value == other.Value;
	}

	// �eID�̑Ή��ɂ��Ă͊e�C�x���g�N���X���Q�Ƃ̂���
	[SerializeField] private int EventID = -1;	// �L�����Z�����ɌĂԃC�x���g��ID
	[SerializeField] private int WindowID = -1;	// ���g��ID

	// �A�N�e�B�u��ԂɂȂ����Ƃ��A�ŏ��ɑI������{�^��
	[SerializeField] private CustomButton FirstSelected = null;

	// ���b�Z�[�W���M�̑���
	[Inject] private readonly IPublisher<int> _eventPublisher = null;

	[Inject] private readonly IPublisher<SelectionHook.UnhookMessage> _unhookPublisher = null;	// �I���̃t�b�N����

	// ���b�Z�[�W��M�̑���
	[Inject] private readonly ISubscriber<int, CustomButton.CancelMessage> _cancelSubscriber = null;  // �L�����Z��
	[Inject] private readonly ISubscriber<int, StateMessage> _stateSubscriber = null;

	// ���݂̏�Ԃ�ۑ�����ReactiveProperty
	// �[�I�ɂ����΁A�l���ύX���ꂽ�Ƃ��Ƀ��b�Z�[�W�𔭐M����ϐ�
	private readonly ReactiveProperty<bool> _isDisplayedRP = new(false);	// �\�����
	private readonly ReactiveProperty<bool> _isActiveRP = new(false);		// �A�N�e�B�u���

	// ReactiveProperty���̂��̂��擾����v���p�e�B
	// ���N���X����̍w�ǎ��ɗp����
	public ReadOnlyReactiveProperty<bool> IsDisplayedRP => _isDisplayedRP;	// �\�������ǂ���
	public ReadOnlyReactiveProperty<bool> IsActiveRP => _isActiveRP;		// �A�N�e�B�u���(=���͂��󂯕t����)���ǂ���

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
	/// �A�N�e�B�u��Ԃ��X�V���ꂽ�Ƃ��̏���
	/// </summary>
	/// <param name="value">�t���O</param>
	private void OnActivate(bool value) {
		if (value) {
			// �A�N�e�B�u��ԂɂȂ�Ƃ��A
			// ���炩���ߌ��߂��I�u�W�F�N�g��I��������
			if (FirstSelected == null) {
				EventSystem.current.SetSelectedGameObject(null);
			} else {
				EventSystem.current.SetSelectedGameObject(FirstSelected.gameObject);
			}
		} else {
			// ��A�N�e�B�u��ԂɂȂ�Ƃ��A
			// �I���̃t�b�N����������
			_unhookPublisher.Publish(new SelectionHook.UnhookMessage());
		}
	}

	/// <summary>
	/// ���g�ɕR�Â���ꂽ�{�^���ŃL�����Z�����͂��������Ƃ��̏���
	/// </summary>
	private void OnCancel() {
		// �L�����Z�����̃C�x���g���Ă�
		// �L�����Z�����̃C�x���g���E�B���h�E�P�ʂŌ��߂邱�Ƃ�
		// �C�x���g�ł̗]�v�ȃ��b�Z�[�W��M�����炷
		_eventPublisher.Publish(EventID);
	}

	/// <summary>
	/// �\����Ԃ��ύX���ꂽ�Ƃ��ɌĂ΂�鏈��
	/// </summary>
	/// <param name="state"></param>
	private void OnChangeState(StateMessage.State state) {
		switch (state) {
			case StateMessage.State.Hiding:		// �\���Ȃ��A�����t�Ȃ�
				IsDisplayed = false;
				IsActive = false;
				break;

			case StateMessage.State.Inactive:	// �\������A�����t�Ȃ�
				IsDisplayed = true;
				IsActive = false;
				break;

			case StateMessage.State.Active:		// �\������A�����t����
				IsDisplayed = true;
				IsActive = true;
				break;

			default:
				break;
		}
	}

	private void Awake() {
		// �K�v�ȃR���|�[�l���g���L���b�V��

		// ���b�Z�[�W��M���̏�����ݒ�
		_isActiveRP.Subscribe(x => OnActivate(x)).AddTo(this.GetCancellationTokenOnDestroy());

		_cancelSubscriber.Subscribe(WindowID, _ => OnCancel()).AddTo(this.GetCancellationTokenOnDestroy());

		_stateSubscriber.Subscribe(WindowID, x => OnChangeState(x.Value)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}