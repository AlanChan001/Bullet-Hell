using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] float laserGrowTime = 2f;

    private bool isGrowing = true;
    private float laserRange;
    private WeaponInfo weaponInfo;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D myCollider;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Indestructible>() && !other.isTrigger)
        {
            isGrowing = false;
        }
    }

    public void UpdateLaserRange(float laserRange)
    {
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;
        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;

            spriteRenderer.size = new Vector2(Mathf.Lerp(1f,laserRange , linearT), 1f);
            myCollider.size = new Vector2(Mathf.Lerp(1, laserRange, linearT),myCollider.size.y);
            myCollider.offset = new Vector2(Mathf.Lerp(1, laserRange, linearT)/2, myCollider.offset.y);
            yield return null;
        }
        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    private void Update()
    {
        
    }

    private void LaserFaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = transform.position - mousePosition;
        transform.right = -direction;
    }

    public void UpdateWeaponInfo(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;

    }
}
