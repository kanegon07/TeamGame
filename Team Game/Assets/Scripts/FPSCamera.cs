using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public GameObject player; // プレイヤーオブジェクトを参照
    private float xRotation = 0f; // 縦の視点回転角度を管理
    public float mouseSensitivity = 1f; // マウス感度を調整する変数

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
        if (Mathf.Abs(mouseX) > 0.001f)
        {
            transform.RotateAround(transform.position, Vector3.up, mouseX);
        }
    }

    // カメラのX軸を中心に垂直回転
    private void RotateCameraVertically(float mouseY)
    {
        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y, 0f);
    }
}
