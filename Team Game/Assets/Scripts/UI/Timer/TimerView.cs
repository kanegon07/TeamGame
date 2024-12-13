using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour {
	private Image _fillImage = null;
	private TMP_Text _text = null;
	private Timer _timer = null;

	private void Awake() {
		_text = GetComponentInChildren<TMP_Text>();
		_timer = GetComponentInParent<Timer>();
	}

	private void Start() {
		_fillImage = transform.Find("Color").GetComponent<Image>();
		_timer.RemainingRP.Subscribe(x => _text.text = Mathf.FloorToInt(x).ToString("D2"));
	}

	private void Update() {
		_fillImage.fillAmount = _timer.Remaining / _timer.Max;
	}
}