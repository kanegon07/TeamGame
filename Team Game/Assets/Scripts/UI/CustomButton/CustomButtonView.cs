using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����{�^���̌����ڂ�ύX�E�Ǘ�����N���X
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonView : MonoBehaviour {
	// �摜
	private Image _image = null;
	// �A�j���[�V�����Đ��p�̃R���|�[�l���g
	private Animator _animator;
	// ����{�^��
	private CustomButton _button = null;

	/// <summary>
	/// �\����Ԃ̕ύX���̏���
	/// </summary>
	/// <param name="flag">�l</param>
	private void OnDisplay(bool flag) {
		// �摜�̕\����Ԃ�ύX
		_image.enabled = flag;
	}

	/// <summary>
	/// �A�N�e�B�u��Ԃ̕ύX���̏���
	/// </summary>
	/// <param name="flag">�l</param>
	private void OnActivate(bool flag) {
		string triggerName = flag ? "Activated" : "Deactivated";
		_animator.SetTrigger(triggerName);
	}

	/// <summary>
	/// EventSystem����̑I����Ԃ̕ύX���̏���
	/// </summary>
	/// <param name="flag">�l</param>
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