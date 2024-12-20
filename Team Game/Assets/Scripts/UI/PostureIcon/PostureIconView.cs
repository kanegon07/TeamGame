using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(PostureIcon))]
public class PostureIconView : MonoBehaviour {
	[SerializeField] private Sprite[] Sprites = null;

	private Image _image = null;
	private PostureIcon _icon = null;

	public void ReflectValue(PostureIcon.Posture posture) {
		switch (posture) {
			case PostureIcon.Posture.Idle:
				_image.sprite = Sprites[0];
				break;

			case PostureIcon.Posture.Gliding:
				_image.sprite = Sprites[1];
				break;

			case PostureIcon.Posture.Sticking:
				_image.sprite = Sprites[2];
				break;

			default:
				break;
		}
	}

	private void Awake() {
		_image = GetComponent<Image>();
		_icon = GetComponent<PostureIcon>();
	}

	private void Start() {
		_icon.PostureRP.Subscribe(x => ReflectValue(x))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}
}