using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

// �^�C�g���V�[���p�̃C�x���g�Ǘ��N���X
[RequireComponent(typeof(AudioSource))]
public class TitleEvents : MonoBehaviour {
	// �o�͐�Ɋ���U��ԍ�
	public enum WindowID {
		Main,			// ���C���E�B���h�E
		StageSelector	// �X�e�[�W�I���E�B���h�E
	}

	// ���͌��Ɋ���U��ԍ�
	public enum EventID {
		OpenStageSelector,
		CloseStageSelector,
		StartStage1,
		StartStage2,
		StartStage3
	}

	// ���b�Z�[�W���M�̑���(����)
	[Inject] private readonly IPublisher<int, Window.StateMessage> _statePublisher = null;

	// ���b�Z�[�W���M�̑���(�񓯊�)
	[Inject] private readonly IAsyncPublisher<WipeEffectController.WipeMessage> _wipeAsyncPublisher = null; // ���C�v����

	// ���b�Z�[�W��M�̑���(����)
	[Inject] private readonly ISubscriber<int> _eventSubscriber = null;

	// BGM�Đ��p
	private AudioSource _audioSource = null;

	/// <summary>
	/// �X�e�[�W�I���E�B���h�E���J��
	/// </summary>
	private void OpenStageSelector() {
		// �X�e�[�W�I���E�B���h�E�̂ݑ�����󂯕t����悤�ɂ���
		_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Inactive));
		_statePublisher.Publish((int)WindowID.StageSelector, new Window.StateMessage(Window.StateMessage.State.Active));
	}

	/// <summary>
	/// �X�e�[�W�I����ʂ����
	/// </summary>
	private void CloseStageSelector() {
		// �X�e�[�W�I���E�B���h�E����A
		// ���C���E�B���h�E��������󂯕t����悤�ɂ���
		_statePublisher.Publish((int)WindowID.StageSelector, new Window.StateMessage(Window.StateMessage.State.Hiding));
		_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Active));
	}

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
		_statePublisher.Publish((byte)WindowID.StageSelector, new Window.StateMessage(Window.StateMessage.State.Inactive));

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
				case (int)EventID.OpenStageSelector:
					OpenStageSelector();
					break;

				case (int)EventID.CloseStageSelector:
					CloseStageSelector();
					break;

				case (int)EventID.StartStage1:
					TransitScene("FujiiScene");
					break;

				case (int)EventID.StartStage2:
					TransitScene("Stage2");
					break;

				case (int)EventID.StartStage3:
					TransitScene("Stage3");
					break;

				default:
					break;
			}
		}).AddTo(this.GetCancellationTokenOnDestroy());
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