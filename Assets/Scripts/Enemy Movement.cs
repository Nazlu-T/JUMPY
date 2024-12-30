using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    Rigidbody2D myRigidbody;
    BoxCollider2D flipEnemyCollider;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        flipEnemyCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;

        FlipEnemy();
    }

    void FlipEnemy()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }
}
