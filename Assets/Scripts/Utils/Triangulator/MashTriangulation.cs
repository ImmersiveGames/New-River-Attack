using System.Collections.Generic;
using UnityEngine;
namespace Utils
{
    public static class MashTriangulation
    {
        /* <summary>
            Make a Mash by array of Vector2 points
            </summary>
            <param name="vertices2D">array with 2d point </param>
            <returns>Game Object</returns>
        */
        public static GameObject MashCreator(Vector2[] vertices2D)
        {
            var tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();

            // Create the Vector3 vertices
            var vertices = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
            }
            // Create the mesh
            var msh = new Mesh()
            {
                vertices = vertices,
                triangles = indices
            };
            msh.RecalculateNormals();
            msh.RecalculateBounds();
            // UV creator
            var uvs = new Vector2[vertices.Length];
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
            }
            //Create Game object
            var go = new GameObject();
            go.AddComponent(typeof(MeshRenderer));
            var filter = go.AddComponent(typeof(MeshFilter)) as MeshFilter;
            filter!.mesh = msh;
            filter.mesh.uv = uvs;
            return go;
        }
        /* <summary>
            </summary>
            <param name="vertices2D">Conjunto de vertices a serem flipados</param>
            <param name="flipx">flipar na horizontal</param>
            <param name="flipy">flipar na vertical</param>
            <returns>uma nova lista com os vertices flipados</returns>
        */
        public static List<Vector2> FlipVertices(List<Vector2> vertices2D, bool flipX = true, bool flipY = false)
        {
            var flipVertices2D = new List<Vector2>();
            for (int i = 0; i < vertices2D.Count; i++)
            {
                float nx = (flipX) ? -vertices2D[i].x : vertices2D[i].x;
                float ny = (flipY) ? -vertices2D[i].y : vertices2D[i].y;
                flipVertices2D.Add(new Vector2(nx, ny));
            }
            return flipVertices2D;
        }

        public static Bounds GetChildRenderBounds(GameObject wallsObjet)
        {
            var bounds = new Bounds(Vector3.zero, Vector3.zero);
            var render = wallsObjet.GetComponentsInChildren<Renderer>();
            if (render == null)
                return bounds;
            foreach (var t in render)
            {
                //if (!t.gameObject.GetComponent<WallsMaster>()) continue;
                if (!t.gameObject.CompareTag("Wall")) continue;
                //Debug.Log("Render: " + t.bounds);
                bounds.Encapsulate(t.bounds);
            }
            return bounds;
        }


        static Bounds GetRenderBounds(GameObject objets)
        {
            var bounds = new Bounds(Vector3.zero, Vector3.zero);
            var render = objets.GetComponent<Renderer>();
            return render != null ? render.bounds : bounds;
        }

        public static Bounds GetBounds(GameObject objets, string tag = null)
        {
            var bounds = GetRenderBounds(objets);
            if (bounds.extents.x != 0)
                return bounds;
            bounds = new Bounds(objets.transform.position, Vector3.zero);
            foreach (Transform child in objets.transform)
            {
                if (tag != null && !child.CompareTag(tag)) continue;
                var childRender = child.GetComponent<Renderer>();
                bounds.Encapsulate(childRender ? childRender.bounds : GetBounds(child.gameObject));
            }
            return bounds;
        }

    }
}
