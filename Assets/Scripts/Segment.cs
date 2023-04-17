using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Segment : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private EdgeCollider2D edgeCollider;
    private float segmentSize;
    private float startAngle;
    private float innerRadius;
    private float outerRadius;
    private MyColor myColor;


    public float SegmentSize
    {
        get { return segmentSize; }
        set { segmentSize = value; }
    }

    public float StartAngle
    {
        get { return startAngle; }
        set { startAngle = value; }
    }

    public float InnerRadius
    {
        get { return innerRadius; }
        set { innerRadius = value; }
    }

    public float OuterRadius
    {
        get { return outerRadius; }
        set { outerRadius = value; }
    }

    public MyColor MyColor
    {
        get { return myColor; }
        set { myColor = value; }
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }


    public void GenerateSegmentMesh()
    {
        float[] ringRadii = transform.parent.parent.gameObject.GetComponent<Wheel>().ringRadii;
        float maxRadius = ringRadii[ringRadii.Length - 1];
        //TBD
        //number of vertices on ONE side of the curve, central not included
        int numVertices = (int)(segmentSize);

        Vector3[] vertices;
        Vector2[] uv;
        int[] triangles;
        Vector2[] path;

        if (innerRadius == 0)
        {
            //Debug.Log("drawing inner segment");
            vertices = new Vector3[numVertices + 1];
            uv = new Vector2[numVertices + 1];
            triangles = new int[(numVertices - 1) * 3];

            float angleStep = segmentSize / (numVertices - 1);
            float currentAngle = startAngle;

            // Center vertex
            vertices[0] = Vector3.zero;
            uv[0] = new Vector2(0.5f, 0.5f);

            // vertices, organized in outer-inner-outer-inner order
            for (int i = 1; i <= numVertices; i++)
            {
                vertices[i] = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0) * outerRadius;
                uv[i] = new Vector2((vertices[i].x / maxRadius + 1) * 0.5f, (vertices[i].y / maxRadius + 1) * 0.5f);
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

            path = new Vector2[vertices.Length + 1];
            path[0] = new Vector2(vertices[0].x, vertices[0].y);
            for (int i=1; i<vertices.Length; i++)
            {
                path[i] = new Vector2(vertices[i].x, vertices[i].y);
            }
            path[vertices.Length] = path[0];

        }
        else
        {
            vertices = new Vector3[2 * numVertices];
            uv = new Vector2[2 * numVertices];
            triangles = new int[(numVertices - 1) * 2 * 3];

            float angleStep = segmentSize / (numVertices - 1);
            float currentAngle = startAngle;

            // vertices, organized in outer-inner-outer-inner order
            for (int i = 0; i < numVertices; i++)
            {
                int vertexIdx = i * 2;
                vertices[vertexIdx] = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0) * outerRadius;
                vertices[vertexIdx + 1] = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0) * innerRadius;
                uv[vertexIdx] = new Vector2((vertices[vertexIdx].x / maxRadius + 1) * 0.5f, (vertices[vertexIdx].y / maxRadius + 1) * 0.5f);
                uv[vertexIdx + 1] = new Vector2((vertices[vertexIdx + 1].x / maxRadius + 1) * 0.5f, (vertices[vertexIdx + 1].y / maxRadius + 1) * 0.5f);
                currentAngle += angleStep;
            }

            // Triangles
            for (int i = 0; i < (numVertices - 1) * 2; i += 2)
            {
                int triangleIndex = i * 3;

                triangles[triangleIndex] = i;
                triangles[triangleIndex + 1] = i + 1;
                triangles[triangleIndex + 2] = i + 2;
                triangles[triangleIndex + 3] = i + 1;
                triangles[triangleIndex + 4] = i + 3;
                triangles[triangleIndex + 5] = i + 2;
            }

            path = new Vector2[vertices.Length + 1 ];
            int arrIdx = 0;

            // Connect all vertices on the inner curve
            for (int i = 1; i < vertices.Length; i += 2)
            {
                path[arrIdx] = new Vector2(vertices[i].x, vertices[i].y);
                arrIdx++;
            }

            // Connect inner curve to outer curve
            path[arrIdx] = new Vector2(vertices[vertices.Length - 2].x, vertices[vertices.Length - 2].y);
            arrIdx++;

            // Connect all vertices on the outer curve, in reverse order
            for (int i = vertices.Length - 4; i >= 0; i -= 2)
            {
                path[arrIdx] = new Vector2(vertices[i].x, vertices[i].y);
                arrIdx++;
            }

            // Close the path by adding the first vertex of the inner curve to the end
            path[arrIdx] = path[0];
            

        }

        edgeCollider.points = path;
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();


        meshFilter.sharedMesh = mesh;
        
        switch (MyColor)
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
        //meshRenderer.sortingLayerName = "Wheel";
    }


    public void RotateSegment(float rotationSpeed)
    {
        transform.Rotate(Vector3.back, rotationSpeed);
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("hitSeg");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       // Debug.Log("coilSeg");
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
    

    private void OnDrawGizmos()
    {
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();

        if (polygonCollider != null)
        {
            // Set Gizmos color
            Gizmos.color = Color.red;

            // Get the path
            Vector2[] path = polygonCollider.GetPath(0);

            // Draw lines between vertices
            for (int i = 0; i < path.Length - 1; i++)
            {
                Gizmos.DrawLine(transform.TransformPoint(path[i]), transform.TransformPoint(path[i + 1]));
            }

            // Close the path by drawing a line between the last and the first vertex
            Gizmos.DrawLine(transform.TransformPoint(path[path.Length - 1]), transform.TransformPoint(path[0]));
        }
    }
    */

}
