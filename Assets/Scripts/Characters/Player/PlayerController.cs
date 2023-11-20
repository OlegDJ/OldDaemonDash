using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region States
    [Header("States")] // Controls
    public bool canMove = true;
    public bool canRotateBody = true;
    public bool canRun = true, isRunning;

    [Space(7.5f)] // Actions
    public bool canAttack = true;
    public bool canDash = true, isDashing;
    public bool canBlock = true, isBlocking;
    public bool canFocus = true, isFocusing, isFocusChanging;

    [Space(7.5f)] // Orientation
    public bool isGrounded;
    public bool isSloped;
    #endregion

    #region Health
    [Header("Health")]
    [SerializeField] private int maxHealth = 400;
    private int health;
    #endregion

    #region Energy
    [Header("Energy")]
    [SerializeField] private float maxEnergy = 200f;
    [SerializeField] private float energyReviveSpeed = 0.5f, energyReduceRun = 0.2f;
    [SerializeField] private float energyReduceAttack = 25f, energyReduceDash = 0.6f, energyReduceDodge = 5f;
    private float energy, desirableDashEnergy;
    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    private float moveSpeed;
    private Vector3 moveDirection, targetVelocity;
    [SerializeField] private Vector3 checkRayOffset = new Vector3(0f, 0.1f, 0f);

    [Space(7.5f)] // Ground Check
    [SerializeField] private float groundDrag = 5f;

    [Space(7.5f)] // Slope Check
    [SerializeField] private float maxSlopeAngle = 45f;
    [SerializeField] private float slopeCheckRayLength = 0.3f;
    private RaycastHit slopeHit;
    #endregion

    #region Rotation
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 0.2f;
    private Vector3 rotationDirection;
    private Vector3 smoothedRotationDirection;

    //private Quaternion lookRotation, targetRotation;

    //private float curBodyYRotation, targetBodyYRotation, bodyYVelocity;
    #endregion

    #region Focus
    [Header("Focus")]
    [SerializeField] private float focusChangeRange = 45f;
    private RaycastHit focusChangeHit;
    #endregion

    #region Dashing
    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 1f, dashCooldown = 5f;
    [SerializeField] private Vector3 dashStartOffset = new Vector3(0f, 20f, 0f);
    private float dashCooldownTimer;
    #endregion

    #region Stuff
    [Header("Stuff")]
    [SerializeField] private Vector3 popUpTextOffset = new Vector3(0f, 1.25f, 0f);
    [SerializeField] private float animationSpeed = 0.05f;
    [HideInInspector] public float movePower, desirableMovePower;
    #endregion

    #region References
    [Header("References")]
    [SerializeField] private CameraController camControl;
    [SerializeField] private GameObject ragdollPrefab;
    private Transform trnsfrm;
    private Rigidbody rb;
    private Animator anim;
    private Manager mngr;
    [HideInInspector] public Focusable focusTarget;
    #endregion

    private void Awake()
    {
        trnsfrm = transform;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        //mngr = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
    }

    private void Start()
    {
        mngr = Manager.mngr;

        canMove = true;

        mngr.ui.SetHealthBarMaxValue(maxHealth);
        health = maxHealth;

        mngr.ui.SetEnergyBarMaxValue(maxEnergy);
        energy = maxEnergy;
    }

    private void HandlePermits()
    {
        canMove = canRotateBody = true;
        canRun = energy > 0f && mngr.input.movement.magnitude > 0f;
        canAttack = energy >= energyReduceAttack;
        canDash = energy >= energyReduceDash && mngr.input.movement.magnitude > 0f && dashCooldownTimer <= 0f;
        canBlock = energy >= energyReduceDodge;
        canFocus = true;

        if (isDashing) canRun = canAttack = canBlock =
                isRunning = isBlocking = isFocusChanging = false;
        if (isBlocking) canDash = canRun = canAttack =
                isDashing = isRunning = false;
        if (isFocusChanging) canMove = canRotateBody = canRun = canAttack = canDash = canBlock =
                isRunning = isDashing = isBlocking = isFocusing = false;

        if (!canFocus) CancelFocusChange();
    }

    public void Action(ActType actionType)
    {
        switch (actionType)
        {
            case ActType.RunPerf:
                if (!canRun) return;
                isRunning = true;
                camControl.RunFOV();
                return;
            case ActType.RunCanc:
                isRunning = false;
                return;
            case ActType.AttackStrtd:
                if (canAttack) Attack();
                return;
            case ActType.FocusChangePerf:
                if (canFocus) PerformFocusChange();
                return;
            case ActType.FocusChangeCanc:
                if (canFocus) CancelFocusChange();
                return;
            case ActType.DashStrtd:
                if (canDash) Dash();
                return;
            case ActType.BlockStrtd:
                if (!canBlock) return;
                isBlocking = true;
                canMove = false;
                return;
            case ActType.BlockCanc:
                isBlocking = false;
                canMove = true;
                return;
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        SlopeCheck();
        if (isFocusChanging) FocusChangeFixedUpdate();
        if (canRotateBody && !isFocusing) NormalRotation();
        Movement();
    }

    private void Update()
    {
        HandlePermits();
        HandleHealth();
        HandleEnergy();
        if (canRotateBody && isFocusing) FocusRotation();
        DashUpdate();
        Animation();
    }

    private void HandleHealth()
    {
        if (health <= 0f) Death();
        health = Mathf.Clamp(health, 0, maxHealth);
        mngr.ui.SetHealthBarValue(health);
    }

    private void HandleEnergy()
    {
        if (energy < maxEnergy && !isBlocking && !isRunning && !isDashing)
            energy += energyReviveSpeed * mngr.GetDeltaTime();
        else if (isRunning)
            energy -= energyReduceRun * mngr.GetDeltaTime();
        else if (isDashing)
            energy -= energyReduceDash * mngr.GetDeltaTime();
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
        Instantiate(ragdollPrefab, transform.position, transform.rotation, transform.parent);

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
        if (canMove && mngr.input.smoothedMovement.magnitude > 0f)
        {
            if (!isDashing)
            {
                if (isRunning) moveSpeed = runSpeed;
                else moveSpeed = walkSpeed;
            }
            else moveSpeed = dashSpeed;

            moveDirection = camControl.mCamTrnsfrm.right * mngr.input.smoothedMovement.x +
                camControl.mCamTrnsfrm.forward * mngr.input.smoothedMovement.y;
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

    private void NormalRotation()
    {
        rotationDirection = camControl.camPivotTrnsfrm.right * mngr.input.movement.x +
            camControl.camPivotTrnsfrm.forward * mngr.input.movement.y;
        rotationDirection.y = 0f;
        rotationDirection.Normalize();

        if (mngr.input.movement.magnitude > 0f)
        {
            smoothedRotationDirection = Vector3.Slerp(trnsfrm.forward, 
                rotationDirection.normalized, rotationSpeed) * mngr.GetFixedDeltaTime();
            trnsfrm.forward = smoothedRotationDirection;

            //lookRotation = Quaternion.LookRotation(rotationDirection);
            //targetRotation = Quaternion.RotateTowards(trnsfrm.rotation, lookRotation, rotationSpeed *
            //mngr.GetDeltaTime());
            //trnsfrm.localRotation = targetRotation;
        }

        //if (mngr.input.movement.magnitude > 0f)
        //{
        //    targetBodyYRotation = Mathf.Atan2(mngr.input.movement.x, mngr.input.movement.y) * Mathf.Rad2Deg;
        //    targetBodyYRotation -= camControl.yAngleOffset;
        //    curBodyYRotation = Mathf.SmoothDampAngle(curBodyYRotation,
        //        targetBodyYRotation, ref bodyYVelocity, rotationSpeed);
        //}
        //trnsfrm.eulerAngles = new Vector3(0f, curBodyYRotation, 0f);
    }

    private void FocusRotation()
    {
        trnsfrm.LookAt(new Vector3(camControl.focusPoint.position.x,
            trnsfrm.position.y, camControl.focusPoint.position.z));
    }

    private void Attack() { energy -= energyReduceAttack; anim.SetTrigger("Attack"); }

    private void TurnFocusChange(bool on)
    {
        if (on) mngr.time.SlowDownTime();
        else mngr.time.SpeedUpTime();

        mngr.postProc.SetColorAdjustmentsSaturation(on);
    }

    private void PerformFocusChange()
    {
        if (isFocusing) camControl.AfterFocus();
        else camControl.StartTransition(true);
        isFocusChanging = true;
        mngr.ui.TurnCrosshair(true);
        TurnFocusChange(true);
    }

    private void CancelFocusChange()
    {
        if (focusTarget != null && canFocus)
        {
            isFocusing = true;
            camControl.BeforeFocus(focusTarget.focusPoints[focusTarget.startFocusPoint].position,
                focusTarget.startFocusPoint,
                focusTarget.focusPoints.ToList(),
                focusTarget.focusPoints[focusTarget.startFocusPoint + 1],
                focusTarget.focusPoints[focusTarget.startFocusPoint - 1]);
        }
        else
        {
            camControl.StartTransition(true);
            mngr.ui.TurnCrosshair(false);
        }
        isFocusChanging = false;
        TurnFocusChange(false);
    }

    private void FocusChangeFixedUpdate()
    {
        if (Physics.Raycast(camControl.mCamTrnsfrm.position, camControl.mCamTrnsfrm.forward,
            out focusChangeHit, focusChangeRange))
        {
            if (!focusChangeHit.transform.GetComponent<Focusable>())
            {
                mngr.ui.ChangeCrosshairAlpha(false);
                focusTarget = null;
                mngr.ui.SetFocusInfoText("Nothing");
            }
            else
            {
                mngr.ui.ChangeCrosshairAlpha(true);
                focusTarget = focusChangeHit.transform.GetComponent<Focusable>();
                mngr.ui.SetFocusInfoText(focusChangeHit.transform.name);
            }
        }
    }

    private void DashUpdate()
    {
        if (dashCooldownTimer > 0f && !isDashing) dashCooldownTimer -= Time.deltaTime;
        else if (dashCooldownTimer < 0f) dashCooldownTimer = 0f;

        if (isDashing)
        {
            if (mngr.input.movement.magnitude == 0f || energy <= 0f) ResetDash();
            camControl.DashFOV();
        }
        else if (!isRunning) camControl.NormalFOV();
    }

    private void Dash()
    {
        isRunning = isBlocking = false;

        dashCooldownTimer = dashCooldown;

        rb.AddForce(dashStartOffset, ForceMode.VelocityChange);

        isDashing = true;

        desirableDashEnergy = energy - energyReduceDash;

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash() { isDashing = false; desirableDashEnergy = 0f; }

    private void Animation()
    {
        if (mngr.input.movement.magnitude > 0f && canMove)
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
