using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.up * 550);
    }

    public void KillOldBullet()
    {
        Destroy(gameObject, 2.0f);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        Destroy(gameObject, 0.0f);
    }
}
