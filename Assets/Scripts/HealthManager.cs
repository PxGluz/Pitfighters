using UnityEngine.UI;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private GameObject redHealth, backHealth;
    [SerializeField] private Rigidbody rb;

    public float maxHealth, animationSpeed;

    [HideInInspector] public float currentHealth;
    private Vector3 targetRed, targetBack;
    private Image backImage;

    private void Start()
    {
        currentHealth = maxHealth;
        targetRed = targetBack = redHealth.transform.localScale = backHealth.transform.localScale = Vector3.one;
        backImage = backHealth.GetComponent<Image>();
    }

    public void TakeDamage(float damage, Vector3 direction)
    {
        Knockback(direction);
        if (currentHealth > damage)
            currentHealth -= damage;
        else
            currentHealth = 0;
        backImage.color = Color.yellow;
        targetRed = redHealth.transform.localScale = targetBack = new Vector3(currentHealth / maxHealth, 1, 1);
    }

    private void Knockback(Vector3 direction)
    {
        const float knockForce = 10f;
        rb.AddForce(direction * knockForce);
    }

    public void Heal(float healAmount)
    {
        if (currentHealth + healAmount < maxHealth)
            currentHealth += healAmount;
        else
            currentHealth = 0;
        backImage.color = Color.green;
        targetRed = backHealth.transform.localScale = targetBack = new Vector3(currentHealth / maxHealth, 1, 1);
    }

    private void Update()
    {
        backHealth.transform.localScale = Vector3.Lerp(backHealth.transform.localScale, targetBack, animationSpeed);
        redHealth.transform.localScale = Vector3.Lerp(redHealth.transform.localScale, targetRed, animationSpeed);
    }
}
