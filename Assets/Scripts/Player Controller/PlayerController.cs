using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Input
    private Vector2 moveInput;
    private float movePower;
    private bool isDashing, isBlocking, isFocusing;
    #endregion

    #region Input Smoothness
    [Header("Input Smoothness")]
    [SerializeField] private float smoothTime = 0.1f;
    private Vector2 curInputVector, smoothInputVelocity;
    #endregion

    #region Health
    [Header("Health")]
    [SerializeField] private int maxHealth = 200;
    private int health;
    #endregion

    #region Energy
    [Header("Energy")]
    [SerializeField] private int maxEnergy = 200;
    private int energy;
    [SerializeField] private int minEnergyReduceAttack = 25, maxEnergyReduceAttack = 50;
    [SerializeField] private int minEnergyReduceDash = 50, maxEnergyReduceDash = 100;
    [SerializeField] private int minEnergyReduceDodge = 1, maxEnergyReduceDodge = 15;
    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 6f;
    private float moveSpeed;
    private Vector3 targetVelocity, moveDirection;
    #endregion

    #region Ground Check
    [Header("Ground Check")]
    [SerializeField] private float groundDrag = 5f;
    private bool isGrounded;
    #endregion

    #region Rotation
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 8f;
    #endregion

    #region Dashing
    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 10f;
    private float dashCooldownTimer;
    #endregion

    #region Camera
    [Header("Camera")]
    [SerializeField] private float cameraNormalFOV = 40f;
    [SerializeField] private float cameraDashFOV = 60f;
    private float desirableFOV;
    [SerializeField] private float transitionDuration = 2f;
    private float normalXMaxSpeed, normalYMaxSpeed;
    #endregion

    #region References
    [Header("References")]
    [SerializeField] private CinemachineFreeLook cinemachineCamera;
    [SerializeField, Tooltip("Empty Game Object That Takes Transform.")] private GameObject fixateTargetGameObject;
    [SerializeField, Tooltip("" +
    "Position, Where the Ground Check Ray Starts." +
    "")]
    private Transform groundCheckRayStartPositionObject;
    [SerializeField] private Transform popUpTextSpawnPosition;
    [SerializeField] private GameObject ragdollPrefab;
    [SerializeField] private Slider healthBar, energyBar;
    private Rigidbody rb;
    private Transform mainCamera;
    private PlayerInputScheme playerInputScheme;
    private Animator anim;
    private List<GameObject> enemies;
    private PopUpTextManager popUpTextManager;
    #endregion

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();
        popUpTextManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<PopUpTextManager>();
    }

    private void Start()
    {
        healthBar.maxValue = maxHealth;
        health = maxHealth;

        energyBar.maxValue = maxEnergy;
        energy = maxEnergy;

        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        fixateTargetGameObject.transform.position = transform.position;

        desirableFOV = cameraNormalFOV;

        normalXMaxSpeed = cinemachineCamera.m_XAxis.m_MaxSpeed;
        normalYMaxSpeed = cinemachineCamera.m_YAxis.m_MaxSpeed;

        moveSpeed = walkSpeed;
    }

    private void FixedUpdate() { Movement(); GroundCheck(); Rotation(); }

    private void Update()
    {
        SmoothMoveInput();
        DashUpdate();
        FocusUpdate();
        Animation();

        if (isBlocking && energy <= maxEnergyReduceDodge) isBlocking = false;

        if (health <= 0f) Death();
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.value = health;

        if (energy < maxEnergy) energy++;
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        energyBar.value = energy;
    }

    private void OnEnable()
    {
        if (playerInputScheme == null)
        {
            playerInputScheme = new PlayerInputScheme();

            playerInputScheme.PlayerMovement.Movement.performed += playerInputScheme => moveInput = playerInputScheme.ReadValue<Vector2>();
            playerInputScheme.Actions.Attack.started += playerInputScheme => { if (energy >= maxEnergyReduceAttack) Attack(); };
            playerInputScheme.Actions.Dash.started += playerInputScheme =>
            {
                if (moveInput.magnitude > 0f && energy >= maxEnergyReduceDash) Dash();
            };
            playerInputScheme.Actions.Block.started += playerInputScheme => { if (energy >= maxEnergyReduceDodge) isBlocking = true; };
            playerInputScheme.Actions.Block.canceled += playerInputScheme => isBlocking = false;
            playerInputScheme.Actions.FocusOnEnemy.started += playerInputScheme =>
            {
                isFocusing = !isFocusing;
                try { fixateTargetGameObject.transform.position = GetClosestEnemy().transform.position; }
                catch { isFocusing = false; }
            };
        }

        playerInputScheme.Enable();
    }

    private void OnDisable() { playerInputScheme.Disable(); }

    private void SmoothMoveInput() { curInputVector = Vector2.SmoothDamp(curInputVector, moveInput, ref smoothInputVelocity, smoothTime); }

    public void TakeDamage(int damage, bool willDodge)
    {
        anim.SetTrigger("Hit");

        if (!isBlocking || isBlocking && !willDodge)
        {
            health -= damage;
            popUpTextManager.DisplayDamagePopUpText("-" + damage.ToString(), popUpTextSpawnPosition);
        }
        else
        {
            energy -= Random.Range(minEnergyReduceDodge, maxEnergyReduceDodge);
            popUpTextManager.DisplayDodgedPopUpText(popUpTextSpawnPosition);
        }
    }

    private void Death()
    {
        GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation, transform.parent);
        cinemachineCamera.m_XAxis.m_MaxSpeed = normalXMaxSpeed;
        cinemachineCamera.m_YAxis.m_MaxSpeed = normalYMaxSpeed;
        cinemachineCamera.m_RecenterToTargetHeading.m_enabled = false;
        cinemachineCamera.m_YAxisRecentering.m_enabled = false;
        cinemachineCamera.m_Follow = ragdoll.transform;
        cinemachineCamera.m_LookAt = ragdoll.transform;
        popUpTextManager.DisplayDeathPopUpText(popUpTextSpawnPosition);
        Destroy(gameObject);
    }

    private void Movement()
    {
        moveDirection = mainCamera.right * curInputVector.x + mainCamera.forward * curInputVector.y;
        moveDirection.y = 0f;

        targetVelocity = moveDirection * moveSpeed;
        targetVelocity.y = rb.velocity.y;

        rb.velocity = targetVelocity;
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(groundCheckRayStartPositionObject.position, Vector3.down, 0.2f);

        if (isGrounded && !isDashing) rb.drag = groundDrag;
        else rb.drag = 0f;
    }

    private void Rotation()
    {
        if (moveInput.magnitude > 0f)
        {
            Quaternion rotateTowards = Quaternion.LookRotation(moveDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rotateTowards, rotationSpeed * Time.deltaTime);

            transform.rotation = targetRotation;
        }
    }

    private void Attack()
    {
        energy -= Random.Range(minEnergyReduceAttack, maxEnergyReduceAttack);
        anim.SetTrigger("Attack");
    }

    private void Dash()
    {
        if (dashCooldownTimer > 0f) return;
        else dashCooldownTimer = dashCooldown;

        isDashing = true;

        energy -= Random.Range(minEnergyReduceDash, maxEnergyReduceDash);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash() { isDashing = false; }

    private void DashUpdate()
    {
        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;

        if (isDashing && moveInput.magnitude == 0f) ResetDash();

        if (isDashing)
        {
            moveSpeed = dashSpeed;
            desirableFOV = Mathf.MoveTowards(desirableFOV, cameraDashFOV, transitionDuration);
        }
        else
        {
            moveSpeed = walkSpeed;
            desirableFOV = Mathf.MoveTowards(desirableFOV, cameraNormalFOV, transitionDuration);
        }

        cinemachineCamera.m_Lens.FieldOfView = desirableFOV;
    }

    private void FocusUpdate()
    {
        if (isFocusing)
        {
            cinemachineCamera.m_XAxis.m_MaxSpeed = 0f;
            cinemachineCamera.m_YAxis.m_MaxSpeed = 0f;

            cinemachineCamera.m_RecenterToTargetHeading.m_enabled = true;
            cinemachineCamera.m_YAxisRecentering.m_enabled = true;
        }
        else
        {
            cinemachineCamera.m_XAxis.m_MaxSpeed = normalXMaxSpeed;
            cinemachineCamera.m_YAxis.m_MaxSpeed = normalYMaxSpeed;

            cinemachineCamera.m_RecenterToTargetHeading.m_enabled = false;
            cinemachineCamera.m_YAxisRecentering.m_enabled = false;

            fixateTargetGameObject.transform.position = transform.position;
        }

        cinemachineCamera.m_LookAt = fixateTargetGameObject.transform;
        if (fixateTargetGameObject.transform.position != transform.position)
        {
            transform.LookAt(new Vector3(fixateTargetGameObject.transform.position.x, transform.position.y, fixateTargetGameObject.transform.position.z));
        }
    }

    public void ResetFocus() { isFocusing = false; }

    private Transform GetClosestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        if (enemies == null) return null;

        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject o in enemies)
        {
            if (o == null)
            {
                enemies.Remove(o);
                return null;
            }

            float dist = Vector3.Distance(o.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = o.transform;
                minDist = dist;
            }
        }

        return tMin;
    }

    private void Animation()
    {
        movePower = Mathf.Clamp01(curInputVector.magnitude);
        anim.SetFloat("Move Power", movePower);

        anim.SetBool("Is Dashing", isDashing);

        anim.SetBool("Is Blocking", isBlocking);
    }
}
