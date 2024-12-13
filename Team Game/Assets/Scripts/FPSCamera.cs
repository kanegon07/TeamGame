using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [Header("対象オブジェクト")]
    public GameObject playerHead; // プレイヤーの頭を回転させるオブジェクト
    public GameObject mainCamera; // メインカメラ
    public GameObject subCamera; //サブカメラ
    public GameObject trajectoryLine; // 弾道予測線のオブジェクト
    public GameObject particleEffect; // パーティクルエフェクトのオブジェクト

    [Header("カメラ設定")]
    private float xRotation = 0f; // 縦の視点回転角度を管理
    public float mouseSensitivity = 1f; // マウス感度を調整する変数

    private List<GameObject> cameraRelatedObjects; // 回転を適用するオブジェクトのリスト

    private void Start()
    {
        // 一括で回転を適用する対象オブジェクトをリストに登録
        cameraRelatedObjects = new List<GameObject> { playerHead, mainCamera, subCamera, trajectoryLine, particleEffect };
    }
    void Update()
    {
        // マウス入力の取得
        Vector2 mouseInput = GetMouseInput();

        // プレイヤーの水平回転
        RotatePlayerHorizontally(mouseInput.x);

        // カメラの垂直回転
        RotateCameraVertically(mouseInput.y);
    }

    // マウスの移動量を取得する
    private Vector2 GetMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        return new Vector2(mouseX, mouseY);
    }

    // プレイヤーの体を水平回転させる
    private void RotatePlayerHorizontally(float mouseX)
    {
        //yRotation += mouseX;
        if (Mathf.Abs(mouseX) > 0.001f)
        {
            transform.RotateAround(transform.position, Vector3.up, mouseX);
        }
    }

    // カメラのX軸を中心に垂直回転
    private void RotateCameraVertically(float mouseY)
    {
        xRotation -= mouseY; // 回転角度を計算
        xRotation = Mathf.Clamp(xRotation, -90f, 62f); // 回転範囲を制限

        // 各オブジェクトに垂直回転を適用
        foreach (var obj in cameraRelatedObjects)
        {
            obj.transform.localRotation = Quaternion.Euler(xRotation, obj.transform.localEulerAngles.y, 0f);
        }
    }
}
