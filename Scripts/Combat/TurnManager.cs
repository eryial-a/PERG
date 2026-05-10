using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
// Keeps tracks of turn
public class TurnManager : MonoBehaviour
{
    public BattleManager battleManager;
    // base player speed, subject for change later
    // StatboardUIs
    public StatBoardUI playerBoard;
    public StatBoardUI enemyBoard;
    // enemy popup ui (attack descriptions)
    public EnemyAttackPopupUI enemyPopup;
    // enemy animations
    [SerializeField] private Animator animator;
    // boss animations
    [SerializeField] private Animator bossAnimator;
    // audiomanager reference and isBoss variable
    AudioManager audioManager;
    bool isBoss = false;
    // grab audio manager and isBoss reference
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // player actions
    int playerActions;
    int enemyActions;
    // based
    int basePlayerActions;
    int baseEnemyActions;
    // bool for players turn
    bool playerTurn = true;
    // process turn bool
    bool processingTurn = false;
    // start round
  public void StartRound()
    {
        Enemy enemy = battleManager.enemy;
        UpdateBoards();
        // returns log if not enemy is there
        if (enemy == null)
        {
            Debug.LogError("No enemy for turn calculation.");
            return;
        }
        // First time actions
        CalculateActions(enemy.speed);
        // Reset actions each round
        playerActions = basePlayerActions;
        enemyActions = baseEnemyActions;
        // sets player turn to true
        playerTurn = true;
        // loga
        Debug.Log("New Round Started");
        Debug.Log("Player Actions: " + playerActions);
        Debug.Log("Enemy Actions: " + enemyActions);
        // updates boards
        UpdateBoards();
    }
    // Calculates action amount based on speed
    void CalculateActions(int enemySpeed)
    {
        basePlayerActions = 1;
        baseEnemyActions = 1;
        // if statements
        int diff = Player.Instance.speed - enemySpeed;
        // player much faster
        if (diff >= 99)
        {
            basePlayerActions = 4;
        }
        else if (diff >= 50)
        {
            basePlayerActions = 3;
            
        }
        else if (diff <= -99)
        {
            basePlayerActions = 1;
        }
        else if (diff <= -50)
        {
            basePlayerActions = 2;
        }
        else
        {
            basePlayerActions = 3;
        }
    }
    // Can player make action
    public bool CanPlayerAct()
    {
        return playerTurn && playerActions > 0;
    }
    // Action used, uses a action point
    public void PlayerUsedAction()
    {
        if (processingTurn) return;
        // player uses action point
        playerActions--;
        UpdateBoards();
        if (playerActions <= 0)
        {
            Enemy enemy = battleManager.enemy;
            // if enemy dead or missing, do not start enemy turn
            if (enemy == null || enemy.health <= 0)
            {
                processingTurn = false;
                return;
            }
            // switches turn
            playerTurn = false;
            processingTurn = true;
            // enemy delay attack
            StartCoroutine(BeginEnemyTurnDelay());
        }
    }
    // Enemy turn
    IEnumerator EnemyTurnRoutine()
    {
        Enemy enemy = battleManager.enemy;

        // enemy actions (while its turn)
        while (enemyActions > 0 && enemy != null)
        {
            // random damage between 75% and 110%
            int minDamage = Mathf.RoundToInt(enemy.damage * 0.90f);
            int maxDamage = Mathf.RoundToInt(enemy.damage * 1.10f);
            // final damage taken
            int finalDamage = Random.Range(minDamage, maxDamage + 1);
            // logs damage
            Debug.Log("Enemy attacks for " + finalDamage);
            // animates enemy and plays sound effect
            if (finalDamage!=0){

                // attack sfx
                isBoss = GameObject.Find("Enemy").GetComponent<Enemy>().isBoss;
                if (isBoss) 
                {
                    audioManager.PlaySFX(audioManager.bossAttack);
                }
                else
                {
                    audioManager.PlaySFX(audioManager.enemyAttack);
                }
                
                // animate
                animator.SetBool("isAttacking", true);
                bossAnimator.SetBool("isAttacking", true);
                yield return new WaitForSeconds(0.1f);
                animator.SetBool("isAttacking", false);
                bossAnimator.SetBool("isAttacking", false);
            }
            // enemy popup is assigned
            if (enemyPopup != null)
            {
                enemyPopup.Show(finalDamage);
            }
            else // enemy pop up isnt assigned
            {
                Debug.LogWarning("Enemy popup UI not assigned!");
            }
            // player takes hit
            Player.Instance.TakeDamage(finalDamage);
            // enemy actions
            enemyActions--;
            UpdateBoards();
            // adds timer to prevent issues
            yield return new WaitForSeconds(0.6f);
        }
        // processes turn
        processingTurn = false;
        StartRound();
    }
    // updates enemy and player stat boards
    public void UpdateBoards()
    {
        if (Player.Instance != null && playerBoard != null)
        {
            playerBoard.UpdateBoard(
                Player.Instance.maxHealth,
                Player.Instance.currentHealth,
                Player.Instance.speed,
                Player.Instance.strength,
                playerActions
            );
        }
        else
        {
            Debug.LogWarning("Player or PlayerBoard missing");
        }
        // enemy null search
        Enemy enemy = battleManager != null ? battleManager.enemy : null;
        if (enemy != null && enemyBoard != null)
        {
            enemyBoard.UpdateBoard(
                enemy.maxHealth,
                enemy.health,
                enemy.speed,
                enemy.damage,
                enemyActions
            );
        }
        else
        {
            Debug.LogWarning("Enemy or EnemyBoard missing");
        }
    }
    // resets turn state
    public void ResetTurn()
    {
        StopAllCoroutines();

        playerTurn = true;
        processingTurn = false;

        playerActions = 0;
        enemyActions = 0;
        basePlayerActions = 0;
        baseEnemyActions = 0;

        Debug.Log("Turn system reset");
    }
    // enemy turn delay
    IEnumerator BeginEnemyTurnDelay()
    {
        yield return new WaitForSeconds(0.7f); // pause after player's final action
        StartCoroutine(EnemyTurnRoutine());
    }
}