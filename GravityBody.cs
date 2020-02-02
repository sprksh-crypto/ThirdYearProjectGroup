using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{

    // Define attributes for convenience
    private Transform bodyTransform;
    private float mass;
    private Rigidbody rb;

    // Allow user to tweak
    public Vector3 initVelocity; // Allow input via inspector
    public GravityBody OrbitTarget = null; // The body that THIS body will orbit around (drag and drop at inspector)
    public Vector3 OrbitNormal;

    // G - arbitrary
    public const float G = 100000f;

    // List of all gravitational bodies
    private static List<GravityBody> AllBodies = new List<GravityBody>();

    private void Awake()
    {
        // Get components of the body
        rb = GetComponent<Rigidbody>();
        bodyTransform = GetComponent<Transform>();

        // Set attributes
        mass = rb.mass;

        AllBodies.Add(this);
    }


    // Start is called before the first frame update
    void Start()
    {



        foreach (GravityBody body in AllBodies)
        {

            // Set orbital velocity, if a body to orbit around is specified 
            if (body != OrbitTarget && body.initVelocity == Vector3.zero && OrbitTarget != null)
            {
                // If user does not specify a normal vector 
                if (OrbitNormal == Vector3.zero)
                {
                    OrbitNormal = new Vector3(0, 1, 0);
                    Debug.Log("Creating normal vector");
                }
                SetOrbitalVelocity(OrbitNormal);
            }
        }

        SetVelocity(initVelocity);

    }

    // Attract another gravitational body 
    public void Attract(GravityBody gb)
    {
        // Vector from target body to this body
        Vector3 r_to_this = this.bodyTransform.position - gb.bodyTransform.position;

        // Newton's law of gravitation
        Vector3 force = r_to_this * G * gb.GetMass() * this.GetMass() / Mathf.Pow(r_to_this.magnitude, 3);
        gb.SetForceTo(force);
    }

    // Set this body to orbit around some other CentreBody, tangential to some normal vector
    private void SetOrbitalVelocity(Vector3 normal)
    {
        Vector3 r_to_target = this.bodyTransform.position - OrbitTarget.bodyTransform.position;
        float magnitude = r_to_target.magnitude;
        Vector3 unitTangent = Vector3.Cross(normal, r_to_target);
        Vector3 orbitalVelocity = Mathf.Sqrt(G * OrbitTarget.GetMass() / Mathf.Pow(magnitude, 3)) * unitTangent;
        initVelocity = orbitalVelocity;
        Debug.Log(initVelocity);
    }

    // Update is called once per x frames
    void FixedUpdate()
    {
        // N^2 interactions, each body acts on all others
        foreach (GravityBody body in AllBodies)
        {
            if (body != this)
            {
                this.Attract(body);
            }
        }
    }

    //// Get Set methods ////
    public float GetMass()
    {
        return mass;
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    public void SetForceTo(Vector3 force)
    {
        rb.AddForce(force);
    }
}
