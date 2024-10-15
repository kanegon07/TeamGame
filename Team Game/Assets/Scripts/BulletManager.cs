using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//弾を生成・管理
public class BulletManager : MonoBehaviour
{
    public GameObject bulletPrefab;  // 弾のプレハブを参照
    public float bulletSpeed = 20f;  // 弾の速度

    private List<GameObject> activeBullets = new List<GameObject>(); // 現在の弾を管理するリスト

    public void Shoot(Vector3 position, Quaternion rotation)
    {
        // 弾を生成
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        activeBullets.Add(bullet); // 生成した弾をリストに追加

        // 弾にリジッドボディがある場合は、前方に力を加えて発射
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = rotation * Vector3.forward * bulletSpeed;  // 回転を考慮して前方に発射
        }

        // 弾の消去処理のためにBulletManagerを設定
        bullet.GetComponent<BulletController>().SetBulletManager(this);
    }

    public void RemoveBullet(GameObject bullet)
    {
        activeBullets.Remove(bullet);
        Destroy(bullet);
    }
}
