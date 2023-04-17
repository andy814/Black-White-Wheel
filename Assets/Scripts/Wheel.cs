using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Utils;

public class Wheel : MonoBehaviour
{
    public float[] ringRadii;
    public int[] numSegments;
    public float[] initRotationSpeeds;
    public float[] rotationSpeedsIncrease;

    public GameObject ringPrefab;
    private Ring[] rings;

    public GameObject playerObject;
    private Player player;

    public Ring[] Rings
    {   
        get { return rings; }
        set { rings = value; }
    }

    private Ring GenerateRing(float innerRadius, float outerRadius, int numSegments,
                                float initrRotationSpeed, float RotationSpeedIncrease)
    {
        
        Ring ring = Instantiate(ringPrefab).GetComponent<Ring>();
        ring.transform.SetParent(transform);
        ring.transform.localPosition = Vector3.zero;
        ring.transform.localRotation = Quaternion.identity;

        ring.InnerRadius = innerRadius;
        ring.OuterRadius = outerRadius;
        ring.NumSegments = numSegments;
        ring.InitRotationSpeed = initrRotationSpeed;
        ring.RotationSpeedIncrease = RotationSpeedIncrease;
        ring.Player = player;
        ring.GenerateSegments();
        return ring;
    }

    private void GenerateRings()
    {
        int numRings=ringRadii.Length-1;
        for (int i = 0; i < numRings; i++)
        {
            rings[i] = GenerateRing(ringRadii[i], ringRadii[i + 1], numSegments[i],
                                    initRotationSpeeds[i], rotationSpeedsIncrease[i]);
        }

    }

    private void Awake()
    {
        rings = new Ring[ringRadii.Length - 1];
        GameData.wheel = this;
        player = playerObject.GetComponent<Player>();
    }

    private void Start()
    {
        GenerateRings();
    }

    private void Update()
    {
        
    }
}
