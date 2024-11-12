using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject subCamera;
    public CharacterController characterController; // プレイヤーのCharacterController

    void Start()
    {
        // 初期状態でmainCameraがアクティブ、subCameraは非アクティブにする
        mainCamera.SetActive(true);
        subCamera.SetActive(false);
    }

    void Update()
    {
        // プレイヤーの速度が0でない場合、移動中とみなす
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
