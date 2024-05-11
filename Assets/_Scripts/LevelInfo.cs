using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Levels/LevelInfo")]
public class LevelInfo : ScriptableObject
{
    public string Title = "default";
    public string SceneName = "SampleScene";
}
