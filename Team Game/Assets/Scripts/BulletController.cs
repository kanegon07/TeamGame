using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�e�̋������Ǘ�
public class BulletController : MonoBehaviour
{
    private BulletManager bulletManager;
    [SerializeField] private float lifespan = 5f; // �e��������܂ł̎���
    private float timer; // �o�ߎ��Ԃ��L�^����ϐ�

    // BulletManager��ݒ肷�郁�\�b�h
    public void SetBulletManager(BulletManager manager)
    {
        bulletManager = manager;
    }

    private void Start()
    {
        // ������
        timer = 0f;
    }

    private void Update()
    {
        // �o�ߎ��Ԃ����Z
        timer += Time.deltaTime;

        // �o�ߎ��Ԃ� lifespan �𒴂�����e������
        if (timer >= lifespan)
        {
            DestroyBullet();
        }
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

        DestroyBullet();
    }

    // �e���������郁�\�b�h
    private void DestroyBullet()
    {
        if (bulletManager != null)
        {
            bulletManager.RemoveBullet(gameObject);
        }
        else
        {
            Destroy(gameObject); // BulletManager���ݒ肳��Ă��Ȃ��ꍇ�͂��̂܂܏���
        }
    }
}
