using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{

    public GameObject asteroid;
    public GameObject rocket;
    private int _startLevelAsteroidsNum;
    private bool _allAsteroidsOffScreen;
    private int levelAsteroidNum;
    private Camera mainCam;
    private int asteroidLife;

    private void Start()
    {
        asteroid.SetActive(false);
        mainCam = Camera.main;
        _startLevelAsteroidsNum = 2;
        CreateAsteroids(_startLevelAsteroidsNum);
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.8f);

        if (asteroidLife <= 0)
        {
            asteroidLife = 6;
            CreateAsteroids(1);
        }

        var sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;
        float sceneRightEdge = sceneWidth / 2;
        float sceneLeftEdge = sceneRightEdge * -1;
        float sceneTopEdge = sceneHeight / 2;
        float sceneBottomEdge = sceneTopEdge * -1;

        _allAsteroidsOffScreen = true;

    }

    private void CreateAsteroids(float asteroidsNum)
    {
        for (int i = 1; i <= asteroidsNum; i++)
        {
            GameObject AsteroidClone = Instantiate(asteroid, new Vector2(Random.Range(-10, 10), 6f), transform.rotation);
            AsteroidClone.GetComponent<Asteroid>().SetGeneration(1);
            AsteroidClone.SetActive(true);
        }
    }

    public void RocketFail()
    {
        Cursor.visible = true;
        print("GAME OVER");
    }

    public void asterodDestroyed()
    {
        asteroidLife--;
    }

    public int startLevelAsteroidsNum
    {
        get { return _startLevelAsteroidsNum; }
    }

    public bool allAsteroidsOffScreen
    {
        get { return _allAsteroidsOffScreen; }
    }

}