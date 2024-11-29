using MessagePipe;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(Rigidbody))]
public class LowerCloud : MonoBehaviour {
	public struct GameOverMessage { }

	[Inject] private IPublisher<GameOverMessage> _gameOverPublisher = null;

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_gameOverPublisher.Publish(new GameOverMessage());
		}
	}
}