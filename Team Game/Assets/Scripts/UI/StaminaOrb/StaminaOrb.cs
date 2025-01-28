using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(SphereCollider))]
public class StaminaOrb : MonoBehaviour {
	[Inject] private readonly ISubscriber<GameEvents.ReserMessage> _resetSubscriber = null;

	private CameraPlayer _player = null;

	private Animator _animator = null;
	private SphereCollider _collider = null;
	private MeshRenderer[] _meshRenderers = null;

	private GameObject _particleSystem = null;

	private void SetEnable(bool flag) {
		_animator.enabled = flag;
		_collider.enabled = flag;
		_particleSystem.SetActive(flag);

		foreach (MeshRenderer renderer in _meshRenderers) {
			renderer.enabled = flag;
		}
	}

	private void Awake() {
		_animator = GetComponentInChildren<Animator>();
		_collider = GetComponent<SphereCollider>();
		_particleSystem = transform.Find("health/Particle System").gameObject;

		_meshRenderers = GetComponentsInChildren<MeshRenderer>();

		_resetSubscriber.Subscribe(_ => SetEnable(true))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}

	private void Start() {
		GameObject playerObj = GameObject.Find("Player_0");
		_player = playerObj.GetComponent<CameraPlayer>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_player.PlayerStaminaRec(10F);
			SetEnable(false);
		}
	}
}