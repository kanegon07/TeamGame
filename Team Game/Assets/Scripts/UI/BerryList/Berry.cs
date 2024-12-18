using MessagePipe;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Berry : MonoBehaviour {
	public struct BerryMessage { }

	[SerializeField] private byte ID = 0;

	[Inject] private IPublisher<byte, BerryMessage> _berryPublisher = null;

	private MeshRenderer _renderer = null;
	private SphereCollider _collider = null;

	private void Awake() {
		_renderer = GetComponent<MeshRenderer>();
		_collider = GetComponent<SphereCollider>();
	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("Player")) {
			_berryPublisher.Publish(ID, new BerryMessage());
			_renderer.enabled = false;
			_collider.enabled = false;
		}
	}
}