using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [Header("�ΏۃI�u�W�F�N�g")]
    public GameObject playerHead; // �v���C���[�̓�����]������I�u�W�F�N�g
    public GameObject mainCamera; // ���C���J����
    public GameObject subCamera; //�T�u�J����
    public GameObject trajectoryLine; // �e���\�����̃I�u�W�F�N�g
    public GameObject particleEffect; // �p�[�e�B�N���G�t�F�N�g�̃I�u�W�F�N�g

    [Header("�J�����ݒ�")]
    private float xRotation = 0f; // �c�̎��_��]�p�x���Ǘ�
    public float mouseSensitivity = 1f; // �}�E�X���x�𒲐�����ϐ�

    private List<GameObject> cameraRelatedObjects; // ��]��K�p����I�u�W�F�N�g�̃��X�g

    private void Start()
    {
        // �ꊇ�ŉ�]��K�p����ΏۃI�u�W�F�N�g�����X�g�ɓo�^
        cameraRelatedObjects = new List<GameObject> { playerHead, mainCamera, subCamera, trajectoryLine, particleEffect };
    }
    void Update()
    {
        // �}�E�X���͂̎擾
        Vector2 mouseInput = GetMouseInput();

        // �v���C���[�̐�����]
        RotatePlayerHorizontally(mouseInput.x);

        // �J�����̐�����]
        RotateCameraVertically(mouseInput.y);
    }

    // �}�E�X�̈ړ��ʂ��擾����
    private Vector2 GetMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        return new Vector2(mouseX, mouseY);
    }

    // �v���C���[�̑̂𐅕���]������
    private void RotatePlayerHorizontally(float mouseX)
    {
        //yRotation += mouseX;
        if (Mathf.Abs(mouseX) > 0.001f)
        {
            transform.RotateAround(transform.position, Vector3.up, mouseX);
        }
    }

    // �J������X���𒆐S�ɐ�����]
    private void RotateCameraVertically(float mouseY)
    {
        xRotation -= mouseY; // ��]�p�x���v�Z
        xRotation = Mathf.Clamp(xRotation, -90f, 62f); // ��]�͈͂𐧌�

        // �e�I�u�W�F�N�g�ɐ�����]��K�p
        foreach (var obj in cameraRelatedObjects)
        {
            obj.transform.localRotation = Quaternion.Euler(xRotation, obj.transform.localEulerAngles.y, 0f);
        }
    }
}
