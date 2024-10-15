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
        // マウスの移動量取得
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity; // 横移動
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity; // 縦移動


        // 水平方向の回転（プレイヤーのY軸回り）
        if (Mathf.Abs(mx) > 0.001f)
        {
            transform.RotateAround(transform.position, Vector3.up, mx); // 横回転
        }

        // 縦方向の回転（カメラのX軸回り）
        xRotation -= my; // 縦の視点移動を追跡
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 上下90度の範囲で制限
        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y, 0f); // 回転を適用
    }
}
