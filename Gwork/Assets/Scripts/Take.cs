using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/InputActions/Take")]
public class Take : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        Dictionary<string, string> takeDictonary = controller.interactableItems.Take(separatedInputWords);
        //try to take something, if successful we get back a dictioonary

        if (takeDictonary != null) //does not equel 
        {
            controller.LogStringWithReturn(controller.TestVerbDictionaryWithNoun(takeDictonary, separatedInputWords[0], separatedInputWords[1]));
        }
    }
}
