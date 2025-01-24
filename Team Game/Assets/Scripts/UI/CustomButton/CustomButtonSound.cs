using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;

// ����{�^����SE��ύX�E�Ǘ�����N���X
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonSound : MonoBehaviour {
	// SE
	[SerializeField] private AudioClip MoveSE = null;	// �ړ�
	[SerializeField] private AudioClip PressSE = null;	// ����
	[SerializeField] private AudioClip CancelSE = null; // �L�����Z��

	// SE�Đ��p
	private AudioSource _audioSource = null;
	// ����{�^��
	private CustomButton _button = null;

	/// <summary>
	/// EventSystem�ɑI�����ꂽ�Ƃ��̏���
	/// </summary>
	private void OnMove() => _audioSource.PlayOneShot(MoveSE);

	/// <summary>
	/// �����ꂽ�Ƃ��̏���
	/// </summary>
	private void OnPress() => _audioSource.PlayOneShot(PressSE);

	/// <summary>
	/// �L�����Z�����ꂽ�Ƃ��̏���
	/// </summary>
	private void OnCancel() => _audioSource.PlayOneShot(CancelSE);

	private void Awake() {
		// �K�v�ȃR���|�[�l���g���L���b�V��
		_audioSource = GetComponent<AudioSource>();
		_button = GetComponent<CustomButton>();
	}

	private void Start() {
		// ���b�Z�[�W��M���̏�����ݒ�
		_button.OnMoveObservable.Subscribe(_ => OnMove())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_button.OnReceiveCallback += OnPress;

		_button.OnCancelObservable.Subscribe(_ => OnCancel())
			.AddTo(this.GetCancellationTokenOnDestroy());
	}

	private void OnDestroy() {
		_button.OnReceiveCallback -= OnPress;
	}
}