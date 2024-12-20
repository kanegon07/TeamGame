using Cysharp.Threading.Tasks;
using MessagePipe;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using VContainer;

/// <summary>
/// �I���t�b�N�N���X
/// ���̃N���X�ɓo�^���ꂽ�I�u�W�F�N�g�́A���̂��̂��o�^�����܂őI����������Ȃ�
/// �Q�[���p�b�h�̏ꍇ�A�I�����O���Ƒ��̃I�u�W�F�N�g��
/// �I�����������ɋl�ނ̂ŁA����̑΍��p
/// </summary>
public class SelectionHook : MonoBehaviour {
	// �o�^���������߂郁�b�Z�[�W
	public struct UnhookMessage { }

	// �Ō�ɑI�����ꂽ�I�u�W�F�N�g
	public GameObject PrevSelected { get; set; } = null;

	[Inject] private ISubscriber<UnhookMessage> _unhookSubscriber = null;

	/// <summary>
	/// EventSystem�̃I�u�W�F�N�g�I����҂R���[�`��
	/// �I�u�W�F�N�g�̑I����Ԃ��O��Ȃ��悤�Ď����A
	/// �Ⴄ���̂��I�����ꂽ�炻������t�b�N����
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
	/// ���݂̃V�[�����I�������Ƃ��̏���
	/// �I�u�W�F�N�g�̓o�^����������
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