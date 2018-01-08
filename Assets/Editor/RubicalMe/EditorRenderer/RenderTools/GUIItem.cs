using UnityEngine;

namespace RubicalMe
{
	namespace RenderTools
	{
		public class GUIItem
		{
			protected static uint GUIItemsAlive;

			public Texture2D Texture {
				get {
					return texture;
				}
			}

			public Rect Rect {
				get {
					return rect;
				}
			}

			public GUISnapMode SnapMode {
				get {
					return snapmode;
				}
			}

			public uint ID {
				get {
					return mID;
				}
			}

			protected Texture2D texture;
			protected Rect rect;
			protected GUISnapMode snapmode;
			protected uint mID;

			protected GUIItem ()
			{
				
			}

			public GUIItem (Texture2D texture)
			{
				Initialize (texture, new Rect (Vector2.zero, new Vector2 (texture.width, texture.height)), GUISnapMode.TopLeft);
			}

			public GUIItem (Texture2D texture, Rect rect)
			{
				Initialize (texture, rect, GUISnapMode.TopLeft);
			}

			public GUIItem (Texture2D texture, Rect rect, GUISnapMode snapmode)
			{
				Initialize (texture, rect, snapmode);
			}

			public void SetTexture (Texture2D texture)
			{
				this.texture = texture;
			}

			public void SetPosition (Rect rect)
			{
				this.rect = rect;
			}

			public void SetSnapMode (GUISnapMode snapmode)
			{
				this.snapmode = snapmode;
			}

			public void Draw (Rect r)
			{
				GUI.DrawTexture (new Rect (r.position + rect.position + GetOffset (r), rect.size), texture);
			}

			public virtual EditorRendererEvent ProcessEvents (Event currentEvent, Rect r)
			{
				return null;
			}

			public bool Contains (Rect r, Vector2 position)
			{
				return new Rect (r.position + rect.position + GetOffset (r), rect.size).Contains (position);
			}

			protected Vector2 GetOffset (Rect r)
			{
				switch (snapmode) {
				case GUISnapMode.TopLeft:
					return Vector2.zero;
				case GUISnapMode.TopCenter:
					return new Vector2 (r.width / 2, 0) - new Vector2 (rect.width / 2, 0);
				case GUISnapMode.TopRight:
					return new Vector2 (r.width, 0) - new Vector2 (rect.width, 0);
				case GUISnapMode.MidLeft:
					return new Vector2 (0, r.height / 2) - new Vector2 (0, rect.height / 2);
				case GUISnapMode.MidCenter:
					return new Vector2 (r.width / 2, r.height / 2) - new Vector2 (rect.width / 2, rect.height / 2);
				case GUISnapMode.MidRight:
					return new Vector2 (r.width, r.height / 2) - new Vector2 (rect.width, rect.height / 2);
				case GUISnapMode.BottomLeft:
					return new Vector2 (0, r.height) - new Vector2 (0, rect.height);
				case GUISnapMode.BottomCenter:
					return new Vector2 (r.width / 2, r.height) - new Vector2 (rect.width / 2, rect.height);
				case GUISnapMode.BottomRight:
					return r.size - rect.size;
				default:
					throw new System.NotImplementedException ("This GUISnapMode has not yet been implemented for SetSnapMode - GUIItem");
				}
			}

			protected void Initialize (Texture2D texture, Rect rect, GUISnapMode snapmode)
			{
				this.texture = texture;
				this.rect = rect;
				this.snapmode = snapmode;
				this.mID = GUIItemsAlive++;
			}
		}
	}
}