using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGame : MonoBehaviour
{
    [SerializeField] Button NewGameMenu;
    [SerializeField] Button Exit;
    bool newGameStarted = false;
    void Start()
    {
        Time.timeScale = 0f;
        if (NewGameMenu != null)
        {
            NewGameMenu.onClick.RemoveListener(StartNewGame);
            NewGameMenu.onClick.AddListener(StartNewGame);
        }
    }
    public void StartNewGame()
    {
        if (newGameStarted) return;
        newGameStarted = true;

        Time.timeScale = 1f;
        if (Exit != null) Exit.gameObject.SetActive(false);
        Cursor.visible = false;
        SceneManager.LoadScene(1);
    }

}
