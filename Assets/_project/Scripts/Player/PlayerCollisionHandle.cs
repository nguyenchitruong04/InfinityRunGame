using UnityEngine;

public class PlayerCollisionHandle : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float collisionCooldown = 1f;
    [SerializeField] float changeSpeed = -2f;
    const string hitString = "Hit";
    const string GROUND_TAG = "groundLayer";
    float cooldownTimer;
    LevelGenerator lvGen;
    Rigidbody playerRigidbody;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

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
        if (collision.gameObject.CompareTag(GROUND_TAG)) return;
        if (cooldownTimer < collisionCooldown) return;

        lvGen.ChangeChunkMoveSpeed(changeSpeed);
        animator.SetTrigger(hitString);
        cooldownTimer = 0f;
        Debug.Log("[PlayerCollisionHandle] Player hit an obstacle. Cooldown reset.");
    }

   

   
}
