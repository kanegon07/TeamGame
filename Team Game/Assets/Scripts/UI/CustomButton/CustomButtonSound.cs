using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;

/// <summary>
/// 自作ボタンのSEを変更・管理するクラス
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonSound : MonoBehaviour {
	// SE
	[SerializeField] private AudioClip SelectSE = null;	// 選択時
	[SerializeField] private AudioClip PressSE = null;	// 押されたとき
	[SerializeField] private AudioClip CancelSE = null;	// キャンセルされたとき

	// 音声を再生するコンポーネント
	private AudioSource _audioSource = null;
	// 自作ボタン
	private CustomButton _button = null;

	/// <summary>
	/// EventSystemに選択されたときの処理
	/// </summary>
	private void OnSelect() => _audioSource.PlayOneShot(SelectSE);

	/// <summary>
	/// 押されたときの処理
	/// </summary>
	private void OnPress() => _audioSource.PlayOneShot(PressSE);

	/// <summary>
	/// キャンセルされたときの処理
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