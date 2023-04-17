using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestDraw : MonoBehaviour
{

    private GameObject myObject;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Start()
    {

        myObject = new GameObject("myObject");
        meshFilter = myObject.AddComponent<MeshFilter>();
        meshRenderer = myObject.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[3];
        Vector2[] uv = new Vector2[3];
        int[] triangles = new int[3];

        float r = 100f;

        vertices[0] = new Vector3(0, 0, 0)*r;
        vertices[1] = new Vector3(0, 1, 0)*r;
        vertices[2] = new Vector3(1, 0, 0)*r;
        uv[0] = new Vector2(0.5f, 0.5f);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices;
        //mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        // Load a material named "TerrainMat" from a folder named "Resources"
        Material material = Resources.Load<Material>("Materials/Error");
        meshRenderer.material = material;

    }

    /*
    private void OnDrawGizmos()
    {
        if (meshFilter != null && meshFilter.mesh != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireMesh(meshFilter.mesh, transform.position, transform.rotation, transform.lossyScale);
        }
    }
    */
}
