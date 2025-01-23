using MessagePipe;
using R3;
using UnityEngine;
using VContainer;

public class Timer : MonoBehaviour {
	[SerializeField] private int TimeLimit = 0;

	[Inject] private readonly IPublisher<int> _eventPublisher = null;

	private readonly ReactiveProperty<float> _remainingRP = new(0);

	private float _time = 0F;

	public float Max => TimeLimit;

	public ReadOnlyReactiveProperty<float> RemainingRP => _remainingRP;

	public float Remaining {
		get { return _remainingRP.Value; }
		set { _remainingRP.Value = value; }
	}

	private void Start() {
		Remaining = Max;
	}

	private void FixedUpdate() {
		if (Remaining <= 0F) {
			_eventPublisher.Publish((int)GameEvents.EventID.Miss);
		}

		Remaining = TimeLimit - _time;

		_time += Time.fixedDeltaTime;
	}
}