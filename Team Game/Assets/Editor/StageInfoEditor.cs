using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(StageInfoTable))]
public class StageInfoEditor : Editor {
	/// <summary>
	/// ファイルの絶対パスを取得する
	/// </summary>
	/// <param name="fileName">ファイル名</param>
	/// <returns>絶対パス</returns>
	public static string AbsolutePath(string fileName) {
		string appPath = Application.dataPath;

		return $"{appPath}/Resources/{fileName}.json";
	}

	/// <summary>
	/// JSONファイルにテーブルを書き込む
	/// </summary>
	/// <param name="fileName">ファイル名</param>
	/// <param name="table">テーブル</param>
	public void Save(string fileName, StageInfoTable table) {
		try {
			// 現在のテーブルのデータをファイルに書き込む
			System.IO.File.WriteAllText(
				AbsolutePath(fileName),
				JsonUtility.ToJson(table, true)
			);

			Debug.Log("Saving completed.");
		} catch (Exception e) {
			// 失敗したらログを出す
			Debug.LogError("Failed to save : " + e.Message);
		}
	}

	/// <summary>
	/// JSONファイルからテーブルを読み出す
	/// </summary>
	/// <param name="fileName">ファイル名</param>
	/// <param name="table">テーブル</param>
	public void Load(string fileName, StageInfoTable table) {
		try {
			// ファイルから読み出したデータを現在のテーブルに反映させる
			JsonUtility.FromJsonOverwrite(
				System.IO.File.ReadAllText(AbsolutePath(fileName)),
				table
			);

			Debug.Log("Loading completed.");
		} catch (Exception e) {
			// 失敗したらログを出す
			Debug.LogError("Failed to save : " + e.Message);
		}
	}

	public override bool RequiresConstantRepaint() => true;

	public override void OnInspectorGUI() {
		StageInfoTable importer = target as StageInfoTable;

		DrawDefaultInspector();

		// セーブボタンを押してセーブ
		if (GUILayout.Button("セーブ")) {
			Save("StageInfoTable", importer);
		}

		// ロードボタンを押してロード
		if (GUILayout.Button("ロード")) {
			Load("StageInfoTable", importer);
		}
	}
}
#endif