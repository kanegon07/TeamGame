using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

// ���U���g�V�[���p�̃C�x���g�Ǘ��N���X
[RequireComponent(typeof(AudioSource))]
public class ResultEvents : MonoBehaviour {
	// �o�͐�Ɋ���U��ԍ�
	public enum WindowID {
		Main	// ���C�����
	}

	// ���͌��Ɋ���U��ԍ�
	public enum EventID {
		ReturnToTitle
	}

	// ���b�Z�[�W���M�̑���(����)
	[Inject] private readonly IPublisher<int, Window.StateMessage> _statePublisher = null;

	// ���b�Z�[�W���M�̑���(�񓯊�)
	[Inject] private readonly IAsyncPublisher<WipeEffectController.WipeMessage> _wipeAsyncPublisher = null;  // ���C�v����

	// ���b�Z�[�W��M�̑���(����)
	[Inject] private readonly ISubscriber<int> _eventSubscriber = null;

	// BGM�Đ��p
	private AudioSource _audioSource = null;

	/// <summary>
	/// ���C�v�������s��
	/// </summary>
	/// <param name="wipesOut">���C�v�A�E�g(������)�������ǂ���</param>
	/// <returns>�񓯊������p�̃^�X�N</returns>
	private async UniTask Wipe(bool wipesOut)
		=> await _wipeAsyncPublisher.PublishAsync(
			new WipeEffectController.WipeMessage(wipesOut),
			this.GetCancellationTokenOnDestroy()
		);

	/// <summary>
	/// �V�[����؂�ւ���
	/// </summary>
	/// <param name="nextScene">���̃V�[���̖��O</param>
	private async void TransitScene(string nextScene) {
		// BGM���~�߂�
		_audioSource.Stop();

		// �J�[�\���̈ʒu���Œ肵�A�����Ȃ��悤�ɂ���
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		// ������󂯕t���Ȃ��悤�ɂ���
		_statePublisher.Publish((byte)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Inactive));

		// ���C�v�A�E�g�̊�����҂�
		await Wipe(true);

		// ���̃V�[����ǂݍ���
		SceneManager.LoadScene(nextScene);
	}

	private void Awake() {
		// �K�v�ȃR���|�[�l���g���L���b�V��
		_audioSource = GetComponent<AudioSource>();

		// ���b�Z�[�W��M���̏�����ݒ�
		_eventSubscriber.Subscribe(x => {
			switch (x) {
				case (int)EventID.ReturnToTitle:
					TransitScene("Title");
					break;

				default:
					break;
			}
		}
		).AddTo(this.GetCancellationTokenOnDestroy());
	}

	private async void Start() {
		// �J�[�\���̈ʒu���Œ肵�A�����Ȃ��悤�ɂ���
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		// ���C�v�C���̊�����҂�
		await Wipe(false);

		_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Active));

		// �J�[�\���̈ʒu�Œ���������A������悤�ɂ���
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		// BGM�̍Đ����n�߂�
		_audioSource.Play();
	}
}