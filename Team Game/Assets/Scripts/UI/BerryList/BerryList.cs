using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(AudioSource))]
public class BerryList : MonoBehaviour {
	[SerializeField] private AudioClip GetSE = null;

	[Inject] private readonly ISubscriber<StageInfo> _stageInfoSubscriber = null;

	[Inject] private readonly ISubscriber<Berry.BerryMessage> _berrySubscriber = null;

	private readonly ReactiveProperty<bool>[] _takenRP = null;

	private AudioSource _audioSource = null;

	public ReadOnlyReactiveProperty<bool> TakenRP(byte id) {
		if (id >= _takenRP.Length) {
			return null;
		}

		return _takenRP[id];
	}

	public void SetTakenRPValue(byte id, bool value) {
		if (id >= _takenRP.Length) {
			return;
		}

		_takenRP[id].Value = value;

		if (value) {
			_audioSource.PlayOneShot(GetSE);
		}
	}

	private void Initialize(int count) {

	}

	private void Awake() {
		_stageInfoSubscriber.Subscribe(x => Initialize(x.BerryCount));

		_berrySubscriber.Subscribe(x => SetTakenRPValue(x.BerryID, true))
				.AddTo(this.GetCancellationTokenOnDestroy());

		_audioSource = GetComponent<AudioSource>();
	}
}