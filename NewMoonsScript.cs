using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMoonsScript : MonoBehaviour
{
    [SerializeField] float a;
    [SerializeField] float b;
    [SerializeField] float c;
    [SerializeField] float alpha;
    [SerializeField] float deltaAlpha;
    [SerializeField] Vector3 center;
    [SerializeField] Transform focus1;

    private void Start()
    {
        
    }

    private void Update()
    {
        center = new Vector3(focus1.position.x + c, 0, focus1.position.z);

        transform.position = new Vector3(center.x + a * Mathf.Cos(alpha), 0, center.z + b * Mathf.Sin(alpha));
        c = Mathf.Sqrt(a * a - b * b);
        alpha += deltaAlpha;
    }
}
