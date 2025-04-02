using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public float jumpForce = 10f;
    public bool isBrownPlatform = false; // Pour les plateformes marron
    public bool isWhitePlatform = false; // Pour les plateformes blanches

    private Collider2D platformCollider; // Pour récupérer le Collider2D

    void Start() {
        platformCollider = GetComponent<Collider2D>(); // Récupère le Collider2D
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (isBrownPlatform) 
                {
                    // On désactive le collider avant de détruire la plateforme
                    platformCollider.enabled = false; // Empêche toute nouvelle collision
                    Destroy(gameObject); // Détruit la plateforme marron après un court délai
                }
                else
                {
                    Vector2 velocity = rb.linearVelocity;
                    velocity.y = jumpForce;
                    rb.linearVelocity = velocity;

                    if (isWhitePlatform)
                    {
                        Destroy(gameObject, 0.1f); // La plateforme blanche disparaît après un saut
                    }
                }
            }
        }
    }
}
