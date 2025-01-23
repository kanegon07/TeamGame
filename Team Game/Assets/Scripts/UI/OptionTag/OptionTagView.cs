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
	/// �\����Ԃ̕ύX���̏���
	/// </summary>
	/// <param name="flag">�l</param>
	private void OnDisplay(bool flag) {
		// �\����Ԃ�ύX
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