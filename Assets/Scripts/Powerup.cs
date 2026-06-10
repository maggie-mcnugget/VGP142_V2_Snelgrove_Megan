using UnityEngine;

public class FloatingPowerup : MonoBehaviour
{
    public float floatHeight = 0.25f;
    public float floatSpeed = 2f;

    public float damageIncrease = 5f;
    public int healthIncrease = 10;

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
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            player.attackDamage += damageIncrease;
        }

        if (health != null)
        {
            health.Heal(healthIncrease);
        }

        if (player != null || health != null)
        {
            Debug.Log("Powerup collected!");
            Destroy(gameObject);
        }
    }
}
