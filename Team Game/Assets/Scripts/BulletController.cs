using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//弾の挙動を管理
public class BulletController : MonoBehaviour
{
    private BulletManager bulletManager;

    // BulletManagerを設定するメソッド
    public void SetBulletManager(BulletManager manager)
    {
        bulletManager = manager;
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

        // BulletManagerを使って自身を破棄する
        if (bulletManager != null)
        {
            bulletManager.RemoveBullet(gameObject);
        }
    }
}
