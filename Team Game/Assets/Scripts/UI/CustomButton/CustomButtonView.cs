using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 自作ボタンの見た目を変更・管理するクラス
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonView : MonoBehaviour {
	// アニメーション再生用のコンポーネント
	private Animator _animator;
	// 自作ボタン
	private CustomButton _button = null;

	private Image _base = null;
	private Image _color = null;
	private Image _decoL = null;
	private Image _decoR = null;

	private TMP_Text _text = null;

	/// <summary>
	/// 表示状態の変更時の処理
	/// </summary>
	/// <param name="flag">値</param>
	private void OnDisplay(bool flag) {
		// 表示状態を変更
		_base.enabled = flag;
		_color.enabled = flag;
		_decoL.enabled = flag;
		_decoR.enabled = flag;
		_text.enabled = flag;
	}

	/// <summary>
	/// アクティブ状態の変更時の処理
	/// </summary>
	/// <param name="flag">値</param>
	private void OnActivate(bool flag) {
		if (flag) {
			_animator.SetFloat(Animator.StringToHash("Speed"), 1F);
			_animator.Play("Activate", 0, 0F);
		} else {
			_animator.SetFloat(Animator.StringToHash("Speed"), -1F);
			_animator.Play("Activate", 0, 1F);
		}
	}

	/// <summary>
	/// EventSystemからの選択状態の変更時の処理
	/// </summary>
	/// <param name="flag">値</param>
	private void OnSelect(bool flag) {
		if (flag) {
			_animator.SetFloat(Animator.StringToHash("Speed"), 1F);
			_animator.Play("Select", 0, 0F);
		} else {
			_animator.SetFloat(Animator.StringToHash("Speed"), -1F);
			_animator.Play("Select", 0, 1F);
		}
	}

	private void Awake() {
		_animator = GetComponent<Animator>();
		_button = GetComponent<CustomButton>();
	}

	private void Start() {
		_base = transform.Find("Base").GetComponent<Image>();
		_color = transform.Find("Color").GetComponent<Image>();
		_decoL = transform.Find("Deco_L").GetComponent<Image>();
		_decoR = transform.Find("Deco_R").GetComponent<Image>();
		_text = GetComponentInChildren<TMP_Text>();

		_button.IsDisplayedRP.Subscribe(x => OnDisplay(x)).AddTo(this.GetCancellationTokenOnDestroy());

		_button.IsActiveRP.Subscribe(x => OnActivate(x)).AddTo(this.GetCancellationTokenOnDestroy());

		_button.OnSelectObservable.Subscribe(_ => OnSelect(true)).AddTo(this.GetCancellationTokenOnDestroy());

		_button.OnDeselectObservable.Subscribe(_ => OnSelect(false)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}