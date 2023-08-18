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
    private bool isBlockedSuccessfully;
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
    [SerializeField] private int maxEnergy = 400;
    [SerializeField] private int energyReviveSpeed = 1;
    [SerializeField] private int energyReduceAttack = 50, energyReduceDash = 100, energyReduceDodge = 20;
    private int energy;
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
    [SerializeField] private float rotationSpeed = 6f;
    private Quaternion rotateTowards;
    private Quaternion targetRotation;
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
    [SerializeField] private float transitionDuration = 2f;
    private float normalXMaxSpeed, normalYMaxSpeed;
    private float desirableFOV;
    #endregion

    #region Focus
    [Header("Focus")]
    [SerializeField] private Vector3 focusCheckSphereOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] private float focusCheckSphereRadius = 20f;
    private Transform focusTarget;
    private bool isFocusTargetSet;
    #endregion

    #region Stuff
    [Header("Stuff")]
    [SerializeField, Tooltip("" +
    "Offset from object's center where ground check ray starts." +
    "")]
    private Vector3 groundCheckRayOffset = new Vector3(0f, 0.1f, 0f);
    [SerializeField, Tooltip("" +
        "Offset from object's center where pop-up text spawns." +
        "")]
    private Vector3 popUpTextOffset = new Vector3(0f, 1.25f, 0f);
    #endregion

    #region References
    [Header("References")]
    [SerializeField] private CinemachineFreeLook cinemachineCamera;
    [SerializeField] private GameObject ragdollPrefab;
    [SerializeField] private Slider healthBar, energyBar;
    private Rigidbody rb;
    private Transform mainCamera;
    private PlayerInputScheme playerInputScheme;
    private List<Transform> focusTargets = new List<Transform>();
    private Animator anim;
    private PopUpTextManager popUpTextManager;
    private NavMeshManager navMeshManager;
    #endregion

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();
        GameObject mainManager = GameObject.FindGameObjectWithTag("Manager");
        popUpTextManager = mainManager.GetComponent<PopUpTextManager>();
        navMeshManager = mainManager.GetComponent<NavMeshManager>();
    }

    private void Start()
    {
        navMeshManager.GenerateNavMeshSurface();

        healthBar.maxValue = maxHealth;
        health = maxHealth;

        energyBar.maxValue = maxEnergy;
        energy = maxEnergy;

        normalXMaxSpeed = cinemachineCamera.m_XAxis.m_MaxSpeed;
        normalYMaxSpeed = cinemachineCamera.m_YAxis.m_MaxSpeed;
        SetCinemachineCamera(false, normalXMaxSpeed, normalYMaxSpeed);
        focusTarget = transform;

        desirableFOV = cameraNormalFOV;

        normalXMaxSpeed = cinemachineCamera.m_XAxis.m_MaxSpeed;
        normalYMaxSpeed = cinemachineCamera.m_YAxis.m_MaxSpeed;

        moveSpeed = walkSpeed;
    }

    private void FixedUpdate() { Movement(); GroundCheck(); Rotation(); }

    private void Update()
    {
        if (health <= 0f) Death();
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.value = health;

        if (energy < maxEnergy) energy += energyReviveSpeed;
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        energyBar.value = energy;

        SmoothMoveInput();
        DashUpdate();
        FocusUpdate();
        Animation();

        if (isBlocking && energy < energyReduceDodge) isBlocking = false;

    }

    private void OnEnable()
    {
        if (playerInputScheme == null)
        {
            playerInputScheme = new PlayerInputScheme();

            playerInputScheme.PlayerMovement.Movement.performed += playerInputScheme => moveInput = playerInputScheme.ReadValue<Vector2>();

            playerInputScheme.Actions.Attack.started += playerInputScheme => { if (energy >= energyReduceAttack) Attack(); };

            playerInputScheme.Actions.Dash.started += playerInputScheme => { if (moveInput.magnitude > 0f && energy >= energyReduceDash) Dash(); };

            playerInputScheme.Actions.Block.started += playerInputScheme => { if (energy >= energyReduceDodge) isBlocking = true; };
            playerInputScheme.Actions.Block.canceled += playerInputScheme => isBlocking = false;

            playerInputScheme.Actions.FocusOnEnemy.started += playerInputScheme => { isFocusTargetSet = false; UpdateFocusTargets(); SetFocusTarget(); };
        }

        playerInputScheme.Enable();
    }

    private void OnDisable() { playerInputScheme.Disable(); }

    private void SmoothMoveInput() { curInputVector = Vector2.SmoothDamp(curInputVector, moveInput, ref smoothInputVelocity, smoothTime); }

    public void TakeDamage(int damage, bool willDodge)
    {
        anim.SetTrigger("Hit");

        if (!isBlocking || isBlocking && !willDodge && !isBlockedSuccessfully)
        {
            health -= damage;
            popUpTextManager.DisplayDamagePopUpText(damage, transform.position + popUpTextOffset);
        }
        else
        {
            energy -= energyReduceDodge;
            popUpTextManager.DisplayDodgedPopUpText(transform.position + popUpTextOffset);
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

        popUpTextManager.DisplayDeathPopUpText(transform.position + popUpTextOffset);

        Destroy(gameObject);
    }

    private void Movement()
    {
        moveDirection = mainCamera.right * curInputVector.x + mainCamera.forward * curInputVector.y;
        moveDirection.y = 0f;

        targetVelocity = moveDirection * moveSpeed;
        if (!isDashing) targetVelocity.y = rb.velocity.y;
        else targetVelocity.y = 0f;

        rb.velocity = targetVelocity;
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position + groundCheckRayOffset, Vector3.down, 0.2f);

        if (isGrounded && !isDashing) rb.drag = groundDrag;
        else rb.drag = 0f;
    }

    private void Rotation()
    {
        if (moveInput.magnitude > 0f)
        {
            rotateTowards = Quaternion.LookRotation(moveDirection);
            targetRotation = Quaternion.Slerp(transform.rotation, rotateTowards, rotationSpeed * Time.deltaTime);
        }
        transform.rotation = targetRotation;
    }

    private void Attack()
    {
        energy -= energyReduceAttack;
        anim.SetTrigger("Attack");
    }

    private void Dash()
    {
        if (dashCooldownTimer > 0f) return;
        else dashCooldownTimer = dashCooldown;

        isDashing = true;

        energy -= energyReduceDash;

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
        if (!isFocusTargetSet)
        {
            cinemachineCamera.m_LookAt = focusTarget;

            if (focusTarget == transform)
            {
                SetCinemachineCamera(false, normalXMaxSpeed, normalYMaxSpeed);
            }
            else
            {
                SetCinemachineCamera(true, 0f, 0f);
            }

            isFocusTargetSet = true;
        }

        if (focusTarget != transform) transform.LookAt(new Vector3(focusTarget.position.x, transform.position.y, focusTarget.position.z));

    }

    private void SetFocusTarget()
    {
        int desiredFocusTargetIndex = focusTargets.IndexOf(focusTarget) + 1;
        if (desiredFocusTargetIndex < focusTargets.Count) focusTarget = focusTargets[desiredFocusTargetIndex];
        else focusTarget = transform;
    }

    private void UpdateFocusTargets()
    {
        focusTargets.Clear();

        focusTargets.Add(transform);

        List<Collider> cols = Physics.OverlapSphere(transform.position + focusCheckSphereOffset, focusCheckSphereRadius).ToList();
        foreach (Collider col in cols) if (col.CompareTag("Enemy")) focusTargets.Add(col.GetComponent<Transform>());
    }

    private void SetCinemachineCamera(bool isRecenteringOn, float xMaxSpeed, float yMaxSpeed)
    {
        cinemachineCamera.m_RecenterToTargetHeading.m_enabled = isRecenteringOn;
        cinemachineCamera.m_YAxisRecentering.m_enabled = isRecenteringOn;

        cinemachineCamera.m_XAxis.m_MaxSpeed = xMaxSpeed;
        cinemachineCamera.m_YAxis.m_MaxSpeed = yMaxSpeed;
    }

    public void DeleteFromFocusTargets(Transform targetToDelete)
    {
        if (focusTargets.Contains(targetToDelete)) focusTargets.Remove(targetToDelete);
        if (focusTarget == targetToDelete) SetFocusTarget();
        isFocusTargetSet = false;
    }

    private void Animation()
    {
        movePower = Mathf.Clamp01(curInputVector.magnitude);
        anim.SetFloat("Move Power", movePower);

        anim.SetBool("Is Dashing", isDashing);

        anim.SetBool("Is Blocking", isBlocking);
    }
}
