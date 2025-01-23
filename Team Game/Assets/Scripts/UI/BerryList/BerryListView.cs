using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BerryList))]
public class BerryListView : MonoBehaviour {
	private GameObject _icon = null;
	private BerryList _list = null;

	private void ReflectValue(int id, bool flag) {
		if (id > _list.Count) {
			return;
		}

		if (flag) {
			transform.GetChild(id - 1).GetComponent<Image>().color = new Color(1F, 1F, 1F, 1F);
		} else {
			transform.GetChild(id - 1).GetComponent<Image>().color = new Color(1F, 1F, 1F, 0.5F);
		}
	}

	private void Awake() {
		_icon = Resources.Load<GameObject>("BerryIcon");
		_list = GetComponent<BerryList>();
	}

	private void Initialize(int count) {
		RectTransform rectTransform = GetComponent<RectTransform>();

		rectTransform.sizeDelta = new Vector2(
			80F * count,
			rectTransform.sizeDelta.y
		);

		for (int i = 0; i < count; ++i) {
			RectTransform iconRect = Instantiate(
				_icon,
				transform
			).GetComponent<RectTransform>();

			iconRect.anchoredPosition = new Vector2(
				-40F * (_list.Count - 1) + 80F * i,
				0F
			);

			_list.TakenRP(i).Subscribe(x => ReflectValue(i, x))
				.AddTo(this.GetCancellationTokenOnDestroy());
		}
	}

	private void Start() {
		_list.CountRP.Subscribe(x => Initialize(x))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}
}