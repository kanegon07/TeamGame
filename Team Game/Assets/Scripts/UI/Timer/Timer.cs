using MessagePipe;
using R3;
using UnityEngine;
using VContainer;

public class Timer : MonoBehaviour {
	[SerializeField] private int TimeLimit = 0;

	[Inject] private IPublisher<GameEvents.GameOverMessage> _gameOverPublisher = null;

	private readonly ReactiveProperty<float> _remainingRP = new(0);

	private float _time = 0F;

	public float Max => TimeLimit;

	public ReadOnlyReactiveProperty<float> RemainingRP => _remainingRP;

	public float Remaining {
		get { return _remainingRP.Value; }
		set { _remainingRP.Value = value; }
	}

	private void FixedUpdate() {
		Remaining = TimeLimit - _time;

		if (Remaining < 0F) {
			_gameOverPublisher.Publish(new GameEvents.GameOverMessage());
		}

		_time += Time.fixedDeltaTime;
	}
}