using MessagePipe;
using UnityEngine;
using VContainer;

public class Halfway : MonoBehaviour {
	[Inject] private readonly IPublisher<Vector3> _halfwayPublisher = null;

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_halfwayPublisher.Publish(transform.position);
		}
	}
}