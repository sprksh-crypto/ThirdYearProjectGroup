using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Moonvelocity : MonoBehaviour
{
    public Rigidbody rb;
    public float semiMajorAxis;
    public const float G = 0.025f;
    private const float Earthmass = 0.015f;
    public Vector3 OrbitNormal;
    public Vector3 Initvelocity;

    private const float Sunmass = 33000f;


    void SetOrbitalVelocity(Vector3 normal)



    {
        if (normal == Vector3.zero)
        {
            normal = new Vector3(0, 1, 0);
        }


        Vector3 moondistance = rb.position - GameObject.Find("Earth").transform.position;
        float moon2earth = moondistance.magnitude;
        Vector3 moonunitTangent = Vector3.Cross(normal, moondistance.normalized);



        Vector3 rbdistance = GameObject.Find("Earth").transform.position - GameObject.Find("Sun").transform.position;

        Vector3 unitTangent = Vector3.Cross(normal, rbdistance.normalized);
       float e2sdistance = rbdistance.magnitude; //moon to sun distance

        Vector3 EarthInitVelocity = Mathf.Sqrt(G * Sunmass / e2sdistance) * unitTangent;

        Vector3 OrbitalVelocityMoon2Earth = Mathf.Sqrt(G * Earthmass /moon2earth) * moonunitTangent;




        //if (semiMajorAxis == 0)

          //{semiMajorAxis = e2sdistance;}



        //Vector3 EarthInitVelocity = Mathf.Sqrt((G * Sunmass) * ((2 / e2sdistance) - (1 / (semiMajorAxis)))) * unitTangent;
        //Vector3 OrbitalVelocityMoon2Earth = Mathf.Sqrt((G * Earthmass) * ((2 / moon2earth) - (1 / (semiMajorAxis)))) * moonunitTangent;




        Vector3 MoonVelocity = EarthInitVelocity + OrbitalVelocityMoon2Earth;

        this.rb.velocity = MoonVelocity;



    }
}


