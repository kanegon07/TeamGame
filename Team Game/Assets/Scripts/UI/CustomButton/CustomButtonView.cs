using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����{�^���̌����ڂ�ύX�E�Ǘ�����N���X
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonView : MonoBehaviour {
	// �A�j���[�V�����Đ��p�̃R���|�[�l���g
	private Animator _animator;
	// ����{�^��
	private CustomButton _button = null;

	private Image _base = null;
	private Image _color = null;
	private Image _decoL = null;
	private Image _decoR = null;

	private TMP_Text _text = null;

	/// <summary>
	/// �\����Ԃ̕ύX���̏���
	/// </summary>
	/// <param name="flag">�l</param>
	private void OnDisplay(bool flag) {
		// �\����Ԃ�ύX
		_base.enabled = flag;
		_color.enabled = flag;
		_decoL.enabled = flag;
		_decoR.enabled = flag;
		_text.enabled = flag;
	}

	/// <summary>
	/// �A�N�e�B�u��Ԃ̕ύX���̏���
	/// </summary>
	/// <param name="flag">�l</param>
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
	/// EventSystem����̑I����Ԃ̕ύX���̏���
	/// </summary>
	/// <param name="flag">�l</param>
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

		_button.IsDisplayedRP.Subscribe(x => OnDisplay(x))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_button.IsActiveRP.Subscribe(x => OnActivate(x))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_button.OnSelectObservable.Subscribe(_ => OnSelect(true))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_button.OnDeselectObservable.Subscribe(_ => OnSelect(false))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}
}