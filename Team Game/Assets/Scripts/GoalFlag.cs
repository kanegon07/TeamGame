using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class GoalFlag : MonoBehaviour {
	[SerializeField] private AudioClip UnlockSE = null;

	[Inject] private readonly IPublisher<int> _eventPublisher = null;

	[Inject] private readonly ISubscriber<bool> _unlockSubscriber = null;

	private AudioSource _audioSource = null;
	private CapsuleCollider _collider = null;
	private GameObject _effect = null;

	private void Unlock(bool flag) {
		_collider.enabled = flag;
		_effect.SetActive(flag);
		if (flag) {
			_audioSource.PlayOneShot(UnlockSE);
		}
	}

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();
		_collider = GetComponent<CapsuleCollider>();
		_effect = transform.Find("Godray").gameObject;

		_effect.SetActive(false);

		_unlockSubscriber.Subscribe(x => Unlock(x))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_eventPublisher.Publish((int)GameEvents.EventID.Clear);
		}
	}
}