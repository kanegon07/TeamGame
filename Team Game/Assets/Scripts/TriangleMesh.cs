using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DownwardArrowMesh : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();

        // 頂点の定義（矢印を下向きに配置）
        Vector3[] vertices = new Vector3[]
        {
            // 矢印の先端部分の頂点（四角錐）
            new Vector3(0, 0, -0.5f),   // 0: 頂点（矢印の先端）
            new Vector3(-0.1f, -0.1f, -0.2f),  // 1: 底面左下
            new Vector3(0.1f, -0.1f, -0.2f),   // 2: 底面右下
            new Vector3(0.1f, 0.1f, -0.2f),    // 3: 底面右上
            new Vector3(-0.1f, 0.1f, -0.2f),   // 4: 底面左上

            // 矢印の軸部分の頂点（四角柱のように配置）
            new Vector3(-0.05f, -0.05f, -0.2f), // 5: 軸左下前
            new Vector3(0.05f, -0.05f, -0.2f),  // 6: 軸右下前
            new Vector3(0.05f, 0.05f, -0.2f),   // 7: 軸右上前
            new Vector3(-0.05f, 0.05f, -0.2f),  // 8: 軸左上前

            new Vector3(-0.05f, -0.05f, 0.5f), // 9: 軸左下後
            new Vector3(0.05f, -0.05f, 0.5f),  // 10: 軸右下後
            new Vector3(0.05f, 0.05f, 0.5f),   // 11: 軸右上後
            new Vector3(-0.05f, 0.05f, 0.5f)   // 12: 軸左上後
        };

        // 各面の三角形インデックスを定義
        int[] triangles = new int[]
        {
            // 矢印の先端（四角錐の側面）
            0, 1, 2,  // 側面1
            0, 2, 3,  // 側面2
            0, 3, 4,  // 側面3
            0, 4, 1,  // 側面4

            // 矢印の先端の底面
            1, 4, 3,
            1, 3, 2,

            // 矢印の軸（四角柱の側面）
            5, 6, 10, // 側面1
            5, 10, 9, // 側面1（続き）

            6, 7, 11, // 側面2
            6, 11, 10, // 側面2（続き）

            7, 8, 12, // 側面3
            7, 12, 11, // 側面3（続き）

            8, 5, 9,  // 側面4
            8, 9, 12, // 側面4（続き）

            // 矢印の軸の前面と背面
            5, 8, 7,
            5, 7, 6,

            9, 10, 11,
            9, 11, 12
        };

        // メッシュに設定
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // メッシュフィルターに設定
        GetComponent<MeshFilter>().mesh = mesh;
    }
}