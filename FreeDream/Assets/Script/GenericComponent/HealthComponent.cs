using UnityEditor;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    private bool isDead = false;
    private DamageController damageControl;
    void Start()
    {
        damageControl = GetComponent<DamageController>();
        FullHeal();
    }

    public bool GetIsDead(){ return isDead; }
    public float GetCurrentHealth(){ return currentHealth; }

    public void FullHeal()
    {
        currentHealth = maxHealth;
    }

    public void HealChange(float _change)
    {
        currentHealth += _change;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth == 0)
        {
            isDead = true;
            damageControl.OnDeath();
        }
        else
        {
            damageControl.OnDamage();
        }

        Debug.Log(currentHealth);
    }
}
