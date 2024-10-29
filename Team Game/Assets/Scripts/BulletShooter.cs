using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ˌ����w��ł���悤�ɂ���
public class BulletShooter : MonoBehaviour
{
    public Transform firePoint; // �e�̔��ˌ��ƂȂ�I�u�W�F�N�g
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
    }
}
