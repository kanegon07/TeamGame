using System;
using UnityEngine;

[Serializable]
public class SignMessage {
	public int SignID = -1;
	public string Name = null;
	public string Message = null;
}

[CreateAssetMenu(fileName = "SignMessageTable", menuName = "ScriptableObject/SignMessageTable", order = 1)]
public class SignMessageTable : ScriptableObject {
	public SignMessage[] Table = null;

	public SignMessage Find(int signID) {
		foreach (SignMessage msg in Table) {
			if (msg.SignID == signID) {
				return msg;
			}
		}

		return null;
	}
}