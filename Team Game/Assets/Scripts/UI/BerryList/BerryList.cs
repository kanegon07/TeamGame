using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(AudioSource))]
public class BerryList : MonoBehaviour {
	public struct UnlockMessage { }

	[SerializeField] private AudioClip GetSE = null;

	[Inject] private readonly IPublisher<UnlockMessage> _unlockPublisher = null;

	[Inject] private readonly ISubscriber<StageInfo> _stageInfoSubscriber = null;

	[Inject] private readonly ISubscriber<Berry.BerryMessage> _berrySubscriber = null;

	private int _count = 0;

	private bool[] _state = null;

	private AudioSource _audioSource = null;

	private GameObject _icon = null;

	public void CheckState() {
		foreach (bool flag in _state) {
			if (!flag) {
				return;
			}
		}

		_unlockPublisher.Publish(new UnlockMessage());
	}

	public void ReflectValue(int id, bool value) {
		if (id >= _count) {
			return;
		}

		_state[id] = value;

		if (value) {
			_audioSource.PlayOneShot(GetSE);
			transform.GetChild(id).GetComponent<Image>().color = new Color(1F, 1F, 1F, 1F);
			CheckState();
		} else {
			transform.GetChild(id).GetComponent<Image>().color = new Color(1F, 1F, 1F, 0.5F);
		}
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

	private void Awake() {
		_icon = Resources.Load<GameObject>("BerryIcon");

		_stageInfoSubscriber.Subscribe(x => Initialize(x.BerryCount))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_berrySubscriber.Subscribe(x => ReflectValue(x.BerryID, true))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_audioSource = GetComponent<AudioSource>();
	}
}