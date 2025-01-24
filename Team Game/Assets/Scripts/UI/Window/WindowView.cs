using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Window))]
public class WindowView : MonoBehaviour {
	// �摜
	private Image _image = null;
	private Window _window = null;

	private void OnDisplay(bool flag) {
		if (_image) {
			_image.enabled = flag;
		}
	}

	private void Awake() {
		// �K�v�ȃR���|�[�l���g���L���b�V��
		_window = GetComponent<Window>();
		_image = GetComponent<Image>();
	}

	private void Start() {
		// ���b�Z�[�W��M���̏�����ݒ�
		_window.IsDisplayedRP.Subscribe(x => OnDisplay(x)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}