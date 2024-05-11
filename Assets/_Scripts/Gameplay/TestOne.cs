using FSM._Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TestOne : MonoBehaviour
{
    public UnityEvent<WinLoseMenu.GameState> GameStateChangedEvent;

    [Header("References")]
    [SerializeField] RowManager rowManager;
    [SerializeField] LockCameraX lockCamera;
    [SerializeField] AbilityStorage _abilityStorage;
    [SerializeField] private TMP_Text _timerText;

    [Header("Settings")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float jumpHeight = 5f;
    [SerializeField] public float longTravelTime = 1.5f;
    [SerializeField] public float longTravelDistance = 10f;
    
    [Header("Crunches")]
    [SerializeField] float rotationTime = 1f;
    [SerializeField] private ParticleSystem _clickEffectInstance;
    [SerializeField] private ParticleSystem _selectEffectInstance;

    private Fsm _fsm;

    private Vector3 _spawnPosition;

    public TMP_Text TimerText => _timerText;

    private void Start()
    {
        _spawnPosition = transform.position;

        _fsm = new Fsm();
        _fsm.AddState(new FsmStateIdle(_fsm, this, rowManager, _abilityStorage));
        _fsm.AddState(new FsmStateMove(_fsm, this, rowManager));
        _fsm.AddState(new FsmStateFall(_fsm, this, rowManager));
        _fsm.AddState(new FsmStateWin(_fsm, transform, rotationTime));
        _fsm.AddState(new FsmStateCheckpoint(_fsm, transform));
        _fsm.AddState(new FsmStateRollback(_fsm));
        _fsm.SetState<FsmStateIdle>();

        _abilityStorage.Init(this, rowManager);
        rowManager.OnPlatformCheñked += UpdateState;

        UpdateViewToDirection();
    }

    private void OnDestroy()
    {
        rowManager.OnPlatformCheñked -= UpdateState;
    }

    private void CameraWin()
    {
        lockCamera.ChangeView(Vector3.forward);
        lockCamera.lockState = LockCameraX.LockState.None;
    }

    void Update()
    {
        _fsm.Update();
    }

    public void MoveToPoint(Transform body, Vector3 toPosition, System.Action callback=null)
    {
        StartCoroutine(MoveFromTo(body, toPosition, callback));
    }

    public void PlayClickEffect(Vector3 position)
    {
        _clickEffectInstance.transform.position = position + Vector3.up * 0.21f;
        _clickEffectInstance.Play();
    }

    private Platform target;
    public void SetTarget(Platform platform)
    {
        target = platform;
        _selectEffectInstance.transform.position = platform.transform.position + Vector3.up * 0.21f;
        _selectEffectInstance.Play();
    }

    public void CancelCast()
    {
        _selectEffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        target = null;
    }

    private void HandleWin()
    {
        CameraWin();
        GameStateChangedEvent?.Invoke(WinLoseMenu.GameState.Victory);
    }

    public void ApplyMove()
    {
        rowManager.ApplyMovement();
        _abilityStorage.ReduceCD();
        _abilityStorage.TakeSnapshot();
        //UpdateState();
    }

    public int GetStateForAbility()
    {
        var state = _fsm.GetCurrentState();

        switch (state)
        {
            case FsmStateFall:
                return 0;
            case FsmStateIdle:
                return 1;
            default:
                return 2;
        }
    }

    private void UpdateState()
    {
        if (rowManager.IsWin())
        {
            _fsm.SetState<FsmStateWin>();
            StartCoroutine(MoveFromTo(transform, rowManager.TargetPointPosition, null));
            HandleWin();
        }
        else if (rowManager.IsFall())
        {
            _fsm.SetState<FsmStateFall>();
        }
        else if (rowManager.IsCheckpoint())
        {
            UpdateViewToDirection();
        }
        else
        {
            _fsm.SetState<FsmStateIdle>();
        }
    }

    private void UpdateViewToDirection()
    {
        var t = _fsm.GetState<FsmStateCheckpoint>();
        if (t != null)
        {
            var dir = rowManager.GetCurentDirection();
            t.SetDirectionToRotate(dir, lockCamera.rotationDuration);
            _fsm.SetState<FsmStateCheckpoint>();
            lockCamera.ChangeView(dir);
        }
    }

    public void Restart()
    {
        rowManager.Restart();
        _abilityStorage.InitCD();
        transform.position = _spawnPosition;
        lockCamera.ResetCameraHolder(_spawnPosition);
        transform.rotation = Quaternion.LookRotation(rowManager.GetCurentDirection(), Vector3.up);
        lockCamera.ChangeView(rowManager.GetCurentDirection());
        _fsm.SetState<FsmStateIdle>();
        GameStateChangedEvent?.Invoke(WinLoseMenu.GameState.Game);
    }

    public void Rollback()
    {
        _fsm.SetState<FsmStateRollback>();
        GameStateChangedEvent?.Invoke(WinLoseMenu.GameState.Game);
        StartCoroutine(MoveBack(transform));
    }

    private IEnumerator MoveBack(Transform objectToMove)
    {
        Vector3 fromPosition = objectToMove.position;
        Vector3 toPositionWithSameY = new Vector3(fromPosition.x, _spawnPosition.y, fromPosition.z);
        float distanceToHeight = Vector3.Distance(fromPosition, toPositionWithSameY);
        float durationToHeight = distanceToHeight / moveSpeed;
        float elapsedTimeToHeight = 0f;

        while (elapsedTimeToHeight < durationToHeight)
        {
            elapsedTimeToHeight += Time.deltaTime;
            float tToHeight = elapsedTimeToHeight / durationToHeight;

            float heightOffset = Mathf.Lerp(fromPosition.y, toPositionWithSameY.y, tToHeight) - objectToMove.position.y;
            Vector3 newPositionToHeight = objectToMove.position + Vector3.up * heightOffset;

            objectToMove.position = newPositionToHeight;

            yield return null;
        }

        rowManager.SetupByShapshot();
        Vector3 toPosition = rowManager.TargetPointPosition;

        toPosition.y = _spawnPosition.y;
        fromPosition = objectToMove.position;
        float distance = Vector3.Distance(fromPosition, toPosition);
        float duration = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            float jumpOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            Vector3 newPosition = Vector3.Lerp(fromPosition, toPosition, t);
            newPosition.y += jumpOffset;

            objectToMove.position = newPosition;

            yield return null;
        }
        objectToMove.position = toPosition;

        _fsm.SetState<FsmStateIdle>();
        rowManager.ApplyMovement();
    }

    public IEnumerator MoveFromTo(Transform objectToMove, Vector3 toPosition, System.Action callback)
    {
        Vector3 fromPosition = objectToMove.position;
        float distance = Vector3.Distance(fromPosition, toPosition);
        float duration = distance > longTravelDistance ? longTravelTime : distance / moveSpeed;
        float elapsedTime = 0f;
        float oldY = objectToMove.position.y;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            float jumpOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            Vector3 newPosition = Vector3.Lerp(fromPosition, toPosition, t);
            newPosition.y = oldY + jumpOffset;

            objectToMove.position = newPosition;

            yield return null;
        }
        toPosition.y = oldY;
        objectToMove.position = toPosition;

        callback?.Invoke();
    }
}
