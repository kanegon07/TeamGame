using R3;
using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviour {
	private TMP_Text _text = null;
	private Timer _timer = null;

	private void Awake() {
		_text = GetComponentInChildren<TMP_Text>();
		_timer = GetComponentInParent<Timer>();
	}

	private void Start() {
		_timer.RemainingRP.Subscribe(x => _text.text = x.ToString("D2"));
	}
}