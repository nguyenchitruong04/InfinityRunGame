using UnityEngine;

public class PlayerCollisionHandle : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float collisionCooldown = 1f;
    [SerializeField] float changeSpeed = -2f;
    const string hitString = "Hit";
    const string PLAYER_TAG = "Player";
    float cooldownTimer;
    LevelGenerator lvGen;
    void Start()
    {
        lvGen = FindFirstObjectByType<LevelGenerator>();
    }
    void Update()
    {
        cooldownTimer += Time.deltaTime;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (cooldownTimer < collisionCooldown) return;
        lvGen.ChangeChunkMoveSpeed(changeSpeed);
        animator.SetTrigger(hitString);
        cooldownTimer = 0f;
            
        
    }
}
