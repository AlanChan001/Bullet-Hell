using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public bool canMeleeAttack = false;

    [SerializeField] private float roamChangeDirTime = 2f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;

    private bool isAttacking = false;
    private Vector2 moveDir;
    private float timeRoaming = 0f;
    private State state;
    private EnemyPathfinding enemyPathfinding;

    private enum State
    {
        Roaming,
        Attacking
    }

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        moveDir =  GetRoamDirection();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            case State.Roaming:
                Roaming();
                break;
            case State.Attacking:
                Attacking();
                break;
            default:
                break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;

        enemyPathfinding.MoveTowards(moveDir);

        if (Vector2.Distance(PlayerController.Instance.transform.position, transform.position) < attackRange)
        {
            state = State.Attacking;
        }

        if (timeRoaming > roamChangeDirTime)
        {
            timeRoaming = 0;
            moveDir = GetRoamDirection();
        }
    }

    private void Attacking()
    {
        if (!isAttacking)
        {
            (enemyType as IEnemy).Attack();
            isAttacking = true;
            StartCoroutine(AttackCooldownRoutine());
        }

        if (Vector2.Distance(PlayerController.Instance.transform.position, transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        enemyPathfinding.StopMoving();
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private Vector2 GetRoamDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    
}
