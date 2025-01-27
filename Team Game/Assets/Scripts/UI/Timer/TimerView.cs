using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour {
	[SerializeField] private Color NormalColor = Color.black;
	[SerializeField] private Color WarningColor = Color.black;
	[SerializeField] private Color DangerColor = Color.black;

	private Image _fillImage = null;
	private TMP_Text _text = null;
	private Timer _timer = null;

	private void Awake() {
		_text = GetComponentInChildren<TMP_Text>();
		_timer = GetComponentInParent<Timer>();
	}

	private void Start() {
		_fillImage = transform.Find("Color").GetComponent<Image>();
		_timer.RemainingRP.Subscribe(x => _text.text = Mathf.CeilToInt(x).ToString("D3"));
	}

	private void Update() {
		float amount = _timer.Remaining / _timer.Max;

		if (amount <= 0.25F) {
			_fillImage.color = DangerColor;
		} else if (amount <= 0.5F) {
			_fillImage.color = WarningColor;
		} else {
			_fillImage.color = NormalColor;
		}

		_fillImage.fillAmount = _timer.Remaining / _timer.Max;
	}
}