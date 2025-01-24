using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using R3.Triggers;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

// R3とMessagePipeを用いた、自作のボタンクラス
[RequireComponent(typeof(ObservableEventTrigger))]
public class CustomButton : MonoBehaviour {
	// キャンセルメッセージ
	public struct CancelMessage { }

	// 各IDの対応については各イベントクラスを参照のこと
	[SerializeField] private int EventID = -1;	// 押下時に呼ぶイベントのID
	[SerializeField] private int WindowID = -1;	// 自身を紐づけるウィンドウのID

	// OnMoveによってフォーカスを変更するボタン
	[SerializeField] private CustomButton SelectOnLeft = null;	// 左
	[SerializeField] private CustomButton SelectOnUp = null;	// 上
	[SerializeField] private CustomButton SelectOnRight = null;	// 右
	[SerializeField] private CustomButton SelectOnDown = null;  // 下

	// メッセージ送信の窓口
	[Inject] private readonly IPublisher<int> _eventPublisher = null;					// イベント

	[Inject] private readonly IPublisher<int, CancelMessage> _cancelPublisher = null;	// キャンセル

	// メッセージ受信の窓口
	[Inject] private readonly ISubscriber<int, Window.StateMessage> _stateSubscriber = null;

	// 現在の状態を保存するReactiveProperty
	// 端的にいえば、値が変更されたときにメッセージを発信する変数
	private readonly ReactiveProperty<bool> _isDisplayedRP = new(false);	// 表示状態
	private readonly ReactiveProperty<bool> _isActiveRP = new(false);		// アクティブ状態(=入力受付の是非)

	// EventSystemからのメッセージを受け取るもの
	private ObservableEventTrigger _eventTrigger = null;

	public Action OnReceiveCallback = null;

	// ReactivePropertyそのものを取得するプロパティ
	// 他クラスからの購読時に用いる
	public ReadOnlyReactiveProperty<bool> IsDisplayedRP => _isDisplayedRP;	// 表示中かどうか
	public ReadOnlyReactiveProperty<bool> IsActiveRP => _isActiveRP;		// アクティブ状態かどうか

	// ReactivePropertyの値を取得・更新するプロパティ
	// 表示中かどうか
	public bool IsDisplayed {
		get => _isDisplayedRP.Value;
		set => _isDisplayedRP.Value = value;
	}

	// アクティブ状態かどうか
	public bool IsActive {
		get => _isActiveRP.Value;
		set => _isActiveRP.Value = _isDisplayedRP.Value && value;
	}

	// EventSystemからのメッセージ
	public Observable<Unit> OnSelectObservable => _eventTrigger.OnSelectAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// 選択

	public Observable<Unit> OnDeselectObservable => _eventTrigger.OnDeselectAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// 選択解除

	public Observable<Unit> OnSubmitObservable => _eventTrigger.OnSubmitAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// 決定

	public Observable<Unit> OnCancelObservable => _eventTrigger.OnCancelAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// キャンセル

	public Observable<AxisEventData> OnMoveObservable => _eventTrigger.OnMoveAsObservable()
		.Where(_ => IsActive);						// 移動

	public Observable<Unit> OnPointerEnterObservable => _eventTrigger.OnPointerEnterAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// マウスオーバー
	
	public Observable<Unit> OnPointerExitObservable => _eventTrigger.OnPointerExitAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// マウスオーバー解除

	public Observable<Unit> OnOnPointerDownObservable => _eventTrigger.OnPointerDownAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// ボタン押下

	public Observable<Unit> OnPointerClickObservable => _eventTrigger.OnPointerClickAsObservable()
		.AsUnitObservable().Where(_ => IsActive);	// ボタン押下->マウスオーバー状態で解除

	// 自分の上でマウスのボタンが押され、違うところで離されたときのメッセージ
	public Observable<Unit> OnPointerUpObservable => _eventTrigger.OnPointerUpAsObservable()
		.AsUnitObservable().Where(_ => IsActive);   // ボタン押下->マウスオーバーでない状態で解除

	/// <summary>
	/// OnPressメッセージ受信時の処理
	/// PressMessageを発信する
	/// </summary>
	protected void OnPress() {
		OnReceiveCallback?.Invoke();
		_eventPublisher.Publish(EventID);
	}

	/// <summary>
	/// OnCancelメッセージ受信時の処理
	/// CancelMessageを発信する
	/// </summary>
	private void OnCancel() => _cancelPublisher.Publish(WindowID, new CancelMessage());

	/// <summary>
	/// OnMoveメッセージ受信時の処理
	/// </summary>
	/// <param name="direction">入力された方向</param>
	private void OnMove(MoveDirection direction) {
		switch (direction) {
			case MoveDirection.Left:	// 左
				if (SelectOnLeft != null && SelectOnLeft.IsActive) {
					// 左のオブジェクトを選択させる
					EventSystem.current.SetSelectedGameObject(SelectOnLeft.gameObject);
				}
				break;

			case MoveDirection.Up:		// 上
				if (SelectOnUp != null && SelectOnUp.IsActive) {
					// 上のオブジェクトを選択させる
					EventSystem.current.SetSelectedGameObject(SelectOnUp.gameObject);
				}
				break;

			case MoveDirection.Right:	// 右
				if (SelectOnRight != null && SelectOnRight.IsActive) {
					// 右のオブジェクトを選択させる
					EventSystem.current.SetSelectedGameObject(SelectOnRight.gameObject);
				}
				break;

			case MoveDirection.Down:	// 下
				if (SelectOnDown != null && SelectOnDown.IsActive) {
					// 下のオブジェクトを選択させる
					EventSystem.current.SetSelectedGameObject(SelectOnDown.gameObject);
				}
				break;

			default:
				break;
		}
	}

	/// <summary>
	/// OnPointerEnterメッセージ受信時の処理
	/// </summary>
	private void OnPointerEnter() {
		// 自身のオブジェクトを選択させる
		EventSystem.current.SetSelectedGameObject(gameObject);
	}

	private void OnChangeState(Window.StateMessage.State state) {
		switch (state) {
			case Window.StateMessage.State.Hiding:
				IsDisplayed = false;
				IsActive = false;
				break;

			case Window.StateMessage.State.Inactive:
				IsDisplayed = true;
				IsActive = false;
				break;

			case Window.StateMessage.State.Active:
				IsDisplayed = true;
				IsActive = true;
				break;

			default:
				break;
		}
	}

	virtual protected void Awake() {
		// 必要なコンポーネントをキャッシュ
		_eventTrigger = GetComponent<ObservableEventTrigger>();

		// メッセージ受信時の処理を設定
		OnSubmitObservable.Subscribe(_ => OnPress()).AddTo(this.GetCancellationTokenOnDestroy());

		OnCancelObservable.Subscribe(_ => OnCancel()).AddTo(this.GetCancellationTokenOnDestroy());

		OnMoveObservable.Subscribe(x => OnMove(x.moveDir)).AddTo(this.GetCancellationTokenOnDestroy());

		OnPointerEnterObservable.Subscribe(_ => OnPointerEnter()).AddTo(this.GetCancellationTokenOnDestroy());

		OnPointerClickObservable.Subscribe(_ => OnPress()).AddTo(this.GetCancellationTokenOnDestroy());

		_stateSubscriber.Subscribe(WindowID, x => OnChangeState(x.Value)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}