using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public GameObject platformPrefab;
    public GameObject miniBoostPlatformPrefab;
    public GameObject boostPlatformPrefab;
    public GameObject brownPlatformPrefab;
    public GameObject whitePlatformPrefab;
    public GameObject platformFromagePrefab;
    public GameObject platformTomatePrefab;
    public GameObject platformThonPrefab;
    public GameObject platformPainPrefab;
    public GameObject platformSaladePrefab;

    public int numberOfPlatforms = 20;
    public float levelWidth = 3f;
    public float generationThreshold = 5f;
    public float minPlatformSpacing = 1f;
    public float maxPlatformSpacing = 3f;
    private float highestY;
    private float lastNonBrownPlatformY;
    private int score = 0;

    private List<GameObject> activePlatforms = new List<GameObject>();
    public Transform player;

    private int level = 1;
    private HashSet<string> collectedIngredients = new HashSet<string>();

    void Start() {
        Vector3 spawnPosition = new Vector3();
        highestY = 0;
        lastNonBrownPlatformY = 0;

        for (int i = 0; i < numberOfPlatforms; i++) {
            spawnPosition.y += GetPlatformSpacing();
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);
            SpawnPlatform(spawnPosition);
        }
    }

    void Update() {
        if (player.position.y > highestY) {
            score = (int)player.position.y; // Score basé sur la hauteur
        }

        if (player.position.y + generationThreshold > highestY) {
            Vector3 spawnPosition = new Vector3(Random.Range(-levelWidth, levelWidth), lastNonBrownPlatformY + GetPlatformSpacing(), 0);
            SpawnPlatform(spawnPosition);
        }

        activePlatforms.RemoveAll(platform => platform == null || platform.transform.position.y < player.position.y - 10f);

        // Mise à jour du niveau en fonction de la position Y du joueur
        if (player.position.y >= 100) {
            level = 3;
        } else if (player.position.y >= 50) {
            level = 2;
        } else {
            level = 1;
        }
    }

    void SpawnPlatform(Vector3 position) {
        float rand = Random.value;
        GameObject platformToSpawn;

        if (rand < 0.05f) {
            platformToSpawn = (rand < 0.025f) ? boostPlatformPrefab : miniBoostPlatformPrefab;
        } else if (rand < 0.10f) {
            platformToSpawn = GetRandomIngredientPlatform();
        } else if (level == 1) {
            platformToSpawn = platformPrefab;
        } else if (level == 2) {
            platformToSpawn = (rand < 0.15f) ? brownPlatformPrefab : platformPrefab;
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

        if (platformToSpawn != brownPlatformPrefab) {
            lastNonBrownPlatformY = position.y;
        }
    }

    GameObject GetRandomIngredientPlatform() {
        float rand = Random.value;
        if (rand < 0.2f) return platformFromagePrefab;
        if (rand < 0.4f) return platformTomatePrefab;
        if (rand < 0.6f) return platformThonPrefab;
        if (rand < 0.8f) return platformPainPrefab;
        return platformSaladePrefab;
    }

    float GetPlatformSpacing() {
        if (level == 1) {
            return Random.Range(minPlatformSpacing, maxPlatformSpacing / 2);
        } else if (level == 2) {
            return Random.Range(maxPlatformSpacing / 2, maxPlatformSpacing * 0.8f);
        } else {
            return maxPlatformSpacing;
        }
    }

    public void CollectIngredient(string ingredient) {
        collectedIngredients.Add(ingredient);
        if (collectedIngredients.Count == 5) {
            score += 1000; // Bonus de 1000 points
            collectedIngredients.Clear();
        }
    }

    public int GetScore() {
        return score;
    }
}
