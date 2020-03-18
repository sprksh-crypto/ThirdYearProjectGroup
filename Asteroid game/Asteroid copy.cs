using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    public GameObject rock;
    public Gameplay gameplay;
    private float maxRotation;
    private float rotationX;
    private float rotationY;
    private float rotationZ;
    private Rigidbody rb;
    private Camera mainCam;
    private float maxSpeed;
    private int _generation;

    void Start()
    {

        mainCam = Camera.main;

        maxRotation = 25f;
        rotationX = Random.Range(-maxRotation, maxRotation);
        rotationY = Random.Range(-maxRotation, maxRotation);
        rotationZ = Random.Range(-maxRotation, maxRotation);

        rb = rock.GetComponent<Rigidbody>();

        float speedX = Random.Range(200f, 800f);
        int selectorX = Random.Range(0, 2);
        float dirX = 0;
        if (selectorX == 1) { dirX = -1; }
        else { dirX = 1; }
        float finalSpeedX = speedX * dirX;
        rb.AddForce(transform.right * finalSpeedX);

        float speedY = Random.Range(200f, 800f);
        int selectorY = Random.Range(0, 2);
        float dirY = 0;
        if (selectorY == 1) { dirY = -1; }
        else { dirY = 1; }
        float finalSpeedY = speedY * dirY;
        rb.AddForce(transform.up * finalSpeedY);

    }

    public void SetGeneration(int generation)
    {
        _generation = generation;
    }

    void Update()
    {
        rock.transform.Rotate(new Vector3(rotationX, rotationY, 0) * Time.deltaTime);
        CheckPosition();
        float dynamicMaxSpeed = 3f;
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -dynamicMaxSpeed, dynamicMaxSpeed), Mathf.Clamp(rb.velocity.y, -dynamicMaxSpeed, dynamicMaxSpeed));
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.name == "Bullet(Clone)")
        {
            if (_generation < 3)
            {
                CreateSmallAsteriods(2);
            }
            Destroy();
        }

        if (collisionInfo.collider.name == "Rocket")
        {
            gameplay.RocketFail();
        }
    }

    void CreateSmallAsteriods(int asteroidsNum)
    {
        int newGeneration = _generation + 1;
        for (int i = 1; i <= asteroidsNum; i++)
        {
            float scaleSize = 0.5f;
            GameObject AsteroidClone = Instantiate(rock, new Vector3(transform.position.x, transform.position.y, 0f), transform.rotation);
            AsteroidClone.transform.localScale = new Vector3(AsteroidClone.transform.localScale.x * scaleSize, AsteroidClone.transform.localScale.y * scaleSize, AsteroidClone.transform.localScale.z * scaleSize);
            AsteroidClone.GetComponent<Asteroid>().SetGeneration(newGeneration);
            AsteroidClone.SetActive(true);
        }
    }

    private void CheckPosition()
    {

        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;
        float sceneRightEdge = sceneWidth / 2;
        float sceneLeftEdge = sceneRightEdge * -1;
        float sceneTopEdge = sceneHeight / 2;
        float sceneBottomEdge = sceneTopEdge * -1;

        float rockOffset;
        if (gameplay.allAsteroidsOffScreen)
        {
            rockOffset = 1.0f;
            float reverseSpeed = 2000.1f;

            if (rock.transform.position.x > sceneRightEdge + rockOffset)
            {
                rock.transform.rotation = Quaternion.identity;
                rb.AddForce(transform.right * (reverseSpeed * (-1)));
            }

            if (rock.transform.position.x < sceneLeftEdge - rockOffset) { rock.transform.rotation = Quaternion.identity; rb.AddForce(transform.right * reverseSpeed); }
            if (rock.transform.position.y > sceneTopEdge + rockOffset)
            {
                rock.transform.rotation = Quaternion.identity;
                rb.AddForce(transform.up * (reverseSpeed * (-1)));
            }

            if (rock.transform.position.y < sceneBottomEdge - rockOffset) { rock.transform.rotation = Quaternion.identity; rb.AddForce(transform.up * reverseSpeed); }
        }
        else
        {
            rockOffset = 2.0f; if (rock.transform.position.x > sceneRightEdge + rockOffset)
            {
                rock.transform.position = new Vector2(sceneLeftEdge - rockOffset, rock.transform.position.y);
            }

            if (rock.transform.position.x < sceneLeftEdge - rockOffset) { rock.transform.position = new Vector2(sceneRightEdge + rockOffset, rock.transform.position.y); }
            if (rock.transform.position.y > sceneTopEdge + rockOffset)
            {
                rock.transform.position = new Vector2(rock.transform.position.x, sceneBottomEdge - rockOffset);
            }

            if (rock.transform.position.y < sceneBottomEdge - rockOffset)
            {
                rock.transform.position = new Vector2(rock.transform.position.x, sceneTopEdge + rockOffset);
            }
        }
    }

    public void Destroy()
    {
        gameplay.asterodDestroyed();
        Destroy(gameObject, 0.01f);
    }

    public void DestroySilent()
    {
        Destroy(gameObject, 0.00f);
    }

}