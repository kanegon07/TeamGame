using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;

/// <summary>
/// ����{�^����SE��ύX�E�Ǘ�����N���X
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonSound : MonoBehaviour {
	// SE
	[SerializeField] private AudioClip SelectSE = null;	// �I����
	[SerializeField] private AudioClip PressSE = null;	// �����ꂽ�Ƃ�
	[SerializeField] private AudioClip CancelSE = null;	// �L�����Z�����ꂽ�Ƃ�

	// �������Đ�����R���|�[�l���g
	private AudioSource _audioSource = null;
	// ����{�^��
	private CustomButton _button = null;

	/// <summary>
	/// EventSystem�ɑI�����ꂽ�Ƃ��̏���
	/// </summary>
	private void OnSelect() => _audioSource.PlayOneShot(SelectSE);

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
		_button.OnSelectObservable.Subscribe(_ => OnSelect()).AddTo(this.GetCancellationTokenOnDestroy());

		_button.OnSubmitObservable.Subscribe(_ => OnPress()).AddTo(this.GetCancellationTokenOnDestroy());
		_button.OnPointerClickObservable.Subscribe(_ => OnPress()).AddTo(this.GetCancellationTokenOnDestroy());

		_button.OnCancelObservable.Subscribe(_ => OnCancel()).AddTo(this.GetCancellationTokenOnDestroy());
	}
}