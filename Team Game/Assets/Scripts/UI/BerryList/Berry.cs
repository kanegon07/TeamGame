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

	[SerializeField] private byte ID = 0;

	[Inject] private readonly IPublisher<BerryMessage> _berryPublisher = null;

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_berryPublisher.Publish(new BerryMessage(ID));
			gameObject.SetActive(false);
		}
	}
}