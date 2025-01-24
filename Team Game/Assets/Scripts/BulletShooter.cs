using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 発射元を指定できるようにする
public class BulletShooter : MonoBehaviour
{
    /*
    public Transform firePoint; // 弾の発射元
    public BulletManager bulletManager; // 弾管理用
    public RayManager rayManager; // ターゲット位置取得用

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootTowardsTarget();
        }
    }

    // ターゲット位置を取得し、発射方向を計算して弾を発射する
    private void ShootTowardsTarget()
    {
        Vector3 targetPosition = rayManager.GetTargetPosition();
        Vector3 direction = CalculateDirection(targetPosition);
        bulletManager.Shoot(firePoint.position, Quaternion.LookRotation(direction));
    }

    // 発射方向を計算
    private Vector3 CalculateDirection(Vector3 targetPosition)
    {
        return (targetPosition - firePoint.position).normalized;
    }*/
}
