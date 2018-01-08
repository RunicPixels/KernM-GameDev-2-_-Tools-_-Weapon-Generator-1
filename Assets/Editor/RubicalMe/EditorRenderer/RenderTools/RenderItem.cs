using UnityEngine;

namespace RubicalMe
{
	namespace RenderTools
	{
		public class RenderItem
		{
			public GameObject GameObject {
				get {
					return gObject;
				}
			}

			public Mesh Mesh {
				get {
					return mesh;
				}
			}

			public Material Material {
				get {
					return material;
				}
			}

			public Matrix4x4 Matrix {
				get {
					return Matrix4x4.TRS (position, rotation, scale);
				}
			}

			public Vector3 Position {
				get {
					return position;
				}
			}


			public RenderItem (GameObject gameObject, Vector3 position, Quaternion rotation)
			{
				Initialize (gameObject, position, rotation, gameObject.transform.localScale);
			}

			public RenderItem (GameObject gameObject, Vector3 position, Vector3 eulerAngles)
			{
				Initialize (gameObject, position, Quaternion.Euler (eulerAngles), gameObject.transform.localScale);
			}

			public RenderItem (GameObject gameObject, Vector3 position, Quaternion rotation, Vector3 scale)
			{
				Initialize (gameObject, position, rotation, scale);
			}

			public RenderItem (Mesh mesh, Material material, Vector3 position, Quaternion rotation, Vector3 scale)
			{
				this.mesh = mesh;
				this.material = material;
				this.position = position;
				this.rotation = rotation;
				this.scale = scale;
			}

			private void Initialize (GameObject gameObject, Vector3 position, Quaternion rotation, Vector3 scale)
			{
				MeshFilter filter;
				MeshRenderer render;
				if ((filter = gameObject.GetComponent <MeshFilter> ()) == null ||
				    (render = gameObject.GetComponent <MeshRenderer> ()) == null) {
					throw new System.ArgumentException ("RenderItem requires a MeshFilter and a MeshRenderer component to be assigned to the gameobject");
				}
				this.mesh = filter.sharedMesh;
				this.material = render.sharedMaterial;
				this.gObject = gameObject;
				this.position = position;
				this.rotation = rotation;
				this.scale = scale;
			}

			private GameObject gObject;
			private Vector3 position;
			private Quaternion rotation;
			private Vector3 scale;
			private Mesh mesh;
			private Material material;
		}
	}
}