using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DebugMouse : MonoBehaviour {
    private bool isCursorLocked = true; // ���݂̃J�[�\���̃��b�N��Ԃ��Ǘ�

    // �ʂ̃X�N���v�g���炢���肽���^�C�~���O������̂�
    // �ǉ������Ă��������܂���
    public void Lock(bool flag) {
        if (flag) {
            LockCursor();
        } else {
            UnlockCursor();
        }
    }

    void Start()
    {
        LockCursor(); // �Q�[���J�n���ɃJ�[�\�����\���ɂ��ă��b�N
    }

    void Update()
    {
        // �E�B���h�E���A�N�e�B�u�ɂȂ����ꍇ���J�[�\�����\���ɂ���
        if (Application.isFocused && isCursorLocked)
        {
            LockCursor();
        }

        // P�L�[�ŃJ�[�\���̃��b�N��Ԃ�؂�ւ�
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isCursorLocked)
            {
                UnlockCursor(); // �J�[�\���̃��b�N������
            }
            else
            {
                LockCursor(); // �J�[�\�������b�N
            }
        }
    }

    // �J�[�\������ʒ����Ƀ��b�N���A��\���ɂ���
    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // �J�[�\���𒆉��ɌŒ�
        Cursor.visible = false; // �J�[�\�����\��
        isCursorLocked = true; // ��Ԃ��X�V
    }

    // �J�[�\���̃��b�N���������A�\������
    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // �J�[�\���̃��b�N������
        Cursor.visible = true; // �J�[�\����\��
        isCursorLocked = false; // ��Ԃ��X�V
    }
}
