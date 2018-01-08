using UnityEngine;
using UnityEditor;
using RubicalMe.Renderers;

namespace RubicalMe
{
	public class Editor_EasySimpleDisplay : Editor
	{
		public override bool HasPreviewGUI ()
		{
			return true;
		}

		protected EditorRenderer Display {
			get {
				return privateDisplay;
			}
			set {
				privateDisplay = value;
			}
		}

		protected EditorRenderer privateDisplay;
		protected Rect previousPosition;

		public virtual void OnEnable ()
		{
			if (Display == null) {
				Display = new EditorRenderer ();
			}
			Display.OnIsDirty += Repaint;
		}

		public virtual void OnDisable ()
		{
			Display.OnIsDirty -= Repaint;
		}

		public override void DrawPreview (Rect previewArea)
		{
			if (Event.current.type == EventType.Repaint) {
				Display.Rect = previewArea;
			}
			if (Display.OnGUI ()) {
				Repaint ();
			}
		}

		public virtual void OnDestroy ()
		{
			Display.Cleanup ();
		}
		
	}
}