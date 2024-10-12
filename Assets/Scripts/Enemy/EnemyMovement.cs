using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyScriptableObject enemyData; // Holds the shared data

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Vector3 lastPosition;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // Move the enemy towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemyData.MoveSpeed * Time.deltaTime);

        // Flip the sprite based on movement direction
        FlipSprite();

        // Store the last position for the next frame
        lastPosition = transform.position;
    }

    void FlipSprite()
    {
        Vector3 movementDirection = transform.position - lastPosition;

        if (movementDirection.x < 0)
            spriteRenderer.flipX = true;
        else if (movementDirection.x > 0)
            spriteRenderer.flipX = false;
    }
}
