using MessagePipe;
using System;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(SphereCollider))]
public class Berry : MonoBehaviour {
	public struct BerryMessage : IEquatable<BerryMessage> {
		public byte BerryID;

		public BerryMessage(byte id) {
			BerryID = id;
		}

		public bool Equals(BerryMessage other)
			=> BerryID == other.BerryID;
	}

	[SerializeField] private byte ID = 0;

	[Inject] private IPublisher<BerryMessage> _berryPublisher = null;

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_berryPublisher.Publish(new BerryMessage(ID));
			gameObject.SetActive(false);
		}
	}
}