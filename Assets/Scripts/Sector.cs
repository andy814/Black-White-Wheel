using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private float sectorSize;
    private float startAngle;
    private float sectorRadius;
    private MyColor sectorColor;

    public float SectorSize
    {
        get { return sectorSize; }
        set { sectorSize = value; }
    }

    public float StartAngle
    {
        get { return startAngle; }
        set { startAngle = value; }
    }

    public float SectorRadius
    {
        get { return sectorRadius; }
        set { sectorRadius = value; }
    }

    public MyColor SectorColor
    {
        get { return sectorColor; }
        set { sectorColor = value; }
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        meshCollider.convex = true; // Enable convex mesh collider
    }

    public void GenerateSectorMesh()
    {
        int numVertices = (int)sectorSize;
        Vector3[] vertices = new Vector3[numVertices + 1];
        Vector2[] uv = new Vector2[numVertices + 1];
        int[] triangles = new int[(numVertices - 1) * 3];

        float angleStep = sectorSize / (numVertices - 1);
        float currentAngle = startAngle;

        // Center vertex
        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0.5f, 0.5f);

        // Outer vertices
        for (int i = 1; i <= numVertices; i++)
        {
            vertices[i] = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0) * sectorRadius;
            uv[i] = new Vector2((vertices[i].x / sectorRadius + 1) * 0.5f, (vertices[i].y / sectorRadius + 1) * 0.5f);
            currentAngle += angleStep;
        }

        // Triangles
        for (int i = 0; i < numVertices - 1; i++)
        {
            int triangleIndex = i * 3;

            triangles[triangleIndex] = 0;
            triangles[triangleIndex + 1] = i + 2;
            triangles[triangleIndex + 2] = i + 1;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;

        switch (sectorColor)
        {
            case MyColor.Black:
                meshRenderer.material = Resources.Load<Material>("Materials/Black");
                break;
            case MyColor.White:
                meshRenderer.material = Resources.Load<Material>("Materials/White");
                break;
            default:
                meshRenderer.material = Resources.Load<Material>("Materials/Error");
                break;
        }
    }

    public void RotateSector(float rotationAmount)
    {
        transform.Rotate(Vector3.back, rotationAmount);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision");
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
