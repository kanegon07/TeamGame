using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayManager : MonoBehaviour
{
    public Transform player;    // �v���C���[��Transform
    public float rayLength = 0.5f; // ���C�̒����i�Z�߁j
    public LayerMask wallLayer; // �ǔ���p�̃��C���[

    private float rayHeightOffset = 0.5f; // ���C�̍����I�t�Z�b�g
    private bool isTouchingWallFront = false; // ���ʂ̕�
    public bool isRayHit = false; // ���C�������������ǂ����̃t���O

    void Update()
    {
        CheckWallDetection();
    }

    void CheckWallDetection()
    {
        // ���ʂɃ��C���΂�
        Vector3 frontDirection = player.forward;   // ���ʕ���

        // ���C���΂��N�_�̈ʒu���v�Z�i���� + �����I�t�Z�b�g�j
        Vector3 rayStartPosition = player.position + Vector3.up * rayHeightOffset;

        // ���C�L���X�g�����s
        isTouchingWallFront = Physics.Raycast(rayStartPosition, frontDirection, rayLength, wallLayer);

        // ���C�����������ꍇ�Ƀt���O��true�ɐݒ�
        if (isTouchingWallFront)
        {
            isRayHit = true;
            //Debug.Log("true�ɁI");
        }
        else
        {
            isRayHit = false;
            //Debug.Log("false�ɁI");
        }

        // �f�o�b�O�p�̃��C��`��
        //Debug.DrawRay(rayStartPosition, frontDirection * rayLength, isTouchingWallFront ? Color.green : Color.red);

        // �ǐڐG�̃��O���o��
        //if (isTouchingWallFront) Debug.Log("�ǂɐG�ꂽ�I");
    }

    /*public Camera mainCamera;  // �J�������Q��
    public BulletManager bulletManager;  // BulletManager���Q��

    // �^�[�Q�b�g�ʒu���擾���郁�\�b�h
    public Vector3 GetTargetPosition()
    {
        // ���C���΂�
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        Vector3 targetPosition;

        // ���C�L���X�g���s��
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // ���C���q�b�g�����ʒu���擾
            targetPosition = hit.point;
        }
        else
        {
            // �q�b�g���Ȃ������ꍇ�́A�J�����̑O�����^�[�Q�b�g�Ƃ���
            targetPosition = ray.origin + ray.direction * 20f;
        }

        return targetPosition;
    }*/

    /*//���ˌ��I�u�W�F�N�g
    public GameObject Emitter_object;

    //���菈�����������Ȃ����C���[�𖳎����邽�߂̃��C���[�}�X�N
    public LayerMask mask = -1;

    // Update is called once per frame
    void FixedUpdate()
    {

        //���ˈʒu�ƕ���
        Vector3 emitpos = Emitter_object.transform.position;
        Vector3 emitdir = Emitter_object.transform.forward;

        //�V�������C���쐬����i���ˈʒu�ƕ���������킷���j
        Ray ray = new Ray(emitpos, emitdir);

        //�����������̂̏�񂪓���RaycastHit������
        RaycastHit hit;

        //�����R���W��������������邽�߂Ƀ��C�����C�L���X�g���Ă����������ǂ����Ԃ�
        bool isHit = Physics.Raycast(ray, out hit, 100.0f, mask);

        //�������Ă���Ȃ�
        if (isHit)
        {
            //���������Q�[���I�u�W�F�N�g���擾
            GameObject hitobject = hit.collider.gameObject;

            //�}�e���A���J���[�ύX
            //hitobject.GetComponent().material.color = Color.green;

            //���������I�u�W�F�N�g�폜
            //Destroy(hitobject);

            //���C�������������W
            Vector3 position = hit.point;

            //���C���������������蔻��I�u�W�F�N�g�̖ʂ̖@��
            Vector3 normal = hit.normal;

            //���C�̕����x�N�g��
            Vector3 direction = ray.direction;

            //���˃x�N�g���i���˕����������x�N�g���j
            Vector3 reflect_direction = 2 * normal * Vector3.Dot(normal, -direction) + direction;

            //���C�Ɣ��˃x�N�g���̂Ȃ��p�x(���W�A���j
            float rad = Mathf.Acos(Vector3.Dot(-ray.direction, reflect_direction) / ray.direction.magnitude * reflect_direction.magnitude);

            //���W�A����x�ɕϊ�
            float deg = rad * Mathf.Rad2Deg;

            //���ˊp�x��90�x�ȏゾ������E�E�E
            //if(deg>90)
            //{  
            //}

            ////////////// �f�o�b�O�p ////////////////

            //�i�f�o�b�O�p�j�p�x��\��
            Debug.Log(deg);

            //�i�f�o�b�O�p�j�V�������˗p���C���쐬����
            Ray reflect_ray = new Ray(position, reflect_direction);

            //�i�f�o�b�O�p�j���C����ʂɕ\������
            Debug.DrawLine(reflect_ray.origin, reflect_ray.origin + reflect_ray.direction * 100, Color.blue, 0);

        }

        ////////////// �f�o�b�O�p ////////////////

        //�i�f�o�b�O�p�j���˃��C��\��
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red, 0);

    }*/
}
