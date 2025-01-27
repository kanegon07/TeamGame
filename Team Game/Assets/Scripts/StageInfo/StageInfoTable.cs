using System;
using UnityEngine;

[Serializable]
public class StageInfo {
	public int StageID = -1;
	public int TimeLimit = 0;
	public int BerryCount = 0;
}

[CreateAssetMenu(fileName = "StageInfoTable", menuName = "ScriptableObject/StageInfoTable", order = 1)]
public class StageInfoTable : ScriptableObject {
	public StageInfo[] Table = null;

	public StageInfo Find(int stageID) {
		foreach (StageInfo info in Table) {
			if (info.StageID == stageID) {
				return info;
			}
		}

		return null;
	}
}