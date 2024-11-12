using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//弾の挙動を管理
public class BulletController : MonoBehaviour
{
    private BulletManager bulletManager;
    [SerializeField] private float lifespan = 5f; // 弾が消えるまでの時間
    private float timer; // 経過時間を記録する変数

    // BulletManagerを設定するメソッド
    public void SetBulletManager(BulletManager manager)
    {
        bulletManager = manager;
    }

    private void Start()
    {
        // 初期化
        timer = 0f;
    }

    private void Update()
    {
        // 経過時間を加算
        timer += Time.deltaTime;

        // 経過時間が lifespan を超えたら弾を消去
        if (timer >= lifespan)
        {
            DestroyBullet();
        }
    }

    // 衝突したときの処理
    private void OnCollisionEnter(Collision collision)
    {
        // ここで必要に応じてエフェクトや音を再生することも可能
        // エフェクトの再生など
        // Instantiate(effectPrefab, transform.position, Quaternion.identity);

        // プレイヤーと衝突した場合は消去しない(現在はテストとしてプレイヤーから飛ばしているためこうしている)
        if (collision.gameObject.CompareTag("Player"))
        {
            return; // プレイヤーに当たった場合は何もしない
        }

        DestroyBullet();
    }

    // 弾を消去するメソッド
    private void DestroyBullet()
    {
        if (bulletManager != null)
        {
            bulletManager.RemoveBullet(gameObject);
        }
        else
        {
            Destroy(gameObject); // BulletManagerが設定されていない場合はそのまま消去
        }
    }
}
