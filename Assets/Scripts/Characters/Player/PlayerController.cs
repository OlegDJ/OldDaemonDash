using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Input
    [Header("Input")]
    [SerializeField] private float inputSmoothValue = 0.5f;
    private Vector2 moveInput, smoothedMoveInput;
    private float movePower, desirableMovePower;
    private bool isDashing, isBlocking;
    #endregion

    #region Health
    [Header("Health")]
    [SerializeField] private int maxHealth = 400;
    private int health;
    #endregion

    #region Energy
    [Header("Energy")]
    [SerializeField] private float maxEnergy = 200f;
    [SerializeField] private float energyReviveSpeed = 0.5f;
    [SerializeField] private float energyReduceAttack = 25f, energyReduceDash = 50f, energyReduceDodge = 5f;
    private float energy;
    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    private float moveSpeed;
    private Vector3 moveDirection, targetVelocity;
    private bool isRunning;
    [SerializeField] private Vector3 checkRayOffset = new Vector3(0f, 0.1f, 0f);

    [Space(7.5f)] // Ground Check
    [SerializeField] private float groundDrag = 5f;
    private bool isGrounded;

    [Space(7.5f)] // Slope Check
    [SerializeField] private float maxSlopeAngle = 45f;
    [SerializeField] private float slopeCheckRayLength = 0.3f;
    private RaycastHit slopeHit;
    private bool isSloped;
    #endregion

    #region Rotation
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 4f;
    private Quaternion rotateTowards, targetRotation;
    #endregion

    #region Dashing
    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.5f, dashCooldown = 5f;
    [SerializeField] private Vector3 dashStartOffset = new Vector3(0f, 20f, 0f);
    private float dashCooldownTimer;
    #endregion

    #region Camera
    [Header("Camera")]
    [SerializeField] private float cameraNormalFOV = 40f;
    [SerializeField] private float cameraDashFOV = 60f, transitionDuration = 0.2f;
    private float normalXSpeed, normalYSpeed, desirableFOV;
    private float changingFocusXSpeed, changingFocusYSpeed;
    [SerializeField] private float changingFocusSpeedMultiplier = 2f;
    #endregion

    #region Stuff
    [Header("Stuff")]
    [SerializeField] private Vector3 popUpTextOffset = new Vector3(0f, 1.25f, 0f);
    [SerializeField] private float animationSpeed = 0.1f;
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
    private TimeManager timeManager;
    private PostProcessingManager pPManager;
    private PopUpTextManager popUpTextManager;
    #endregion

    private float GetDeltaTime() { return 1.0f / Time.unscaledDeltaTime * Time.deltaTime; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();
        GameObject mainManager = GameObject.FindGameObjectWithTag("Manager");
        timeManager = mainManager.GetComponent<TimeManager>();
        pPManager = mainManager.GetComponent<PostProcessingManager>();
        popUpTextManager = mainManager.GetComponent<PopUpTextManager>();
    }

    private void Start()
    {
        healthBar.maxValue = maxHealth;
        health = maxHealth;

        energyBar.maxValue = maxEnergy;
        energy = maxEnergy;

        normalXSpeed = cinemachineCamera.m_XAxis.m_MaxSpeed;
        normalYSpeed = cinemachineCamera.m_YAxis.m_MaxSpeed;
        changingFocusXSpeed = normalXSpeed * changingFocusSpeedMultiplier;
        changingFocusYSpeed = normalYSpeed * changingFocusSpeedMultiplier;
        SetCinemachineCameraRecentering(false);
        SetCinemachineCameraSpeed(normalXSpeed, normalYSpeed);

        desirableFOV = cameraNormalFOV;
    }
    
    private void OnEnable()
    {
        if (playerInputScheme == null)
        {
            playerInputScheme = new PlayerInputScheme();

            playerInputScheme.PlayerMovement.Movement.performed += playerInputScheme =>
            moveInput = playerInputScheme.ReadValue<Vector2>();

            playerInputScheme.PlayerMovement.Run.performed += playerInputScheme =>
            {
                if (energy > 0f && !isDashing) isRunning = true;
            };
            playerInputScheme.PlayerMovement.Run.canceled += playerInputScheme => isRunning = false;

            playerInputScheme.Actions.Attack.started += playerInputScheme =>
            {
                if (energy >= energyReduceAttack) Attack();
            };

            playerInputScheme.Actions.FocusOnEnemy.performed += playerInputScheme => SetFocusChange(true);
            playerInputScheme.Actions.FocusOnEnemy.canceled += playerInputScheme => SetFocusChange(false);

            playerInputScheme.Actions.Dash.started += playerInputScheme =>
            {
                if (moveInput.magnitude > 0f && energy >= energyReduceDash) Dash();
            };

            playerInputScheme.Actions.Block.started += playerInputScheme =>
            {
                if (energy >= energyReduceDodge) isBlocking = true;
            };
            playerInputScheme.Actions.Block.canceled += playerInputScheme => isBlocking = false;
        }

        playerInputScheme.Enable();
    }

    private void OnDisable() { playerInputScheme.Disable(); }

    private void FixedUpdate() { GroundCheck(); SlopeCheck(); Movement(); }

    private void Update()
    {
        HandleHealth();
        HandleEnergy();
        SmoothInput();
        DashUpdate();
        Animation();
    }

    private void LateUpdate() { Rotation(); }

    private void SmoothInput()
    {
        if (smoothedMoveInput != moveInput) smoothedMoveInput =
                Vector2.MoveTowards(smoothedMoveInput, moveInput, inputSmoothValue * GetDeltaTime());
    }

    private void HandleHealth()
    {
        if (health <= 0f) Death();
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.value = health;
    }

    private void HandleEnergy()
    {
        if (energy < maxEnergy && !isBlocking)
        {
            if (moveInput.magnitude <= 0f) energy += energyReviveSpeed * GetDeltaTime();
            else if (!isRunning) energy += energyReviveSpeed * GetDeltaTime();
        }
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        energyBar.value = energy;
    }

    public void TakeDamage(int damage, bool willDodge)
    {
        anim.SetTrigger("Hit");

        if (!isBlocking || isBlocking && !willDodge)
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
        GameObject ragdoll =
            Instantiate(ragdollPrefab, transform.position, transform.rotation, transform.parent);

        SetCinemachineCameraSpeed(normalXSpeed, normalYSpeed);
        SetCinemachineCameraRecentering(false);

        cinemachineCamera.m_Follow = ragdoll.transform;
        cinemachineCamera.m_LookAt = ragdoll.transform;

        popUpTextManager.DisplayDeathPopUpText(transform.position + popUpTextOffset);

        Destroy(gameObject);
    }

    private void GroundCheck()
    {
        if (!isSloped) isGrounded = Physics.Raycast(transform.position + checkRayOffset, Vector3.down, 0.2f);
        else isGrounded = false;

        if ((isGrounded || isSloped) && !isDashing) rb.drag = groundDrag;
        else rb.drag = 0f;
    }

    private void SlopeCheck()
    {
        if (Physics.Raycast(transform.position + checkRayOffset,
            Vector3.down, out slopeHit, slopeCheckRayLength))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle <= maxSlopeAngle && angle != 0f) isSloped = true;
        }
        else isSloped = false;
    }

    private void Movement()
    {
        if (isRunning && energy <= 0f) isRunning = false;

        if (!isDashing)
        {
            if (isRunning) moveSpeed = runSpeed;
            else moveSpeed = walkSpeed;
        }
        else moveSpeed = dashSpeed;

        moveDirection = mainCamera.right * smoothedMoveInput.x + mainCamera.forward * smoothedMoveInput.y;
        moveDirection.y = 0f;
        moveDirection.Normalize();

        if (smoothedMoveInput.magnitude > 0f)
        {
            rb.useGravity = true;

            if (isSloped) targetVelocity = GetSlopeMoveDirection() * moveSpeed;
            else
            {
                targetVelocity = moveDirection * moveSpeed;

                if (isDashing) targetVelocity.y = 0f;
                else targetVelocity.y = rb.velocity.y;
            }
        }
        else
        {
            targetVelocity = Vector3.zero;

            if (isSloped) rb.useGravity = false;
            else
            {
                targetVelocity.y = rb.velocity.y;
                rb.useGravity = true;
            }
        }

        rb.velocity = targetVelocity;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void Rotation()
    {
        if (moveInput.magnitude > 0f)
        {
            Vector3 rawMoveDir = mainCamera.right * moveInput.x + mainCamera.forward * moveInput.y;
            rawMoveDir.y = 0f;
            rawMoveDir.Normalize();

            rotateTowards = Quaternion.LookRotation(rawMoveDir);
            targetRotation = Quaternion.Lerp(transform.rotation, rotateTowards, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }

    private void Attack()
    {
        energy -= energyReduceAttack;
        anim.SetTrigger("Attack");
    }

    private void SetFocusChange(bool isEnabled)
    {
        if (isEnabled)
        {
            SetCinemachineCameraSpeed(changingFocusXSpeed, changingFocusYSpeed);
            timeManager.SlowDownTime();
            pPManager.SetColorAdjustmentsSaturation(true);
        }
        else
        {
            SetCinemachineCameraSpeed(normalXSpeed, normalYSpeed);
            timeManager.SpeedUpTime();
            pPManager.SetColorAdjustmentsSaturation(false);
        }
    }

    private void DashUpdate()
    {
        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;

        if (isDashing)
        {
            if (moveInput.magnitude == 0f) ResetDash();
            desirableFOV =
                Mathf.MoveTowards(desirableFOV, cameraDashFOV, transitionDuration * GetDeltaTime());
        }
        else desirableFOV =
                Mathf.MoveTowards(desirableFOV, cameraNormalFOV, transitionDuration * GetDeltaTime());

        cinemachineCamera.m_Lens.FieldOfView = desirableFOV;
    }

    private void Dash()
    {
        if (dashCooldownTimer > 0f) return;
        else dashCooldownTimer = dashCooldown;

        rb.AddForce(dashStartOffset, ForceMode.VelocityChange);

        isDashing = true;

        energy -= energyReduceDash;

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash() { isDashing = false; }

    private void SetCinemachineCameraRecentering(bool isRecenteringOn)
    {
        cinemachineCamera.m_RecenterToTargetHeading.m_enabled = isRecenteringOn;
        cinemachineCamera.m_YAxisRecentering.m_enabled = isRecenteringOn;
    }

    private void SetCinemachineCameraSpeed(float xSpeed, float ySpeed)
    {
        cinemachineCamera.m_XAxis.m_MaxSpeed = xSpeed;
        cinemachineCamera.m_YAxis.m_MaxSpeed = ySpeed;
    }

    private void Animation()
    {
        if (moveInput.magnitude > 0f)
        {
            if (isRunning) desirableMovePower = 1f;
            else desirableMovePower = 0.5f;
        }
        else desirableMovePower = 0f;
        if (movePower != desirableMovePower)
            movePower = Mathf.MoveTowards(movePower, desirableMovePower, animationSpeed * GetDeltaTime());
        anim.SetFloat("Move Power", Mathf.Clamp01(movePower));

        anim.SetBool("Is Dashing", isDashing);

        anim.SetBool("Is Blocking", isBlocking);
    }
}
