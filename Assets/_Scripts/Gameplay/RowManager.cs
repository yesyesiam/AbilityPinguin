using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RowManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Platform defaultPlatformPrefab;
    [SerializeField] private Platform checkpointPlatformPrefab;

    [Header("Settings")]
    [SerializeField] float distanceBetween = 2;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform finishPoint;

    [Header("Ñrutches")]
    [SerializeField] private bool IsTestGeneration = true;
    [SerializeField] private ColdfeetConfig coldfeetConfig;
    [SerializeField] private TMP_Text _timerText;
    public event Action OnPlatformCheñked;
    public event Action<Platform> OnPlatformChanged;

    private List<Platform> platforms;
    [SerializeField] public List<Stage> stages = new List<Stage>();

    private List<PlatformData> currentPlatformsData;
    private List<PlatformData> previousPlatformsData;
    private RowManagerSnaphot currentManagerSnaphot;
    private RowManagerSnaphot prevManagerSnaphot;

    private Vector3 currentSpawnPoint;
    private Platform currentCheckpointPlatform;
    private Platform prevCheckpointPlatform;

    public int CurrentRow { get; private set; }
    public int CurrentIndex { get; private set; }
    public int CurrentStage { get; private set; }
    public Vector3 TargetPointPosition { get; private set; }

    public bool CanRollback => previousPlatformsData != null && currentPlatformsData != null;
    private Platform CurrentPlatform { get; set; }
    private Platform TargetPlatform { get; set; }
    void Start()
    {
        currentSpawnPoint = startPoint.position;
        platforms = SpawnStage();
        CurrentRow = -1;
        CurrentIndex = 0;
        TakeSnapshot();
    }

    public Vector3 GetCurentDirection()
    {
        Vector3 result = Vector3.forward;

        if(CurrentStage < stages.Count)
        {
            result = stages[CurrentStage].GetDirectionVector();
        }

        return result;
    }

    public bool MoveToPlatform(Platform platform)
    {
        if (platform.row == CurrentRow + 1)
        {
            TargetPlatform = platform;
            TargetPointPosition = platform.gameObject.transform.position;
            CurrentRow++;
            CurrentIndex = platform.index;
            return true;
        }
        return false;
    }

    public void TpToPlatform(Platform platform)
    {
        TargetPlatform = platform;
        TargetPointPosition = platform.gameObject.transform.position;
        
        CurrentRow = platform.row;
        CurrentIndex = platform.index;
    }

    public void ApplyMovement()
    {
        if (CurrentRow == platforms.Count/2)
        {
            if (CurrentStage < stages.Count - 1)
            {
                CurrentStage++;
                CurrentRow = -1;
                CurrentIndex = 0;
                platforms = SpawnStage();
            }
            else
            {
                CurrentStage++;
                TargetPointPosition = finishPoint.position;
            }
            OnPlatformCheñked?.Invoke();
            //CurrentPlatform = TargetPlatform;
            SetNewCurrentPlatform();
            OnPlatformChanged?.Invoke(CurrentPlatform);
            ClearShapshots();
            TakeSnapshot();
        }
        else if (CurrentRow < platforms.Count / 2)
        {
            SetNewCurrentPlatform();
            //CurrentPlatform = TargetPlatform;
            UpdatePlatform(CurrentPlatform);
            TakeSnapshot();
        }
    }

    private void SetNewCurrentPlatform()
    {
        if (CurrentPlatform != null && CurrentPlatform.IsBuffed)
        {
            CurrentPlatform.buff.EventChangeTimer -= UpdateText;
            UpdateText(String.Empty);
        }
        CurrentPlatform = TargetPlatform;
        if (CurrentPlatform != null && CurrentPlatform.IsBuffed)
        {
            CurrentPlatform.buff.EventChangeTimer += UpdateText;
        }
    }
    public bool IsWin()
    {
        return CurrentRow == platforms.Count / 2;
    }

    public bool IsFall()
    {
        return CurrentRow >= 0 && CurrentPlatform.IsBreakable;
    }

    public bool IsCheckpoint()
    {
        return CurrentRow == -1;
    }

    public void UpdatePlatform(Platform platform)
    {
        if (TargetPlatform == platform)
        {
            if (CurrentRow < platforms.Count / 2 && CurrentRow >= 0)
            {
                PlaySound(platform);
                CurrentPlatform.PerformPlatformAction();
                OnPlatformChanged?.Invoke(CurrentPlatform);
                OnPlatformCheñked?.Invoke();
            }
        }
    }

    public void PlaySound(Platform platform)
    {
        int soundIndex = platform.IsBreakable ? 1 : 0;
        if (platform.IsAlive)
        {
            AudioManager.instance.PlaySoundEffectByIndex(soundIndex);
        }
    }

    public void Restart()
    {
        Destroy(currentCheckpointPlatform.gameObject);
        CurrentStage = 0;
        CurrentRow = -1;
        CurrentIndex = 0;
        TargetPointPosition = startPoint.position;
        currentSpawnPoint = startPoint.position;

        CurrentPlatform = null;
        TargetPlatform = null;

        platforms = SpawnStage();

        ClearShapshots();
        TakeSnapshot();
    }

    private void ClearPrevTiles()
    {
        if (prevCheckpointPlatform != null)
        {
            Destroy(prevCheckpointPlatform.gameObject);
        }
        if (platforms != null)
        {
            for (int i = 0; i < platforms.Count; i+=2)
            {
                Destroy(platforms[i].transform.parent.gameObject);
            }

            platforms.Clear();
        }
    }

    private List<Platform> SpawnStage()
    {
        Stage stage = stages[CurrentStage];
        Vector3 currentDirection = stage.GetDirectionVector();
        ClearPrevTiles();

        List<Platform> newPlatforms = new List<Platform>();
        Vector3 leftOffset = new Vector3(-1.53f, 0.0f, -0.3f);
        Vector3 rightOffset = new Vector3(1.53f, 0.0f, -0.3f);

        int i = 0;
        for (; i < stage.Count; i++)
        {
            currentSpawnPoint += currentDirection * distanceBetween;
            GameObject rowObject = new GameObject("Row");
            rowObject.transform.position = currentSpawnPoint;
            rowObject.transform.rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            var leftPlatform = Instantiate(defaultPlatformPrefab);
            leftPlatform.transform.parent = rowObject.transform;
            leftPlatform.transform.localPosition = leftPlatform.transform.position + leftOffset;
            var rightPlatform = Instantiate(defaultPlatformPrefab);
            rightPlatform.transform.parent = rowObject.transform;
            rightPlatform.transform.localPosition = rightPlatform.transform.position + rightOffset;

            leftPlatform.index = 0;
            leftPlatform.row = i;

            rightPlatform.index = 1;
            rightPlatform.row = i;

            if(GetRandomIndex(i % 2) == 0){ leftPlatform.IsBreakable = true; }
            else{ rightPlatform.IsBreakable = true; }

            newPlatforms.Add(leftPlatform);
            newPlatforms.Add(rightPlatform);
        }
        currentSpawnPoint += currentDirection * distanceBetween;

        prevCheckpointPlatform = currentCheckpointPlatform;
        if (prevCheckpointPlatform != null)
        {
            prevCheckpointPlatform.row = -1;
        }
        currentCheckpointPlatform = Instantiate(checkpointPlatformPrefab, currentSpawnPoint, Quaternion.identity);
        currentCheckpointPlatform.row = i;
        return newPlatforms;
    }

    public void SetupByShapshot()
    {
        CurrentRow = prevManagerSnaphot.Row;
        CurrentIndex = prevManagerSnaphot.Column;
        for (int i = 0; i < platforms.Count; i++)
        {
            platforms[i].Setup(previousPlatformsData[i].IsAlive);
            platforms[i].RemoveBuff();
            if (previousPlatformsData[i].IsBuffed)
            {
                var debuff = new FreezeDebuff();
                debuff.Init(platforms[i], this, coldfeetConfig.FreezedPlatformPrefab, coldfeetConfig.Duration);
                platforms[i].AddBuff(debuff);
            }
            
            if(CurrentRow == platforms[i].row && CurrentIndex == platforms[i].index)
            {
                TargetPointPosition = platforms[i].transform.position;
                TargetPlatform = platforms[i];
            }
        }

        if (CurrentRow==-1)
        {
            if(prevCheckpointPlatform == null)
            {
                TargetPointPosition = startPoint.position;
                TargetPlatform = null;
            }
            else
            {
                TargetPointPosition = prevCheckpointPlatform.transform.position;
                TargetPlatform = prevCheckpointPlatform;
            }
        }
    }

    private void TakeSnapshot()
    {
        previousPlatformsData = currentPlatformsData;
        prevManagerSnaphot = currentManagerSnaphot;

        currentManagerSnaphot = new RowManagerSnaphot() { Row = CurrentRow, Column = CurrentIndex };

        List<PlatformData> data = new List<PlatformData>();
        for (int i = 0; i < platforms.Count; i++)
        {
            data.Add(new PlatformData() { IsAlive = platforms[i].IsAlive, IsBuffed = platforms[i].IsBuffed });
        }

        currentPlatformsData = data;
    }

    private void ClearShapshots()
    {
        previousPlatformsData = null;
        currentPlatformsData = null;

        prevManagerSnaphot = null;
        currentManagerSnaphot = null;
    }

    private int GetRandomIndex(int index)
    {
        if (IsTestGeneration)
        {
            return index;
        }
        return UnityEngine.Random.Range(0, 2);
    }

    private void UpdateText(string t)
    {
        _timerText.text = t;
    }
}

public class PlatformData
{
    public bool IsAlive { get; set; }
    public bool IsBuffed { get; set; }
}
public class RowManagerSnaphot
{
    public int Row { get; set; }
    public int Column { get; set; }
}
