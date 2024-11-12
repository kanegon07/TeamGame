using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ˌ����w��ł���悤�ɂ���
public class BulletShooter : MonoBehaviour
{
    /*public Transform firePoint; // �e�̔��ˌ��ƂȂ�I�u�W�F�N�g
    public BulletManager bulletManager; // BulletManager���Q�Ƃ���ϐ�
    public RayManager rayManager; // RayManager���Q�Ƃ���ϐ�

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 targetPosition = rayManager.GetTargetPosition(); // �^�[�Q�b�g�ʒu���擾
            Vector3 direction = (targetPosition - firePoint.position).normalized; // ���˕������v�Z
            bulletManager.Shoot(firePoint.position, Quaternion.LookRotation(direction)); // �e�𔭎�
        }
    }*/

    public Transform firePoint; // �e�̔��ˌ�
    public BulletManager bulletManager; // �e�Ǘ��p
    public RayManager rayManager; // �^�[�Q�b�g�ʒu�擾�p

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootTowardsTarget();
        }
    }

    // �^�[�Q�b�g�ʒu���擾���A���˕������v�Z���Ēe�𔭎˂���
    private void ShootTowardsTarget()
    {
        Vector3 targetPosition = rayManager.GetTargetPosition();
        Vector3 direction = CalculateDirection(targetPosition);
        bulletManager.Shoot(firePoint.position, Quaternion.LookRotation(direction));
    }

    // ���˕������v�Z
    private Vector3 CalculateDirection(Vector3 targetPosition)
    {
        return (targetPosition - firePoint.position).normalized;
    }
}
