using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttacker : MonoBehaviour, IEnemy
{
    private SpriteRenderer spriteRenderer;
    private Animator myAnimator;
    private EnemyPathfinding enemyPathfinding;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        enemyPathfinding = GetComponent<EnemyPathfinding>();
    }

    public void Attack()
    {
        PlayerHealth.Instance.TakeDamage(1,transform);
        //myAnimator.SetTrigger("Attack");
        if (transform.position.x < PlayerController.Instance.transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        if (transform.position.x > PlayerController.Instance.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void Death() { }
}