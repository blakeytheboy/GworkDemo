using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public struct EnemyDialogueLine
{
    public int rollValue;       // Roll value at which the dialogue line triggers
    public string dialogueLine; // Dialogue line spoken by the enemy
}

public class TurnBasedCombat : MonoBehaviour
{
    public int playerBaseDamage = 10;   // Base damage player's weapon can deal
    public int enemyBaseDamage = 5;     // Base damage of the enemy
    public int MinRoll = 1;       // Minimum roll value for the player and enemy (d20)
    public int MaxRoll = 20;      // Maximum roll value for the player and enemy (d20)
    public int playerMinMissRoll = 1;   // Minimum roll value for miss chance (player)
    public int playerMaxMissRoll = 5;   // Maximum roll value for miss chance (player)
    public int enemyMinMissRoll = 1;    // Minimum roll value for miss chance (enemy)
    public int enemyMaxMissRoll = 5;    // Maximum roll value for miss chance (enemy)
    public int playerMinCriticalRoll = 18;  // Minimum roll value for player's critical hit
    public int playerMaxCriticalRoll = 20;  // Maximum roll value for player's critical hit
    public int enemyMinCriticalRoll = 18;   // Minimum roll value for enemy's critical hit
    public int enemyMaxCriticalRoll = 20;   // Maximum roll value for enemy's critical hit
    public EnemyDialogueLine[] enemyDialogues; // Array of enemy dialogue lines

    public int playerHealth = 100;   // Player's initial health
    public int enemyHealth = 100;    // Enemy's initial health

    public TMP_InputField inputField;         // TMP_InputField for player input
    public TMP_Text combatLogText;            // TMP_Text for displaying combat log
    public TMP_Text playerHealthText;         // TMP_Text for displaying player's health
    public TMP_Text enemyHealthText;          // TMP_Text for displaying enemy's health

    public string sceneToLoad;                // Name of the scene to load after battle ends

    private bool isInCombat = true; // Indicates whether the player is currently in combat
    private bool playerTurn = true;  // Indicates whether it's the player's turn

    private void Start()
    {
        combatLogText.text = "Entered combat! Type 'Roll' to engage!";
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
                int roll = Random.Range(MinRoll, MaxRoll + 1);
                inputField.text = ""; // Clear the input field

                combatLogText.text += $"\nPlayer rolls: {roll}";

                if (roll >= playerMinMissRoll && roll <= playerMaxMissRoll)
                {
                    combatLogText.text += "\nYou missed the enemy!";
                }
                else if (roll >= playerMinCriticalRoll && roll <= playerMaxCriticalRoll)
                {
                    int damage = playerBaseDamage * 2;
                    enemyHealth -= damage;
                    combatLogText.text += $"\nCritical hit! You dealt {damage} damage to the enemy.";
                }
                else
                {
                    int damage = playerBaseDamage;
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
        int roll = Random.Range(MinRoll, MaxRoll + 1);
        combatLogText.text += $"\nEnemy rolls: {roll}";

        for (int i = 0; i < enemyDialogues.Length; i++)
        {
            if (roll == enemyDialogues[i].rollValue)
            {
                combatLogText.text += $"\nEnemy says: {enemyDialogues[i].dialogueLine}";
                break;
            }
        }

        if (roll >= enemyMinMissRoll && roll <= enemyMaxMissRoll)
        {
            combatLogText.text += "\nThe enemy missed you!";
        }
        else if (roll >= enemyMinCriticalRoll && roll <= enemyMaxCriticalRoll)
        {
            int damage = enemyBaseDamage * 2;
            playerHealth -= damage;
            combatLogText.text += $"\nCritical hit! The enemy dealt {damage} damage to you.";
        }
        else
        {
            int damage = enemyBaseDamage;
            playerHealth -= damage;
            combatLogText.text += $"\nThe enemy dealt {damage} damage to you.";
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
