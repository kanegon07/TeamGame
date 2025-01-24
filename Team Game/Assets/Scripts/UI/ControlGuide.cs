using Cysharp.Threading.Tasks;
using MessagePipe;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ControlGuide : MonoBehaviour {
	[SerializeField] private int WindowID = -1;

	[Inject] private readonly ISubscriber<int, Window.StateMessage> _stateSubscriber = null;

	private Image[] _images = null;
	private TMP_Text[] _texts = null;

	private void Awake() {
		_images = GetComponentsInChildren<Image>();
		_texts = GetComponentsInChildren<TMP_Text>();

		_stateSubscriber.Subscribe(WindowID, x => {
			if (x.Value == Window.StateMessage.State.Hiding) {
				foreach (var image in _images) {
					image.enabled = false;
				}

				foreach (var text in _texts) {
					text.enabled = false;
				}
			} else {
				foreach (var image in _images) {
					image.enabled = true;
				}

				foreach (var text in _texts) {
					text.enabled = true;
				}
			}
		}).AddTo(this.GetCancellationTokenOnDestroy());
	}
}