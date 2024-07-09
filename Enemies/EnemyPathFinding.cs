using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockback;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    private void FixedUpdate()
    {
        if (knockback && knockback.gettingKnockedBack) { return; }
        
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);

        if (moveDir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        
    }

    public void MoveTowards(Vector2 moveDir)
    {
        this.moveDir = moveDir;

    }

    public void StopMoving()
    {
        moveDir = Vector3.zero;
    }
}
