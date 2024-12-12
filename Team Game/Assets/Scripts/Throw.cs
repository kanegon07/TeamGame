using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    /*public GameObject itemPrefab;
    public LineRenderer lineRenderer; // �O�Ղ�\�����郉�C�������_���[
    public float projectileSpeed = 10f; // �����鑬�x
    public float gravity = -9.81f; // �d��

    private EffectManager effectManager; // EffectManager�ւ̎Q��
    
    void Start()
    {
        // EffectManager���擾
        effectManager = GetComponent<EffectManager>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) // �E�N���b�N������
        {
            UpdateTrajectory(); // �O�Ղ̍X�V
        }

        if (Input.GetMouseButtonUp(1)) // �E�N���b�N�𗣂�����
        {
            ThrowItem(); // �A�C�e���𓊂���
            ClearTrajectory(); // �O�Ղ�����
        }
    }

    // �O�Ղ̍X�V
    void UpdateTrajectory()
    {
        lineRenderer.positionCount = 0; // �����̃|�C���g���N���A
        Vector3 startPosition = transform.position; // �����n�߂̈ʒu
        Vector3 startVelocity = transform.forward * projectileSpeed; // ����������Ƒ��x

        // ������e�̋O�����v�Z
        for (float t = 0; t < 2f; t += 0.1f) // 0.1�b���ƂɃ|�C���g���v�Z
        {
            float x = startPosition.x + startVelocity.x * t;
            float y = startPosition.y + (startVelocity.y * t) + (0.5f * gravity * t * t);
            float z = startPosition.z + startVelocity.z * t;

            Vector3 point = new Vector3(x, y, z);

            // ���C�L���X�g�ŏՓ˔���
            if (t > 0)
            {
                Vector3 previousPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                if (Physics.Linecast(previousPoint, point, out RaycastHit hit))
                {
                    effectManager.PlayFallingPointEffect(hit.point, Quaternion.identity);

                    // �O�Ճ��C���̏I�_���Փˈʒu�ɐݒ�
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    return;
                }
            }

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
        }
    }

    // �A�C�e���𓊂��鏈��
    void ThrowItem()
    {
        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Rigidbody itemRb = item.GetComponent<Rigidbody>();
        itemRb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        Destroy(item, 3.5f); // 3.5�b��ɒe������
    }

    // �O�Ղ��������郁�\�b�h
    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0; // �O�Ղ�����
    }*/

    public GameObject itemPrefab;
    public LineRenderer lineRenderer; // �O�Ղ�\�����郉�C�������_���[
    public float baseProjectileSpeed = 10f; // ��{�̓����鑬�x
    public float maxProjectileSpeed = 20f; // �ő哊���鑬�x
    public float gravity = -9.81f; // �d��
    public float speedIncreaseTime = 2f; // ���x���ő�ɂȂ�܂ł̎���

    private EffectManager effectManager; // EffectManager�ւ̎Q��
    private float holdTime = 0f; // �E�N���b�N�����������鎞��

    void Start()
    {
        // EffectManager���擾
        effectManager = GetComponent<EffectManager>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) // �E�N���b�N������
        {
            holdTime += Time.deltaTime; // �E�N���b�N���Ԃ̌v��
            UpdateTrajectory(); // �O�Ղ̍X�V
        }

        if (Input.GetMouseButtonUp(1)) // �E�N���b�N�𗣂�����
        {
            ThrowItem(); // �A�C�e���𓊂���
            ClearTrajectory(); // �O�Ղ�����
            holdTime = 0f; // �E�N���b�N���ԃ��Z�b�g
        }
    }

    // �O�Ղ̍X�V
    void UpdateTrajectory()
    {
        lineRenderer.positionCount = 0; // �����̃|�C���g���N���A
        Vector3 startPosition = transform.position; // �����n�߂̈ʒu
        float adjustedSpeed = Mathf.Lerp(baseProjectileSpeed, maxProjectileSpeed, holdTime / speedIncreaseTime); // ���x����
        Vector3 startVelocity = transform.forward * adjustedSpeed; // ����������Ƒ��x

        // ������e�̋O�����v�Z
        for (float t = 0; t < 2f; t += 0.1f) // 0.1�b���ƂɃ|�C���g���v�Z
        {
            float x = startPosition.x + startVelocity.x * t;
            float y = startPosition.y + (startVelocity.y * t) + (0.5f * gravity * t * t);
            float z = startPosition.z + startVelocity.z * t;

            Vector3 point = new Vector3(x, y, z);

            // ���C�L���X�g�ŏՓ˔���
            if (t > 0)
            {
                Vector3 previousPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                if (Physics.Linecast(previousPoint, point, out RaycastHit hit))
                {
                    effectManager.PlayFallingPointEffect(hit.point, Quaternion.identity);

                    // �O�Ճ��C���̏I�_���Փˈʒu�ɐݒ�
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    return;
                }
            }

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
        }
    }

    // �A�C�e���𓊂��鏈��
    void ThrowItem()
    {
        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Rigidbody itemRb = item.GetComponent<Rigidbody>();

        // �E�N���b�N���ԂɊ�Â��đ��x�𒲐�
        float adjustedSpeed = Mathf.Lerp(baseProjectileSpeed, maxProjectileSpeed, holdTime / speedIncreaseTime); // ���x����

        // ������͂�ǉ�
        itemRb.AddForce(transform.forward * adjustedSpeed, ForceMode.Impulse);

        Destroy(item, 3.5f); // 3.5�b��ɒe������
    }

    // �O�Ղ��������郁�\�b�h
    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0; // �O�Ղ�����
    }
}