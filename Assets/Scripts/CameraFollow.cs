using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float margin = 1.0f; // Marge vers le bas

    void LateUpdate () {
        if (target.position.y - margin > transform.position.y)
        {
            Vector3 newPos = new Vector3(transform.position.x, target.position.y - margin, transform.position.z);
            transform.position = newPos;
        }
    }
}