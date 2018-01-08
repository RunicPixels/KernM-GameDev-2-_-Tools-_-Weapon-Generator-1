using UnityEngine;
using UnityEditor;
using RubicalMe.Renderers;

namespace RubicalMe
{
	public class EditorWindow_EasySimpleDisplay : EditorWindow
	{
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

		public virtual Rect InteractiveRendererDisplay {
			get {
				return new Rect (
					new Vector2 (0, position.height < 600 ? position.height / 2 : position.height - 300), 
					new Vector2 (position.width, position.height < 600 ? position.height / 2 : 300)
				); 
			}
		}

		protected bool PositionChanged {
			get {
				if (position == previousPosition) {
					return false;
				} else {
					previousPosition = position;
					return true;
				}
			}
		}

		public virtual void OnGUI ()
		{
			if (Event.current.type == EventType.Repaint) {
				if (PositionChanged) {
					Display.Rect = InteractiveRendererDisplay;
				}
			}
			if (Display.OnGUI ()) {
				Repaint ();
			}
		}

		public virtual void OnEnable ()
		{
			if (Display == null) {
				Display = new EditorRenderer (InteractiveRendererDisplay);
			}
			Display.OnIsDirty += Repaint;
		}

		public virtual void OnDisable ()
		{
			Display.OnIsDirty -= Repaint;
		}

		public virtual void OnDestroy ()
		{
			Display.Cleanup ();
		}

	}
}
