using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Input
    [Header("Input")]
    [SerializeField] private float inputSmoothValue = 0.5f;
    private Vector2 moveInput, smoothedMoveInput, rotInput;
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
    [SerializeField] private bool canMove = true;
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
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float cameraClamp = 60f, changeFocusSpeed = 2f;
    private Vector3 rotationDirection, focusTargetRotation;
    private Quaternion lookRotation, rotateTowards, targetRotation, deltaRotation;
    private float xRotation;
    #endregion

    #region Focus
    private bool isFocusChanging;
    #endregion

    #region Dashing
    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 1f, dashCooldown = 5f;
    [SerializeField] private Vector3 dashStartOffset = new Vector3(0f, 20f, 0f);
    private float dashCooldownTimer;
    #endregion

    #region Camera
    [Header("Camera")]
    [SerializeField] private float normalFOV = 40f;
    [SerializeField] private float dashFOV = 60f, fOVChangeSpeed = 1f;
    private float desirableFOV;
    #endregion

    #region Stuff
    [Header("Stuff")]
    [SerializeField] private Vector3 popUpTextOffset = new Vector3(0f, 1.25f, 0f);
    [SerializeField] private float animationSpeed = 0.05f;
    #endregion

    #region References
    [Header("References")]
    [SerializeField] private CinemachineFreeLook normalVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera shoulderVirtualCamera;
    [SerializeField] private Transform focusChangeCameraHandler;
    [SerializeField] private GameObject ragdollPrefab;
    private Transform trnsfrm;
    private Rigidbody rb;
    private Transform mainCamera;
    private PlayerInputScheme playerInputScheme;
    private Focusable focusObject;
    private Animator anim;
    private Manager mngr;
    #endregion

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        trnsfrm = transform;
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();
        GameObject mainManager = GameObject.FindGameObjectWithTag("Manager");
        mngr = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
    }

    private void Start()
    {
        mngr.ui.SetHealthBarMaxValue(maxHealth);
        health = maxHealth;

        mngr.ui.SetEnergyBarMaxValue(maxEnergy);
        energy = maxEnergy;

        desirableFOV = normalFOV;
        SetCamerasPriorities(1, 0);
    }
    
    private void OnEnable()
    {
        if (playerInputScheme == null)
        {
            playerInputScheme = new PlayerInputScheme();

            playerInputScheme.PlayerMovement.Movement.performed += playerInputScheme =>
            moveInput = playerInputScheme.ReadValue<Vector2>();

            playerInputScheme.PlayerMovement.Rotation.performed += playerInputScheme =>
            rotInput = playerInputScheme.ReadValue<Vector2>();

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

    private void FixedUpdate()
    {
        GroundCheck();
        SlopeCheck();
        FixedRotation();
        Movement();
    }

    private void Update()
    {
        SmoothInput();
        HandleHealth();
        HandleEnergy();
        Rotation();
        DashUpdate();
        Animation();
    }

    private void DashUpdate()
    {
        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;

        if (isDashing)
        {
            if (moveInput.magnitude == 0f) ResetDash();
            desirableFOV = dashFOV;
        }
        else desirableFOV = normalFOV;

        if (normalVirtualCamera.m_Lens.FieldOfView != desirableFOV)
            normalVirtualCamera.m_Lens.FieldOfView =
                Mathf.MoveTowards(normalVirtualCamera.m_Lens.FieldOfView,
                desirableFOV, fOVChangeSpeed * mngr.GetDeltaTime());
    }

    private void SmoothInput()
    {
        if (smoothedMoveInput != moveInput) smoothedMoveInput =
                Vector2.MoveTowards(smoothedMoveInput, moveInput, inputSmoothValue * mngr.GetDeltaTime());
    }

    private void HandleHealth()
    {
        if (health <= 0f) Death();
        health = Mathf.Clamp(health, 0, maxHealth);
        mngr.ui.SetHealthBarValue(health);
    }

    private void HandleEnergy()
    {
        if ((moveInput.magnitude <= 0f || !isRunning) && energy < maxEnergy && !isBlocking)
            energy += energyReviveSpeed * mngr.GetDeltaTime();
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        mngr.ui.SetEnergyBarValue(energy);
    }

    public void TakeDamage(int damage, bool willDodge)
    {
        anim.SetTrigger("Hit");

        if (!isBlocking || isBlocking && !willDodge)
        {
            health -= damage;
            mngr.popUpTxt.DisplayDamagePopUpText(damage, transform.position + popUpTextOffset);
        }
        else
        {
            energy -= energyReduceDodge;
            mngr.popUpTxt.DisplayDodgedPopUpText(transform.position + popUpTextOffset);
        }
    }

    private void Death()
    {
        GameObject ragdoll =
            Instantiate(ragdollPrefab, transform.position, transform.rotation, transform.parent);

        normalVirtualCamera.m_Follow = ragdoll.transform;
        normalVirtualCamera.m_LookAt = ragdoll.transform;

        mngr.popUpTxt.DisplayDeathPopUpText(transform.position + popUpTextOffset);

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
        if (canMove && smoothedMoveInput.magnitude > 0f)
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

    private void FixedRotation()
    {
        if (!isFocusChanging)
        {
            rotationDirection = mainCamera.right * moveInput.x + mainCamera.forward * moveInput.y;
            rotationDirection.y = 0f;
            rotationDirection.Normalize();

            if (moveInput.magnitude > 0f)
            {
                lookRotation = Quaternion.LookRotation(rotationDirection);
                rotateTowards = Quaternion.RotateTowards(trnsfrm.rotation, lookRotation, rotationSpeed * mngr.GetFixedDeltaTime());
                rb.MoveRotation(rotateTowards);
                //targetRotation = Quaternion.RotateTowards(trnsfrm.rotation, lookRotation, rotationSpeed * mngr.GetFixedDeltaTime());
                //trnsfrm.rotation = targetRotation;
            }
        }
        else
        {
            deltaRotation = Quaternion.Euler(Vector3.up * rotInput.x * changeFocusSpeed * mngr.GetFixedUnscaledDeltaTime());
            rb.MoveRotation(trnsfrm.rotation * deltaRotation);
            //trnsfrm.Rotate(Vector3.up * rotInput.x * rotationSpeed * mngr.GetUnscaledDeltaTime());
        }
    }

    private void Rotation()
    {
        if (isFocusChanging)
        {
            xRotation -= rotInput.y * changeFocusSpeed * mngr.GetUnscaledDeltaTime();
            xRotation = Mathf.Clamp(xRotation, -cameraClamp, cameraClamp);
            focusChangeCameraHandler.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    private void Attack()
    {
        energy -= energyReduceAttack;
        anim.SetTrigger("Attack");
    }

    private void SetFocusChange(bool isEnabled)
    {
        isFocusChanging = isEnabled;

        if (isEnabled)
        {
            mngr.time.SlowDownTime();
            SetCamerasPriorities(0, 1);
            canMove = false;
        }
        else
        {
            mngr.time.SpeedUpTime();
            SetCamerasPriorities(1, 0);
            canMove = true;
        }

        mngr.postProc.SetColorAdjustmentsSaturation(isEnabled);
    }

    private void StartFocus()
    {
        //isFocusing = true;
    }

    private void EndFocus()
    {
        //isFocusing = false;
    }

    private void SetCamerasPriorities(int normal, int shoulder)
    {
        normalVirtualCamera.Priority = normal;
        shoulderVirtualCamera.Priority = shoulder;
    }

    private void Dash()
    {
        if (dashCooldownTimer > 0f && !canMove) return;
        else dashCooldownTimer = dashCooldown;

        rb.AddForce(dashStartOffset, ForceMode.VelocityChange);

        isDashing = true;

        energy -= energyReduceDash;

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash() { isDashing = false; }

    private void Animation()
    {
        if (moveInput.magnitude > 0f && canMove)
        {
            if (isRunning) desirableMovePower = 1f;
            else desirableMovePower = 0.5f;
        }
        else desirableMovePower = 0f;
        if (movePower != desirableMovePower)
            movePower = Mathf.MoveTowards(movePower, desirableMovePower, animationSpeed * mngr.GetDeltaTime());
        anim.SetFloat("Move Power", Mathf.Clamp01(movePower));

        anim.SetBool("Is Dashing", isDashing);

        anim.SetBool("Is Blocking", isBlocking);
    }
}