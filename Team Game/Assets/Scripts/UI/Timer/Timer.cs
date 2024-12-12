using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Timer : MonoBehaviour {
	[SerializeField] private int TimeLimit = 0;

	private readonly ReactiveProperty<int> _remainingRP = new(0);

	private Slider _slider = null;
	private float _time = 0F;

	public ReadOnlyReactiveProperty<int> RemainingRP => _remainingRP;

	public int Remaining {
		get { return _remainingRP.Value; }
		set { _remainingRP.Value = value; }
	}

	private void Awake() {
		_slider = GetComponent<Slider>();
	}

	private void Update() {
		_time += Time.deltaTime;

		Remaining = TimeLimit - (int)_time;
		_slider.value = TimeLimit - _time;
	}
}