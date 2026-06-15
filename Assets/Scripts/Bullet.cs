using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 25f;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponentInParent<EnemyController>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("Bullet dealt " + damage + " damage");
        }

        Destroy(gameObject);
    }
}
