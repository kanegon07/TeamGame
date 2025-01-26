using MessagePipe;
using VContainer;
using UnityEngine;
using System;

public class Sign : MonoBehaviour {
	public struct TutorialMessage : IEquatable<TutorialMessage> {
		public bool Flag;
		public string Message;

		public TutorialMessage(bool flag, string message) {
			Flag = flag;
			Message = message;
		}

		public readonly bool Equals(TutorialMessage other)
			=> Flag == other.Flag && Message == other.Message;
	}

	[SerializeField] private int ID = -1;

	[Inject] private readonly IPublisher<TutorialMessage> _tutorialPublisher = null;

	private SignMessage _message = null;

	private void Awake() {
		_message = Resources.Load<SignMessageTable>("SignMessageTable").Find(ID);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_tutorialPublisher.Publish(new TutorialMessage(true, _message.Message));
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			_tutorialPublisher.Publish(new TutorialMessage(false, null));
		}
	}
}