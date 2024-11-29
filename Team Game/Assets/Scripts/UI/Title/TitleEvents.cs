using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

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

	[Inject] private ISubscriber<byte, CustomButton.PressMessage> _pressSubscriber = null;
	[Inject] private ISubscriber<byte, CustomButton.CancelMessage> _cancelSubscriber = null;

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

	private void TransitScene(string nextScene) {
		SceneManager.LoadScene(nextScene);
	}

	private void Awake() {
		_pressSubscriber.Subscribe((byte)ButtonID.Start, _ => OpenStageSelector())
			.AddTo(this.GetCancellationTokenOnDestroy());

		_pressSubscriber.Subscribe((byte)ButtonID.Stage1, _ => TransitScene("Stage1"))
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

	private void Start() {
		_displayPublisher.Publish((byte)WindowID.Main, new Window.DisplayMessage(true));
		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(true));

		Time.timeScale = 1F;
	}
}