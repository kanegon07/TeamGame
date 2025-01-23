using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;

// 自作ボタンのSEを変更・管理するクラス
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonSound : MonoBehaviour {
	// SE
	[SerializeField] private AudioClip MoveSE = null;	// 移動
	[SerializeField] private AudioClip PressSE = null;	// 決定
	[SerializeField] private AudioClip CancelSE = null; // キャンセル

	// SE再生用
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
		// 必要なコンポーネントをキャッシュ
		_audioSource = GetComponent<AudioSource>();
		_button = GetComponent<CustomButton>();
	}

	private void Start() {
		// メッセージ受信時の処理を設定
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