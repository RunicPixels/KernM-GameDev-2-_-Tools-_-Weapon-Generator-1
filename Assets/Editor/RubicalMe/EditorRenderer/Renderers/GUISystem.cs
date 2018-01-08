using UnityEngine;
using RubicalMe.RenderTools;
using System.Collections.Generic;

namespace RubicalMe
{
	namespace Renderers
	{
		public class GUISystem
		{

			public int GUIQueueSize {
				get {
					return GUIRenderQueue.Count;
				}
			}

			private List<GUIItem> GUIRenderQueue;

			public GUISystem ()
			{
				Clear ();
			}

			public void Draw (Rect r)
			{
				if (GUIQueueSize > 0) {
					DrawGUIItems (r);
				}
			}

			public EditorRendererEvent ProcessEvents (Event currentEvent, Rect r)
			{
				EditorRendererEvent e = null;
				for (int i = GUIQueueSize - 1; i >= 0; i--) {
					if ((e = GUIRenderQueue [i].ProcessEvents (currentEvent, r)) != null) {
						break;
					}
				}
				return e;
			}

			/// <summary>
			/// Adds a button with assigned texture to the GUISystem.
			/// </summary>
			/// <returns>The unique ID of the button, relevant for listening to interaction.</returns>
			public uint AddButton (Texture2D texture)
			{
				GUIItem newItem = new Button (texture);
				GUIRenderQueue.Add (newItem);
				return newItem.ID;
			}

			/// <summary>
			/// Adds a button with assigned texture to the GUISystem.
			/// </summary>
			/// <returns>The unique ID of the button, relevant for listening to interaction.</returns>
			public uint AddButton (Texture2D texture, Vector2 position)
			{
				GUIItem newItem = new Button (texture, new Rect (position, new Vector2 (texture.width, texture.height)));
				GUIRenderQueue.Add (newItem);
				return newItem.ID;
			}

			/// <summary>
			/// Adds a button with assigned texture to the GUISystem.
			/// </summary>
			/// <returns>The unique ID of the button, relevant for listening to interaction.</returns>
			public uint AddButton (Texture2D texture, Vector2 position, GUISnapMode snapmode)
			{
				GUIItem newItem = new Button (texture, new Rect (position, new Vector2 (texture.width, texture.height)), snapmode);
				GUIRenderQueue.Add (newItem);
				return newItem.ID;
			}

			/// <summary>
			/// Adds a button with assigned texture to the GUISystem.
			/// </summary>
			/// <returns>The unique ID of the button, relevant for listening to interaction.</returns>
			public uint AddButton (Texture2D texture, Rect rect, GUISnapMode snapmode)
			{
				GUIItem newItem = new Button (texture, rect, snapmode);
				GUIRenderQueue.Add (newItem);
				return newItem.ID;
			}

			public void Clear ()
			{
				GUIRenderQueue = new List<GUIItem> ();
			}

			protected void DrawGUIItems (Rect r)
			{
				for (int i = 0; i < GUIQueueSize; i++) {
					GUIRenderQueue [i].Draw (r);
				}
			}
		}
	}
}
