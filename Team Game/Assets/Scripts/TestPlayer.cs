using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private float speed = 5.0f;
    public Transform cameraTransform; // �J������Transform���Q�Ƃ��邽�߂̕ϐ�

    // �G�t�F�N�g�Đ��p�̃X�N���v�g���擾���邽�߂̕ϐ�
    public EffectManager _effectScript;

    void Start()
    {
        // �G�t�F�N�g�Đ��p�̃X�N���v�g���擾
        _effectScript = GetComponent<EffectManager>();
    }

    void Update()
    {
        // �J�����̑O�����ƉE�������擾
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // �㉺�����̓��������O���Đ����ړ��������l��
        forward.y = 0;
        right.y = 0;

        // ���K�����ĕ����x�N�g�����擾
        forward.Normalize();
        right.Normalize();

        // �v���C���[�̈ړ��������v�Z
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = (right * horizontalInput + forward * verticalInput).normalized;

        // �v���C���[�̈ʒu���X�V
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
}
