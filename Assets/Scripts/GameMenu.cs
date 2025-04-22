using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public GameObject menuUI;
    private bool isMenuOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuUI.SetActive(isMenuOpen);
        Time.timeScale = isMenuOpen ? 0f : 1f; // pause/resume game
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
