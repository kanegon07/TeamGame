using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BerryList))]
public class BerryListView : MonoBehaviour {
	private Image[] _icons = null;
	private Image _backGround = null;
	private BerryList _list = null;

	private void ReflectValue(byte id, bool flag) {
		if (flag) {
			_icons[id].color = new(1F, 1F, 1F, 1F);
		} else {
			_icons[id].color = new(0.5F, 0.5F, 0.5F, 0.5F);
		}
	}

	private void Awake() {


		for (byte i = 0; i < _icons.Length; ++i) {
			_icons[i] = transform.GetChild(1).GetChild(i).GetComponent<Image>();
		}

		_list = GetComponent<BerryList>();
	}

	private void Start() {
		_list.TakenRP(0).Subscribe(x => ReflectValue(0, x))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_list.TakenRP(1).Subscribe(x => ReflectValue(1, x))
			.AddTo(this.GetCancellationTokenOnDestroy());

		_list.TakenRP(2).Subscribe(x => ReflectValue(2, x))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}
}