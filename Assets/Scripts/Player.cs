using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

    public float movementSpeed = 10f;
    public float screenWidth = 10f; // Largeur de l'écran (ajustable dynamiquement si nécessaire)
    public float deathThreshold = -10f; // Position Y en dessous de laquelle le joueur meurt

    Rigidbody2D rb;
    float movement = 0f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        movement = Input.GetAxis("Horizontal") * movementSpeed;

        // Gestion du passage d'un bord à l'autre
        Vector3 playerPosition = transform.position;
        float halfWidth = screenWidth / 2;

        if (playerPosition.x > halfWidth) {
            playerPosition.x = -halfWidth;
        } else if (playerPosition.x < -halfWidth) {
            playerPosition.x = halfWidth;
        }

        transform.position = playerPosition;

        // Vérifie si le joueur est tombé en dessous du seuil de mort
        if (playerPosition.y < deathThreshold) {
            Die();
        }
    }

    void FixedUpdate() {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = movement;
        rb.linearVelocity = velocity;
    }

    void Die() {
        Debug.Log("Game Over!");
        Destroy(gameObject); // Supprime le joueur du jeu
    }
}
