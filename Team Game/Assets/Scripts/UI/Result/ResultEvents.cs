using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

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

	private async UniTask Wipe(bool wipesOut)
		=> await _wipeAsyncPublisher.PublishAsync(
			new WipeEffectController.WipeMessage(wipesOut),
			this.GetCancellationTokenOnDestroy()
		);

	private async void TransitScene(string nextScene) {
		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(false));

		Time.timeScale = 0F;

		await Wipe(true);

		SceneManager.LoadScene(nextScene);
	}

	private void Awake() {
		_pressSubscriber.Subscribe((byte)ButtonID.ReturnToTitle, _ => TransitScene("Title"))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}

	private async void Start() {
		_displayPublisher.Publish((byte)WindowID.Main, new Window.DisplayMessage(true));

		await Wipe(false);

		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(true));

		Time.timeScale = 1F;
	}
}