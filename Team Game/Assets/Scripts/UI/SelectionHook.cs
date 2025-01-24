using Cysharp.Threading.Tasks;
using MessagePipe;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using VContainer;

// �I���t�b�N�N���X
// ���̃N���X�ɓo�^���ꂽ�I�u�W�F�N�g�́A���̂��̂��o�^�����܂őI����������Ȃ�
// �Q�[���p�b�h�̏ꍇ�A�I�����O���Ƒ��̃I�u�W�F�N�g��
// �I���������Ȃ������ŋl�݂��˂Ȃ��̂ŁA����̑΍��p
public class SelectionHook : MonoBehaviour {
	// �o�^���������߂郁�b�Z�[�W
	public struct UnhookMessage { }

	[Inject] private ISubscriber<UnhookMessage> _unhookSubscriber = null;

	private GameObject _prevSelected = null;

	/// <summary>
	/// EventSystem�̃I�u�W�F�N�g�I����҂R���[�`��
	/// �I�u�W�F�N�g�̑I����Ԃ��O��Ȃ��悤�Ď����A
	/// �Ⴄ���̂��I�����ꂽ�炻������t�b�N����
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
	/// ���݂̃V�[�����I�������Ƃ��̏���
	/// �I�u�W�F�N�g�̓o�^����������
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