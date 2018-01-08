using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RubicalMe.RenderTools;

namespace RubicalMe
{
	namespace Renderers
	{
		/// <summary>
		/// <para>
		/// The EditorRenderer is the most basic Renderer in the RubicalMe XXXX tool. 
		/// It allows for simple previews of 3D scenes via a simple Texture return.
		/// </para>
		/// <para>
		/// The options built in:
		/// </para>
		/// <para>
		/// - adding objects at a position, rotation and scale 
		/// </para>
		/// <para>
		/// - render all added objects to a texture
		/// </para>
		/// </summary>
		public class EditorRenderer
		{
			public delegate void EditorRendererMessage ();

			public Camera Camera {
				get {
					return renderUtil.camera;
				}
			}

			public int RenderQueueSize {
				get {
					return RenderQueue.Count;
				}
			}

			public Rect Rect {
				get {
					return rect;
				}
				set {
					rect = value;
				}
			}

			public EditorRendererMessage OnIsDirty;
			protected PreviewRenderUtility renderUtil;
			protected List<RenderItem> RenderQueue;
			protected Rect rect;

			public EditorRenderer ()
			{
				Setup (new Rect (0, 0, 1, 1));
			}

			public EditorRenderer (Rect r)
			{
				Setup (r);
			}

			// THIS IS NOT A UNITY-CALLED METHOD
			public virtual bool OnGUI ()
			{
				if (Event.current.type == EventType.Repaint) {
					Display ();
				}
				return false;
			}

			/// <summary>
			/// Render the added objects and return as Texture.
			/// </summary>
			/// <param name="r">The Rect component used for dimensions.</param>
			/// <param name="background">The GUIStyle which will be used for the background.</param>
			public Texture Render (Rect r)
			{
				if (renderUtil == null) {
					throw new System.NullReferenceException ("RenderUtil was not initialized");
				}

				renderUtil.BeginPreview (r, GUI.skin.box);

				foreach (RenderItem rItem in RenderQueue) {
					renderUtil.DrawMesh (rItem.Mesh, rItem.Matrix, rItem.Material, 0);
				}

				renderUtil.camera.Render ();

				return renderUtil.EndPreview ();
			}

			public void ClearRenderQueue ()
			{
				RenderQueue = new List<RenderItem> ();
				CallDirty ();
			}

			/// <summary>
			/// Adds the specified gameObject at the zero point. Localrotation of gameObject will be assumed as rotation. Localscale of gameObject will be assumed as scale.
			/// </summary>
			/// <param name="gameObject">Object to render, MeshRenderer and MeshFilter componens are required.</param>
			public void AddGameObject (GameObject gameObject)
			{
				AddGameObjectInternal (gameObject, Vector3.zero, gameObject.transform.localRotation, gameObject.transform.localScale);
				CallDirty ();
			}

			/// <summary>
			/// Adds the specified gameObject at position and applies rotation. Localscale of gameObject will be assumed as scale.
			/// </summary>
			/// <param name="gameObject">Object to render, MeshRenderer and MeshFilter componens are required.</param>
			public void AddGameObject (GameObject gameObject, Vector3 position, Quaternion rotation)
			{
				AddGameObjectInternal (gameObject, position, rotation, gameObject.transform.localScale);
				CallDirty ();
			}

			/// <summary>
			/// Adds the specified gameObject at position and applies rotation of eulerangles. Localscale of gameObject will be assumed as scale.
			/// </summary>
			/// <param name="gameObject">Object to render, MeshRenderer and MeshFilter componens are required.</param>
			public void AddGameObject (GameObject gameObject, Vector3 position, Vector3 eulerAngles)
			{
				AddGameObjectInternal (gameObject, position, Quaternion.Euler (eulerAngles), gameObject.transform.localScale);
				CallDirty ();
			}

			/// <summary>
			/// Adds the specified gameObject at position and applies rotation of eulerangles and scale. 
			/// </summary>
			/// <param name="gameObject">Object to render, MeshRenderer and MeshFilter componens are required.</param>
			public void AddGameObject (GameObject gameObject, Vector3 position, Vector3 eulerAngles, Vector3 scale)
			{
				AddGameObjectInternal (gameObject, position, Quaternion.Euler (eulerAngles), scale);
				CallDirty ();
			}

			/// <summary>
			/// Adds the specified gameObject at position and applies rotation and scale. 
			/// </summary>
			/// <param name="gameObject">Object to render, MeshRenderer and MeshFilter componens are required.</param>
			public void AddGameObject (GameObject gameObject, Vector3 position, Quaternion rotation, Vector3 scale)
			{
				AddGameObjectInternal (gameObject, position, rotation, scale);
				CallDirty ();
			}

			/// <summary>
			/// Adds a Texture2D in world space on a plane with a width of 1 unit and a ratio-scaled height.
			/// </summary>
			public void AddTexture (Texture2D texture)
			{
				AddTextureInternal (texture, Vector3.zero, Quaternion.identity, new Vector3 (1, 1, 1), 100);
			}

			/// <summary>
			/// Adds a Texture2D in world space on a plane with a width of 1 unit and a ratio-scaled height.
			/// </summary>
			public void AddTexture (Texture2D texture, float pixelRatio)
			{
				AddTextureInternal (texture, Vector3.zero, Quaternion.identity, new Vector3 (1, 1, 1), pixelRatio);
			}

			/// <summary>
			/// Adds a Texture2D in world space on a plane with a width of 1 unit and a ratio-scaled height.
			/// </summary>
			public void AddTexture (Texture2D texture, Vector3 position, float pixelRatio)
			{
				AddTextureInternal (texture, position, Quaternion.identity, new Vector3 (1, 1, 1), pixelRatio);
			}

			/// <summary>
			/// Adds a Texture2D in world space on a plane with a width of 1 unit and a ratio-scaled height.
			/// </summary>
			public void AddTexture (Texture2D texture, Vector3 position, Quaternion rotation, float pixelRatio)
			{
				AddTextureInternal (texture, position, rotation, new Vector3 (1, 1, 1), pixelRatio);
			}

			/// <summary>
			/// Adds a Texture2D in world space on a plane with a width of 1 unit and a ratio-scaled height.
			/// </summary>
			public void AddTexture (Texture2D texture, Vector3 position, Quaternion rotation, Vector3 scale, float pixelRatio)
			{
				AddTextureInternal (texture, position, rotation, scale, pixelRatio);
			}

			protected void AddTextureInternal (Texture2D texture, Vector3 position, Quaternion rotation, Vector3 scale, float pixelRatio)
			{

				float width = texture.height / pixelRatio;
				float height = texture.width / pixelRatio;

				Mesh m = new Mesh ();
				m.vertices = new Vector3[] {
					new Vector3 (-width / 2, -height / 2 /** yScaleFactor*/, 0.01f),
					new Vector3 (width / 2, -height / 2, 0.01f),
					new Vector3 (width / 2, height / 2, 0.01f),
					new Vector3 (-width / 2, height / 2, 0.01f)
				};

				foreach (var item in m.vertices) {
					Debug.Log (item);
				}

				m.uv = new Vector2[] { 
					new Vector2 (0, 0), 
					new Vector2 (0, 1), 
					new Vector2 (1, 1), 
					new Vector2 (1, 0) 
				};
				m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
				m.RecalculateNormals ();

				Material mat = new Material (Shader.Find ("Unlit/Transparent"));
				mat.mainTexture = texture;
				Vector3 faceCameraEuler = new Vector3 (0, 180, 90);

				RenderQueue.Add (new RenderItem (m, mat, position, Quaternion.Euler (rotation.eulerAngles + faceCameraEuler), scale));
				CallDirty ();
			}

			protected void AddGameObjectInternal (GameObject gameObject, Vector3 position, Quaternion rotation, Vector3 scale)
			{
				if (gameObject.transform.childCount != 0) {
					for (int i = 0; i < gameObject.transform.childCount; i++) {
						GameObject child = gameObject.transform.GetChild (i).gameObject;
						AddGameObjectInternal (
							child, 
							(position + MultiplyScales (rotation * child.transform.localPosition, scale)), 
							Quaternion.Euler (child.transform.localRotation.eulerAngles + rotation.eulerAngles), 
							MultiplyScales (child.transform.localScale, scale)
						);
					}
				}
				RenderQueue.Add (new RenderItem (gameObject, position, rotation, scale));
			}

			private Vector3 MultiplyScales (Vector3 lhs, Vector3 rhs)
			{
				return new Vector3 (lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
			}

			protected virtual void Setup (Rect r)
			{
				renderUtil = new PreviewRenderUtility ();
				renderUtil.camera.transform.position = new Vector3 (0, 0, -10);
				renderUtil.camera.transform.rotation = Quaternion.identity;
				renderUtil.camera.orthographic = true;
				renderUtil.camera.farClipPlane = 150;
				renderUtil.lights [1].enabled = true;
				renderUtil.lights [0].transform.rotation = Quaternion.Euler (20, -10, 0);
				renderUtil.lights [0].intensity = 1f;
				renderUtil.lights [1].transform.rotation = Quaternion.Euler (50, 50, 0);
				renderUtil.lights [1].intensity = 0.35f;
				ClearRenderQueue ();
				this.rect = r;
			}

			protected virtual void Display ()
			{
				GUI.DrawTexture (rect, Render (rect), ScaleMode.StretchToFill, false);
			}

			public void Cleanup ()
			{
				renderUtil.Cleanup ();
			}

			protected void CallDirty ()
			{
				if (OnIsDirty != null) {
					OnIsDirty ();
				}
			}

			void OnDestroy ()
			{
				Cleanup ();
			}
		}
	}
}