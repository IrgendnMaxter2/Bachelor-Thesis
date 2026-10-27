using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameWonScreen;

    private bool endScreenShown;

    private void Awake()
    {
        if (gameWonScreen != null)
            gameWonScreen.SetActive(false);
    }

    public void ShowGameWon()
    {
        ShowScreen(gameWonScreen);
    }

    private void ShowScreen(GameObject screenToShow)
    {
        if (endScreenShown)
            return;

        if (screenToShow == null)
        {
            Debug.LogError("Assign the requested end screen in the Inspector.", this);
            return;
        }

        screenToShow.SetActive(true);
        endScreenShown = true;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}