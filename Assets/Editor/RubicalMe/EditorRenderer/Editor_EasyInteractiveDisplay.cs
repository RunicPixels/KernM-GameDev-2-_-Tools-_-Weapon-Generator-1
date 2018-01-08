using UnityEngine;
using UnityEditor;
using RubicalMe.Renderers;

namespace RubicalMe
{
	public class Editor_EasyInteractiveDisplay : Editor_EasySimpleDisplay
	{
		new protected InteractiveEditorRenderer Display {
			get {
				return privateDisplay as InteractiveEditorRenderer;
			}
			set {
				privateDisplay = value;
			}
		}

		public override void OnEnable ()
		{
			if (Display == null) {
				Display = new InteractiveEditorRenderer ();
			}
			Display.OnGUIEvent += EventHandler;
			Display.OnIsDirty += Repaint;
		}

		public override void OnDisable ()
		{
			Display.OnGUIEvent -= EventHandler;
			Display.OnIsDirty -= Repaint;
		}

		protected virtual void EventHandler (EditorRendererEvent e)
		{
			throw new System.NotImplementedException ("EventHandler has not been overridden but GUIEvens are called from " + this.GetType ());
		}
	}
}