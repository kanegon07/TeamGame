using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(OptionTag))]
public class OptionTagSound : MonoBehaviour {
	[SerializeField] private AudioClip _se = null;

	private AudioSource _audioSource = null;
	private OptionTag _optionTag = null;

	private void OnPress() => _audioSource.PlayOneShot(_se);

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();
		_optionTag = GetComponent<OptionTag>();
	}

	private void Start() {
		_optionTag.OnReceiveCallback += OnPress;
	}

	private void OnDestroy() {
		_optionTag.OnReceiveCallback -= OnPress;
	}
}