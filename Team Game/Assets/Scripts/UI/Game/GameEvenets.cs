using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class GameEvents : MonoBehaviour {
	public enum WindowID : byte {
		Main,
		Option
	}

	public enum ButtonID : byte {
		Resume,
		Title
	}

	[SerializeField] private CameraPlayer Player = null;

	[Inject] private IPublisher<bool> _optionStatePublisher = null;
	[Inject] private IPublisher<byte, Window.DisplayMessage> _displayPublisher = null;
	[Inject] private IPublisher<byte, Window.ActivateMessage> _activatePublisher = null;

	[Inject] private ISubscriber<OptionTag.OptionMessage> _optionSubscriber = null;
	[Inject] private ISubscriber<byte, CustomButton.PressMessage> _pressSubscriber = null;
	[Inject] private ISubscriber<byte, CustomButton.CancelMessage> _cancelSubscriber = null;
	[Inject] private ISubscriber<GoalFlag.GoalMessage> _goalSubscriber = null;
//	[Inject] private ISubscriber<LowerCloud.GameOverMessage> _gameOverSUbscriber = null;

	private void OpenOption() {
		Time.timeScale = 0F;

		Player.enabled = false;

		_displayPublisher.Publish((byte)WindowID.Option, new Window.DisplayMessage(true));

		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(false));
		_activatePublisher.Publish((byte)WindowID.Option, new Window.ActivateMessage(true));

		Cursor.visible = true;

		_optionStatePublisher.Publish(true);
	}

	private void CloseOption() {
		_activatePublisher.Publish((byte)WindowID.Option, new Window.ActivateMessage(false));
		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(true));

		_displayPublisher.Publish((byte)WindowID.Option, new Window.DisplayMessage(false));

		Player.enabled = true;

		Time.timeScale = 1F;

		Cursor.visible = false;

		_optionStatePublisher.Publish(false);
	}

	private void ReturnToTitle() {
		SceneManager.LoadScene("Title");
	}

	private void EnterNextScene(string nextScene) {
		Player.enabled = false;

		Time.timeScale = 0F;

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

		_pressSubscriber.Subscribe((byte)ButtonID.Title, _ => ReturnToTitle())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_cancelSubscriber.Subscribe((byte)ButtonID.Resume, _ => CloseOption())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_cancelSubscriber.Subscribe((byte)ButtonID.Title, _ => CloseOption())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_goalSubscriber.Subscribe(x => EnterNextScene(x.NextScene)).AddTo(this.GetCancellationTokenOnDestroy());
	}

	private void Start() {
		_displayPublisher.Publish((byte)WindowID.Main, new Window.DisplayMessage(true));
		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(true));

		Time.timeScale = 1F;
	}
}