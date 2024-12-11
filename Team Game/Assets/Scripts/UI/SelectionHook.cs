using Cysharp.Threading.Tasks;
using MessagePipe;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using VContainer;

/// <summary>
/// 選択フッククラス
/// このクラスに登録されたオブジェクトは、他のものが登録されるまで選択解除されない
/// ゲームパッドの場合、選択が外れると他のオブジェクトを
/// 選択し直せずに詰むので、それの対策用
/// </summary>
public class SelectionHook : MonoBehaviour {
	// 登録解除を求めるメッセージ
	public struct UnhookMessage { }

	// 最後に選択されたオブジェクト
	public GameObject PrevSelected { get; set; } = null;

	[Inject] private ISubscriber<UnhookMessage> _unhookSubscriber = null;

	/// <summary>
	/// EventSystemのオブジェクト選択を待つコルーチン
	/// オブジェクトの選択状態が外れないよう監視し、
	/// 違うものが選択されたらそちらをフックする
	/// </summary>
	/// <returns></returns>
	private IEnumerator RestrictSelection() {
		while (true) {
			yield return new WaitUntil(
				() => EventSystem.current.currentSelectedGameObject == null
			);

			if (PrevSelected == null) {
				continue;
			}

			EventSystem.current.SetSelectedGameObject(PrevSelected);
		}
	}

	/// <summary>
	/// 現在のシーンが終了したときの処理
	/// オブジェクトの登録を解除する
	/// </summary>
	private void ResetSelection() => PrevSelected = null;

	private void Awake() {
		_unhookSubscriber.Subscribe(_ => ResetSelection()).AddTo(this.GetCancellationTokenOnDestroy());
	}

	private void Start() {
		StartCoroutine(RestrictSelection());

		SceneManager.sceneUnloaded += _ => ResetSelection();
	}
}