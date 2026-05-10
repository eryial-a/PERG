using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    private bool pendingReset = false;
    // awake
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    // start
    void Start()
    {
        if (pendingReset)
        {
            pendingReset = false;
            ApplyRunStartReset();
        }
    }
    // called when enemy dies
    public void EnterDecisionScene()
    {
        SceneFade.Instance.LoadScene("Decission");
    }
    public void ExitFunction()
    {
        SceneFade.Instance.LoadScene("MainMenu");
    }
    // called after decision is made
    public void ReturnToCombat()
    {
        Player.Instance.madeDecision = false;
        pendingReset = true;
        SceneFade.Instance.LoadScene("Game");
    }
    void ApplyRunStartReset()
    {
        Debug.Log("Resetting deck + spawning new enemy");
        // Spawns new enemy apon start reset
        HandManager hand = FindFirstObjectByType<HandManager>();
        TurnManager turn = FindFirstObjectByType<TurnManager>();
        if (hand != null)
            hand.ResetDeckAndHand();
        // resets turn
        if (turn != null)
            turn.ResetTurn();
    }
    public void ResetRun()
    {
        Player.Instance.ResetToBase();
        HandManager hand = FindFirstObjectByType<HandManager>();
        TurnManager turn = FindFirstObjectByType<TurnManager>();
        BattleManager battle = FindFirstObjectByType<BattleManager>();
        // null
        if (hand != null)
            hand.ResetDeckAndHand();
        // null
        if (turn != null)
            turn.ResetTurn();
        // null
        if (battle != null)
            battle.enemy = null;
        // log
        Debug.Log("Run fully reset.");
    }
}