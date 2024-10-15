using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�e�̋������Ǘ�
public class BulletController : MonoBehaviour
{
    private BulletManager bulletManager;

    // BulletManager��ݒ肷�郁�\�b�h
    public void SetBulletManager(BulletManager manager)
    {
        bulletManager = manager;
    }

    // �Փ˂����Ƃ��̏���
    private void OnCollisionEnter(Collision collision)
    {
        // �����ŕK�v�ɉ����ăG�t�F�N�g�≹���Đ����邱�Ƃ��\
        // �G�t�F�N�g�̍Đ��Ȃ�
        // Instantiate(effectPrefab, transform.position, Quaternion.identity);

        // �v���C���[�ƏՓ˂����ꍇ�͏������Ȃ�(���݂̓e�X�g�Ƃ��ăv���C���[�����΂��Ă��邽�߂������Ă���)
        if (collision.gameObject.CompareTag("Player"))
        {
            return; // �v���C���[�ɓ��������ꍇ�͉������Ȃ�
        }

        // BulletManager���g���Ď��g��j������
        if (bulletManager != null)
        {
            bulletManager.RemoveBullet(gameObject);
        }
    }
}
