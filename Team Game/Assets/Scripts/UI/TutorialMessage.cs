using Cysharp.Threading.Tasks;
using MessagePipe;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TutorialMessage : MonoBehaviour {
	[Inject] private readonly ISubscriber<Sign.TutorialMessage> _turorialSubscriber = null;

	private Image _background = null;
	private TMP_Text _text = null;

	private void Display(Sign.TutorialMessage message) {
		if (message.Flag) {
			_background.enabled = true;
			_text.enabled = true;

			_text.text = message.Message;
		} else {
			_background.enabled = false;
			_text.enabled = false;

			_text.text = "";
		}
	}

	private void Awake() {
		_background = GetComponent<Image>();
		_text = GetComponentInChildren<TMP_Text>();

		_turorialSubscriber.Subscribe(x => Display(x))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}
}