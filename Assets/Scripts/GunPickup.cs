using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public float floatHeight = 0.25f;
    public float floatSpeed = 2f;

    private Vector3 startPos;


    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + Vector3.up * yOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.hasGun = true;
            Destroy(gameObject);
        }
    }
}
