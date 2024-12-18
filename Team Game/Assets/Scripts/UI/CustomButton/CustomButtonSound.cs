using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;
using VContainer;

/// <summary>
/// ����{�^����SE��ύX�E�Ǘ�����N���X
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonSound : MonoBehaviour {
	// SE
	[SerializeField] private AudioClip MoveSE = null;	// �ړ���
	[SerializeField] private AudioClip PressSE = null;	// �����ꂽ�Ƃ�
	[SerializeField] private AudioClip CancelSE = null; // �L�����Z�����ꂽ�Ƃ�

	// ���b�Z�[�W��M�̑���
	[Inject] private ISubscriber<byte, CustomButton.PressMessage> _pressSubscriber = null;     // �{�^���������b�Z�[�W
	[Inject] private ISubscriber<byte, CustomButton.CancelMessage> _cancelSubscriber = null;   // �L�����Z�����b�Z�[�W

	// �������Đ�����R���|�[�l���g
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
		_audioSource = GetComponent<AudioSource>();
		_button = GetComponent<CustomButton>();
	}

	private void Start() {
		_button.OnMoveObservable.Subscribe(_ => OnMove())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_pressSubscriber.Subscribe(_button.ID, _ => OnPress())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_cancelSubscriber.Subscribe(_button.ID, _ => OnCancel())
			.AddTo(this.GetCancellationTokenOnDestroy());
	}
}