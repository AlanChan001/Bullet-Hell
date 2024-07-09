using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    public float roamChangeDirTime = 2f;
    public bool stopMovingWhileAttacking = false;
    [Range(0, 359)]  public  float angleSpread;
    public int projectilesPerBurst;
    public float timeBetweenBurst;
    public int burstCount;
    public float restTime = 3f;

    public bool stagger;
    [Tooltip("Stagger must be enabled for oscillate to work properly")]
    public bool oscillate;
    public float timeBetweenProjectiles = 0.1f;

    public GameObject bulletPrefab;
    [SerializeField] float attackRange = 10f;
    [SerializeField] private float startingDistance = 0.1f;
    public float bulletMoveSpeed;

    public bool isShooting = false;
    public Vector2 moveDir;
    public float timeRoaming = 0f;
    public bool canMove = true;
    private State state;
    public EnemyPathfinding enemyPathfinding;

    private void OnValidate()
    {
        if (!stagger) { oscillate = false; }
        if (timeBetweenBurst < 0.1f) { timeBetweenBurst = 0.1f; }
        if (restTime < 0.1f) { restTime = 0.1f; }
        if (startingDistance < 0.1f) { startingDistance = 0.1f; }
        if (angleSpread == 0) { projectilesPerBurst = 1; }
        if (bulletMoveSpeed <= 0) { bulletMoveSpeed = 0.1f; }
    }

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

    protected virtual void Start()
    {
        moveDir = GetRoamDirection();
    }

    protected virtual void Update()
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
        if (Vector2.Distance(PlayerController.Instance.transform.position, transform.position) > attackRange)
        {
            state = State.Roaming;
        }
        if (!isShooting)
        {
            Attack();
        }
        if (stopMovingWhileAttacking && !canMove)
        {
            enemyPathfinding.StopMoving();
        }
        else
        {
            timeRoaming += Time.deltaTime;
            enemyPathfinding.MoveTowards(moveDir);
            if (timeRoaming > roamChangeDirTime)
            {
                timeRoaming = 0f;
                moveDir = GetRoamDirection();
            }
        }
    }



    private Vector2 GetRoamDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }


    public void Attack()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    protected virtual IEnumerator ShootRoutine()
    {
        isShooting = true;
        canMove = false;
        float currentAngle, startAngle, angleStep, endAngle;

        TargetConeOfInfluence(out currentAngle, out startAngle, out angleStep, out endAngle);

        for (int i = 0; i < burstCount; i++)
        {
            if (!oscillate)
            {
                TargetConeOfInfluence(out currentAngle, out startAngle, out angleStep, out endAngle);
            }

            if (oscillate && i % 2 == 0)
            { 
                TargetConeOfInfluence(out currentAngle, out startAngle, out angleStep, out endAngle);
            }

            else if (oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);

                }
                currentAngle += angleStep;

                if (stagger) {yield return new WaitForSeconds(timeBetweenProjectiles); }

            }

            currentAngle = startAngle;

            yield return new WaitForSeconds(timeBetweenBurst); 
        }
        canMove = true;
        yield return new WaitForSeconds(restTime);
        isShooting = false;

    }

    public void TargetConeOfInfluence(out float currentAngle, out float startAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        currentAngle = targetAngle;
        startAngle = targetAngle;
        endAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0f;
        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2;
            startAngle = targetAngle - angleSpread / 2;
            endAngle = targetAngle + angleSpread / 2;
            currentAngle = startAngle;
        }
    }

    public Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);
        return pos;
    }

    public virtual void Death() 
    {
    }
}
