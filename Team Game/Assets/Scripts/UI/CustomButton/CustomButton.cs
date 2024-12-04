using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

/// <summary>
/// R3とMessagePipeを用いた、自作のボタンクラス
/// </summary>
[RequireComponent(typeof(ObservableEventTrigger))]
public class CustomButton : MonoBehaviour {
	/// <summary>
	/// ボタンが押されたときに発信するメッセージ
	/// </summary>
	public struct PressMessage { }

	/// <summary>
	/// キャンセルされたときに発信するメッセージ
	/// </summary>
	public struct CancelMessage { }

	// 識別番号
	// イベントとの対応は各シーンでのイベント管理クラスを参照のこと
	[SerializeField] private byte ID = 0;

	// OnMoveによってフォーカスを変更するボタン
	[SerializeField] private CustomButton SelectOnLeft = null;	// 左
	[SerializeField] private CustomButton SelectOnUp = null;	// 上
	[SerializeField] private CustomButton SelectOnRight = null;	// 右
	[SerializeField] private CustomButton SelectOnDown = null;	// 下

	// メッセージ発信の窓口
	// IDをキーとすることで、「どのボタンからのメッセージか」を区別する
	[Inject] private IPublisher<byte, PressMessage> _pressPublisher = null;		// ボタン押下メッセージ
	[Inject] private IPublisher<byte, CancelMessage> _cancelPublisher = null;	// キャンセルメッセージ

	// ウィンドウクラスからのメッセージ受信の窓口
	[Inject] private ISubscriber<int, Window.DisplayMessage> _displaySubscrber = null;		// 表示or非表示のメッセージ
	[Inject] private ISubscriber<int, Window.ActivateMessage> _activateSubscrber = null;	// アクティブ状態変更のメッセージ

	// 現在の状態を保存するReactiveProperty
	// 端的にいえば、値が変更されたときにメッセージを発信する変数
	private readonly ReactiveProperty<bool> _isDisplayedRP = new(false);	// 表示中かどうか
	private readonly ReactiveProperty<bool> _isActiveRP = new(false);		// アクティブ状態(=入力を受け付ける)かどうか

	// EventSystemからのメッセージを受け取るもの
	private ObservableEventTrigger _eventTrigger = null;

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
		set => _isActiveRP.Value = value;
	}

	// EventSystemに選択されたときのメッセージ
	public Observable<Unit> OnSelectObservable => _eventTrigger.OnSelectAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// EventSystemからの選択が解除されたときのメッセージ
	public Observable<Unit> OnDeselectObservable => _eventTrigger.OnDeselectAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// 選択中にSubmitボタンが入力されたときのメッセージ
	public Observable<Unit> OnSubmitObservable => _eventTrigger.OnSubmitAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// 選択中にCancelボタンが入力されたときのメッセージ
	public Observable<Unit> OnCancelObservable => _eventTrigger.OnCancelAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// 選択中にMoveボタンが入力されたときのメッセージ
	public Observable<AxisEventData> OnMoveObservable => _eventTrigger.OnMoveAsObservable()
		.Where(_ => IsActive);

	// マウスポインターが重なったときのメッセージ
	public Observable<Unit> OnPointerEnterObservable => _eventTrigger.OnPointerEnterAsObservable()
		.AsUnitObservable().Where(_ => IsActive);
	
	// マウスポインタ―が離れたときのメッセージ
	public Observable<Unit> OnPointerExitObservable => _eventTrigger.OnPointerExitAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// 自分の上でマウスのボタンが押されたときのメッセージ
	public Observable<Unit> OnOnPointerDownObservable => _eventTrigger.OnPointerDownAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// 自分の上でマウスのボタンが押され、離されたときのメッセージ
	public Observable<Unit> OnPointerClickObservable => _eventTrigger.OnPointerClickAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	// 自分の上でマウスのボタンが押され、違うところで離されたときのメッセージ
	public Observable<Unit> OnPointerUpObservable => _eventTrigger.OnPointerUpAsObservable()
		.AsUnitObservable().Where(_ => IsActive);

	/// <summary>
	/// OnSelectメッセージ受信時の処理
	/// 選択フッククラスに自身を登録する
	/// </summary>
	private void OnSelect() {
		if (EventSystem.current.gameObject.TryGetComponent(out SelectionHook hook)) {
			hook.PrevSelected = gameObject;
		}
	}

	/// <summary>
	/// OnPressメッセージ受信時の処理
	/// PressMessageを発信する
	/// </summary>
	private void OnPress() => _pressPublisher.Publish(ID, new PressMessage());

	/// <summary>
	/// OnCancelメッセージ受信時の処理
	/// CancelMessageを発信する
	/// </summary>
	private void OnCancel() => _cancelPublisher.Publish(ID, new CancelMessage());

	/// <summary>
	/// OnMoveメッセージ受信時の処理
	/// </summary>
	/// <param name="direction">入力された方向</param>
	private void OnMove(MoveDirection direction) {
		switch (direction) {
			case MoveDirection.Left:	// 左
				if (SelectOnLeft != null && SelectOnLeft.IsActive) {
					// EventSystemにSelectOnLeftのオブジェクトを選択させる
					EventSystem.current.SetSelectedGameObject(SelectOnLeft.gameObject);
				}
				break;

			case MoveDirection.Up:		// 上
				if (SelectOnUp != null && SelectOnUp.IsActive) {
					// EventSystemにSelectOnUpのオブジェクトを選択させる
					EventSystem.current.SetSelectedGameObject(SelectOnUp.gameObject);
				}
				break;

			case MoveDirection.Right:	// 右
				if (SelectOnRight != null && SelectOnRight.IsActive) {
					// EventSystemにSelectOnRightのオブジェクトを選択させる
					EventSystem.current.SetSelectedGameObject(SelectOnRight.gameObject);
				}
				break;

			case MoveDirection.Down:	// 下
				if (SelectOnDown != null && SelectOnDown.IsActive) {
					// EventSystemにSelectOnDownのオブジェクトを選択させる
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
		// EventSystemに自身のオブジェクトを選択させる
		EventSystem.current.SetSelectedGameObject(gameObject);
	}

	/// <summary>
	/// DisplayMessage受信時の処理
	/// 自身の表示状態も変更する
	/// </summary>
	/// <param name="value">受け取った値</param>
	private void OnDisplay(bool value) => IsDisplayed = value;

	/// <summary>
	/// ActivateMessage受信時の処理
	/// 自身のアクティブ状態も変更する
	/// </summary>
	/// <param name="value">受け取った値</param>
	private void OnActivate(bool value) => IsActive = value;

	private void Awake() {
		_eventTrigger = GetComponent<ObservableEventTrigger>();

		OnSelectObservable.Subscribe(_ => OnSelect()).AddTo(this.GetCancellationTokenOnDestroy());

		OnSubmitObservable.Subscribe(_ => OnPress()).AddTo(this.GetCancellationTokenOnDestroy());

		OnCancelObservable.Subscribe(_ => OnCancel()).AddTo(this.GetCancellationTokenOnDestroy());

		OnMoveObservable.Subscribe(x => OnMove(x.moveDir)).AddTo(this.GetCancellationTokenOnDestroy());

		OnPointerEnterObservable.Subscribe(_ => OnPointerEnter()).AddTo(this.GetCancellationTokenOnDestroy());

		OnPointerClickObservable.Subscribe(_ => OnPress()).AddTo(this.GetCancellationTokenOnDestroy());

		_displaySubscrber.Subscribe(transform.parent.GetInstanceID(), x => OnDisplay(x.Value)).AddTo(this.GetCancellationTokenOnDestroy());

		_activateSubscrber.Subscribe(transform.parent.GetInstanceID(), x => OnActivate(x.Value)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}