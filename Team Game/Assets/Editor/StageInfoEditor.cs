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
		try {
			// ���݂̃e�[�u���̃f�[�^���t�@�C���ɏ�������
			System.IO.File.WriteAllText(
				AbsolutePath(fileName),
				JsonUtility.ToJson(table, true)
			);

			Debug.Log("Saving completed.");
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
		try {
			// �t�@�C������ǂݏo�����f�[�^�����݂̃e�[�u���ɔ��f������
			JsonUtility.FromJsonOverwrite(
				System.IO.File.ReadAllText(AbsolutePath(fileName)),
				table
			);

			Debug.Log("Loading completed.");
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