using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

// ウィンドウクラス
public class Window : MonoBehaviour {
	public struct StateMessage : IEquatable<StateMessage> {
		public enum State {
			Hiding,
			Inactive,
			Active
		}

		public State Value;

		public StateMessage(State value) {
			Value = value;
		}

		public readonly bool Equals(StateMessage other) => Value == other.Value;
	}

	// 各IDの対応については各イベントクラスを参照のこと
	[SerializeField] private int EventID = -1;	// キャンセル時に呼ぶイベントのID
	[SerializeField] private int WindowID = -1;	// 自身のID

	// アクティブ状態になったとき、最初に選択するボタン
	[SerializeField] private CustomButton FirstSelected = null;

	// メッセージ発信の窓口
	[Inject] private readonly IPublisher<int> _eventPublisher = null;

	[Inject] private readonly IPublisher<SelectionHook.UnhookMessage> _unhookPublisher = null;	// 選択のフック解除

	// メッセージ受信の窓口
	[Inject] private readonly ISubscriber<int, CustomButton.CancelMessage> _cancelSubscriber = null;  // キャンセル
	[Inject] private readonly ISubscriber<int, StateMessage> _stateSubscriber = null;

	// 現在の状態を保存するReactiveProperty
	// 端的にいえば、値が変更されたときにメッセージを発信する変数
	private readonly ReactiveProperty<bool> _isDisplayedRP = new(false);	// 表示状態
	private readonly ReactiveProperty<bool> _isActiveRP = new(false);		// アクティブ状態

	// ReactivePropertyそのものを取得するプロパティ
	// 他クラスからの購読時に用いる
	public ReadOnlyReactiveProperty<bool> IsDisplayedRP => _isDisplayedRP;	// 表示中かどうか
	public ReadOnlyReactiveProperty<bool> IsActiveRP => _isActiveRP;		// アクティブ状態(=入力を受け付ける)かどうか

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

	/// <summary>
	/// アクティブ状態が更新されたときの処理
	/// </summary>
	/// <param name="value">フラグ</param>
	private void OnActivate(bool value) {
		if (value) {
			// アクティブ状態になるとき、
			// あらかじめ決めたオブジェクトを選択させる
			if (FirstSelected == null) {
				EventSystem.current.SetSelectedGameObject(null);
			} else {
				EventSystem.current.SetSelectedGameObject(FirstSelected.gameObject);
			}
		} else {
			// 非アクティブ状態になるとき、
			// 選択のフックを解除する
			_unhookPublisher.Publish(new SelectionHook.UnhookMessage());
		}
	}

	/// <summary>
	/// 自身に紐づけられたボタンでキャンセル入力があったときの処理
	/// </summary>
	private void OnCancel() {
		// キャンセル時のイベントを呼ぶ
		// キャンセル時のイベントをウィンドウ単位で決めることで
		// イベントでの余計なメッセージ受信を減らす
		_eventPublisher.Publish(EventID);
	}

	/// <summary>
	/// 表示状態が変更されたときに呼ばれる処理
	/// </summary>
	/// <param name="state"></param>
	private void OnChangeState(StateMessage.State state) {
		switch (state) {
			case StateMessage.State.Hiding:		// 表示なし、操作受付なし
				IsDisplayed = false;
				IsActive = false;
				break;

			case StateMessage.State.Inactive:	// 表示あり、操作受付なし
				IsDisplayed = true;
				IsActive = false;
				break;

			case StateMessage.State.Active:		// 表示あり、操作受付あり
				IsDisplayed = true;
				IsActive = true;
				break;

			default:
				break;
		}
	}

	private void Awake() {
		// 必要なコンポーネントをキャッシュ

		// メッセージ受信時の処理を設定
		_isActiveRP.Subscribe(x => OnActivate(x)).AddTo(this.GetCancellationTokenOnDestroy());

		_cancelSubscriber.Subscribe(WindowID, _ => OnCancel()).AddTo(this.GetCancellationTokenOnDestroy());

		_stateSubscriber.Subscribe(WindowID, x => OnChangeState(x.Value)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}