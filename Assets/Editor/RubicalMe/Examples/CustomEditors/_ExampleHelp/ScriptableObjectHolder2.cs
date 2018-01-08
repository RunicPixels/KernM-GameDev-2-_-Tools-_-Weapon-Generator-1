using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ObjectHolder2", menuName = "ObjectHolder2", order = 1)]
public class ScriptableObjectHolder2 : ScriptableObject
{
	public GameObject prefab;
	public Vector3 rotation;
}