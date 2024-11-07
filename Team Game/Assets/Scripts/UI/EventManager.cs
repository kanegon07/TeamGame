using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {
	//イベント待ちを記録するDictionary
	private Dictionary <byte, UnityEvent> eventDictionary = null;

	//シングルトン化処理
	private static EventManager eventManager = null;

	public static EventManager Instance {
		get {
			if (!eventManager) {
				eventManager = FindAnyObjectByType(typeof(EventManager)) as EventManager;

				if (!eventManager) {
					Debug.LogError("このシーン内に有効なEventManagerが存在していません！");
				} else {
					eventManager.Init();
				}
			}

			return eventManager;
		}
	}

	//イベントスタンバイ開始
	//listerに関数名を渡すことで、
	//eventNameイベントがTrigger（下記）されると関数が呼び出されます
	public static void StartListening(byte eventID, UnityAction listener) {
		if (Instance.eventDictionary.TryGetValue(eventID, out UnityEvent thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new();
			thisEvent.AddListener(listener);
			Instance.eventDictionary.Add(eventID, thisEvent);
		}
	}

	//オブジェクトを破棄するときは
	//これでスタンバイ状態を解かないと
	//スタンバイ記録がずっと堆積していってしまいます
	public static void StopListening(byte eventID, UnityAction listener) {
		if (eventManager == null) {
			return;
		}
		if (Instance.eventDictionary.TryGetValue(eventID, out UnityEvent thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}

	//イベントをTriggerします
	//AddListnerで登録していた関数全てが呼び出されます
	public static void TriggerEvent(byte eventID) {
		if (Instance.eventDictionary.TryGetValue(eventID, out UnityEvent thisEvent)) {
			thisEvent.Invoke();
		}
	}

	private void Init() {
		eventDictionary ??= new();

		DontDestroyOnLoad(gameObject);
	}
}