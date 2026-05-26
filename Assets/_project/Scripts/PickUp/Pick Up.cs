using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    const string PLAYER_TAG = "Player";
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            OnPickUp();
            Destroy(gameObject);
        }
    }
    protected abstract void OnPickUp();
}
