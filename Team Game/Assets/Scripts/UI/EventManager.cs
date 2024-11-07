using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {
	//�C�x���g�҂����L�^����Dictionary
	private Dictionary <byte, UnityEvent> eventDictionary = null;

	//�V���O���g��������
	private static EventManager eventManager = null;

	public static EventManager Instance {
		get {
			if (!eventManager) {
				eventManager = FindAnyObjectByType(typeof(EventManager)) as EventManager;

				if (!eventManager) {
					Debug.LogError("���̃V�[�����ɗL����EventManager�����݂��Ă��܂���I");
				} else {
					eventManager.Init();
				}
			}

			return eventManager;
		}
	}

	//�C�x���g�X�^���o�C�J�n
	//lister�Ɋ֐�����n�����ƂŁA
	//eventName�C�x���g��Trigger�i���L�j�����Ɗ֐����Ăяo����܂�
	public static void StartListening(byte eventID, UnityAction listener) {
		if (Instance.eventDictionary.TryGetValue(eventID, out UnityEvent thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new();
			thisEvent.AddListener(listener);
			Instance.eventDictionary.Add(eventID, thisEvent);
		}
	}

	//�I�u�W�F�N�g��j������Ƃ���
	//����ŃX�^���o�C��Ԃ������Ȃ���
	//�X�^���o�C�L�^�������Ƒ͐ς��Ă����Ă��܂��܂�
	public static void StopListening(byte eventID, UnityAction listener) {
		if (eventManager == null) {
			return;
		}
		if (Instance.eventDictionary.TryGetValue(eventID, out UnityEvent thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}

	//�C�x���g��Trigger���܂�
	//AddListner�œo�^���Ă����֐��S�Ă��Ăяo����܂�
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