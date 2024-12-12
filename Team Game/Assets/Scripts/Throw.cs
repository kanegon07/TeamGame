using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    /*public GameObject itemPrefab;
    public LineRenderer lineRenderer; // 軌跡を表示するラインレンダラー
    public float projectileSpeed = 10f; // 投げる速度
    public float gravity = -9.81f; // 重力

    private EffectManager effectManager; // EffectManagerへの参照
    
    void Start()
    {
        // EffectManagerを取得
        effectManager = GetComponent<EffectManager>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) // 右クリック長押し
        {
            UpdateTrajectory(); // 軌跡の更新
        }

        if (Input.GetMouseButtonUp(1)) // 右クリックを離したら
        {
            ThrowItem(); // アイテムを投げる
            ClearTrajectory(); // 軌跡を消去
        }
    }

    // 軌跡の更新
    void UpdateTrajectory()
    {
        lineRenderer.positionCount = 0; // 既存のポイントをクリア
        Vector3 startPosition = transform.position; // 投げ始めの位置
        Vector3 startVelocity = transform.forward * projectileSpeed; // 投げる方向と速度

        // 投げる弾の軌道を計算
        for (float t = 0; t < 2f; t += 0.1f) // 0.1秒ごとにポイントを計算
        {
            float x = startPosition.x + startVelocity.x * t;
            float y = startPosition.y + (startVelocity.y * t) + (0.5f * gravity * t * t);
            float z = startPosition.z + startVelocity.z * t;

            Vector3 point = new Vector3(x, y, z);

            // レイキャストで衝突判定
            if (t > 0)
            {
                Vector3 previousPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                if (Physics.Linecast(previousPoint, point, out RaycastHit hit))
                {
                    effectManager.PlayFallingPointEffect(hit.point, Quaternion.identity);

                    // 軌跡ラインの終点を衝突位置に設定
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    return;
                }
            }

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
        }
    }

    // アイテムを投げる処理
    void ThrowItem()
    {
        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Rigidbody itemRb = item.GetComponent<Rigidbody>();
        itemRb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        Destroy(item, 3.5f); // 3.5秒後に弾を消去
    }

    // 軌跡を消去するメソッド
    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0; // 軌跡を消去
    }*/

    public GameObject itemPrefab;
    public LineRenderer lineRenderer; // 軌跡を表示するラインレンダラー
    public float baseProjectileSpeed = 10f; // 基本の投げる速度
    public float maxProjectileSpeed = 20f; // 最大投げる速度
    public float gravity = -9.81f; // 重力
    public float speedIncreaseTime = 2f; // 速度が最大になるまでの時間

    private EffectManager effectManager; // EffectManagerへの参照
    private float holdTime = 0f; // 右クリックを押し続ける時間

    void Start()
    {
        // EffectManagerを取得
        effectManager = GetComponent<EffectManager>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) // 右クリック長押し
        {
            holdTime += Time.deltaTime; // 右クリック時間の計測
            UpdateTrajectory(); // 軌跡の更新
        }

        if (Input.GetMouseButtonUp(1)) // 右クリックを離したら
        {
            ThrowItem(); // アイテムを投げる
            ClearTrajectory(); // 軌跡を消去
            holdTime = 0f; // 右クリック時間リセット
        }
    }

    // 軌跡の更新
    void UpdateTrajectory()
    {
        lineRenderer.positionCount = 0; // 既存のポイントをクリア
        Vector3 startPosition = transform.position; // 投げ始めの位置
        float adjustedSpeed = Mathf.Lerp(baseProjectileSpeed, maxProjectileSpeed, holdTime / speedIncreaseTime); // 速度を補間
        Vector3 startVelocity = transform.forward * adjustedSpeed; // 投げる方向と速度

        // 投げる弾の軌道を計算
        for (float t = 0; t < 2f; t += 0.1f) // 0.1秒ごとにポイントを計算
        {
            float x = startPosition.x + startVelocity.x * t;
            float y = startPosition.y + (startVelocity.y * t) + (0.5f * gravity * t * t);
            float z = startPosition.z + startVelocity.z * t;

            Vector3 point = new Vector3(x, y, z);

            // レイキャストで衝突判定
            if (t > 0)
            {
                Vector3 previousPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                if (Physics.Linecast(previousPoint, point, out RaycastHit hit))
                {
                    effectManager.PlayFallingPointEffect(hit.point, Quaternion.identity);

                    // 軌跡ラインの終点を衝突位置に設定
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    return;
                }
            }

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
        }
    }

    // アイテムを投げる処理
    void ThrowItem()
    {
        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Rigidbody itemRb = item.GetComponent<Rigidbody>();

        // 右クリック時間に基づいて速度を調整
        float adjustedSpeed = Mathf.Lerp(baseProjectileSpeed, maxProjectileSpeed, holdTime / speedIncreaseTime); // 速度を補間

        // 投げる力を追加
        itemRb.AddForce(transform.forward * adjustedSpeed, ForceMode.Impulse);

        Destroy(item, 3.5f); // 3.5秒後に弾を消去
    }

    // 軌跡を消去するメソッド
    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0; // 軌跡を消去
    }
}