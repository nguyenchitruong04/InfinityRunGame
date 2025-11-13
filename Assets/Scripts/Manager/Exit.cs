using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    [SerializeField] Button exitButton;

    void Awake()
    {
        if (exitButton == null)
            exitButton = GetComponent<Button>();
    }

    void OnEnable()
    {
        if (exitButton != null)
        {
            exitButton.onClick.RemoveListener(ExitGame);
            exitButton.onClick.AddListener(ExitGame);
        }
    }

    void OnDisable()
    {
        if (exitButton != null)
            exitButton.onClick.RemoveListener(ExitGame);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
