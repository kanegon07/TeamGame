using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private float speed = 5.0f;
    public Transform cameraTransform; // カメラのTransformを参照するための変数

    void Update()
    {
        // カメラの前方向と右方向を取得
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // 上下方向の動きを除外して水平移動だけを考慮
        forward.y = 0;
        right.y = 0;

        // 正規化して方向ベクトルを取得
        forward.Normalize();
        right.Normalize();

        // プレイヤーの移動方向を計算
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = (right * horizontalInput + forward * verticalInput).normalized;

        // プレイヤーの位置を更新
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
}
