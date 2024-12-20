using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {
	//�C�x���g�҂����L�^����Dictionary
	private Dictionary <byte, UnityEvent> _eventDictionary = new();

	//�V���O���g��������
	private static EventManager _instance = null;

	public static EventManager Instance {
		get {
			if (!_instance) {
				_instance = FindAnyObjectByType(typeof(EventManager)) as EventManager;

				if (!_instance) {
					Debug.LogError("���̃V�[�����ɗL����EventManager�����݂��Ă��܂���I");
				} else {
					_instance.Init();
				}
			}

			return _instance;
		}
	}

	//�C�x���g�X�^���o�C�J�n
	//lister�Ɋ֐�����n�����ƂŁA
	//eventName�C�x���g��Trigger�i���L�j�����Ɗ֐����Ăяo����܂�
	public static void StartListening(byte eventID, UnityAction listener) {
		if (Instance._eventDictionary.TryGetValue(eventID, out UnityEvent thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new();
			thisEvent.AddListener(listener);
			Instance._eventDictionary.Add(eventID, thisEvent);
		}
	}

	//�I�u�W�F�N�g��j������Ƃ���
	//����ŃX�^���o�C��Ԃ������Ȃ���
	//�X�^���o�C�L�^�������Ƒ͐ς��Ă����Ă��܂��܂�
	public static void StopListening(byte eventID, UnityAction listener) {
		if (_instance == null) {
			return;
		}

		if (Instance._eventDictionary.TryGetValue(eventID, out UnityEvent thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}

	//�C�x���g��Trigger���܂�
	//AddListner�œo�^���Ă����֐��S�Ă��Ăяo����܂�
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