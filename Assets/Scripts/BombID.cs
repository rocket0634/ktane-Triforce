using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This component is required to be on the top level of the prefab if used.
public class BombID : MonoBehaviour {

	public string ModID
	{
		get
		{
			return GetComponent<ModSource>().ModName;
		}
	}
	public string ID;
}
