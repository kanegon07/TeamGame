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
        // �}�E�X�̈ړ��ʂ��擾
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // �����ړ�
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // �����ړ�

        // �v���C���[�̐�����]�iY����]�j
        RotatePlayerHorizontally(mouseX);

        // �J�����̐�����]�iX����]�j
        RotateCameraVertically(mouseY);
    }

    // �v���C���[��Y���𒆐S�ɐ�����]
    private void RotatePlayerHorizontally(float mouseX)
    {
        if (Mathf.Abs(mouseX) > 0.001f)
        {
            transform.RotateAround(transform.position, Vector3.up, mouseX);
        }
    }

    // �J������X���𒆐S�ɐ�����]
    private void RotateCameraVertically(float mouseY)
    {
        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y, 0f);
    }
}
