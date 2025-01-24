using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

// タイトルシーン用のイベント管理クラス
[RequireComponent(typeof(AudioSource))]
public class TitleEvents : MonoBehaviour {
	// 出力先に割り振る番号
	public enum WindowID {
		Main,			// メインウィンドウ
		StageSelector	// ステージ選択ウィンドウ
	}

	// 入力元に割り振る番号
	public enum EventID {
		OpenStageSelector,
		CloseStageSelector,
		StartStage1,
		StartStage2,
		StartStage3
	}

	// メッセージ送信の窓口(同期)
	[Inject] private readonly IPublisher<int, Window.StateMessage> _statePublisher = null;

	// メッセージ送信の窓口(非同期)
	[Inject] private readonly IAsyncPublisher<WipeEffectController.WipeMessage> _wipeAsyncPublisher = null; // ワイプ処理

	// メッセージ受信の窓口(同期)
	[Inject] private readonly ISubscriber<int> _eventSubscriber = null;

	// BGM再生用
	private AudioSource _audioSource = null;

	/// <summary>
	/// ステージ選択ウィンドウを開く
	/// </summary>
	private void OpenStageSelector() {
		// ステージ選択ウィンドウのみ操作を受け付けるようにする
		_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Inactive));
		_statePublisher.Publish((int)WindowID.StageSelector, new Window.StateMessage(Window.StateMessage.State.Active));
	}

	/// <summary>
	/// ステージ選択画面を閉じる
	/// </summary>
	private void CloseStageSelector() {
		// ステージ選択ウィンドウを閉じ、
		// メインウィンドウが操作を受け付けるようにする
		_statePublisher.Publish((int)WindowID.StageSelector, new Window.StateMessage(Window.StateMessage.State.Hiding));
		_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Active));
	}

	/// <summary>
	/// ワイプ処理を行う
	/// </summary>
	/// <param name="wipesOut">ワイプアウト(消える)処理かどうか</param>
	/// <returns>非同期処理用のタスク</returns>
	private async UniTask Wipe(bool wipesOut)
		=> await _wipeAsyncPublisher.PublishAsync(
			new WipeEffectController.WipeMessage(wipesOut),
			this.GetCancellationTokenOnDestroy()
		);

	/// <summary>
	/// シーンを切り替える
	/// </summary>
	/// <param name="nextScene">次のシーンの名前</param>
	private async void TransitScene(string nextScene) {
		// BGMを止める
		_audioSource.Stop();

		// カーソルの位置を固定し、見えないようにする
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		// 操作を受け付けないようにする
		_statePublisher.Publish((byte)WindowID.StageSelector, new Window.StateMessage(Window.StateMessage.State.Inactive));

		// ワイプアウトの完了を待つ
		await Wipe(true);

		// 次のシーンを読み込む
		SceneManager.LoadScene(nextScene);
	}

	private void Awake() {
		// 必要なコンポーネントをキャッシュ
		_audioSource = GetComponent<AudioSource>();

		// メッセージ受信時の処理を設定
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
		// カーソルの位置を固定し、見えないようにする
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		// ワイプインの完了を待つ
		await Wipe(false);

		_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Active));

		// カーソルの位置固定を解除し、見えるようにする
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		// BGMの再生を始める
		_audioSource.Play();
	}
}