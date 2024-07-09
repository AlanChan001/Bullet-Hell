using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] GameObject slashAnimPrefab;
    [SerializeField] private float swordAttackCD;
    [SerializeField] private WeaponInfo weaponinfo;
    
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private Transform weaponCollider;
    private GameObject slashAnim;
    private Transform slashAnimSpawnPoint;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashAnimSpawnPoint").transform;
    }


    private void Update()
    {
        MouseFollowWithOffset();
        
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponinfo;
    }

    public void Attack()
    {
                myAnimator.SetTrigger("Attack");
                weaponCollider.gameObject.SetActive(true);
                slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
                slashAnim.transform.parent = this.transform.parent;    
        
    }

    

    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);

    }

    public void SwingUpFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(180, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;

        }
    }

    public void SwingDownFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;

        }
    }

    void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg ;


        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 180, angle);
        }
        else if (mousePos.x > playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }
}
