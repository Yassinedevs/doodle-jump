using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public GameObject platformPrefab;
    public GameObject miniBoostPlatformPrefab;
    public GameObject boostPlatformPrefab;
    public GameObject brownPlatformPrefab;
    public GameObject whitePlatformPrefab;

    public int numberOfPlatforms = 20;
    public float levelWidth = 3f;
    public float generationThreshold = 5f;
    public float minPlatformSpacing = 5f;
    public float maxPlatformSpacing = 40f;
    private float highestY;
    private float lastNonBrownPlatformY;

    private List<GameObject> activePlatforms = new List<GameObject>();
    public Transform player;

    private int level = 1;

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
            if (rand < 0.025f) {
                platformToSpawn = boostPlatformPrefab; // 2.5% chance
            } else {
                platformToSpawn = miniBoostPlatformPrefab; // 2.5% chance
            }
        } else if (level == 1) {
            platformToSpawn = platformPrefab; // Niveau 1 : uniquement plateformes vertes, plus nombreuses
        } else if (level == 2) {
            platformToSpawn = (rand < 0.15f) ? brownPlatformPrefab : platformPrefab; // Niveau 2 : plateformes marrons commencent à apparaître
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

        // Ne met à jour lastNonBrownPlatformY que si la plateforme n'est pas marron
        if (platformToSpawn != brownPlatformPrefab) {
            lastNonBrownPlatformY = position.y;
        }
    }

    float GetPlatformSpacing() {
        if (level == 1) {
            return Random.Range(minPlatformSpacing, maxPlatformSpacing / 2); // Plus de plateformes, espacement réduit
        } else if (level == 2) {
            return Random.Range(maxPlatformSpacing / 2, maxPlatformSpacing * 0.8f); // Moins de plateformes
        } else {
            return maxPlatformSpacing; // Niveau 3 : espacement maximum
        }
    }
}
