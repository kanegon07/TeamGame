using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CustomButton))]
public class CustomButtonView : MonoBehaviour {
	[SerializeField] private Color NormalColor = Color.black;
	[SerializeField] private Color SelectedColor = Color.black;
	[SerializeField] private Color InactiveColor = Color.black;

	private Image _image = null;
	private TMP_Text _text = null;
	private CustomButton _button = null;

	private void OnDisplay(bool flag) {
		_image.enabled = flag;
		if (_text != null) {
			_text.enabled = flag;
		}
	}

	private void OnActivate(bool flag) {
		if (flag) {
			_image.color = NormalColor;
		} else {
			_image.color = InactiveColor;
		}
	}

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