using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(StageInfoTable))]
public class StageInfoEditor : Editor {
	/// <summary>
	/// �t�@�C���̐�΃p�X���擾����
	/// </summary>
	/// <param name="fileName">�t�@�C����</param>
	/// <returns>��΃p�X</returns>
	public static string AbsolutePath(string fileName) {
		string appPath = Application.dataPath;

		return $"{appPath}/Resources/{fileName}.json";
	}

	/// <summary>
	/// JSON�t�@�C���Ƀe�[�u������������
	/// </summary>
	/// <param name="fileName">�t�@�C����</param>
	/// <param name="table">�e�[�u��</param>
	public void Save(string fileName, StageInfoTable table) {
		// �t�@�C���̐�΃p�X���擾����
		string absolutePath = AbsolutePath(fileName);

		// �e�[�u���𕶎���ɕϊ�����
		string jsonString = JsonUtility.ToJson(table, true);

		try {
			// �t�@�C���ɏ�������
			System.IO.File.WriteAllText(absolutePath, jsonString);
		} catch (Exception e) {
			// ���s�����烍�O���o��
			Debug.LogError("Failed to save : " + e.Message);
		}
	}

	/// <summary>
	/// JSON�t�@�C������e�[�u����ǂݏo��
	/// </summary>
	/// <param name="fileName">�t�@�C����</param>
	/// <param name="table">�e�[�u��</param>
	public void Load(string fileName, StageInfoTable table) {
		// �t�@�C���̐�΃p�X���擾����
		string absolutePath = AbsolutePath(fileName);

		// �t�@�C������ǂݍ���
		string jsonString = System.IO.File.ReadAllText(absolutePath);

		try {
			// �e�[�u���ɔ��f������
			JsonUtility.FromJsonOverwrite(jsonString, table);
		} catch (Exception e) {
			// ���s�����烍�O���o��
			Debug.LogError("Failed to save : " + e.Message);
		}
	}

	public override bool RequiresConstantRepaint() => true;

	public override void OnInspectorGUI() {
		StageInfoTable importer = target as StageInfoTable;

		DrawDefaultInspector();

		// �Z�[�u�{�^���������ăZ�[�u
		if (GUILayout.Button("�Z�[�u")) {
			Save("StageInfoTable", importer);
		}

		// ���[�h�{�^���������ă��[�h
		if (GUILayout.Button("���[�h")) {
			Load("StageInfoTable", importer);
		}
	}
}
#endif