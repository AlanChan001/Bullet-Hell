using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private MonoBehaviour Enemy;

    private Knockback knockback;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (knockback)
        {
            knockback.GetKnockedBack(PlayerController.Instance.transform, 15f);
        }
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(DetectDeathRoutine());
    }

    private IEnumerator DetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetDefaultMatTime());
        DetectDeath();

    }

    void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GetComponent<PickUpSpawner>()?.DropItems();
            Instantiate(deathVFXPrefab,transform.position, Quaternion.identity);
            (Enemy as IEnemy).Death();
            Destroy(gameObject);
        }

    }
}
