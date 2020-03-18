using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Attractor : MonoBehaviour
{
    private Rigidbody rb;


    public const float G = 0.025f;

    public Vector3 OrbitNormal;
    public Vector3 Initvelocity;
    public float semiMajorAxis;
    public Attractor target = null;
    public static List<Attractor> attractors = new List<Attractor>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        attractors.Add(this);
    }

    private void Start()
    {

        if (target != null && Initvelocity == Vector3.zero)
        {
            SetOrbitalVelocity(OrbitNormal);
        }
    }

    private void FixedUpdate()
    {
        foreach (Attractor attractor in attractors)
        {
            if (attractor != this) Attract(attractor);
        }
    }

    internal int GetRadius()
    {
        throw new NotImplementedException();
    }

    public void Attract(Attractor obToAttract)
    {

        Rigidbody rbToAtrract = obToAttract.rb;
        Vector3 direction = rb.position - rbToAtrract.position;
        float distance = direction.magnitude;


        float forceMagnitude = (G * rb.mass * rbToAtrract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAtrract.AddForce(force);
    }

    void SetOrbitalVelocity(Vector3 normal)

        


    {
    
        if (normal == Vector3.zero)
        {
            normal = new Vector3(0, 1, 0);
        }


        Vector3 direction = rb.position - target.rb.position;
        float distance = direction.magnitude;

        Vector3 unitTangent = Vector3.Cross(normal, direction.normalized);
     

        if (semiMajorAxis == 0)
        {
            semiMajorAxis = distance;
        }

        
        Vector3 InitVelocity = Mathf.Sqrt((G * target.rb.mass) * ((2 / distance) - (1 / (semiMajorAxis)))) * unitTangent;


        this.rb.velocity = InitVelocity;









    }





}

