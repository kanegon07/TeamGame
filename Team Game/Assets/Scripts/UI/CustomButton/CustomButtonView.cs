using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 自作ボタンの見た目を変更・管理するクラス
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonView : MonoBehaviour {
	// 画像
	private Image _image = null;
	// アニメーション再生用のコンポーネント
	private Animator _animator;
	// 自作ボタン
	private CustomButton _button = null;

	/// <summary>
	/// 表示状態の変更時の処理
	/// </summary>
	/// <param name="flag">値</param>
	private void OnDisplay(bool flag) {
		// 画像の表示状態を変更
		_image.enabled = flag;
	}

	/// <summary>
	/// アクティブ状態の変更時の処理
	/// </summary>
	/// <param name="flag">値</param>
	private void OnActivate(bool flag) {
		string triggerName = flag ? "Activated" : "Deactivated";
		_animator.SetTrigger(triggerName);
	}

	/// <summary>
	/// EventSystemからの選択状態の変更時の処理
	/// </summary>
	/// <param name="flag">値</param>
	private void OnSelect(bool flag) {
		string triggerName = flag ? "Selected" : "Deselected";
		_animator.SetTrigger(triggerName);
	}

	private void Awake() {
		_image = GetComponent<Image>();
		_animator = GetComponent<Animator>();
		_button = GetComponent<CustomButton>();
	}

	private void Start() {
		_button.IsDisplayedRP.Subscribe(x => OnDisplay(x)).AddTo(this.GetCancellationTokenOnDestroy());

		_button.IsActiveRP.Subscribe(x => OnActivate(x)).AddTo(this.GetCancellationTokenOnDestroy());

		_button.OnSelectObservable.Subscribe(_ => OnSelect(true)).AddTo(this.GetCancellationTokenOnDestroy());

		_button.OnDeselectObservable.Subscribe(_ => OnSelect(false)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}