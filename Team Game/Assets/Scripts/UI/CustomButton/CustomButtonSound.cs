using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;
using VContainer;

/// <summary>
/// 自作ボタンのSEを変更・管理するクラス
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonSound : MonoBehaviour {
	// SE
	[SerializeField] private AudioClip MoveSE = null;	// 移動時
	[SerializeField] private AudioClip PressSE = null;	// 押されたとき
	[SerializeField] private AudioClip CancelSE = null; // キャンセルされたとき

	// メッセージ受信の窓口
	[Inject] private ISubscriber<byte, CustomButton.PressMessage> _pressSubscriber = null;     // ボタン押下メッセージ
	[Inject] private ISubscriber<byte, CustomButton.CancelMessage> _cancelSubscriber = null;   // キャンセルメッセージ

	// 音声を再生するコンポーネント
	private AudioSource _audioSource = null;
	// 自作ボタン
	private CustomButton _button = null;

	/// <summary>
	/// EventSystemに選択されたときの処理
	/// </summary>
	private void OnMove() => _audioSource.PlayOneShot(MoveSE);

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
		_button.OnMoveObservable.Subscribe(_ => OnMove())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_pressSubscriber.Subscribe(_button.ID, _ => OnPress())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_cancelSubscriber.Subscribe(_button.ID, _ => OnCancel())
			.AddTo(this.GetCancellationTokenOnDestroy());
	}
}