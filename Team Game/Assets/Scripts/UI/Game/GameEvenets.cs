using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class GameEvents : MonoBehaviour {
	public struct GameOverMessage { }

	public enum WindowID : byte {
		Main,
		Option
	}

	public enum ButtonID : byte {
		Resume,
		Title
	}

	[SerializeField] private CameraPlayer Player = null;
	[SerializeField] private FPSCamera Camera = null;
	[SerializeField] private DebugMouse MouseLock = null;

	[Inject] private IPublisher<bool> _optionStatePublisher = null;
	[Inject] private IPublisher<byte, Window.DisplayMessage> _displayPublisher = null;
	[Inject] private IPublisher<byte, Window.ActivateMessage> _activatePublisher = null;

	[Inject] private IAsyncPublisher<WipeEffectController.WipeMessage> _wipeAsyncPublisher = null;

	[Inject] private ISubscriber<OptionTag.OptionMessage> _optionSubscriber = null;
	[Inject] private ISubscriber<byte, CustomButton.PressMessage> _pressSubscriber = null;
	[Inject] private ISubscriber<byte, CustomButton.CancelMessage> _cancelSubscriber = null;
	[Inject] private ISubscriber<GoalFlag.GoalMessage> _goalSubscriber = null;
	[Inject] private ISubscriber<GameOverMessage> _gameOverSubscriber = null;

	private void OpenOption() {
		Time.timeScale = 0F;

		Player.enabled = false;
		Camera.enabled = false;

		_displayPublisher.Publish((byte)WindowID.Option, new Window.DisplayMessage(true));

		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(false));
		_activatePublisher.Publish((byte)WindowID.Option, new Window.ActivateMessage(true));

		MouseLock.Lock(false);

		_optionStatePublisher.Publish(true);
	}

	private void CloseOption() {
		_activatePublisher.Publish((byte)WindowID.Option, new Window.ActivateMessage(false));
		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(true));

		_displayPublisher.Publish((byte)WindowID.Option, new Window.DisplayMessage(false));

		Player.enabled = true;
		Camera.enabled = true;

		Time.timeScale = 1F;

		MouseLock.Lock(true);

		_optionStatePublisher.Publish(false);
	}

	private async UniTask Wipe(bool wipesOut)
		=> await _wipeAsyncPublisher.PublishAsync(
			new WipeEffectController.WipeMessage(wipesOut),
			this.GetCancellationTokenOnDestroy()
		);

	private async void TransitScene(string nextScene) {
		MouseLock.Lock(true);

		Player.enabled = false;
		Camera.enabled = false;

		await Wipe(true);

		SceneManager.LoadScene(nextScene);
	}

	private void Awake() {
		_optionSubscriber.Subscribe(x => {
			if (x.Value) {
				OpenOption();
			} else {
				CloseOption();
			}
		}).AddTo(this.GetCancellationTokenOnDestroy());

		_pressSubscriber.Subscribe((byte)ButtonID.Resume, _ => CloseOption())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_pressSubscriber.Subscribe((byte)ButtonID.Title, _ => TransitScene("Title"))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_cancelSubscriber.Subscribe((byte)ButtonID.Resume, _ => CloseOption())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_cancelSubscriber.Subscribe((byte)ButtonID.Title, _ => CloseOption())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_goalSubscriber.Subscribe(x => TransitScene(x.NextScene)).AddTo(this.GetCancellationTokenOnDestroy());

		_gameOverSubscriber.Subscribe(x => TransitScene(SceneManager.GetActiveScene().name));
	}

	private async void Start() {
		MouseLock.Lock(true);

		Player.enabled = false;
		Camera.enabled = false;

		_displayPublisher.Publish((byte)WindowID.Main, new Window.DisplayMessage(true));

		await Wipe(false);

		Player.enabled = true;
		Camera.enabled = true;

		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(true));

		SceneManager.sceneUnloaded += (_) => {
			Player = null;
			Camera = null;
			MouseLock = null;
		};
	}
}