using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

[RequireComponent(typeof(AudioSource))]
public class ResultEvents : MonoBehaviour {
	public enum WindowID : byte {
		Main,
	}

	public enum ButtonID : byte {
		ReturnToTitle
	}

	[Inject] private IPublisher<byte, Window.DisplayMessage> _displayPublisher = null;
	[Inject] private IPublisher<byte, Window.ActivateMessage> _activatePublisher = null;

	[Inject] private IAsyncPublisher<WipeEffectController.WipeMessage> _wipeAsyncPublisher = null;

	[Inject] private ISubscriber<byte, CustomButton.PressMessage> _pressSubscriber = null;

	private AudioSource _audioSource = null;

	private async UniTask Wipe(bool wipesOut)
		=> await _wipeAsyncPublisher.PublishAsync(
			new WipeEffectController.WipeMessage(wipesOut),
			this.GetCancellationTokenOnDestroy()
		);

	private async void TransitScene(string nextScene) {
		_audioSource.Stop();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(false));

		await Wipe(true);

		SceneManager.LoadScene(nextScene);
	}

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();

		_pressSubscriber.Subscribe((byte)ButtonID.ReturnToTitle, _ => TransitScene("Title"))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}

	private async void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		_displayPublisher.Publish((byte)WindowID.Main, new Window.DisplayMessage(true));

		await Wipe(false);

		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(true));

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}