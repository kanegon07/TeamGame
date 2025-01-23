using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageInfo {
	public int StageID;
	public int TimeLimit;
	public int BerryCount;
}

[CreateAssetMenu(fileName = "StageInfoTable", menuName = "ScriptableObject/StageInfoTable", order = 1)]
public class StageInfoTable : ScriptableObject {
	public List<StageInfo> Table;
}