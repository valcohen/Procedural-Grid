using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MyGrid : MonoBehaviour {

    public int xSize = 10;
    public int ySize = 5;
    public float drawDelay = 0.01f;

    private Vector3[]   vertices;
    private Mesh        mesh;

    private void Awake() {
        Debug.Log ("Awake!");
        StartCoroutine(Generate ());
    }

    private IEnumerator Generate () {
        var wait = new WaitForSeconds (drawDelay);

        GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++ ) {
            for (int x = 0; x <= xSize; x++, i++) {
                vertices[i] = new Vector3(x, y);
            }
        }
        mesh.vertices = vertices;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
            for (int x = 0; x < xSize; x++, ti += 6, vi++) {
                triangles [ti] = vi;
                triangles [ti + 1] = vi + xSize + 1;
                triangles [ti + 2] = vi + 1;
                triangles [ti + 3] = vi + 1;
                triangles [ti + 4] = vi + xSize + 1;
                triangles [ti + 5] = vi + xSize + 2;

                mesh.triangles = triangles;
                yield return wait;
            }
        }

    }

    private void OnDrawGizmos() {
        if (vertices == null) {
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++) {
            Gizmos.DrawSphere (transform.TransformPoint(vertices [i]), 0.1f);
        }
    }
}
