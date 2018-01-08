using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RubicalMe.RenderTools;


// thanks to https://gist.github.com/prodigga/53ab658e1a818cd4ddfd

namespace RubicalMe
{
	namespace Renderers
	{

		public class InteractiveEditorRenderer : EditorRenderer
		{
			public delegate void InputEvent ();

			public delegate void GUIEvent (EditorRendererEvent e);

			public GUISystem GUISystem {
				get {
					if (guiSystem == null) {
						return new GUISystem ();
					}
					return guiSystem;
				}
			}

			public GUIEvent OnGUIEvent;
			public InputEvent OnMouseDown;
			public InputEvent OnMouseUp;
			public Vector2 mousePosition;
			public Vector2 mousePositionWorld;

			protected GUISystem guiSystem;

			public InteractiveEditorRenderer ()
			{
				guiSystem = new GUISystem ();
			}

			public InteractiveEditorRenderer (Rect r)
			{
				this.rect = r;
				guiSystem = new GUISystem ();
			}

			// THIS IS NOT A UNITY-CALLED METHOD
			public override bool OnGUI ()
			{
				if (ProcessEvents (Event.current)) {
					return true;
				}
				return base.OnGUI ();
			}

			public List<RenderItem> GetRenderItemsAt (Vector3 position)
			{
				List<RenderItem> rItems = new List<RenderItem> ();
				foreach (RenderItem rItem in RenderQueue) {
					if (rItem.Position == position) {
						rItems.Add (rItem);
					}
				}
				return rItems;
			}

			public void ReplaceRenderItem (RenderItem oldItem, RenderItem newItem)
			{
				int index = RenderQueue.IndexOf (oldItem);
				RenderQueue.Remove (oldItem);
				RenderQueue.Insert (index, newItem);
				CallDirty ();
			}

			protected override void Display ()
			{
				base.Display ();
				guiSystem.Draw (rect);
			}

			protected bool ProcessEvents (Event currentEvent)
			{	
				if (rect.Contains (currentEvent.mousePosition)) {
					//	mousePosition = currentEvent.mousePosition - rect.position;
					EditorRendererEvent e = guiSystem.ProcessEvents (currentEvent, rect);
					if (e != null) {
						if (OnGUIEvent != null) {
							OnGUIEvent (e);
						}
						return true;
					}

					if (renderUtil.camera.orthographic) {
						Vector2 invertedY = new Vector2 (mousePosition.x, rect.height - mousePosition.y);
						mousePositionWorld = renderUtil.camera.ScreenToWorldPoint (invertedY * renderUtil.GetScaleFactor (rect.width, rect.height));
					}

					switch (currentEvent.type) {
					case EventType.MouseDown:
						if (OnMouseDown != null) {
							OnMouseDown ();
						}
						break;

					case EventType.MouseUp:
						if (OnMouseUp != null) {
							OnMouseUp ();
						}
						break;
					}
				} else {
					mousePosition = new Vector2 (-1, -1);
					mousePositionWorld = Vector2.zero;
				}

				return false;
			}
		}
	}
}