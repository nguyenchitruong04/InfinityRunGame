using UnityEngine;

public class Apple : PickUp
{
    [SerializeField] float speedBoost = 3f;
    LevelGenerator levelGen;
    public void Init(LevelGenerator levelGenerator)
    {
        this.levelGen = levelGenerator;
    }

    protected override void OnPickUp()
    {
        if (levelGen != null)
        {
            levelGen.ChangeChunkMoveSpeed(speedBoost);
        }
    }
}
