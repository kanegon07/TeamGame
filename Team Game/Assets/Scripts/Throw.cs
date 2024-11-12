using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Throw : MonoBehaviour
{
    public GameObject itemPrefab;
    public LineRenderer lineRenderer; // ラインレンダラーを参照
    public GameObject arrowHeadPrefab; // 矢印の先端のオブジェクト
    public float projectileSpeed = 10f; // 投げる速度
    public float gravity = -9.81f; // 重力

    private GameObject arrowHeadInstance;

    void Start()
    {
        // 矢印の先端をインスタンス化して非表示にする
        arrowHeadInstance = Instantiate(arrowHeadPrefab);
        arrowHeadInstance.SetActive(false);
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
            if (t > 0) // t > 0 のときからチェックを始める
            {
                Vector3 previousPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                if (Physics.Linecast(previousPoint, point, out RaycastHit hit))
                {
                    // 矢印の位置を衝突位置に設定し、表示する
                    arrowHeadInstance.transform.position = hit.point;
                    arrowHeadInstance.transform.rotation = Quaternion.LookRotation(hit.normal); // 矢印の向きをヒット面の法線に合わせる
                    arrowHeadInstance.SetActive(true);

                    // 軌跡ラインの終点を衝突位置に設定
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    return; // 衝突したので処理を中断
                }
            }

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);

            // 矢印の位置を終点に設定し、表示する
            arrowHeadInstance.transform.position = point;
            arrowHeadInstance.transform.rotation = Quaternion.LookRotation(startVelocity); // 矢印の向きを速度の方向に合わせる
            arrowHeadInstance.SetActive(true);
        }
    }

    // アイテムを投げる処理
    void ThrowItem()
    {
        GameObject rock = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Rigidbody rockRb = rock.GetComponent<Rigidbody>();
        rockRb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        Destroy(rock, 3.5f); // 3.5秒後に弾を消去
    }

    // 軌跡を消去するメソッド
    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0; // 軌跡を消去
        arrowHeadInstance.SetActive(false); // 矢印の先端を非表示にする
    }
}*/
/*public class Throw : MonoBehaviour
{
    public GameObject itemPrefab;
    public LineRenderer lineRenderer; // ラインレンダラーを参照
    public GameObject arrowHeadPrefab; // 矢印の先端のオブジェクト
    public float projectileSpeed = 10f; // 投げる速度
    public float gravity = -9.81f; // 重力

    private GameObject arrowHeadInstance;
    private EffectManager effectScript; // エフェクトマネージャーへの参照

    void Start()
    {
        // 矢印の先端をインスタンス化して非表示にする
        arrowHeadInstance = Instantiate(arrowHeadPrefab);
        arrowHeadInstance.SetActive(false);

        // エフェクト再生用のスクリプトを取得
        effectScript = GetComponent<EffectManager>();
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
            if (t > 0) // t > 0 のときからチェックを始める
            {
                Vector3 previousPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                if (Physics.Linecast(previousPoint, point, out RaycastHit hit))
                {
                    // 矢印の位置を衝突位置に設定し、表示する
                    arrowHeadInstance.transform.position = hit.point; // 矢印の位置を衝突位置に設定
                    arrowHeadInstance.transform.rotation = Quaternion.LookRotation(hit.normal); // 矢印の向きをヒット面の法線に合わせる
                    arrowHeadInstance.SetActive(true);

                    // 軌跡ラインの終点を衝突位置に設定
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    return; // 衝突したので処理を中断
                }
            }

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);

            // 矢印の位置を終点に設定し、表示する
            arrowHeadInstance.transform.position = point;
            arrowHeadInstance.transform.rotation = Quaternion.LookRotation(startVelocity); // 矢印の向きを速度の方向に合わせる
            arrowHeadInstance.SetActive(true);

            //effectScript.PlayFallingPointEffect(arrowHeadInstance.transform.position, transform.rotation, transform);
        }
    }

    // アイテムを投げる処理
    void ThrowItem()
    {
        GameObject rock = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Rigidbody rockRb = rock.GetComponent<Rigidbody>();
        rockRb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        Destroy(rock, 3.5f); // 3.5秒後に弾を消去
    }

    // 軌跡を消去するメソッド
    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0; // 軌跡を消去
        arrowHeadInstance.SetActive(false); // 矢印の先端を非表示にする
    }
}*/

public class Throw : MonoBehaviour
{
    public GameObject itemPrefab;
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
    }
}

/*public class Throw : MonoBehaviour
{
    public GameObject itemPrefab;
    public LineRenderer lineRenderer;
    public GameObject arrowHeadPrefab;
    public float projectileSpeed = 10f;
    public float gravity = -9.81f;

    // エフェクト再生用のスクリプトを取得するための変数
    public EffectManager _effectScript;

    private GameObject arrowHeadInstance;
    private GameObject lastItem; // 直前に投げたアイテム

    void Start()
    {
        arrowHeadInstance = Instantiate(arrowHeadPrefab);
        arrowHeadInstance.SetActive(false);

        // エフェクト再生用のスクリプトを取得
        _effectScript = GetComponent<EffectManager>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            UpdateTrajectory();
        }

        if (Input.GetMouseButtonUp(1))
        {
            ThrowItem();
            ClearTrajectory();
        }
    }

    void UpdateTrajectory()
    {
        lineRenderer.positionCount = 0;
        Vector3 startPosition = transform.position;
        Vector3 startVelocity = transform.forward * projectileSpeed;

        for (float t = 0; t < 2f; t += 0.1f)
        {
            float x = startPosition.x + startVelocity.x * t;
            float y = startPosition.y + (startVelocity.y * t) + (0.5f * gravity * t * t);
            float z = startPosition.z + startVelocity.z * t;

            Vector3 point = new Vector3(x, y, z);

            if (t > 0)
            {
                Vector3 previousPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                if (Physics.Linecast(previousPoint, point, out RaycastHit hit))
                {
                    arrowHeadInstance.transform.position = hit.point;
                    arrowHeadInstance.transform.rotation = Quaternion.LookRotation(hit.normal);
                    arrowHeadInstance.SetActive(true);

                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    return;
                }
            }

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
        }
    }

    void ThrowItem()
    {
        // 既存のアイテムがあれば軌跡と矢印を消去
        if (lastItem != null)
        {
            ClearTrajectory();
        }

        lastItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Rigidbody rockRb = lastItem.GetComponent<Rigidbody>();
        rockRb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        Destroy(lastItem, 3.5f);
    }

    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0;
        arrowHeadInstance.SetActive(false);
    }
}*/