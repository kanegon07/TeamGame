using Cysharp.Threading.Tasks;
using MessagePipe;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using VContainer;

// 選択フッククラス
// このクラスに登録されたオブジェクトは、他のものが登録されるまで選択解除されない
// ゲームパッドの場合、選択が外れると他のオブジェクトを
// 選択し直せないせいで詰みかねないので、それの対策用
public class SelectionHook : MonoBehaviour {
	// 登録解除を求めるメッセージ
	public struct UnhookMessage { }

	[Inject] private ISubscriber<UnhookMessage> _unhookSubscriber = null;

	private GameObject _prevSelected = null;

	/// <summary>
	/// EventSystemのオブジェクト選択を待つコルーチン
	/// オブジェクトの選択状態が外れないよう監視し、
	/// 違うものが選択されたらそちらをフックする
	/// </summary>
	/// <returns></returns>
	private IEnumerator RestrictSelection() {
		while (true) {
			GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

			yield return new WaitUntil(
				() => currentSelected != _prevSelected
			);

			if (currentSelected == null) {
				if (_prevSelected == null) {
					continue;
				}

				EventSystem.current.SetSelectedGameObject(_prevSelected);
			} else {
				_prevSelected = currentSelected;
			}
		}
	}

	/// <summary>
	/// 現在のシーンが終了したときの処理
	/// オブジェクトの登録を解除する
	/// </summary>
	private void ResetSelection() {
		_prevSelected = null;
		EventSystem.current.SetSelectedGameObject(null);
	}

	private void Awake() {
		_unhookSubscriber.Subscribe(_ => ResetSelection()).AddTo(this.GetCancellationTokenOnDestroy());
	}

	private void Start() {
		StartCoroutine(RestrictSelection());
	}
}