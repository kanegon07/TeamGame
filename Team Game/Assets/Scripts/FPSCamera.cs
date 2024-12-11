using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{

    
    public GameObject rotationObj; // �v���C���[�I�u�W�F�N�g���Q��
    public GameObject cameraObj;
    public GameObject subCameraObj;
    public GameObject lineObj;
    public GameObject particleObj;
    private float xRotation = 0f; // �c�̎��_��]�p�x���Ǘ�
    public float yRotation = 0f;
    public float mouseSensitivity = 1f; // �}�E�X���x�𒲐�����ϐ�
    

    private void Start()
    {
        
       
    }
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
        //yRotation += mouseX;
        if (Mathf.Abs(mouseX) > 0.001f)
        {
            transform.RotateAround(transform.position, Vector3.up, mouseX);
        }

        //Trans = transform;
        //player_Transform.rotation= Quaternion.Euler(Trans.rotation.eulerAngles.x,0f, Trans.rotation.eulerAngles.z);
        //player.GetTransform(player_Transform);
        
    }

    // �J������X���𒆐S�ɐ�����]
    private void RotateCameraVertically(float mouseY)
    {
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 62f);
        rotationObj.transform.localRotation = Quaternion.Euler(xRotation, rotationObj.transform.localEulerAngles.y, 0f);
        cameraObj.transform.localRotation = Quaternion.Euler(xRotation, cameraObj.transform.localEulerAngles.y, 0f);
        subCameraObj.transform.localRotation = Quaternion.Euler(xRotation, subCameraObj.transform.localEulerAngles.y, 0f);
        lineObj.transform.localRotation = Quaternion.Euler(xRotation, lineObj.transform.localEulerAngles.y, 0f);
        particleObj.transform.localRotation = Quaternion.Euler(xRotation, particleObj.transform.localEulerAngles.y, 0f);
    }
    
    /*
    public Transform playerBody; // �v���C���[��Transform�i����]�p�j
    public Transform head;       // �����i�c��]�p�j
    public float mouseSensitivity = 100f; // �}�E�X���x
    public bool invertX = false;  // X�����]
    public bool invertY = false;  // Y�����]

    private float verticalRotation = 0f; // �c�����̉�]��ێ�

    void Start()
    {
        // �����ʒu��ݒ� (��: �O��������)
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        // �}�E�X���͎擾
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (invertX) mouseX = -mouseX;
        if (invertY) mouseY = -mouseY;

        // �������̉�]�F�v���C���[�S��
        playerBody.Rotate(Vector3.up, mouseX);

        // �c�����̉�]�F�����̂�
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // �㉺90�x�ɐ���
        head.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
    */
}
