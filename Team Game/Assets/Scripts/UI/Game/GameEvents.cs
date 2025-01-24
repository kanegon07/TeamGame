using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

// ゲームシーン用のイベント管理クラス
[RequireComponent(typeof(AudioSource))]
public class GameEvents : MonoBehaviour {
	// 出力先に割り振る番号
	public enum WindowID {
		Main,	// メインウィンドウ
		Option	// オプションウィンドウ
	}

	// 入力元に割り振る番号
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

	// 他のオブジェクトの参照
	[SerializeField] private CameraPlayer Player = null;	// プレイヤー
	[SerializeField] private FPSCamera Camera = null;		// カメラ
	[SerializeField] private DebugMouse MouseLock = null;	// マウスロック

	// SEデータ
	[SerializeField] private AudioClip OptionSE = null;
	[SerializeField] private AudioClip ClearSE = null;		// ゴール
	[SerializeField] private AudioClip MissSE = null;		// ゲームオーバー

	// メッセージ送信の窓口(同期)
	[Inject] private readonly IPublisher<int, Window.StateMessage> _statePublisher = null;

	[Inject] private readonly IPublisher<StageInfo> _stageInfoPublisher = null;

	// メッセージ送信の窓口(非同期)
	[Inject] private readonly IAsyncPublisher<WipeEffectController.WipeMessage> _wipeAsyncPublisher = null; // ワイプ処理

	// メッセージ受信の窓口(同期)
	[Inject] private readonly ISubscriber<int> _eventSubscriber = null;

	// BGM再生用
	private AudioSource _audioSource = null;

	private StageInfo _stageInfo = null;

	/// <summary>
	/// オプションを開く
	/// </summary>
	private void OpenOption() {
		_audioSource.PlayOneShot(OptionSE);

		// ゲーム内時間を止める
		Time.timeScale = 0F;

		// プレイヤーとカメラが操作を受け付けないようにする
		Player.enabled = false;
		Camera.enabled = false;

		// オプションウィンドウのみ操作を受け付けるようにする
		_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Inactive));
		_statePublisher.Publish((int)WindowID.Option, new Window.StateMessage(Window.StateMessage.State.Active));

		// カーソルの位置固定を解除し、見えるようにする
		MouseLock.Lock(false);
	}

	/// <summary>
	/// オプションを閉じる
	/// </summary>
	private void CloseOption() {
		// オプションウィンドウを閉じ、
		// メインウィンドウが操作を受け付けるようにする
		_statePublisher.Publish((int)WindowID.Option, new Window.StateMessage(Window.StateMessage.State.Hiding));
		_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Active));

		// プレイヤーとカメラが操作を受け付けるようにする
		Player.enabled = true;
		Camera.enabled = true;

		// ゲーム内時間を元通り進める
		Time.timeScale = 1F;

		// カーソルの位置を固定し、非表示にする
		MouseLock.Lock(true);
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
	/// <param name="type">遷移タイプ</param>
	/// <param name="nextScene">次のシーンの名前</param>
	private async void TransitScene(TransitType type, string nextScene) {
		// BGMを止める
		_audioSource.Stop();

		// 遷移タイプによって細かい処理を変える
		switch (type) {
			case TransitType.Abort:	// オプションから中断するとき
				// オプションウィンドウが操作を受け付けないようにする
				_statePublisher.Publish((int)WindowID.Option, new Window.StateMessage(Window.StateMessage.State.Inactive));
				break;

			case TransitType.Miss:
				// ミス時のSEを再生する
				_audioSource.PlayOneShot(MissSE);
				// メインウィンドウが操作を受け付けないようにする
				_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Inactive));
				break;

			case TransitType.Clear:
				// クリア時のSEを再生する
				_audioSource.PlayOneShot(ClearSE);
				// メインウィンドウが操作を受け付けないようにする
				_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Inactive));
				break;

			default:
				break;
		}

		// カーソルの位置を固定し、見えないようにする
		MouseLock.Lock(true);

		// 操作を受け付けないようにする
		Player.enabled = false;
		Camera.enabled = false;

		// ワイプアウトの完了を待つ
		await Wipe(true);

		// 次のシーンを読み込む
		SceneManager.LoadScene(nextScene);
	}

	private void Awake() {
		// 必要なコンポーネントをキャッシュ
		_audioSource = GetComponent<AudioSource>();

		_stageInfo = Resources.Load<StageInfoTable>("StageInfoTable").Find(StageID);

		// メッセージ受信時の処理を設定
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

		// カーソルの位置を固定し、見えないようにする
		MouseLock.Lock(true);

		// 操作を受け付けないようにする
		Player.enabled = false;
		Camera.enabled = false;

		// ワイプインの完了を待つ
		await Wipe(false);

		// 操作を受け付けるようにする
		Player.enabled = true;
		Camera.enabled = true;

		_statePublisher.Publish((int)WindowID.Main, new Window.StateMessage(Window.StateMessage.State.Active));

		// 現在のシーンが削除されたとき、
		// 他オブジェクトの参照を解除する
		SceneManager.sceneUnloaded += (_) => {
			Player = null;
			Camera = null;
			MouseLock = null;
		};

		// BGMの再生を始める
		_audioSource.Play();
	}
}