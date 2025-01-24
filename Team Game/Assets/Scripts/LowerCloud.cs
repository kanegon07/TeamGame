using MessagePipe;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(MeshCollider))]
public class LowerCloud : MonoBehaviour {
	[Inject] private readonly IPublisher<int> _eventPublisher = null;

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("PlayerHit")) {
			_eventPublisher.Publish((int)GameEvents.EventID.Miss);
		}
	}
}