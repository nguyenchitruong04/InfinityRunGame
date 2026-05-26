using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Coin : MonoBehaviour
{
    [SerializeField] int ScoreValue = 100;

    const string PLAYER_TAG = "Player";
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            ScoreManager.Instance?.AddScore(ScoreValue);
            Destroy(gameObject);
        }
    }
}
