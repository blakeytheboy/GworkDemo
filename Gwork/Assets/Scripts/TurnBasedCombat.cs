using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public struct EnemyDialogueLine
{
    public int rollValue; //roll value at which the dialogue line triggers
    public string dialogueLine; //dialogue
}

public class TurnBasedCombat : MonoBehaviour
{
    public int playerBaseDamage = 10; //base player damage
    public int enemyBaseDamage = 5;  //abse enemy damage
    public int MinRoll = 1;//minimum die roll
    public int MaxRoll = 20;  //max die roll
    public int playerMinMissRoll = 1;//mimimum roll for miss player
    public int playerMaxMissRoll = 5; //maxiumum roll for miss player
    public int enemyMinMissRoll = 1; //mimimum roll for miss PLAYER
    public int enemyMaxMissRoll = 5;  //maxiumum roll for miss ENEMY
    public int playerMinCriticalRoll = 18;// minimum player hit roll
    public int playerMaxCriticalRoll = 20;// maximum player hit roll
    public int enemyMinCriticalRoll = 18; // minimum ENEMY hit roll
    public int enemyMaxCriticalRoll = 20; // maxiumum ENEMY hit roll

    public int playerHealth = 100;   // player health
    public int enemyHealth = 100;    // enemy health

    public TMP_InputField inputField; //player input
    public TMP_Text combatLogText; //combat log n dialogue
    public TMP_Text playerHealthText; //player health
    public TMP_Text enemyHealthText; //enemy health

    public string sceneToLoad; //scene to load if player wins

    private bool isInCombat = true; //checks to see if the player is currently in combat or not
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
            return; //dont process combat  if not in combat
        }

        if (playerTurn && Input.GetKeyDown(KeyCode.Return)) //check for player input of...
        {
            string input = inputField.text.ToLower();

            if (input == "roll") //checks for roll
            {
                int roll = Random.Range(MinRoll, MaxRoll + 1);
                inputField.text = ""; //clear input

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
                    isInCombat = false; //exit combat
                    Invoke(nameof(LoadSceneAfterBattle), 2f); //load new scene if player wins after x seconds
                    return;
                }

                playerTurn = false; //begin enemy turn
                combatLogText.text += "\nEnemy's turn.";
                Debug.Log("enemy turn");
                Invoke(nameof(EnemyTurn), 1f); // wait x seconds before enemy rolls
            }
        }
    }

    private void EnemyTurn()
    {
        int roll = Random.Range(MinRoll, MaxRoll + 1);
        combatLogText.text += $"\nEnemy rolls: {roll}";
        //roll die for enemy attack

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
            isInCombat = false; //exit combat
            Invoke(nameof(LoadGameOverScene), 2f); //load game over if i die
            return;
        }

        playerTurn = true; //switch back to me
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