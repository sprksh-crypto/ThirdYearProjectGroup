using System.Collections;

using System.Collections.Generic;

using UnityEngine;





public class SpaceBodies : MonoBehaviour

{
    private float G = .6674f;



    // List of all gravitational objects

    public static List<SpaceBodies> bodies = new List<SpaceBodies>();

    private static bool listSorted;

    private static int totalBodies;

    public float semiMajorAxis;



    // INSTANCE VARIABLES //

    private Transform bodyTransform;

    public Rigidbody rb;



    // User input variables

    public SpaceBodies OrbitTarget = null; // The body that THIS body will orbit around (drag and drop at inspector)

    private Vector3 OrbitNormal;  // The normal vector to the plane of orbit

    private Vector3 initVelocity;

    private int numOrbitParents;



    public float inclination;
    private float initialDisplacementY;

    public Vector3 rotationalAxisVector;
    public float angularSpeed;


    // Called before Start()

    private void Awake()

    {

        // Get components of the body
        bodyTransform = GetComponent<Transform>();




        // Add this body to the list of all gravitational objects

        bodies.Add(this);

        totalBodies++;



        FindNumberOfOrbitParents();

    }



    private void Start()

    {

        // Ensure that this operation is only done once across all instances

        if (!listSorted)

        {

            SortList();

            listSorted = true;

            SetInitialVelocities();

        }

    }

    public void AdjustSpeed(float time)
    {
        Time.timeScale = time;
    }


    // Update is called once per x frames

    void FixedUpdate()

    {

        // N^2 interactions, each body acts on all others

        foreach (SpaceBodies body in bodies)

        {

            if (body != this)

            {

                this.Attract(body);

            }

        }

        foreach (SpaceBodies body in bodies)

        {

            
            body.transform.Rotate(body.rotationalAxisVector, Time.deltaTime * angularSpeed);

        }


    }



    private void FindNumberOfOrbitParents()

    {

        SpaceBodies target = OrbitTarget;

        while (target != null)

        {

            numOrbitParents++;

            target = target.OrbitTarget;

        }

    }



    private static void SortList()

    {

        bodies.Sort((x, y) => x.numOrbitParents.CompareTo(y.numOrbitParents));

    }



    // Attract another gravitational body

    public void Attract(SpaceBodies body)

    {

        Vector3 r = this.bodyTransform.position - body.bodyTransform.position;



        // Newton's law of gravitation

        Vector3 force = r * G * body.rb.mass * this.rb.mass / Mathf.Pow(r.magnitude, 3);

        body.rb.AddForce(force);

    }



    private static void SetInitialVelocities()

    {

        foreach (SpaceBodies body in bodies)

        {

            if (body.UsesOrbitalVelocity())

            {

                body.SetInitVelocityToOrbital();



                if (body.numOrbitParents >= 1)

                {

                    body.initVelocity += body.OrbitTarget.initVelocity;

                }



                body.SetVelocityTo(body.initVelocity);

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

        Vector3 r = this.bodyTransform.position - OrbitTarget.bodyTransform.position;

        float initialDisplacementY = (float)(Mathf.Sin(this.inclination * Mathf.PI / 180) * r.magnitude) + OrbitTarget.initialDisplacementY;

        this.initialDisplacementY = initialDisplacementY;

        this.transform.Translate(Vector3.up * initialDisplacementY);



        float distance = r.magnitude;

        if (semiMajorAxis == 0)
        {
            semiMajorAxis = distance;
        }

        Vector3 unitTangent = Vector3.Cross(OrbitNormal, r.normalized);

        Vector3 orbitalVelocity = Mathf.Sqrt((G * OrbitTarget.rb.mass) * ((2 / distance) - (1 / semiMajorAxis))) * unitTangent;



        // Setting this body's initial velocity as the calculated orbit velocity

        initVelocity = orbitalVelocity;


    }



    private bool UsesOrbitalVelocity()

    {

        return (OrbitTarget != null && initVelocity == Vector3.zero);

    }




    //This is needed because it doesn't like just putting it to rb.velocity directly, otherwise I get errors.

    public void SetVelocityTo(Vector3 velocity)

    {

        rb.velocity = velocity;


    }




}