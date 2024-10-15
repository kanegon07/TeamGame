using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�e�𐶐��E�Ǘ�
public class BulletManager : MonoBehaviour
{
    public GameObject bulletPrefab;  // �e�̃v���n�u���Q��
    public float bulletSpeed = 20f;  // �e�̑��x

    private List<GameObject> activeBullets = new List<GameObject>(); // ���݂̒e���Ǘ����郊�X�g

    public void Shoot(Vector3 position, Quaternion rotation)
    {
        // �e�𐶐�
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        activeBullets.Add(bullet); // ���������e�����X�g�ɒǉ�

        // �e�Ƀ��W�b�h�{�f�B������ꍇ�́A�O���ɗ͂������Ĕ���
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = rotation * Vector3.forward * bulletSpeed;  // ��]���l�����đO���ɔ���
        }

        // �e�̏��������̂��߂�BulletManager��ݒ�
        bullet.GetComponent<BulletController>().SetBulletManager(this);
    }

    public void RemoveBullet(GameObject bullet)
    {
        activeBullets.Remove(bullet);
        Destroy(bullet);
    }
}
