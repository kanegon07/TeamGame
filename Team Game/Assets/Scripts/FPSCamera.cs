using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{

    
    public GameObject rotationObj; // プレイヤーオブジェクトを参照
    public GameObject cameraObj;
    public GameObject subCameraObj;
    public GameObject lineObj;
    public GameObject particleObj;
    private float xRotation = 0f; // 縦の視点回転角度を管理
    public float yRotation = 0f;
    public float mouseSensitivity = 1f; // マウス感度を調整する変数
    

    private void Start()
    {
        
       
    }
    void Update()
    {
        // マウスの移動量を取得
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // 水平移動
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // 垂直移動

        // プレイヤーの水平回転（Y軸回転）
        RotatePlayerHorizontally(mouseX);

        // カメラの垂直回転（X軸回転）
        RotateCameraVertically(mouseY);
    }

    // プレイヤーのY軸を中心に水平回転
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

    // カメラのX軸を中心に垂直回転
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
    public Transform playerBody; // プレイヤーのTransform（横回転用）
    public Transform head;       // 頭部（縦回転用）
    public float mouseSensitivity = 100f; // マウス感度
    public bool invertX = false;  // X軸反転
    public bool invertY = false;  // Y軸反転

    private float verticalRotation = 0f; // 縦方向の回転を保持

    void Start()
    {
        // 初期位置を設定 (例: 前方を向く)
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        // マウス入力取得
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (invertX) mouseX = -mouseX;
        if (invertY) mouseY = -mouseY;

        // 横方向の回転：プレイヤー全体
        playerBody.Rotate(Vector3.up, mouseX);

        // 縦方向の回転：頭部のみ
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // 上下90度に制限
        head.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
    */
}
