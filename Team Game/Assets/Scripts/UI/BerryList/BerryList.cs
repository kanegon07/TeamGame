using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using System.Linq;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(AudioSource))]
public class BerryList : MonoBehaviour {
	[SerializeField] private AudioClip GetSE = null;

	[Inject] private readonly ISubscriber<StageInfo> _stageInfoSubscriber = null;

	[Inject] private readonly ISubscriber<Berry.BerryMessage> _berrySubscriber = null;

	private readonly ReactiveProperty<int> _countRP = new(0);

	private ReactiveProperty<bool>[] _takenRP = null;

	private AudioSource _audioSource = null;

	public ReadOnlyReactiveProperty<int> CountRP => _countRP;

	public int Count {
		get { return _countRP.Value; }
		set { _countRP.Value = value; }
	}

	public ReadOnlyReactiveProperty<bool> TakenRP(int id) {
		if (id >= _takenRP.Length) {
			return null;
		}

		return _takenRP[id];
	}

	public void SetTakenRPValue(int id, bool value) {
		if (id >= _takenRP.Length) {
			return;
		}

		_takenRP[id].Value = value;

		if (value) {
			_audioSource.PlayOneShot(GetSE);
		}
	}

	private void Initialize(int count) {
		_takenRP = Enumerable.Repeat(new ReactiveProperty<bool>(false), count).ToArray();

		Count = count;
	}

	private void Awake() {
		_stageInfoSubscriber.Subscribe(x => Initialize(x.BerryCount))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_berrySubscriber.Subscribe(x => SetTakenRPValue(x.BerryID, true))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_audioSource = GetComponent<AudioSource>();
	}
}