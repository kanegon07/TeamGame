using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public GameObject player; // �v���C���[�I�u�W�F�N�g���Q��
    private float xRotation = 0f; // �c�̎��_��]�p�x���Ǘ�
    public float mouseSensitivity = 1f; // �}�E�X���x�𒲐�����ϐ�

    void Update()
    {
        // �}�E�X�̈ړ��ʎ擾
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity; // ���ړ�
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity; // �c�ړ�


        // ���������̉�]�i�v���C���[��Y�����j
        if (Mathf.Abs(mx) > 0.001f)
        {
            transform.RotateAround(transform.position, Vector3.up, mx); // ����]
        }

        // �c�����̉�]�i�J������X�����j
        xRotation -= my; // �c�̎��_�ړ���ǐ�
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // �㉺90�x�͈̔͂Ő���
        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y, 0f); // ��]��K�p
    }
}
