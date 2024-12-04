using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 自作ボタンの見た目を変更・管理するクラス
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonView : MonoBehaviour {
	// 各状態での色
	[SerializeField] private Color NormalColor = Color.black;	// 通常時
	[SerializeField] private Color SelectedColor = Color.black;	// 選択時
	[SerializeField] private Color InactiveColor = Color.black;	// 非アクティブ(=入力を受け付けない)時

	// 画像
	private Image _image = null;
	// テキスト
	private TMP_Text _text = null;
	// 自作ボタン
	private CustomButton _button = null;

	/// <summary>
	/// 表示状態の変更時の処理
	/// </summary>
	/// <param name="flag">値</param>
	private void OnDisplay(bool flag) {
		// 画像の表示状態を変更
		_image.enabled = flag;
		if (_text != null) {
			// テキストはあるとは限らないので先にチェックする
			_text.enabled = flag;
		}
	}

	/// <summary>
	/// アクティブ状態の変更時の処理
	/// </summary>
	/// <param name="flag">値</param>
	private void OnActivate(bool flag) => _image.color = flag ? NormalColor : InactiveColor;

	/// <summary>
	/// EventSystemからの選択状態の変更時の処理
	/// </summary>
	/// <param name="flag">値</param>
	private void OnSelect(bool flag) => _image.color = flag ? SelectedColor : NormalColor;

	private void Awake() {
		_image = GetComponent<Image>();
		_text = GetComponentInChildren<TMP_Text>();
		_button = GetComponent<CustomButton>();
	}

	private void Start() {
		_button.IsDisplayedRP.Subscribe(x => OnDisplay(x)).AddTo(this.GetCancellationTokenOnDestroy());

		_button.IsActiveRP.Subscribe(x => OnActivate(x)).AddTo(this.GetCancellationTokenOnDestroy());

		_button.OnSelectObservable.Subscribe(_ => OnSelect(true)).AddTo(this.GetCancellationTokenOnDestroy());

		_button.OnDeselectObservable.Subscribe(_ => OnSelect(false)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}