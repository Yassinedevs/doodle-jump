using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public float jumpForce = 10f;
    public bool isBrownPlatform = false; // Pour les plateformes marron
    public bool isWhitePlatform = false; // Pour les plateformes blanches
    public bool isIngredientPlatform = false; // Indique si c'est une plateforme avec un ingrédient
    public string ingredientType; // Type d'ingrédient pour identification
    public GameObject normalPlatformPrefab; // Référence à la plateforme classique pour remplacement

    private Collider2D platformCollider;

    void Start() {
        platformCollider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.relativeVelocity.y <= 0f) {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            if (rb != null) {
                if (isBrownPlatform) {
                    platformCollider.enabled = false;
                    Destroy(gameObject);
                } 
                else if (isIngredientPlatform) {
                    CollectIngredient();
                }
                else {
                    Vector2 velocity = rb.linearVelocity;
                    velocity.y = jumpForce;
                    rb.linearVelocity = velocity;
                    if (isWhitePlatform) {
                        Destroy(gameObject, 0.1f);
                    }
                }
            }
        }
    }

    void CollectIngredient() {
        LevelGenerator levelGen = Object.FindFirstObjectByType<LevelGenerator>();
        if (levelGen != null) {
            levelGen.CollectIngredient(ingredientType);
        }
        ReplaceWithNormalPlatform();
    }

    void ReplaceWithNormalPlatform() {
        if (normalPlatformPrefab != null) {
            Instantiate(normalPlatformPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
