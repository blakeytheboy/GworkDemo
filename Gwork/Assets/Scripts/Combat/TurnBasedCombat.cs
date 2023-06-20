using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TurnBasedCombat : MonoBehaviour
{
    public int playerMinDamage = 1;  // Minimum damage player's weapon can deal
    public int playerMaxDamage = 20; // Maximum damage player's weapon can deal
    public int enemyBaseDamage = 5;  // Base damage of the enemy
    public int minRoll = 1;          // Minimum roll value (d20)
    public int maxRoll = 20;         // Maximum roll value (d20)

    public int playerHealth = 100;   // Player's initial health
    public int enemyHealth = 100;    // Enemy's initial health

    public TMP_InputField inputField;   // TMP_InputField for player input
    public TMP_Text combatLogText;      // TMP_Text for displaying combat log
    public TMP_Text playerHealthText;   // TMP_Text for displaying player's health
    public TMP_Text enemyHealthText;    // TMP_Text for displaying enemy's health

    public string sceneToLoad;          // Name of the scene to load after battle ends

    private bool isInCombat = true; // Indicates whether the player is currently in combat
    private bool playerTurn = true;  // Indicates whether it's the player's turn

    private void Start()
    {
        combatLogText.text = "Entered combat!";
        UpdateHealthTexts();
    }

    private void Update()
    {
        if (!isInCombat)
        {
            return; // Don't process combat logic if not in combat
        }

        // Check for player input
        if (playerTurn && Input.GetKeyDown(KeyCode.Return))
        {
            string input = inputField.text.ToLower();

            if (input == "roll")
            {
                int roll = Random.Range(minRoll, maxRoll + 1);
                inputField.text = ""; // Clear the input field

                combatLogText.text += $"\nPlayer rolls: {roll}";

                if (roll == 1)
                {
                    combatLogText.text += "\nYou missed the enemy!";
                }
                else if (roll == 20)
                {
                    int damage = Random.Range(playerMinDamage, playerMaxDamage + 1) * 2;
                    enemyHealth -= damage;
                    combatLogText.text += $"\nCritical hit! You dealt {damage} damage to the enemy.";
                }
                else
                {
                    int damage = Random.Range(playerMinDamage, playerMaxDamage + 1);
                    enemyHealth -= damage;
                    combatLogText.text += $"\nYou dealt {damage} damage to the enemy.";
                }

                UpdateHealthTexts();

                if (enemyHealth <= 0)
                {
                    combatLogText.text += "\nEnemy defeated! You win!";
                    isInCombat = false; // Exit combat
                    Invoke(nameof(LoadSceneAfterBattle), 2f); // Load the new scene after 2 seconds
                    return;
                }

                playerTurn = false; // Switch to enemy's turn
                combatLogText.text += "\nEnemy's turn.";
                Invoke(nameof(EnemyTurn), 1f); // Wait for 1 second before executing the enemy's turn
            }
        }
    }

    private void EnemyTurn()
    {
        // Roll the dice for the enemy's attack
        int roll = Random.Range(minRoll, maxRoll + 1);
        combatLogText.text += $"\nEnemy rolls: {roll}";

        if (roll == 1)
        {
            combatLogText.text += "\nThe enemy missed you!";
        }
        else if (roll == 20)
        {
            int damage = enemyBaseDamage * 2;
            playerHealth -= damage;
            combatLogText.text += $"\nCritical hit! The enemy dealt {damage} damage to you.";
        }
        else
        {
            playerHealth -= enemyBaseDamage;
            combatLogText.text += $"\nThe enemy dealt {enemyBaseDamage} damage to you.";
        }

        UpdateHealthTexts();

        if (playerHealth <= 0)
        {
            combatLogText.text += "\nYou were defeated! Game over!";
            isInCombat = false; // Exit combat
            Invoke(nameof(LoadGameOverScene), 2f); // Load the game over scene after 2 seconds
            return;
        }

        playerTurn = true; // Switch back to player's turn
        combatLogText.text += "\nPlayer's turn.";
    }

    private void UpdateHealthTexts()
    {
        playerHealthText.text = $"Player Health: {playerHealth}";
        enemyHealthText.text = $"Enemy Health: {enemyHealth}";
    }

    private void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void LoadGameOverScene()
    {
        SceneManager.LoadScene("Gameover");
    }
}
