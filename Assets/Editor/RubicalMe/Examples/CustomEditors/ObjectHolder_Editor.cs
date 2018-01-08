#define SIMPLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RubicalMe;



#if SIMPLE
[CustomEditor (typeof(ScriptableObjectHolder))]
public class ObjectHolder_Editor : Editor_EasySimpleDisplay
{
	SerializedProperty prefab;

	public override void OnEnable ()
	{
		base.OnEnable ();
		prefab = serializedObject.FindProperty ("prefab");
		SetObjectInDisplay ();
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (prefab, true);
		if (EditorGUI.EndChangeCheck ()) {
			SetObjectInDisplay ();
		}

		serializedObject.ApplyModifiedProperties ();
	}

	private void SetObjectInDisplay ()
	{
		Display.ClearRenderQueue ();
		if (prefab.arraySize != 0) {
			for (int i = 0; i < prefab.arraySize; i++) {
				Display.AddGameObject (prefab.GetArrayElementAtIndex (i).objectReferenceValue as GameObject, new Vector3 (i, 0, 0), Quaternion.identity);
			}
		}
	}
}












#else
[CustomEditor (typeof(ScriptableObjectHolder))]
public class ObjectHolder_Editor : Editor
{
	public override bool HasPreviewGUI ()
	{
		return true;
	}

	SerializedProperty prefab;
	protected PreviewRenderUtility renderUtil;

	public void OnEnable ()
	{
		prefab = serializedObject.FindProperty ("prefab");
		renderUtil = new PreviewRenderUtility ();
		renderUtil.camera.transform.position = new Vector3 (0, 0, -10);
		renderUtil.camera.transform.rotation = Quaternion.identity;
		renderUtil.camera.orthographic = true;
		renderUtil.camera.farClipPlane = 150;
		renderUtil.lights [1].enabled = true;
		renderUtil.lights [0].transform.rotation = Quaternion.Euler (20, -10, 0);
		renderUtil.lights [0].intensity = 1f;
		renderUtil.lights [1].transform.rotation = Quaternion.Euler (50, 50, 0);
		renderUtil.lights [1].intensity = 0.35f;
	}

	public void OnDisable ()
	{
		renderUtil.Cleanup ();
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (prefab, true);
		if (EditorGUI.EndChangeCheck ()) {
		}

		serializedObject.ApplyModifiedProperties ();
	}

	public override void DrawPreview (Rect previewArea)
	{
		if (Event.current.type == EventType.Repaint && prefab.arraySize != 0) {
			if (renderUtil == null) {
				throw new System.NullReferenceException ("RenderUtil was not initialized");
			}


			renderUtil.BeginPreview (previewArea, GUI.skin.box);

			for (int i = 0; i < prefab.arraySize; i++) {
				GameObject rItem = prefab.GetArrayElementAtIndex (i).objectReferenceValue as GameObject;
				renderUtil.DrawMesh (
					rItem.GetComponent <MeshFilter> ().sharedMesh, 
					Matrix4x4.identity, 
					rItem.GetComponent <MeshRenderer> ().sharedMaterial,
					0
				);
			}


			renderUtil.camera.Render ();

			GUI.DrawTexture (previewArea, renderUtil.EndPreview ());
		}
	}
}
#endif