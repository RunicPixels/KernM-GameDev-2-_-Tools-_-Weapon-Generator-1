using UnityEngine;
using UnityEditor;
using RubicalMe;

public class Example_EasySimpleDisplay : EditorWindow_EasySimpleDisplay
{
	private Texture2D gameObject;

	private Vector3 rotationEuler;

	[MenuItem ("Tools/RubicalMe/Examples/EasySimpleDisplay")]
	static void Init ()
	{
		// Get existing open window or if none, make a new one:
		Example_EasySimpleDisplay window = (Example_EasySimpleDisplay)EditorWindow.GetWindow (typeof(Example_EasySimpleDisplay));
		window.Show ();
	}

	public override void OnGUI ()
	{
		base.OnGUI ();

		EditorGUI.BeginChangeCheck ();
		gameObject = (Texture2D)EditorGUILayout.ObjectField (gameObject, typeof(Texture2D), true);
		if (EditorGUI.EndChangeCheck ()) {
			rotationEuler = Vector3.zero;
			SetObjectInDisplay ();
		}

		EditorGUI.BeginChangeCheck ();
		rotationEuler = EditorGUILayout.Vector3Field ("Rotation", rotationEuler);
		if (EditorGUI.EndChangeCheck ()) {
			SetObjectInDisplay ();
		}
	}

	private void SetObjectInDisplay ()
	{
		Display.ClearRenderQueue ();
		Display.AddTexture (gameObject, 50);
	}
}
