using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInput : MonoBehaviour 
{
    public TMP_InputField inputField;

    GameController controller;

    void Awake()
    {
        controller = GetComponent<GameController> (); 
        inputField.onEndEdit.AddListener (AcceptStringInput); //awake makes the input field always accessable even when script isnt called
    }

    void AcceptStringInput(string userInput) //take string input...
    {
        userInput = userInput.ToLower (); //make all the text input lowercase so its easier to compare
        controller.LogStringWithReturn (userInput); //display what user typed so player can see it

        char[] delimiterCharacters = { ' ' }; //looks for the spaces between words to tell them apart
        string[] separatedInputWords = userInput.Split (delimiterCharacters);
        //take what the player has typed, look for spaces, and seperate the characters between the spaces into strings in our array of words

        for (int i = 0; i < controller.inputActions.Length; i++)  
        {
            InputAction inputAction = controller.inputActions [i];
            if (inputAction.keyWord == separatedInputWords [0]) //action is first (go, use etc) will check if it has its
            {
                inputAction.RespondToInput (controller, separatedInputWords); //if the action is TRUE, then it will check second word in array
            }
        }

        InputComplete ();

    }

    void InputComplete()
    {
        controller.DisplayLoggedText ();
        inputField.ActivateInputField (); //when you hit return, it takes focus away from input field so i want to activate it again imediately
        inputField.text = null; //make it empty so the player can type something new
    }

}