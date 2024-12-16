using MessagePipe;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(MeshCollider))]
public class LowerCloud : MonoBehaviour {
	[Inject] private IPublisher<GameEvents.GameOverMessage> _gameOverPublisher = null;

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Player")) {
			_gameOverPublisher.Publish(new GameEvents.GameOverMessage());
		}
	}
}