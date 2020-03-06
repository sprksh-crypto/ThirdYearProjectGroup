using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GravityBody : MonoBehaviour
{

    // STATIC VARIABLES //
    // G - arbitrary
    public static float G = 5000000f;

    public static float defaultG = G;

    // List of all gravitational objects
    public static List<GravityBody> AllBodies = new List<GravityBody>();
    public static bool listSorted;
    public static int totalBodies;


    // INSTANCE VARIABLES //
    private Transform bodyTransform;
    private Rigidbody rb;

    // User input variables
    public GravityBody orbitTarget = null; // The body that THIS body will orbit around (drag and drop at inspector)
    public Vector3 orbitNormal;  // The normal vector to the plane of orbit
    public Vector3 initVelocity; // Allow input via inspector
    public float semiMajorAxis;
    private int numOrbitParents;

    private float radius;

    // Record data variables
    public bool recordData;
    private string saveFilePath;

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

        orbitNormal.Normalize();

        FindNumberOfOrbitParents();

        if (recordData)
        {
            CreateSaveFile();
        }
    }

    private void Start()
    {
        // Ensure that this operation is only done once
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

        if (recordData)
        {
            RecordData();
        }
    }

    private void FindNumberOfOrbitParents()
    {
        GravityBody target = orbitTarget;
        while (target != null)
        {
            numOrbitParents++;
            target = target.orbitTarget;
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
        if (gb != null)
        {
            Vector3 r_to_this = this.bodyTransform.position - gb.bodyTransform.position;

            // Newton's law of gravitation
            Vector3 force = r_to_this * G * gb.rb.mass * this.rb.mass / Mathf.Pow(r_to_this.magnitude, 3);
            gb.SetForceTo(force);
        }
    }

    // Sets initial velocites for all gravity bodies, in order of how many orbital target parents they have
    private static void SetInitialVelocities()
    {
        foreach (GravityBody gb in AllBodies)
        {
            if (gb.UsesOrbitalVelocity())
            {
                gb.SetInitVelocityToOrbital();

                if (gb.numOrbitParents >= 1)
                {
                    gb.initVelocity += gb.orbitTarget.initVelocity;
                }
            }
            gb.SetVelocityTo(gb.initVelocity);
        }
    }

    // If has a target and doesn't have a user given initial velocity
    private bool UsesOrbitalVelocity()
    {
        return (orbitTarget != null && initVelocity == Vector3.zero);
    }

    // Set this body's initial velocity to orbit around its target, where orbital plane is defined by its tangent vector OrbitNormal
    private void SetInitVelocityToOrbital()
    {
        Vector3 orbitalVelocity;
        if (orbitNormal == Vector3.zero)
        {
            orbitNormal = new Vector3(0, 1, 0);
        }

        Vector3 r_to_this = this.bodyTransform.position - orbitTarget.bodyTransform.position;
        float distance = r_to_this.magnitude;
        Vector3 tangentUnitToOrbitalPlane = Vector3.Cross(orbitNormal, r_to_this.normalized);

        if (semiMajorAxis == 0)
        {
            semiMajorAxis = distance;
        }

        // Using the Vis Viva equation, which generalises to arbitrary semi major axis (for circulator orbit, semi major axis is current distance)
        orbitalVelocity = Mathf.Sqrt((G * orbitTarget.rb.mass) *  ((2 / distance) - (1 / (semiMajorAxis))))  * tangentUnitToOrbitalPlane;

        // Setting this body's initial velocity as the calculated orbit velocity
        initVelocity = orbitalVelocity;
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

    private void OnTriggerEnter(Collider other)
    {
        GravityBody gbOther = other.GetComponent<GravityBody>();
        if (this.rb.mass > gbOther.rb.mass)
        {
            Destroy(other.gameObject);
        }
    }

    // Currently records distance from orbit target
    // Can be modified to record energy
    private void RecordData()
    {
        float currentEnergy = 0f;
        foreach (GravityBody gb in AllBodies)
        {
            if (gb != this)
            {
                currentEnergy += -G * gb.rb.mass / Vector3.Distance(gb.bodyTransform.position, this.bodyTransform.position);
            }
        }

        float distance = Vector3.Distance(orbitTarget.bodyTransform.position, this.bodyTransform.position);

        // Append the distance and add a new line character -> "\n"
        File.AppendAllText(saveFilePath, distance.ToString() + "\n");
    }

    // Creates a text file at the location /Assets/PlanetData/planetname.txt
    private void CreateSaveFile()
    {
        saveFilePath = String.Format("{0}/PlanetData/{1}.txt", Application.dataPath, transform.name);

        if (File.Exists(saveFilePath))
        {
            File.WriteAllText(saveFilePath, "");
        }
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
        return this.transform;
    }

    public float GetRadius()
    {
        return this.radius;
    }
}
