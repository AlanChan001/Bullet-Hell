using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject grapeProjectilePrefab;
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
        myAnimator.SetTrigger("Attack");
        if (transform.position.x < PlayerController.Instance.transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        if (transform.position.x > PlayerController.Instance.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

    }

    public void SpawnProjectileAnimEvent()
    {
        Instantiate(grapeProjectilePrefab, transform.position, Quaternion.identity);
    }

    public void Death(){ }
}
