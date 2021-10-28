using UnityEngine;

public class Zone : MonoBehaviour
{
    public Clothes linkedClothes;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            linkedClothes.SetClothes(other);
        }
    }
}
