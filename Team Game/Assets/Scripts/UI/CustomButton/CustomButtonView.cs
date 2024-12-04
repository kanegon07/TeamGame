using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����{�^���̌����ڂ�ύX�E�Ǘ�����N���X
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonView : MonoBehaviour {
	// �e��Ԃł̐F
	[SerializeField] private Color NormalColor = Color.black;	// �ʏ펞
	[SerializeField] private Color SelectedColor = Color.black;	// �I����
	[SerializeField] private Color InactiveColor = Color.black;	// ��A�N�e�B�u(=���͂��󂯕t���Ȃ�)��

	// �摜
	private Image _image = null;
	// �e�L�X�g
	private TMP_Text _text = null;
	// ����{�^��
	private CustomButton _button = null;

	/// <summary>
	/// �\����Ԃ̕ύX���̏���
	/// </summary>
	/// <param name="flag">�l</param>
	private void OnDisplay(bool flag) {
		// �摜�̕\����Ԃ�ύX
		_image.enabled = flag;
		if (_text != null) {
			// �e�L�X�g�͂���Ƃ͌���Ȃ��̂Ő�Ƀ`�F�b�N����
			_text.enabled = flag;
		}
	}

	/// <summary>
	/// �A�N�e�B�u��Ԃ̕ύX���̏���
	/// </summary>
	/// <param name="flag">�l</param>
	private void OnActivate(bool flag) => _image.color = flag ? NormalColor : InactiveColor;

	/// <summary>
	/// EventSystem����̑I����Ԃ̕ύX���̏���
	/// </summary>
	/// <param name="flag">�l</param>
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