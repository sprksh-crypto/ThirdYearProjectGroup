using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{

    // STATIC VARIABLES //
    // G - arbitrary
    public static float G = 1000f;

    // List of all gravitational objects
    public static List<GravityBody> AllBodies = new List<GravityBody>();
    public static bool listSorted;
    public static int totalBodies;


    // INSTANCE VARIABLES //
    private Transform bodyTransform;
    private Rigidbody rb;

    // User input variables
    public GravityBody OrbitTarget = null; // The body that THIS body will orbit around (drag and drop at inspector)
    public Vector3 OrbitNormal;  // The normal vector to the plane of orbit
    public Vector3 initVelocity; // Allow input via inspector
    private int numOrbitParents;

    private float radius;

    // Called before Start()
    private void Awake()
    {
        // Get components of the body
        rb = GetComponent<Rigidbody>();
        bodyTransform = GetComponent<Transform>();
        radius = GetComponent<SphereCollider>().radius;

        // Add this body to the list of all gravitational objects
        AllBodies.Add(this);
        totalBodies++;

        FindNumberOfOrbitParents(); 
    }

    private void Start()
    {
        // Ensure that this operation is only done once across all instances
        if (!listSorted)
        {
            SortGBList();
            listSorted = true;
            SetInitialVelocities();
        }
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

    private void FindNumberOfOrbitParents()
    {
        GravityBody target = OrbitTarget;
        while (target != null)
        {
            numOrbitParents++;
            target = target.OrbitTarget;
        }
    }

    private static void SortGBList()
    {
        AllBodies.Sort((x, y) => x.numOrbitParents.CompareTo(y.numOrbitParents));
        PrintGravityBodyNames();
    }

    // Attract another gravitational body
    public void Attract(GravityBody gb)
    {
        Vector3 r_to_this = this.bodyTransform.position - gb.bodyTransform.position;

        // Newton's law of gravitation
        Vector3 force = r_to_this * G * gb.rb.mass * this.rb.mass / Mathf.Pow(r_to_this.magnitude, 3);
        gb.SetForceTo(force);
    }

    private static void SetInitialVelocities()
    {
        foreach (GravityBody gb in AllBodies)
        {
            if (gb.UsesOrbitalVelocity())
            {
                gb.SetInitVelocityToOrbital();

                if (gb.numOrbitParents >= 1)
                {
                    gb.initVelocity += gb.OrbitTarget.initVelocity;
                }
                gb.SetVelocityTo(gb.initVelocity);
            }
        }
    }

    // Set this body's initial velocity to orbit around its target, where orbital plane is defined by its tangent vector OrbitNormal
    private void SetInitVelocityToOrbital()
    {
        if (OrbitNormal == Vector3.zero)
        {
            OrbitNormal = new Vector3(0, 1, 0);
        }

        Vector3 r_to_this = this.bodyTransform.position - OrbitTarget.bodyTransform.position;
        float distance = r_to_this.magnitude;
        Vector3 tangentUnitToOrbitalPlane = Vector3.Cross(OrbitNormal, r_to_this.normalized);
        Vector3 orbitalVelocity = Mathf.Sqrt(G * OrbitTarget.rb.mass / distance) * tangentUnitToOrbitalPlane;

        // Setting this body's initial velocity as the calculated orbit velocity
        initVelocity = orbitalVelocity;
    }

    private bool UsesOrbitalVelocity()
    {
        return (OrbitTarget != null && initVelocity == Vector3.zero);
    }

    // Print list of the names of all the gravitational bodies in console
    private static void PrintGravityBodyNames()
    {
        List<string> gbNames = new List<string>
        {
            "Gravity body list:"
        };
        foreach (GravityBody gb in AllBodies)
        {
            gbNames.Add(gb.transform.root.name + " | ");
        }
        Debug.Log(string.Join("", gbNames));
    }

    //// Get Set methods ////
    public void SetVelocityTo(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    public void SetForceTo(Vector3 force)
    {
        rb.AddForce(force);
    }

    public Transform GetTransform()
    {
        return this.bodyTransform;
    }

    public float GetRadius()
    {
        return this.radius;
    }
}
