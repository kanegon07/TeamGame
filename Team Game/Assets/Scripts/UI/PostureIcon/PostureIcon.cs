using R3;
using UnityEngine;

public class PostureIcon : MonoBehaviour {
	public enum Posture {
		Idle,
		Gliding,
		Sticking
	}

	[SerializeField] CameraPlayer Player = null;

	private readonly ReactiveProperty<Posture> _postureRP = new(Posture.Idle);

	public ReadOnlyReactiveProperty<Posture> PostureRP => _postureRP;

	public Posture CurrentPosture {
		get { return _postureRP.Value; }
		set { _postureRP.Value = value; }
	}

	private void FixedUpdate() {
		if (Player.FlyFlg) {
			CurrentPosture = Posture.Gliding;
		} else if (Player.StickWall) {
			CurrentPosture = Posture.Sticking;
		} else {
			CurrentPosture = Posture.Idle;
		}
	}
}