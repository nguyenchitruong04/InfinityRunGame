using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] Vector3 rotationSpeed = new Vector3(0f, 100f, 0f); 
    // X: quay ngang, Y: quay dọc, Z: xoay trục trước

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
