using UnityEngine;

public class EnemyStatTest : MonoBehaviour
{
    public int normalEnemyTests = 10;
    public int bossEnemyTests = 3;
    // START
    void Start()
    {
        Debug.Log("===== ENEMY STAT TEST =====");
        // runs both
        TestNormalEnemies();
        TestBossEnemies();
    }
    // TEST ENEMY
    void TestNormalEnemies()
    {
        Debug.Log("---- Testing Normal Enemies ----");

        for (int i = 0; i < normalEnemyTests; i++)
        {
            GameObject enemyObj = new GameObject("TestEnemy");
            // generates enemy
            Enemy enemy = enemyObj.AddComponent<Enemy>();
            enemy.isBoss = false;

            enemy.RandomizeStats();

            int total = enemy.health + enemy.damage + enemy.speed;
            // logs stats
            Debug.Log("Enemy " + (i + 1) +
                " | HP: " + enemy.health +
                " DMG: " + enemy.damage +
                " SPD: " + enemy.speed +
                " TOTAL: " + total);
            // prints if points were not evenly distrubuted
            if (total != 1000)
                Debug.LogError("ERROR: Normal enemy stats do not equal 1000!");

            Destroy(enemyObj);
        }
    }
    // TEST BOSS
    void TestBossEnemies()
    {
        Debug.Log("---- Testing Boss Enemies ----");

        for (int i = 0; i < bossEnemyTests; i++)
        {
            GameObject enemyObj = new GameObject("TestBoss");
            // generates boss
            Enemy enemy = enemyObj.AddComponent<Enemy>();
            enemy.isBoss = true;

            enemy.RandomizeStats();

            int total = enemy.health + enemy.damage + enemy.speed;
            // logs stats
            Debug.Log("Boss " + (i + 1) +
                " | HP: " + enemy.health +
                " DMG: " + enemy.damage +
                " SPD: " + enemy.speed +
                " TOTAL: " + total);
            // prints if points were not evenly distrubuted
            if (total != 2000)
                Debug.LogError("ERROR: Boss stats do not equal 2000!");

            Destroy(enemyObj);
        }
    }
}