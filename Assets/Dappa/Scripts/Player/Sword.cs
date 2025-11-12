using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private GameObject slashUltiAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private Transform ultimateCollider;
    [SerializeField] private float swordAttackCD = .5f;
    [SerializeField] private float ultimateCD = 10f;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;
    private Knockback knockback;
    private bool attackButtonDown, isAttacking = false;
    private bool ultiButtonDown, isUltiing = false;

    private GameObject slashAnim;
    private GameObject ultiAnim;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        knockback = GetComponentInParent<Knockback>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        playerControls.Combat.Ultimate.started += _ => StartUlti();
        playerControls.Combat.Ultimate.canceled += _ => StopUlti();
    }

    private void Update()
    {
        if (knockback.GettingKnockedBack) { return; }
        MouseFollowWithOffset();
        Attack();
        Ulti();
    }

    private void StartUlti()
    {
        ultiButtonDown = true;
    }

    private void StopUlti()
    {
        ultiButtonDown = false;
    }

    private void Ulti()
    {
        if (ultiButtonDown && !isUltiing)
        {
            isUltiing = true;
            myAnimator.SetTrigger("Ultimate");
            ultimateCollider.gameObject.SetActive(true);
            Debug.Log("Ultiing");

            SoundManager.Instance.PlayUltiSound();

            ultiAnim = Instantiate(slashUltiAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            ultiAnim.transform.parent = this.transform.parent;
            StartCoroutine(UltiCDRoutine());
        }
    }

    private IEnumerator UltiCDRoutine()
    {
        yield return new WaitForSeconds(ultimateCD);
        isUltiing = false;
    }
    private void DoneUltiingAnimEvent()
    {
        Debug.Log("Done Ultiing");
        ultimateCollider.gameObject.SetActive(false);
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }


    private void Attack()
    {
        if (attackButtonDown && !isAttacking)
        {
            isAttacking = true;
            myAnimator.SetTrigger("Attack");
            Debug.Log("Attacking");

            SoundManager.Instance.PlayAttackSound();

            weaponCollider.gameObject.SetActive(true);
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;
            StartCoroutine(AttackCDRoutine());
        }
    }
    
    private IEnumerator AttackCDRoutine()
    {
        yield return new WaitForSeconds(swordAttackCD);
        isAttacking = false;
    }

    private void DoneAttackingAnimEvent()
    {
        Debug.Log("Done Attacking");
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(180, 0, 0);

        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}