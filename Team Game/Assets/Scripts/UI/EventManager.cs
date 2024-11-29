using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {
	//イベント待ちを記録するDictionary
	private Dictionary <byte, UnityEvent> _eventDictionary = new();

	//シングルトン化処理
	private static EventManager _instance = null;

	public static EventManager Instance {
		get {
			if (!_instance) {
				_instance = FindAnyObjectByType(typeof(EventManager)) as EventManager;

				if (!_instance) {
					Debug.LogError("このシーン内に有効なEventManagerが存在していません！");
				} else {
					_instance.Init();
				}
			}

			return _instance;
		}
	}

	//イベントスタンバイ開始
	//listerに関数名を渡すことで、
	//eventNameイベントがTrigger（下記）されると関数が呼び出されます
	public static void StartListening(byte eventID, UnityAction listener) {
		if (Instance._eventDictionary.TryGetValue(eventID, out UnityEvent thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new();
			thisEvent.AddListener(listener);
			Instance._eventDictionary.Add(eventID, thisEvent);
		}
	}

	//オブジェクトを破棄するときは
	//これでスタンバイ状態を解かないと
	//スタンバイ記録がずっと堆積していってしまいます
	public static void StopListening(byte eventID, UnityAction listener) {
		if (_instance == null) {
			return;
		}

		if (Instance._eventDictionary.TryGetValue(eventID, out UnityEvent thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}

	//イベントをTriggerします
	//AddListnerで登録していた関数全てが呼び出されます
	public static void TriggerEvent(byte eventID) {
		if (Instance._eventDictionary.TryGetValue(eventID, out UnityEvent thisEvent)) {
			thisEvent.Invoke();
		}
	}

	private void Init() {
		_eventDictionary ??= new();

		DontDestroyOnLoad(gameObject);
	}
}