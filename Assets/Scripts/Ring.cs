using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private float innerRadius;
    private float outerRadius;
    private int numSegments;
    private float initRotationSpeed;
    private float rotationSpeedIncrease;

    public GameObject segmentPrefab;
    private Segment[] segments;
    private Player player;

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

    public int NumSegments
    {
        get { return numSegments; }
        set { numSegments = value; }
    }

    public float InitRotationSpeed
    {
        get { return initRotationSpeed; }
        set { initRotationSpeed = value; }
    }

    public float RotationSpeedIncrease
    {
        get { return rotationSpeedIncrease; }
        set { rotationSpeedIncrease = value; }
    }


    public GameObject SegmentPrefab
    {
        get { return segmentPrefab; }
        set { segmentPrefab = value; }
    }

    public Segment[] Segments
    {
        get { return segments; }
        set { segments = value; }
    }

    public Player Player 
    {
        get { return player; }
        set { player = value;  }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GenerateSegments()
    {
        segments=new Segment[numSegments];
        float totalSegmentAngle = 0f;
        float minDeg = 360 / numSegments / 2f;
        float maxDeg = 360 / numSegments * 1.5f;
        List<float> segmentSizes = Utils.RandX(numSegments, 360, minDeg, maxDeg);

        for (int i = 0; i < numSegments; i++)
        {
            float segmentSize = segmentSizes[i]; // Random sector size between 0.1 and 0.9 // in angle unit
            float startAngle = totalSegmentAngle;
            float endAngle = startAngle + segmentSize;
            MyColor myColor = (MyColor)(i % 2); // Alternate between black and white sectors
            segments[i] = GenerateSegment(segmentSize, startAngle, myColor);
            totalSegmentAngle += segmentSize;
        }
        //Debug.Log("segment generated");
    }

    private Segment GenerateSegment(float segmentSize, float startAngle, MyColor myColor)
    {
        Segment segment = Instantiate(segmentPrefab).GetComponent<Segment>();
        segment.transform.SetParent(transform);
        segment.transform.localPosition = Vector3.zero;
        segment.transform.localRotation = Quaternion.identity;

        segment.SegmentSize = segmentSize;
        segment.StartAngle = startAngle;
        segment.InnerRadius = innerRadius;
        segment.OuterRadius = outerRadius;
        segment.MyColor = myColor;

        segment.GenerateSegmentMesh();

        return segment;
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            return;
        float rotationSpeed = initRotationSpeed + rotationSpeedIncrease * player.SurviveTime;
        transform.Rotate(Vector3.back, rotationSpeed);
    }
}
