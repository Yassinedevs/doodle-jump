using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public GameObject platformPrefab;
    public GameObject bluePlatformPrefab;
    public GameObject brownPlatformPrefab;
    public GameObject whitePlatformPrefab;

    public int numberOfPlatforms = 20;
    public float levelWidth = 3f;
    public float generationThreshold = 5f;
    public float platformSpacing = 5f; // Distance minimale entre plateformes
    private float highestY;
    private float startTime;

    private List<GameObject> activePlatforms = new List<GameObject>();
    public Transform player;

    void Start() {
        Vector3 spawnPosition = new Vector3();
        highestY = 0;
        startTime = Time.time;

        for (int i = 0; i < numberOfPlatforms; i++) {
            spawnPosition.y += platformSpacing;
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);
            SpawnPlatform(spawnPosition);
        }
    }

    void Update() {
        if (player.position.y + generationThreshold > highestY) {
            Vector3 spawnPosition = new Vector3(Random.Range(-levelWidth, levelWidth), highestY + platformSpacing, 0);
            SpawnPlatform(spawnPosition);
        }

        activePlatforms.RemoveAll(platform => platform == null || platform.transform.position.y < player.position.y - 10f);
    }

    void SpawnPlatform(Vector3 position) {
        float rand = Random.value;
        float elapsedTime = Time.time - startTime;
        GameObject platformToSpawn;

        if (rand < 0.05f) {
            platformToSpawn = bluePlatformPrefab; // 5% chance tout le long
        } else if (elapsedTime < 60f) {
            platformToSpawn = platformPrefab; // Première minute : seulement plateformes vertes
        } else if (elapsedTime < 120f) {
            platformToSpawn = (rand < 0.15f) ? brownPlatformPrefab : platformPrefab; // Deuxième minute : plateformes marrons commencent à apparaître
        } else {
            if (rand < 0.15f) {
                platformToSpawn = brownPlatformPrefab;
            } else if (rand < 0.30f) {
                platformToSpawn = whitePlatformPrefab;
            } else {
                platformToSpawn = platformPrefab;
            }
        }

        GameObject newPlatform = Instantiate(platformToSpawn, position, Quaternion.identity);
        activePlatforms.Add(newPlatform);
        highestY = position.y;
    }
}
