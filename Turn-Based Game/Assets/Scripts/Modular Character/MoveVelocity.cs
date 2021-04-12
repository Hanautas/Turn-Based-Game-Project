using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour, IMoveVelocity {

    private float moveSpeed;

    private Vector3 velocityVector;
    private Rigidbody2D rb2D;

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();

        moveSpeed = 2.5f;
    }

    public void SetVelocity(Vector3 velocityVector) {
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate() {
        rb2D.velocity = velocityVector * moveSpeed;
    }

    public void Disable() {
        this.enabled = false;
        rb2D.velocity = Vector3.zero;
    }

    public void Enable() {
        this.enabled = true;
    }

}
