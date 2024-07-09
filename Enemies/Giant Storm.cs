using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GiantStorm : Shooter
{
    [SerializeField] private string BossName;
    [SerializeField] private GameObject FireBlock;

    private int currentHealth;
    private int maxHealth;
    private EnemyHealth enemyHealth;
    private TMP_Text healthText;
    private Slider healthSlider;

    protected override void Start()
    {
        base.Start();
        BossSlider.Instance.ActivateSlider();
        enemyHealth = GetComponent<EnemyHealth>();
        maxHealth = enemyHealth.maxHealth;
    }

    protected override void Update()
    {
        base.Update();
        UpdateHealthSliderAndText();
    }

    protected override IEnumerator ShootRoutine()
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

                if (stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); }

            }
            projectilesPerBurst++;
            currentAngle = startAngle;

            yield return new WaitForSeconds(timeBetweenBurst);
        }
        canMove = true;
        projectilesPerBurst -= burstCount;
        yield return new WaitForSeconds(restTime);
        isShooting = false;

    }

    public void UpdateHealthSliderAndText()
    {
        if (healthSlider == null)
        { 
            healthSlider = GameObject.Find("Boss Health Slider").GetComponent<Slider>();
            healthSlider.maxValue = maxHealth;
        }

        if (healthText == null)
        {
            healthText = GameObject.Find("Boss Health Text").GetComponent<TMP_Text>();
        }
        currentHealth = enemyHealth.currentHealth;
        if (currentHealth <= maxHealth / 2)
        {
            changeAttackMode();
        }
        healthSlider.value = currentHealth;
        healthText.text = BossName + " " + currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public void changeAttackMode()
    {
        stopMovingWhileAttacking = false;
        stagger = true;
        oscillate = true;
        enemyPathfinding.moveSpeed = 5;
        roamChangeDirTime = 1;
        timeBetweenBurst = 0.5f;
    }

    public override void Death()
    {
        Destroy(FireBlock.gameObject);
        healthSlider.gameObject.SetActive(false);
    }
}
