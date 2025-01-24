using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

// �Q�[���V�[���p�̃C�x���g�Ǘ��N���X
[RequireComponent(typeof(AudioSource))]
public class GameEvents : MonoBehaviour {
	// �o�͐�Ɋ���U��ԍ�
	public enum WindowID {
		Main,	// ���C���E�B���h�E
		Option	// �I�v�V�����E�B���h�E
	}

	// ���͌��Ɋ���U��ԍ�
	public enum EventID {
		OpenOption,
		CloseOption,
		ReturnToTitle,
		Miss,
		Clear
	}

	public enum TransitType {
		Abort,
		Miss,
		Clear
	}

	[SerializeField] private int StageID = 0;

	// ���̃I�u�W�F�N�g�̎Q��
	[SerializeField] private CameraPlayer Player = null;	// �v���C���[
	[SerializeField] private FPSCamera Camera = null;		// �J����
	[SerializeField] private DebugMouse MouseLock = null;	// �}�E�X���b�N

	// SE�f�[�^
	[SerializeField] private AudioClip OptionSE = null;
	[SerializeField] private AudioClip ClearSE = null;		// �S�[��
	[SerializeField] private AudioClip MissSE = null;		// �Q�[���I�[�o�[

	// ���b�Z�[�W���M�̑���(����)
	[Inject] private readonly IPublisher<int, Window.StateMessage> _statePublisher = null;

	[Inject] private readonly IPublisher<StageInfo> _stageInfoPublisher = null;

	// ���b�Z�[�W���M�̑���(�񓯊�)
	[Inject] private readonly IAsyncPublisher<WipeEffectController.WipeMessage> _wipeAsyncPublisher = null; // ���C�v����

	// ���b�Z�[�W��M�̑���(����)
	[Inject] private readonly ISubscriber<int> _eventSubscriber = null;

	// BGM�Đ��p
	private AudioSource _audioSource = null;

	private StageInfo _stageInfo = null;

	/// <summary>
	/// �I�v�V�������J��
	/// </summary>
	private void OpenOption() {
		_audioSource.PlayOneShot(OptionSE);

		// �Q�[�������Ԃ��~�߂�
		Time.timeScale = 0F;

		// �v���C���[�ƃJ������������󂯕t���Ȃ��悤�ɂ���
		Player.enabled = false;
		Camera.enabled = false;

		// �I�v�V�����E�B���h�E�̂ݑ�����󂯕t����悤�ɂ���
		_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Inactive));
		_statePublisher.Publish((int)WindowID.Option, new Window.StateMessage(Window.StateMessage.State.Active));

		// �J�[�\���̈ʒu�Œ���������A������悤�ɂ���
		MouseLock.Lock(false);
	}

	/// <summary>
	/// �I�v�V���������
	/// </summary>
	private void CloseOption() {
		// �I�v�V�����E�B���h�E����A
		// ���C���E�B���h�E��������󂯕t����悤�ɂ���
		_statePublisher.Publish((int)WindowID.Option, new Window.StateMessage(Window.StateMessage.State.Hiding));
		_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Active));

		// �v���C���[�ƃJ������������󂯕t����悤�ɂ���
		Player.enabled = true;
		Camera.enabled = true;

		// �Q�[�������Ԃ����ʂ�i�߂�
		Time.timeScale = 1F;

		// �J�[�\���̈ʒu���Œ肵�A��\���ɂ���
		MouseLock.Lock(true);
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
	/// <param name="type">�J�ڃ^�C�v</param>
	/// <param name="nextScene">���̃V�[���̖��O</param>
	private async void TransitScene(TransitType type, string nextScene) {
		// BGM���~�߂�
		_audioSource.Stop();

		// �J�ڃ^�C�v�ɂ���čׂ���������ς���
		switch (type) {
			case TransitType.Abort:	// �I�v�V�������璆�f����Ƃ�
				// �I�v�V�����E�B���h�E��������󂯕t���Ȃ��悤�ɂ���
				_statePublisher.Publish((int)WindowID.Option, new Window.StateMessage(Window.StateMessage.State.Inactive));
				break;

			case TransitType.Miss:
				// �~�X����SE���Đ�����
				_audioSource.PlayOneShot(MissSE);
				// ���C���E�B���h�E��������󂯕t���Ȃ��悤�ɂ���
				_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Inactive));
				break;

			case TransitType.Clear:
				// �N���A����SE���Đ�����
				_audioSource.PlayOneShot(ClearSE);
				// ���C���E�B���h�E��������󂯕t���Ȃ��悤�ɂ���
				_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Inactive));
				break;

			default:
				break;
		}

		// �J�[�\���̈ʒu���Œ肵�A�����Ȃ��悤�ɂ���
		MouseLock.Lock(true);

		// ������󂯕t���Ȃ��悤�ɂ���
		Player.enabled = false;
		Camera.enabled = false;

		// ���C�v�A�E�g�̊�����҂�
		await Wipe(true);

		// ���̃V�[����ǂݍ���
		SceneManager.LoadScene(nextScene);
	}

	private void Awake() {
		// �K�v�ȃR���|�[�l���g���L���b�V��
		_audioSource = GetComponent<AudioSource>();

		_stageInfo = Resources.Load<StageInfoTable>("StageInfoTable").Find(StageID);

		// ���b�Z�[�W��M���̏�����ݒ�
		_eventSubscriber.Subscribe(x => {
			switch (x) {
				case (int)EventID.OpenOption:
					OpenOption();
					break;

				case (int)EventID.CloseOption:
					CloseOption();
					break;

				case (int)EventID.ReturnToTitle:
					TransitScene(TransitType.Abort, "Title");
					break;

				case (int)EventID.Miss:
					TransitScene(TransitType.Miss, SceneManager.GetActiveScene().name);
					break;

				case (int)EventID.Clear:
					TransitScene(TransitType.Clear, "Result");
					break;
			}
		}).AddTo(this.GetCancellationTokenOnDestroy());
	}

	private async void Start() {
		_stageInfoPublisher.Publish(_stageInfo);

		// �J�[�\���̈ʒu���Œ肵�A�����Ȃ��悤�ɂ���
		MouseLock.Lock(true);

		// ������󂯕t���Ȃ��悤�ɂ���
		Player.enabled = false;
		Camera.enabled = false;

		// ���C�v�C���̊�����҂�
		await Wipe(false);

		// ������󂯕t����悤�ɂ���
		Player.enabled = true;
		Camera.enabled = true;

		_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Active));

		// ���݂̃V�[�����폜���ꂽ�Ƃ��A
		// ���I�u�W�F�N�g�̎Q�Ƃ���������
		SceneManager.sceneUnloaded += (_) => {
			Player = null;
			Camera = null;
			MouseLock = null;
		};

		// BGM�̍Đ����n�߂�
		_audioSource.Play();
	}
}