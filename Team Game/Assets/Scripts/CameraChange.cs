using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject subCamera;
    public CharacterController characterController; // �v���C���[��CharacterController

    void Start()
    {
        // ������Ԃ�mainCamera���A�N�e�B�u�AsubCamera�͔�A�N�e�B�u�ɂ���
        mainCamera.SetActive(true);
        subCamera.SetActive(false);
    }

    void Update()
    {
        // �v���C���[�̑��x��0�łȂ��ꍇ�A�ړ����Ƃ݂Ȃ�
        if (characterController.velocity.magnitude > 0.01f)
        {
            if (mainCamera.activeSelf)
            {
                mainCamera.SetActive(false);
                subCamera.SetActive(true);
            }
        }
        else
        {
            if (!mainCamera.activeSelf)
            {
                mainCamera.SetActive(true);
                subCamera.SetActive(false);
            }
        }
    }
}
