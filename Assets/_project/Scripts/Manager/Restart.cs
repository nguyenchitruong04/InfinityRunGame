using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] GameObject GOmenu;
    [SerializeField] GameObject mainGameUI;

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        GOmenu.SetActive(false);
        mainGameUI.SetActive(true);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
