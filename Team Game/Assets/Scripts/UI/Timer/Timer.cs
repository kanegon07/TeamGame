using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;
using VContainer;

public class Timer : MonoBehaviour {
	[Inject] private readonly IPublisher<int> _eventPublisher = null;

	[Inject] private readonly ISubscriber<StageInfo> _stageInfoSubscriber = null;

	[Inject] private readonly ISubscriber<GameEvents.ReserMessage> _resetSubscriber = null;

	private readonly ReactiveProperty<float> _remainingRP = new(0);

	private float _time = 0F;

	public float Max { get; set; } = 0F;

	public ReadOnlyReactiveProperty<float> RemainingRP => _remainingRP;

	public float Remaining {
		get { return _remainingRP.Value; }
		set { _remainingRP.Value = value; }
	}

	private void Initialize(int timeLimit) {
		Remaining = Max = timeLimit;
	}

	private void ResetTime() {
		Remaining = Max;
		_time = 0F;
	}

	private void Awake() {
		_stageInfoSubscriber.Subscribe(x => Initialize(x.TimeLimit))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_resetSubscriber.Subscribe(_ => ResetTime())
			.AddTo(this.GetCancellationTokenOnDestroy());
	}

	private void FixedUpdate() {
		if (Remaining <= 0F) {
			_eventPublisher.Publish((int)GameEvents.EventID.Miss);
		}

		Remaining = Max - _time;

		_time += Time.fixedDeltaTime;
	}
}