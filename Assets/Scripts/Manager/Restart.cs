using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] GameObject GOmenu;

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        Cursor.visible = false;
    }
}
