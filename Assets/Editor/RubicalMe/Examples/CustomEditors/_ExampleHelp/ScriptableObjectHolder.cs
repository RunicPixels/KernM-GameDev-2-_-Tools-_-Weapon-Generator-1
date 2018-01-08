using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ObjectHolder", menuName = "ObjectHolder", order = 1)]
public class ScriptableObjectHolder : ScriptableObject
{
	public GameObject[] prefab;
}