using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(AudioSource))]
public class BerryList : MonoBehaviour, IRequestHandler<bool, bool[]> {
	[Inject] private readonly IPublisher<bool> _unlockPublisher = null;

	[Inject] private readonly IPublisher<int, bool> _berryPublisher = null;

	[Inject] private readonly ISubscriber<StageInfo> _stageInfoSubscriber = null;

	[Inject] private readonly ISubscriber<Berry.BerryMessage> _berrySubscriber = null;

	[Inject] private readonly ISubscriber<bool[]> _berryResetSubscriber = null;

	private int _count = 0;

	private bool[] _state = null;

	private GameObject _icon = null;

	public void CheckState() {
		foreach (bool flag in _state) {
			if (!flag) {
				_unlockPublisher.Publish(false);
				return;
			}
		}

		_unlockPublisher.Publish(true);
	}

	public void ReflectValue(int id, bool value) {
		if (id >= _count) {
			return;
		}

		_state[id] = value;

		if (value) {
			transform.GetChild(id).GetComponent<Image>().color = new Color(1F, 1F, 1F, 1F);
		} else {
			transform.GetChild(id).GetComponent<Image>().color = new Color(1F, 1F, 1F, 0.5F);
		}

		CheckState();
	}

	private void Initialize(int count) {
		_count = count;

		RectTransform rectTransform = GetComponent<RectTransform>();

		rectTransform.sizeDelta = new Vector2(
			80F * count,
			rectTransform.sizeDelta.y
		);

		for (int i = 0; i < count; ++i) {
			RectTransform iconRect = Instantiate(
				_icon,
				transform
			).GetComponent<RectTransform>();

			iconRect.anchoredPosition = new Vector2(
				-40F * (_count - 1) + 80F * i,
				0F
			);
		}

		_state = new bool[count];
	}

	private void ResetState(bool[] state) {
		for (int i = 0; i < _state.Length; ++i) {
			_berryPublisher.Publish(i, state[i]);
			ReflectValue(i, state[i]);
		}
	}

	public bool[] Invoke(bool fills) {
		if (fills) {
			for (int i = 0; i < _state.Length; ++i) {
				ReflectValue(i, true);
			}
		}

		return _state;
	}

	private void Awake() {
		_icon = Resources.Load<GameObject>("BerryIcon");

		_stageInfoSubscriber.Subscribe(x => Initialize(x.BerryCount))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_berrySubscriber.Subscribe(x => ReflectValue(x.BerryID, true))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_berryResetSubscriber.Subscribe(x => ResetState(x))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}
}