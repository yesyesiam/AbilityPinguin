using UnityEngine;

public class WinLoseMenu : MonoBehaviour
{
    public enum GameState { Game, Victory, Defeat }
    [SerializeField] private GameState currentState;

    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject defeatScreen;

    public void SetCurrentState(GameState state)
    {
        currentState = state;
        UpdateScreens();
    }

    void UpdateScreens()
    {
        victoryScreen.SetActive(currentState == GameState.Victory);
        defeatScreen.SetActive(currentState == GameState.Defeat);
    }
}
