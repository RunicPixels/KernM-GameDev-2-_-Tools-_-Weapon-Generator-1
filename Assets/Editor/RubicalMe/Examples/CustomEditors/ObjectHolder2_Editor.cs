using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RubicalMe;

[CustomEditor (typeof(ScriptableObjectHolder2))]
public class ObjectHolder2_Editor : Editor_EasyInteractiveDisplay
{
	SerializedProperty prefab;
	SerializedProperty rotation;

	private uint buttonBackID;
	private uint buttonForwardID;

	public override void OnEnable ()
	{
		base.OnEnable ();
		prefab = serializedObject.FindProperty ("prefab");
		rotation = serializedObject.FindProperty ("rotation");

		if (prefab.objectReferenceValue != null) {
			SetObjectInDisplay ();
		}

		Texture2D backButtonTex = Resources.Load ("back") as Texture2D;
		Texture2D forwardButtonTex = Resources.Load ("forward") as Texture2D;
		buttonBackID = Display.GUISystem.AddButton (backButtonTex, Vector2.zero, GUISnapMode.BottomLeft);
		buttonForwardID = Display.GUISystem.AddButton (forwardButtonTex, Vector2.zero, GUISnapMode.BottomRight);
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (prefab);
		if (EditorGUI.EndChangeCheck ()) {
			rotation.vector3Value = Vector3.zero;
			SetObjectInDisplay ();
		}

		serializedObject.ApplyModifiedProperties ();
	}

	protected override void EventHandler (EditorRendererEvent e)
	{
		serializedObject.Update ();
		switch (e.type) {
		case EditorRendererEvent.EventType.clicked:
			if (e.buttonID == buttonBackID) {
				rotation.vector3Value -= new Vector3 (0, 45, 0);
				serializedObject.ApplyModifiedProperties ();
				SetObjectInDisplay ();
			}
			if (e.buttonID == buttonForwardID) {
				rotation.vector3Value += new Vector3 (0, 45, 0);
				serializedObject.ApplyModifiedProperties ();
				SetObjectInDisplay ();
			}
			break;
		default:
			throw new System.NotImplementedException ("EventType has not been implemented: " + e.type);
		}
	}

	private void SetObjectInDisplay ()
	{
		Display.ClearRenderQueue ();
		Display.AddGameObject (prefab.objectReferenceValue as GameObject, Vector3.zero, rotation.vector3Value);
	}
}