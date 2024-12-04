using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// ウィンドウクラス
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class Window : MonoBehaviour {
	/// <summary>
	/// 表示状態が変更されたときに発信するメッセージ
	/// </summary>
	public struct DisplayMessage : IEquatable<DisplayMessage> {
		// 値
		public bool Value;

		public DisplayMessage(bool value) {
			Value = value;
		}

		public readonly bool Equals(DisplayMessage other) => Value == other.Value;
	}

	/// <summary>
	/// アクティブ状態(=入力を受け付けるか)が変更されたときに発信するメッセージ
	/// </summary>
	public struct ActivateMessage : IEquatable<ActivateMessage> {
		// 値
		public bool Value;

		public ActivateMessage(bool value) {
			Value = value;
		}

		public readonly bool Equals(ActivateMessage other) => Value == other.Value;
	}

	// 識別番号
	// イベントとの対応は各シーンでのイベント管理クラスを参照のこと
	[SerializeField] private byte ID = 0;
	// アクティブ状態になったとき、最初に選択するボタン
	[SerializeField] private CustomButton FirstSelected = null;

	// メッセージ発信の窓口
	// インスタンスIDをキーとすることで、「どのウィンドウからのメッセージか」を区別する
	[Inject] private IPublisher<int, DisplayMessage> _displayPublisher = null;		// 表示or非表示のメッセージ
	[Inject] private IPublisher<int, ActivateMessage> _activatePublisher = null;	// アクティブ状態変更のメッセージ

	// メッセージ受信の窓口
	[Inject] private ISubscriber<byte, DisplayMessage> _displaySubscriber = null;	// 表示or非表示のメッセージ
	[Inject] private ISubscriber<byte, ActivateMessage> _activateSubscriber = null; // アクティブ状態変更のメッセージ

	// 現在の状態を保存するReactiveProperty
	// 端的にいえば、値が変更されたときにメッセージを発信する変数
	private readonly ReactiveProperty<bool> _isDisplayedRP = new(false);	// 表示中かどうか
	private readonly ReactiveProperty<bool> _isActiveRP = new(false);		// アクティブ状態(=入力を受け付ける)かどうか

	// 画像
	private Image _image = null;
	// キャンバスグループ
	private CanvasGroup _canvasGroup = null;

	// ReactivePropertyそのものを取得するプロパティ
	// 他クラスからの購読時に用いる
	public ReadOnlyReactiveProperty<bool> IsDisplayedRP => _isDisplayedRP;	// 表示中かどうか
	public ReadOnlyReactiveProperty<bool> IsActiveRP => _isActiveRP;        // アクティブ状態(=入力を受け付ける)かどうか

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
	/// 表示状態の変更時の処理
	/// 受信したメッセージを中継(?)する
	/// </summary>
	/// <param name="msg">受信したメッセージ</param>
	private void OnDisplay(DisplayMessage msg) {
		IsDisplayed = msg.Value;

		if (_image != null) {
			_image.enabled = msg.Value;
		}

		_displayPublisher.Publish(transform.GetInstanceID(), msg);
	}

	/// <summary>
	/// アクティブ状態の変更時の処理
	/// 受信したメッセージを中継(?)する
	/// </summary>
	/// <param name="msg">受信したメッセージ/param>
	private void OnActivate(ActivateMessage msg) {
		IsActive = msg.Value;

		_canvasGroup.interactable = msg.Value;

		_activatePublisher.Publish(transform.GetInstanceID(), msg);

		if (msg.Value) {
			if (FirstSelected == null) {
				EventSystem.current.SetSelectedGameObject(null);
			} else {
				EventSystem.current.SetSelectedGameObject(FirstSelected.gameObject);
			}
		}
	}

	private void Awake() {
		_image = GetComponent<Image>();
		_canvasGroup = GetComponent<CanvasGroup>();

		_displaySubscriber.Subscribe(ID, x => OnDisplay(x)).AddTo(this.GetCancellationTokenOnDestroy());

		_activateSubscriber.Subscribe(ID, x => OnActivate(x)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}