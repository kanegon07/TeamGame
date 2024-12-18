using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BerryList))]
public class BerryListView : MonoBehaviour {
	private Image[] _images = new Image[]{
		null, null, null
	};
	private BerryList _list = null;

	private void ReflectValue(byte id, bool flag) {
		if (flag) {
			_images[id].color = new(1F, 1F, 1F, 1F);
		} else {
			_images[id].color = new(0.5F, 0.5F, 0.5F, 0.5F);
		}
	}

	private void Awake() {
		for (byte i = 0; i < _images.Length; ++i) {
			_images[i] = transform.GetChild(1).GetChild(i).GetComponent<Image>();
		}

		_list = GetComponent<BerryList>();
	}

	private void Start() {
		for (byte i = 0; i < _images.Length; ++i) {
			_list.TakenRP(i).Subscribe(x => ReflectValue(i, x))
				.AddTo(this.GetCancellationTokenOnDestroy());
		}
	}
}