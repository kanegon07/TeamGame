using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

[RequireComponent(typeof(AudioSource))]
public class TitleEvents : MonoBehaviour {
	public enum WindowID : byte {
		Main,
		StageSelector
	}

	public enum ButtonID : byte {
		Start,
		Stage1,
		Stage2,
		Stage3
	}

	[Inject] private IPublisher<byte, Window.DisplayMessage> _displayPublisher = null;
	[Inject] private IPublisher<byte, Window.ActivateMessage> _activatePublisher = null;

	[Inject] private IAsyncPublisher<WipeEffectController.WipeMessage> _wipeAsyncPublisher = null;

	[Inject] private ISubscriber<byte, CustomButton.PressMessage> _pressSubscriber = null;
	[Inject] private ISubscriber<byte, CustomButton.CancelMessage> _cancelSubscriber = null;

	private AudioSource _audioSource = null;

	private void OpenStageSelector() {
		_displayPublisher.Publish((byte)WindowID.StageSelector, new Window.DisplayMessage(true));

		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(false));
		_activatePublisher.Publish((byte)WindowID.StageSelector, new Window.ActivateMessage(true));
	}

	private void CloseStageSelector() {
		_activatePublisher.Publish((byte)WindowID.StageSelector, new Window.ActivateMessage(false));
		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(true));

		_displayPublisher.Publish((byte)WindowID.StageSelector, new Window.DisplayMessage(false));
	}

	private async UniTask Wipe(bool wipesOut)
		=> await _wipeAsyncPublisher.PublishAsync(
			new WipeEffectController.WipeMessage(wipesOut),
			this.GetCancellationTokenOnDestroy()
		);

	private async void TransitScene(string nextScene) {
		_audioSource.Stop();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		_activatePublisher.Publish((byte)WindowID.StageSelector, new Window.ActivateMessage(false));

		await Wipe(true);

		SceneManager.LoadScene(nextScene);
	}

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();

		_pressSubscriber.Subscribe((byte)ButtonID.Start, _ => OpenStageSelector())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_pressSubscriber.Subscribe((byte)ButtonID.Stage1, _ => TransitScene("FujiiScene"))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_pressSubscriber.Subscribe((byte)ButtonID.Stage2, _ => TransitScene("Stage2"))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_pressSubscriber.Subscribe((byte)ButtonID.Stage3, _ => TransitScene("Stage3"))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_cancelSubscriber.Subscribe((byte)ButtonID.Stage1, _ => CloseStageSelector())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_cancelSubscriber.Subscribe((byte)ButtonID.Stage2, _ => CloseStageSelector())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_cancelSubscriber.Subscribe((byte)ButtonID.Stage3, _ => CloseStageSelector())
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