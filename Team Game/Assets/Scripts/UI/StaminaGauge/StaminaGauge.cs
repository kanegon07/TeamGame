using R3;
using UnityEngine;

public class StaminaGauge : MonoBehaviour {
	[SerializeField] private CameraPlayer Player = null;

	private readonly ReactiveProperty<float> _staminaRP = new(0F);

	public ReadOnlyReactiveProperty<float> StaminaRP => _staminaRP;

	public float Stamina {
		get { return _staminaRP.Value; }
		set { _staminaRP.Value = value; }
	}

	private void FixedUpdate() {
		_staminaRP.Value = Player.Player_Stamina;
	}
}