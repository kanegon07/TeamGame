using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Throw : MonoBehaviour
{
    public GameObject itemPrefab;
    public LineRenderer lineRenderer; // ���C�������_���[���Q��
    public GameObject arrowHeadPrefab; // ���̐�[�̃I�u�W�F�N�g
    public float projectileSpeed = 10f; // �����鑬�x
    public float gravity = -9.81f; // �d��

    private GameObject arrowHeadInstance;

    void Start()
    {
        // ���̐�[���C���X�^���X�����Ĕ�\���ɂ���
        arrowHeadInstance = Instantiate(arrowHeadPrefab);
        arrowHeadInstance.SetActive(false);
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
            if (t > 0) // t > 0 �̂Ƃ�����`�F�b�N���n�߂�
            {
                Vector3 previousPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                if (Physics.Linecast(previousPoint, point, out RaycastHit hit))
                {
                    // ���̈ʒu���Փˈʒu�ɐݒ肵�A�\������
                    arrowHeadInstance.transform.position = hit.point;
                    arrowHeadInstance.transform.rotation = Quaternion.LookRotation(hit.normal); // ���̌������q�b�g�ʂ̖@���ɍ��킹��
                    arrowHeadInstance.SetActive(true);

                    // �O�Ճ��C���̏I�_���Փˈʒu�ɐݒ�
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    return; // �Փ˂����̂ŏ����𒆒f
                }
            }

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);

            // ���̈ʒu���I�_�ɐݒ肵�A�\������
            arrowHeadInstance.transform.position = point;
            arrowHeadInstance.transform.rotation = Quaternion.LookRotation(startVelocity); // ���̌����𑬓x�̕����ɍ��킹��
            arrowHeadInstance.SetActive(true);
        }
    }

    // �A�C�e���𓊂��鏈��
    void ThrowItem()
    {
        GameObject rock = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Rigidbody rockRb = rock.GetComponent<Rigidbody>();
        rockRb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        Destroy(rock, 3.5f); // 3.5�b��ɒe������
    }

    // �O�Ղ��������郁�\�b�h
    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0; // �O�Ղ�����
        arrowHeadInstance.SetActive(false); // ���̐�[���\���ɂ���
    }
}*/
/*public class Throw : MonoBehaviour
{
    public GameObject itemPrefab;
    public LineRenderer lineRenderer; // ���C�������_���[���Q��
    public GameObject arrowHeadPrefab; // ���̐�[�̃I�u�W�F�N�g
    public float projectileSpeed = 10f; // �����鑬�x
    public float gravity = -9.81f; // �d��

    private GameObject arrowHeadInstance;
    private EffectManager effectScript; // �G�t�F�N�g�}�l�[�W���[�ւ̎Q��

    void Start()
    {
        // ���̐�[���C���X�^���X�����Ĕ�\���ɂ���
        arrowHeadInstance = Instantiate(arrowHeadPrefab);
        arrowHeadInstance.SetActive(false);

        // �G�t�F�N�g�Đ��p�̃X�N���v�g���擾
        effectScript = GetComponent<EffectManager>();
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
            if (t > 0) // t > 0 �̂Ƃ�����`�F�b�N���n�߂�
            {
                Vector3 previousPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                if (Physics.Linecast(previousPoint, point, out RaycastHit hit))
                {
                    // ���̈ʒu���Փˈʒu�ɐݒ肵�A�\������
                    arrowHeadInstance.transform.position = hit.point; // ���̈ʒu���Փˈʒu�ɐݒ�
                    arrowHeadInstance.transform.rotation = Quaternion.LookRotation(hit.normal); // ���̌������q�b�g�ʂ̖@���ɍ��킹��
                    arrowHeadInstance.SetActive(true);

                    // �O�Ճ��C���̏I�_���Փˈʒu�ɐݒ�
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    return; // �Փ˂����̂ŏ����𒆒f
                }
            }

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);

            // ���̈ʒu���I�_�ɐݒ肵�A�\������
            arrowHeadInstance.transform.position = point;
            arrowHeadInstance.transform.rotation = Quaternion.LookRotation(startVelocity); // ���̌����𑬓x�̕����ɍ��킹��
            arrowHeadInstance.SetActive(true);

            //effectScript.PlayFallingPointEffect(arrowHeadInstance.transform.position, transform.rotation, transform);
        }
    }

    // �A�C�e���𓊂��鏈��
    void ThrowItem()
    {
        GameObject rock = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Rigidbody rockRb = rock.GetComponent<Rigidbody>();
        rockRb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        Destroy(rock, 3.5f); // 3.5�b��ɒe������
    }

    // �O�Ղ��������郁�\�b�h
    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0; // �O�Ղ�����
        arrowHeadInstance.SetActive(false); // ���̐�[���\���ɂ���
    }
}*/

public class Throw : MonoBehaviour
{
    public GameObject itemPrefab;
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
    }
}

/*public class Throw : MonoBehaviour
{
    public GameObject itemPrefab;
    public LineRenderer lineRenderer;
    public GameObject arrowHeadPrefab;
    public float projectileSpeed = 10f;
    public float gravity = -9.81f;

    // �G�t�F�N�g�Đ��p�̃X�N���v�g���擾���邽�߂̕ϐ�
    public EffectManager _effectScript;

    private GameObject arrowHeadInstance;
    private GameObject lastItem; // ���O�ɓ������A�C�e��

    void Start()
    {
        arrowHeadInstance = Instantiate(arrowHeadPrefab);
        arrowHeadInstance.SetActive(false);

        // �G�t�F�N�g�Đ��p�̃X�N���v�g���擾
        _effectScript = GetComponent<EffectManager>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            UpdateTrajectory();
        }

        if (Input.GetMouseButtonUp(1))
        {
            ThrowItem();
            ClearTrajectory();
        }
    }

    void UpdateTrajectory()
    {
        lineRenderer.positionCount = 0;
        Vector3 startPosition = transform.position;
        Vector3 startVelocity = transform.forward * projectileSpeed;

        for (float t = 0; t < 2f; t += 0.1f)
        {
            float x = startPosition.x + startVelocity.x * t;
            float y = startPosition.y + (startVelocity.y * t) + (0.5f * gravity * t * t);
            float z = startPosition.z + startVelocity.z * t;

            Vector3 point = new Vector3(x, y, z);

            if (t > 0)
            {
                Vector3 previousPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                if (Physics.Linecast(previousPoint, point, out RaycastHit hit))
                {
                    arrowHeadInstance.transform.position = hit.point;
                    arrowHeadInstance.transform.rotation = Quaternion.LookRotation(hit.normal);
                    arrowHeadInstance.SetActive(true);

                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    return;
                }
            }

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
        }
    }

    void ThrowItem()
    {
        // �����̃A�C�e��������΋O�ՂƖ�������
        if (lastItem != null)
        {
            ClearTrajectory();
        }

        lastItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Rigidbody rockRb = lastItem.GetComponent<Rigidbody>();
        rockRb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        Destroy(lastItem, 3.5f);
    }

    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0;
        arrowHeadInstance.SetActive(false);
    }
}*/