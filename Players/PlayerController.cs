using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }


    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailrenderer;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private Knockback knockback;

    private bool facingLeft = false;
    private bool isDashing = false;
    private bool isCheating = false;


    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();    
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();
        playerControls.CheatMode.Cheat.performed += _ => EnableCheat();
        ActiveInventory.Instance.EquipStartingWeapon();
    }


    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }


    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Move();
        AdjustPlayerFacingDirection();
        CheatMode();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }


    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>().normalized;
        CheatMode();
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (knockback.gettingKnockedBack || PlayerHealth.Instance.IsDead)
        {
            return;
        }
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
            FacingLeft = true;
        }
        else if (mousePos.x > playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = false;
            FacingLeft = false;
        }
    }

    private void Dash()
    {
        if (!isDashing)
        {
            if (Stamina.Instance.CurrentStamina <= 0) { return; }
            Stamina.Instance.UseStamina();
            isDashing = true;
            myTrailrenderer.emitting = true;
            moveSpeed *= dashSpeed;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        
        float dashTime = 0.2f;
        float dashCD = 0.8f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed /= dashSpeed;
        myTrailrenderer.emitting = false;

        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    private void CheatMode()
    {
        if (isCheating)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        else
        {
            GetComponent<CapsuleCollider2D>().enabled = true;
        }
    }

    private void EnableCheat()
    { 
        isCheating = !isCheating;
    }
}
