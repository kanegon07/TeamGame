using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class StaminaOrb : MonoBehaviour {
	CameraPlayer _player = null;

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_player.PlayerStaminaRec(10F);
			gameObject.SetActive(false);
		}
	}

	private void Start() {
		GameObject playerObj = GameObject.Find("Player_0");
		_player = playerObj.GetComponent<CameraPlayer>();
	}
}