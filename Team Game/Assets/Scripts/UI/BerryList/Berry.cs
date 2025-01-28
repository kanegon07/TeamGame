using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(SphereCollider))]
public class Berry : MonoBehaviour {
	public struct BerryMessage : IEquatable<BerryMessage> {
		public int BerryID;

		public BerryMessage(int id) {
			BerryID = id;
		}

		public readonly bool Equals(BerryMessage other)
			=> BerryID == other.BerryID;
	}

	[SerializeField] private int ID = 0;
	[SerializeField] private AudioClip SE = null;

	[Inject] private readonly IPublisher<BerryMessage> _berryPublisher = null;

	[Inject] private readonly ISubscriber<int, bool> _berrySubscriber = null;

	private Animator _animator = null;
	private SphereCollider _collider = null;
	private AudioSource _audioSource = null;
	private MeshRenderer[] _meshRenderers = null;

	private void SetEnable(bool flag) {
		_animator.enabled = !flag;
		_collider.enabled = !flag;

		foreach (MeshRenderer renderer in _meshRenderers) {
			renderer.enabled = !flag;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_audioSource.PlayOneShot(SE);
			SetEnable(true);
			_berryPublisher.Publish(new BerryMessage(ID));
		}
	}

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();
		_animator = GetComponent<Animator>();
		_collider = GetComponent<SphereCollider>();

		_meshRenderers = GetComponentsInChildren<MeshRenderer>();

		_berrySubscriber.Subscribe(ID, x => SetEnable(x))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}
}