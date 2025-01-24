using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(OptionTag))]
public class OptionTagView : MonoBehaviour {
	private Image _image = null;
	private OptionTag _optionTag = null;

	/// <summary>
	/// 表示状態の変更時の処理
	/// </summary>
	/// <param name="flag">値</param>
	private void OnDisplay(bool flag) {
		// 表示状態を変更
		_image.enabled = flag;
	}

	public void Awake() {
		_image = GetComponent<Image>();
		_optionTag = GetComponent<OptionTag>();
	}

	public void Start() {
		_optionTag.IsDisplayedRP.Subscribe(x => OnDisplay(x))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}
}