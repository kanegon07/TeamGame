using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonSound : MonoBehaviour {
	[SerializeField] private AudioClip SelectSE = null;
	[SerializeField] private AudioClip PressSE = null;
	[SerializeField] private AudioClip CancelSE = null;

	private AudioSource _audioSource = null;
	private CustomButton _button = null;

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();
		_button = GetComponent<CustomButton>();
	}

	private void Start() {
		_button.OnSelectObservable.Subscribe(
			_ => _audioSource.PlayOneShot(SelectSE)
		).AddTo(this.GetCancellationTokenOnDestroy());

		_button.OnSubmitObservable.Subscribe(_ => _audioSource.PlayOneShot(PressSE));
		_button.OnPointerClickObservable.Subscribe(_ => _audioSource.PlayOneShot(PressSE));

		_button.OnCancelObservable.Subscribe(_ => _audioSource.PlayOneShot(CancelSE));
	}
}