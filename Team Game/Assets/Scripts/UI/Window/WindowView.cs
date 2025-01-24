using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Window))]
public class WindowView : MonoBehaviour {
	// 画像
	private Image _image = null;
	private Window _window = null;

	private void OnDisplay(bool flag) {
		if (_image) {
			_image.enabled = flag;
		}
	}

	private void Awake() {
		// 必要なコンポーネントをキャッシュ
		_window = GetComponent<Window>();
		_image = GetComponent<Image>();
	}

	private void Start() {
		// メッセージ受信時の処理を設定
		_window.IsDisplayedRP.Subscribe(x => OnDisplay(x)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}