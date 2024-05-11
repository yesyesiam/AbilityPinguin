using Newtonsoft.Json;

public class PlayerData
{
    private int _unlockedLevelsCount;

    public int UnlockedLevelsCount
    {
        get { return _unlockedLevelsCount; }
        set
        {
            if (value >= 1 && value <= 4)
            {
                _unlockedLevelsCount = value;
            }
            else
            {
                _unlockedLevelsCount = 1;
            }
        }
    }

    private int _qualityLevel;
    
    public int QualityLevel
    {
        get { return _qualityLevel; }
        set
        {
            if (value >= 0 && value <= 3)
            {
                _qualityLevel = value;
            }
            else
            {
                _qualityLevel = 0;
                //throw new ArgumentOutOfRangeException(nameof(QualityLevel), "value must be 0 to 3.");
            }
        }
    }

    public float Volume { get; set; }

    public PlayerData()
    {
        _qualityLevel = 1;
        Volume = 0.7f;

        UnlockedLevelsCount = 1;
    }

    [JsonConstructor]
    public PlayerData(int qualityLevel, float volume, int unlockedLevelsCount)
    {
        QualityLevel = qualityLevel;
        Volume = volume;

        UnlockedLevelsCount = unlockedLevelsCount;
    }
}
