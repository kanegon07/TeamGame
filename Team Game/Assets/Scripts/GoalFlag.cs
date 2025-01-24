using MessagePipe;
using System;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(Rigidbody))]
public class GoalFlag : MonoBehaviour {
	[Inject] private readonly IPublisher<int> _eventPublisher = null;

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_eventPublisher.Publish((int)GameEvents.EventID.Clear);
		}
	}
}