using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;

    private Animator animator;

    public float attackRange = 2.5f;
    public float attackDamage = 25f;
    public LayerMask enemyLayer;

    public float attackCooldown = 0.5f;
    private float attackTimer = 0f;

    private int equippedWeapon = 0;

    public bool hasGun = false;
    public bool hasSword = false;

    public GameObject gunModel;
    public GameObject swordModel;

    public GameObject bulletPrefab;
    public Transform firePoint;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        enemyLayer = LayerMask.GetMask("Enemy");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gunModel.SetActive(false);
        swordModel.SetActive(false);
    }

    void Update()
    {

        attackTimer -= Time.deltaTime;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        float speed = new Vector3(moveDirection.x, 0, moveDirection.z).magnitude;
        animator.SetFloat("Speed", speed);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;

        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        if (Input.GetMouseButtonDown(0) && attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && hasSword)
        {
            equippedWeapon = 2;

            swordModel.SetActive(true);
            gunModel.SetActive(false);

            animator.SetInteger("weaponType", 2);
            Debug.Log("Sword Selected");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && hasGun)
        {
            equippedWeapon = 1;

            gunModel.SetActive(true);
            swordModel.SetActive(false);

            animator.SetInteger("weaponType", 1);
            Debug.Log("Gun Selected");
        }
    }

    private void Attack()
    {
        switch (equippedWeapon)
        {
            case 1: // Gun
                animator.SetTrigger("GunAttack");
                FireProjectile();
                Debug.Log("Gun Attack");
                break;

            case 2: // Sword
                animator.SetTrigger("SwordAttack");
                DealAttackDamage();
                Debug.Log("Sword Attack");
                break;

            default:
                animator.SetTrigger("Attack");
                DealAttackDamage();
                break;
        }
    }

    private void FireProjectile()
    {
        Debug.Log("Spawning bullet");

        Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );
    }

    public void DealAttackDamage()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        foreach (EnemyController enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance <= attackRange)
            {
                Debug.Log("Enemy hit by distance check!");
                enemy.TakeDamage(attackDamage);
            }
        }
    }

}
