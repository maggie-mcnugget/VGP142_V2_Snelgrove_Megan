using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private Animator animator;
    public bool isDead { get; private set; }

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Player took {damage}. HP: {currentHealth}");

        animator.SetTrigger("getHit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log("Healed: " + amount);
    }

    private void Die()
    {
        isDead = true;

        animator.applyRootMotion = true;
        GetComponent<PlayerController>().enabled = false;

        animator.SetTrigger("hasDied");

        CharacterController controller = GetComponent<CharacterController>();
        controller.enabled = false;

        Debug.Log("Player has died.");
    }
}
