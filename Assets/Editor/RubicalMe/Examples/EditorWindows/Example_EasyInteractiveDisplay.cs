using UnityEngine;
using UnityEditor;
using RubicalMe;

public class Example_EasyInteractiveDisplay : EditorWindow_EasyInteractiveDisplay
{
	private GameObject gameObject;
	private Vector3 rotationEuler;

	private uint buttonBackID;
	private uint buttonForwardID;

	[MenuItem ("Tools/RubicalMe/Examples/EasyInteractiveDisplay")]
	static void Init ()
	{
		// Get existing open window or if none, make a new one:
		Example_EasyInteractiveDisplay window = (Example_EasyInteractiveDisplay)EditorWindow.GetWindow (typeof(Example_EasyInteractiveDisplay));
		window.Show ();
	}

	public override void OnGUI ()
	{
		base.OnGUI ();
		EditorGUI.BeginChangeCheck ();
		gameObject = (GameObject)EditorGUILayout.ObjectField (gameObject, typeof(GameObject), true);
		if (EditorGUI.EndChangeCheck ()) {
			rotationEuler = Vector3.zero;
			SetObjectInDisplay ();
		}
	}

	public override void OnEnable ()
	{
		base.OnEnable ();
		Texture2D backButtonTex = Resources.Load ("back") as Texture2D;
		Texture2D forwardButtonTex = Resources.Load ("forward") as Texture2D;
		buttonBackID = Display.GUISystem.AddButton (backButtonTex, Vector2.zero, GUISnapMode.BottomLeft);
		buttonForwardID = Display.GUISystem.AddButton (forwardButtonTex, Vector2.zero, GUISnapMode.BottomRight);
	}

	protected override void EventHandler (EditorRendererEvent e)
	{
		switch (e.type) {
		case EditorRendererEvent.EventType.clicked:
			if (e.buttonID == buttonBackID) {
				rotationEuler -= new Vector3 (0, 45, 0);
				SetObjectInDisplay ();
			}
			if (e.buttonID == buttonForwardID) {
				rotationEuler += new Vector3 (0, 45, 0);
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
		Display.AddGameObject (gameObject, Vector3.zero, rotationEuler);
	}
}