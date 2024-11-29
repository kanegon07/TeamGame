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

	[Inject] private ISubscriber<byte, CustomButton.PressMessage> _pressSubscriber = null;

	private void ReturnToTitle() {
		SceneManager.LoadScene("Title");
	}

	private void Awake() {
		_pressSubscriber.Subscribe((byte)ButtonID.ReturnToTitle, _ => ReturnToTitle())
			.AddTo(this.GetCancellationTokenOnDestroy());
	}

	private void Start() {
		_displayPublisher.Publish((byte)WindowID.Main, new Window.DisplayMessage(true));
		_activatePublisher.Publish((byte)WindowID.Main, new Window.ActivateMessage(true));

		Time.timeScale = 1F;
	}
}