using UnityEngine;

public class Coin : PickUp
{
    ScoreManager scoreManager;
    public void Init(ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
    }

    protected override void OnPickUp()
    {
        scoreManager.AddScore(100);
        Destroy(gameObject);
    }
}
