using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LockCameraX : CinemachineExtension
{
    public Transform player;
    public Transform cameraHolder;


    public float rotationDuration = 0.5f;
    private bool isRotating = false;
    private Quaternion desiredRotation;
    private Quaternion initialRotation;
    private float rotationTime = 0f;

    public LockState lockState = LockState.LockX;

    public enum LockState : byte
    {
        None,
        LockX,
        LockZ
    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var q = state.RawOrientation.eulerAngles;
            q.y = cameraHolder.eulerAngles.y;
            state.RawOrientation.eulerAngles = q;

            var newPos = player.position;
            if (lockState==LockState.LockX)
            {
                newPos.x = cameraHolder.position.x;
            }
            else if(lockState == LockState.LockZ)
            {
                newPos.z = cameraHolder.position.z;
            }
            
            cameraHolder.position = newPos;
        }
    }

    public void ChangeView(Vector3 direction)
    {
        initialRotation = cameraHolder.rotation;
        desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
        isRotating = true;
    }

    public void ResetCameraHolder(Vector3 newPos)
    {
        cameraHolder.position = newPos;
    }

    private void Update()
    {
        if (isRotating)
        {
            rotationTime += Time.deltaTime;
            float t = Mathf.Clamp01(rotationTime / rotationDuration);
            cameraHolder.rotation = Quaternion.Lerp(initialRotation, desiredRotation, t);
            if (t >= 1.0f)
            {
                isRotating = false;
                rotationTime = 0f;

                if (lockState != LockState.None)
                {
                    if(Mathf.Approximately(Vector3.Dot(cameraHolder.forward, Vector3.forward), 1f) ||
                        Mathf.Approximately(Vector3.Dot(cameraHolder.forward, Vector3.back), 1f)){
                        lockState = LockState.LockX;
                    }
                    else
                    {
                        lockState = LockState.LockZ;
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                ChangeView(Vector3.forward);
            } 
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeView(Vector3.back);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeView(Vector3.left);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangeView(Vector3.right);
            }
        }

    }
}