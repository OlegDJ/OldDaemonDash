using UnityEngine;

[RequireComponent(typeof(Manager))]
public class InputManager : MonoBehaviour
{
    [Header("Smoothing")]
    public float inputSmoothValue = 0.5f;

    [Header("Values")]
    public Vector2 movement;
    public Vector2 smoothedMovement, rotation;
    public float focusPointMovement;

    private Manager mngr;
    private PlayerInputScheme playerInputScheme;
    private PlayerController playerController;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        mngr = GetComponent<Manager>();
        playerController = mngr.playerController;
    }

    public void EnableFunction()
    {
        if (playerInputScheme == null)
        {
            playerInputScheme = new PlayerInputScheme();

            // Controls
            playerInputScheme.Controls.Movement.performed += playerInputScheme =>
            movement = playerInputScheme.ReadValue<Vector2>();
            playerInputScheme.Controls.Rotation.performed += playerInputScheme =>
            rotation = playerInputScheme.ReadValue<Vector2>();

            playerInputScheme.Controls.Run.performed += playerInputScheme =>
            playerController.Action(ActType.RunPerf);
            playerInputScheme.Controls.Run.canceled += playerInputScheme =>
            playerController.Action(ActType.RunCanc);

            // Actions
            playerInputScheme.Actions.Attack.started += playerInputScheme =>
            playerController.Action(ActType.AttackStrtd);

            playerInputScheme.Actions.Dash.started += playerInputScheme =>
            playerController.Action(ActType.DashStrtd);

            playerInputScheme.Actions.Block.started += playerInputScheme =>
            playerController.Action(ActType.BlockStrtd);
            playerInputScheme.Actions.Block.canceled += playerInputScheme =>
            playerController.Action(ActType.BlockCanc);

            playerInputScheme.Actions.Focus.performed += playerInputScheme =>
            playerController.Action(ActType.FocusChangePerf);
            playerInputScheme.Actions.Focus.canceled += playerInputScheme =>
            playerController.Action(ActType.FocusChangeCanc);

            playerInputScheme.Actions.MoveFocusPoint.performed += playerInputScheme =>
            focusPointMovement = playerInputScheme.ReadValue<Vector2>().y;
        }

        playerInputScheme.Enable();
    }
    public void DisableFunction() { playerInputScheme.Disable(); }

    public void UpdateFunction()
    {
        if (smoothedMovement != movement) smoothedMovement = 
                Vector2.MoveTowards(smoothedMovement, movement, inputSmoothValue * mngr.GetUnscaledDeltaTime());
    }
}

public enum ActType
{
    RunPerf, RunCanc,
    AttackStrtd,
    FocusChangePerf, FocusChangeCanc,
    DashStrtd,
    BlockStrtd, BlockCanc
}