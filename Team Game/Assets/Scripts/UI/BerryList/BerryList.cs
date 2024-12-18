using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;
using VContainer;

public class BerryList : MonoBehaviour {
	[Inject] private ISubscriber<byte, Berry.BerryMessage> _berrySubscriber = null;

	private ReactiveProperty<bool>[] _takenRP = new ReactiveProperty<bool>[] {
		new(), new(), new()
	};

	public ReadOnlyReactiveProperty<bool> TakenRP(byte id) {
		if (id >= _takenRP.Length) {
			return null;
		}

		return _takenRP[id];
	}

	public bool GetTakenRPValue(byte id) {
		if (id >= _takenRP.Length) {
			return false;
		}

		return _takenRP[id].Value;
	}

	public void SetTakenRPValue(byte id, bool value) {
		if (id >= _takenRP.Length) {
			return;
		}

		_takenRP[id].Value = value;
	}

	private void Awake() {
		for (byte i = 0; i < _takenRP.Length; ++i) {
			_berrySubscriber.Subscribe(i, _ => SetTakenRPValue(i, true))
				.AddTo(this.GetCancellationTokenOnDestroy());
		}
	}
}