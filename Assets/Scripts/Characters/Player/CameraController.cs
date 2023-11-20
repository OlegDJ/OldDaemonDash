using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Bools
    [Header("Bools")]
    public bool canRotateCamera = true;
    private bool isTransitioningCamera, normalTransition;
    #endregion

    #region Rotation
    [Header("Rotation")]
    [SerializeField] private float sensivity = 6f;
    [SerializeField] private Vector2 camClamp = new Vector2(-45f, 45f);
    [SerializeField] private Vector3 normalStartOffset = new Vector3(0f, 0f, -2f),
        focusStartOffset = new Vector3(0.3f, 0.2f, -1f);
    private Vector3 normalOffset, focusOffset;
    private float xRotation, yRotation;
    [HideInInspector] public float yAngleOffset;
    // [SerializeField] private float camSmoothTime = 0.1f;
    // private float curXRotation, curYRotation, camXVelocity, camYVelocity;
    #endregion

    #region Focus
    [Header("Focus")]
    [SerializeField] private float focusPointMoveSpeed = 0.2f;
    [SerializeField] private float transitionSpeed = 0.1f;
    public int focusPointIndex;
    #endregion

    #region Field of View
    [Header("Field of View")]
    [SerializeField] private float normalFOV = 40f;
    [SerializeField] private float dashFOV = 60f, runFOV = 45f, fOVChangeSpeed = 1f;
    private float desirableFOV;
    #endregion

    #region References
    [Header("References")]
    [SerializeField] private LayerMask obstacleLayer;
    public Transform focusPoint;
    private Manager mngr;
    [HideInInspector] public Camera mCam;
    [HideInInspector] public Transform mCamTrnsfrm, camPivotTrnsfrm;
    [HideInInspector] public List<Transform> focusTargetPoints = new List<Transform>();
    public Transform nextPoint, previousPoint;
    #endregion

    private void Awake()
    {
        mCam = Camera.main;
        mCamTrnsfrm = mCam.transform;
        camPivotTrnsfrm = transform;
    }

    private void Start()
    {
        mngr = Manager.mngr;

        normalOffset = normalStartOffset;
        focusOffset = focusStartOffset;

        desirableFOV = normalFOV;
    }

    private void Update()
    {
        ControlFOV();
        if (isTransitioningCamera) TransitionCamera();
        if (canRotateCamera) mCamTrnsfrm.localPosition =
            (mngr.playerController.isFocusChanging || mngr.playerController.isFocusing) ? 
            focusOffset : normalOffset;
        if (canRotateCamera) Rotation();
    }

    public void StartTransition(bool normal)
    {
        isTransitioningCamera = true;
        canRotateCamera = false;
        normalTransition = normal;
        //if (!normal)
        //{
        //    mCamTrnsfrm.localRotation = Quaternion.identity;
        //    desXRot = mngr.player.transform.rotation.eulerAngles.x;
        //    desYRot = mngr.player.transform.rotation.eulerAngles.y;
        //}
    }

    private void TransitionCamera()
    {
        if (normalTransition)
        {
            if (mCamTrnsfrm.localPosition == (mngr.playerController.isFocusChanging ?
                focusOffset : normalOffset))
            {
                canRotateCamera = true;
                isTransitioningCamera = false;
            }
            else
            {
                if (mngr.playerController.isFocusChanging)
                {
                    mCamTrnsfrm.localPosition = Vector3.MoveTowards(mCamTrnsfrm.localPosition,
                        focusOffset, transitionSpeed * mngr.GetUnscaledDeltaTime());
                }
                else
                {
                    mCamTrnsfrm.localPosition = Vector3.MoveTowards(mCamTrnsfrm.localPosition,
                        normalOffset, transitionSpeed * mngr.GetUnscaledDeltaTime());
                }
            }
        }
        //else
        //{
        //    if (mCamTrnsfrm.rotation == Quaternion.identity && xRotation == desXRot && yRotation == desYRot)
        //    {
        //        canRotateCamera = true;
        //        isTransitioningCamera = false;
        //        mngr.playerController.isFocusChanging = mngr.playerController.isFocusing = false;
        //    }
        //    else
        //    {
        //        xRotation = Mathf.MoveTowards(xRotation, 0f, transitionSpeed);
        //        yRotation = Mathf.MoveTowards(yRotation, 0f, transitionSpeed);
        //        camPivotTrnsfrm.localEulerAngles = new Vector3(xRotation, yRotation, 0f);
        //    }
        //}
    }

    private void Rotation()
    {
        if (!mngr.playerController.isFocusing)
        {
            yRotation += mngr.input.rotation.x * sensivity;
            if (yRotation >= 360f || yRotation <= -360f) yRotation = 0f;
            xRotation -= mngr.input.rotation.y * sensivity;
            xRotation = Mathf.Clamp(xRotation, camClamp.x, camClamp.y);

            camPivotTrnsfrm.eulerAngles = new Vector3(xRotation, yRotation, 0f);
            yAngleOffset = Mathf.Atan2(camPivotTrnsfrm.forward.z, camPivotTrnsfrm.forward.x) *
                Mathf.Rad2Deg - 90f;

            //curXRotation = Mathf.SmoothDampAngle(curXRotation, xRotation, ref camXVelocity, camSmoothTime);
            //curYRotation = Mathf.SmoothDampAngle(curYRotation, yRotation, ref camYVelocity, camSmoothTime);
            //camPivotTrnsfrm.eulerAngles = new Vector3(curXRotation, curYRotation, 0f);
            //xRotation = camPivotTrnsfrm.eulerAngles.x;
            //yRotation = camPivotTrnsfrm.eulerAngles.y;

            if (mngr.playerController.isFocusChanging)
            {
                Ray camRay = new Ray(camPivotTrnsfrm.position, -camPivotTrnsfrm.forward);
                float maxDistance = Mathf.Abs(focusStartOffset.z);
                if (Physics.SphereCast(camRay, 0.25f, out RaycastHit hit,
                    Mathf.Abs(focusStartOffset.z), obstacleLayer))
                    maxDistance = (hit.point - camPivotTrnsfrm.position).magnitude - 0.25f;
                focusOffset.z = -maxDistance;
            }
            else
            {
                Ray camRay = new Ray(camPivotTrnsfrm.position, -camPivotTrnsfrm.forward);
                float maxDistance = Mathf.Abs(normalStartOffset.z);
                if (Physics.SphereCast(camRay, 0.25f, out RaycastHit hit,
                    Mathf.Abs(normalStartOffset.z), obstacleLayer))
                    maxDistance = (hit.point - camPivotTrnsfrm.position).magnitude - 0.25f;
                normalOffset.z = -maxDistance;
            }
        }
        else
        {
            float pointMove = mngr.input.focusPointMovement;
            if (pointMove > 0f)
                focusPoint.position = Vector3.MoveTowards(focusPoint.position, nextPoint.position,
                    focusPointMoveSpeed * mngr.GetUnscaledDeltaTime());
            else if (pointMove < 0f)
                focusPoint.position = Vector3.MoveTowards(focusPoint.position, previousPoint.position,
                    focusPointMoveSpeed * mngr.GetUnscaledDeltaTime());

            if (focusPoint.position == nextPoint.position &&
                focusTargetPoints[focusTargetPoints.Count - 1].position != focusPoint.position)
            {
                previousPoint = focusTargetPoints[focusPointIndex];
                nextPoint = focusTargetPoints[focusPointIndex + 1];
                focusPointIndex++;
            }
            if (focusPoint.position == previousPoint.position &&
                focusTargetPoints[0].position != focusPoint.position)
            {
                nextPoint = focusTargetPoints[focusPointIndex];
                previousPoint = focusTargetPoints[focusPointIndex - 1];
                focusPointIndex--;
            }
            camPivotTrnsfrm.LookAt(focusPoint);
            mCamTrnsfrm.LookAt(focusPoint);
        }
    }

    public void BeforeFocus(Vector3 startPos, int index,
        List<Transform> points, Transform next, Transform prev)
    {
        focusPoint.position = startPos;
        focusPointIndex = index;
        nextPoint = next;
        previousPoint = prev;
        focusTargetPoints = points;
    }

    public void AfterFocus()
    {
        mCamTrnsfrm.localRotation = Quaternion.identity;
        xRotation = camPivotTrnsfrm.eulerAngles.x;
        yRotation = camPivotTrnsfrm.eulerAngles.y;
    }

    private void ControlFOV()
    {
        if (mCam.fieldOfView != desirableFOV)
            mCam.fieldOfView = Mathf.MoveTowards(
                mCam.fieldOfView, desirableFOV, fOVChangeSpeed * mngr.GetDeltaTime());
    }

    public void NormalFOV() { desirableFOV = normalFOV; }

    public void RunFOV() { desirableFOV = runFOV; }

    public void DashFOV() { desirableFOV = dashFOV; }
}
