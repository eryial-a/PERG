using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    // is boss bool
    public bool isBoss = false;
    // enemy stats
    public int maxHealth;
    public int health;
    public int damage;
    public int speed;
    // audiomanager reference
    AudioManager audioManager;
    // grab audio manager reference
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // is dead bool
    bool isDead = false;
    // point place holder
    int totalPoints;
    // refrence
    public EnemyHealthBar healthBar;
    // Game objects
    public GameObject EnemySprite;
    public GameObject BossSprite;
    // private positions
    private Vector3 enemyOriginalPos;
    private Vector3 bossOriginalPos;
    // enemy animations
    [SerializeField] private Animator animator;
    // boss animations
    [SerializeField] private Animator bossAnimator;
    // creates enemy
    void Start()
    {   
        // sprites original positions
        enemyOriginalPos = EnemySprite.transform.localPosition;
        bossOriginalPos = BossSprite.transform.localPosition;
    isBoss = (Player.Instance.enemiesKilled > 0 &&
              Player.Instance.enemiesKilled % 5 == 0); // 6th wave boss spawns, then 10th, then every 5th after a boss spawns
        RandomizeStats();  
        PositionChange();
        PrintStats();
        // healthbar
        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
        FindFirstObjectByType<TurnManager>()?.UpdateBoards();
        FindFirstObjectByType<TurnManager>()?.StartRound();
    }
    // assigns randomized values based off of points
    public void RandomizeStats()
    {
        // base pools
        int basePoints = isBoss ? 2000 : 1000;
        // apon enemy defeat add scale
        int scaling = Player.Instance.enemiesDefeated * 200;
        // scales
        int totalPoints = basePoints + scaling;
        // remaing points
        int remaining = totalPoints;
        // stats
        int minHealth;
        int minDamage;
        int minSpeed;
        int maxSpeed;
        // boss stats
        if (isBoss){
            maxSpeed = 150; // (speed capped for fairness)
            minSpeed = 50; // (but now cant be too slow either)
            minDamage = 300; // strong
            minHealth = 500; // beefy
        }
        else
        {
            maxSpeed = 200; // flash
            minSpeed = 1; // a literal molusk
            minDamage = 100; // good slap in the face
            minHealth = 300; // vegan diet
        }
        // Health
        health = Random.Range(minHealth, remaining - (minDamage + maxSpeed));
        remaining -= health;
        // Damage
        damage = Random.Range(minDamage, remaining - minSpeed);
        remaining -= damage;
        // Speed (capped at 150)
        speed = Mathf.Min(remaining, maxSpeed);
        remaining -= speed;
        // If we capped speed, redistribute leftover points
        health += remaining;
        maxHealth = health;
    }
    void PrintStats()
    {
        if (isBoss) // prints if boss
            Debug.Log("=-=*BOSS*=-=");
        else // prints if enemy
            Debug.Log("--=Enemy=--");
        // stats
        Debug.Log("Health: " + health);
        Debug.Log("Damage: " + damage);
        Debug.Log("Speed: " + speed);
        // prints all points used
        Debug.Log("Points used: " + (health + damage + speed));
    }
    // damage taken class
    public void OncomingDamage(float amount)
    {
        if (isDead) return;
        int finalDamage = Mathf.RoundToInt(amount);
        // enemy damage taken
        health -= finalDamage;
        // audio for being hit
        audioManager.PlaySFX(audioManager.enemyHit);
        // animation for being hit
        animator.SetBool("isHit", true);
        bossAnimator.SetBool("isHit", true);
        Debug.Log("Enemy took " + finalDamage + " damage");
        Player.Instance.totalDamageDealt += finalDamage;
        // updates health bar
        FindFirstObjectByType<TurnManager>()?.UpdateBoards();
        healthBar.SetHealth(health);
        // reset bool and waits to allow animation to play
        StartCoroutine(waiter());
        // DEATH
        if (health <= 0)
        {
            Death();
        }
    }
    // Waiter class
    IEnumerator waiter()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isHit", false);
        bossAnimator.SetBool("isHit", false);
    }
    // destroys enemy and loads new scene
    void Death()
    {
        if (isDead) return;
        isDead = true;
        // logs
        Debug.Log("Enemy died");
        if (isBoss)
        {
            Player.Instance.bossesKilled++;
        }
        // scaling
        Player.Instance.enemiesDefeated++;
        // end game stats
        Player.Instance.enemiesKilled++;
        Player.Instance.currentWave++;
        // scene change to decision scene 
        if (GameStateManager.Instance != null)
        {
            // play death sound effect
            if (isBoss)
            {
                audioManager.PlaySFX(audioManager.bossDeath);
            }
            else
            {
                audioManager.PlaySFX(audioManager.enemyDeath);
            }
            // animates enemy
            animator.SetBool("isDead", true);
            bossAnimator.SetBool("isDead", true);
            GameStateManager.Instance.EnterDecisionScene();
        }
        else
        {
            Debug.LogError("GameStateManager instance is missing!");
        }
        Destroy(gameObject);
    }
    // changes positions of game sprites
    void PositionChange()
    {
        if (EnemySprite == null || BossSprite == null) return;

        if (isBoss)
        {
            EnemySprite.SetActive(false);
            BossSprite.SetActive(true);
            // tweaks boss positioning
            Vector3 pos = bossOriginalPos;
            pos.y += 50f;
            pos.x += 30f;
            BossSprite.transform.localPosition = pos;
        }
        else
        {
            BossSprite.SetActive(false);
            EnemySprite.SetActive(true);
            // restore normal position
            EnemySprite.transform.localPosition = enemyOriginalPos;
        }
    }
}
