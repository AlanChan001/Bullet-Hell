using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destoyVFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DamageSource>()||(other.gameObject.GetComponent<Projectile>()))
        {
            if (other.GetComponent<Projectile>() && other.GetComponent<Projectile>().isEnemyProjectile)
            {
                return;
            }

            PickUpSpawner pickUpSpawner = GetComponent<PickUpSpawner>();
            pickUpSpawner?.DropItems();
            Instantiate(destoyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }


    }

}
