using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    // Base Player // More classes implemented later
    [Header("Base Stats")]
    public int maxHealth = 2500;
    public int speed = 100;
    public float strength = 1;
    // inventory
    [Header("Inventory")]
    public int healthPotions = 3;
    // run time stats HEALTH
    [Header("Runtime Stats")]
    public int currentHealth;
    public bool strengthBuffActive = false;
    // initialized bool
    private bool initialized = false;
    // enemies defeated (scales enemy difficulty)
    public int enemiesDefeated = 0;
    // used for player stats
    public int enemiesKilled = 0;
    public bool madeDecision = false;
    [Header("End Game Statistics")]
    // combat
    public int totalDamageDealt = 0;
    public int totalDamageReceived = 0;
    public int totalHealthRegenerated = 0;
    // progression
    public int bossesKilled = 0;
    // consumables
    public int potionsUsed = 0;
    public int currentWave = 1;
    // awake
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (!initialized)
        {
            currentHealth = maxHealth;
            initialized = true;
        }
    }
    // damage taken
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        // log amount of damage and current health
        Debug.Log("Player took " + amount + " damage.");
        Debug.Log("Player HP: " + currentHealth);
        totalDamageReceived += amount;
        // DEATH
        if (currentHealth <= 0)
        {
            Death();
        }
    }
    // healing potion item
    public void Heal(int amount)
    {
        int oldHealth = currentHealth;
        currentHealth += amount;
        // If heals goes beyond player max health reset to current to max health
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        int actualHeal = currentHealth - oldHealth;
        totalHealthRegenerated += actualHeal;
        Debug.Log("Player healed " + amount);
    }
    // DEATH
    void Death()
    {
        enemiesKilled-= bossesKilled;
        Debug.Log("Player died. Game Over.");
        // Load GameOver scene
        SceneFade.Instance.LoadScene("GameOver");
    }
    public void ResetToBase()
    {
        madeDecision = false;
        // Reset core stats
        maxHealth = 2500;
        speed = 100;
        strength = 1;
        // Reset runtime
        currentHealth = maxHealth;
        strengthBuffActive = false;
        // Reset inventory
        healthPotions = 3;
        // reset values
        enemiesDefeated = 0;
        enemiesKilled = 0;
        totalDamageDealt = 0;
        totalDamageReceived = 0;
        totalHealthRegenerated = 0;
        // progression
        bossesKilled = 0;
        // consumables
        potionsUsed = 0;
        currentWave = 1;
        // values have been reset
        Debug.Log("Player reset to base values.");
    }
}