using UnityEngine;

namespace RubicalMe
{
	namespace RenderTools
	{
		public class Button : GUIItem
		{

			public Button (Texture2D texture)
			{
				Initialize (texture, new Rect (Vector2.zero, new Vector2 (texture.width, texture.height)), GUISnapMode.TopLeft);
			}

			public Button (Texture2D texture, Rect rect)
			{
				Initialize (texture, rect, GUISnapMode.TopLeft);
			}

			public Button (Texture2D texture, Rect rect, GUISnapMode snapmode)
			{
				Initialize (texture, rect, snapmode);
			}

			public override EditorRendererEvent ProcessEvents (Event currentEvent, Rect r)
			{
				//	Debug.Log ("ProcessHandler called on button");
				if (!Contains (r, currentEvent.mousePosition)) {
					//	Debug.Log ("Doesn't seem to contain");
					return null;
				}

				switch (currentEvent.type) {
				case EventType.MouseDown:
					if (currentEvent.button == 0) {
						return new EditorRendererEvent (ID, EditorRendererEvent.EventType.clicked);
					}
					break;
				default:
					return null;
				}

				return null;
			}
		}
	}
}