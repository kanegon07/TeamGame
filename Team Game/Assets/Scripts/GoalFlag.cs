using MessagePipe;
using System;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(Rigidbody))]
public class GoalFlag : MonoBehaviour {
	public struct GoalMessage : IEquatable<GoalMessage> {
		public string NextScene;

		public GoalMessage(string nextScene) {
			NextScene = nextScene;
		}

		public bool Equals(GoalMessage other) {
			return NextScene == other.NextScene;
		}
	}

	[SerializeField] private string _nextScene = "";

	[Inject] private IPublisher<GoalMessage> _goalPublisher = null;

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_goalPublisher.Publish(new GoalMessage(_nextScene));
		}
	}
}