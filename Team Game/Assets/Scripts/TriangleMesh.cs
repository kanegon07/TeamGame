using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DownwardArrowMesh : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();

        // ���_�̒�`�i�����������ɔz�u�j
        Vector3[] vertices = new Vector3[]
        {
            // ���̐�[�����̒��_�i�l�p���j
            new Vector3(0, 0, -0.5f),   // 0: ���_�i���̐�[�j
            new Vector3(-0.1f, -0.1f, -0.2f),  // 1: ��ʍ���
            new Vector3(0.1f, -0.1f, -0.2f),   // 2: ��ʉE��
            new Vector3(0.1f, 0.1f, -0.2f),    // 3: ��ʉE��
            new Vector3(-0.1f, 0.1f, -0.2f),   // 4: ��ʍ���

            // ���̎������̒��_�i�l�p���̂悤�ɔz�u�j
            new Vector3(-0.05f, -0.05f, -0.2f), // 5: �������O
            new Vector3(0.05f, -0.05f, -0.2f),  // 6: ���E���O
            new Vector3(0.05f, 0.05f, -0.2f),   // 7: ���E��O
            new Vector3(-0.05f, 0.05f, -0.2f),  // 8: ������O

            new Vector3(-0.05f, -0.05f, 0.5f), // 9: ��������
            new Vector3(0.05f, -0.05f, 0.5f),  // 10: ���E����
            new Vector3(0.05f, 0.05f, 0.5f),   // 11: ���E���
            new Vector3(-0.05f, 0.05f, 0.5f)   // 12: �������
        };

        // �e�ʂ̎O�p�`�C���f�b�N�X���`
        int[] triangles = new int[]
        {
            // ���̐�[�i�l�p���̑��ʁj
            0, 1, 2,  // ����1
            0, 2, 3,  // ����2
            0, 3, 4,  // ����3
            0, 4, 1,  // ����4

            // ���̐�[�̒��
            1, 4, 3,
            1, 3, 2,

            // ���̎��i�l�p���̑��ʁj
            5, 6, 10, // ����1
            5, 10, 9, // ����1�i�����j

            6, 7, 11, // ����2
            6, 11, 10, // ����2�i�����j

            7, 8, 12, // ����3
            7, 12, 11, // ����3�i�����j

            8, 5, 9,  // ����4
            8, 9, 12, // ����4�i�����j

            // ���̎��̑O�ʂƔw��
            5, 8, 7,
            5, 7, 6,

            9, 10, 11,
            9, 11, 12
        };

        // ���b�V���ɐݒ�
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // ���b�V���t�B���^�[�ɐݒ�
        GetComponent<MeshFilter>().mesh = mesh;
    }
}