using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{

    public TextMeshProUGUI displayText;
    public InputAction[] inputActions;

    [HideInInspector] public RoomNavigation roomNavigation; //dont want to see it in inspector, but we want other scripts to accesss it hense public
    [HideInInspector] public List<string> interactionDescriptionsInRoom = new List<string>(); //all the things we can interact with in the room (exits, items)
    [HideInInspector] public InteractableItems interactableItems;

    //new list of strings called action log, equal to a new list of strings
    List<string> actionLog = new List<string>(); 
    // represents a typed lsit of objects tha can be accessed by index (able to search and sort)
    //does not need size decalred, must be specified using <>


    // Use this for initialization
    void Awake() //called even when script is disabled
    {
        interactableItems = GetComponent<InteractableItems>();
        roomNavigation = GetComponent<RoomNavigation>();
    }

    void Start()
    {
        DisplayRoomText();   //room description
        DisplayLoggedText();  //display action log (exits, items etc)
    }

    public void DisplayLoggedText()
        // will display everything in the action log, currently
    {
        string logAsText = string.Join("\n", actionLog.ToArray());
        //.Join brings the seperate strings together, then .ToArray makes the list an array, on a new line

        displayText.text = logAsText;
        //display to the screen the logged text
    }

    public void DisplayRoomText()
    {
        ClearCollectionsForNewRoom(); 

        UnpackRoom(); //calls unpack room

        string joinedInteractionDescriptions = string.Join("\n", interactionDescriptionsInRoom.ToArray()); // converting list of interactions into an array and joining it together in one string, with each item on a new line

        string combinedText = roomNavigation.currentRoom.description + "\n" + joinedInteractionDescriptions; //display the room description, new line, display interaction descriptions

        LogStringWithReturn(combinedText);
        //how to display room description, which calls the list to display on new line
    }

    void UnpackRoom() 
    {
        roomNavigation.UnpackExitsInRoom(); //call roomNav to unpack all the exits
        PrepareObjectsToTakeOrExamine(roomNavigation.currentRoom);
    }

    void PrepareObjectsToTakeOrExamine(Room currentRoom)
    {
        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Length; i++)
        {
            string descriptionNotInInventory = interactableItems.GetObjectsNotInInventory(currentRoom, i);
            if (descriptionNotInInventory != null)
            {
                interactionDescriptionsInRoom.Add(descriptionNotInInventory);
            }

            InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i];

            for (int j = 0; j < interactableInRoom.interactions.Length; j++)
            {
                Interaction interaction = interactableInRoom.interactions[j];
                if (interaction.inputAction.keyWord == "examine")
                {
                    interactableItems.examineDictionary.Add(interactableInRoom.noun, interaction.textResponse);
                }

                if (interaction.inputAction.keyWord == "take")
                {
                    interactableItems.takeDictionary.Add(interactableInRoom.noun, interaction.textResponse);
                }
            }
        }
    }

    public string TestVerbDictionaryWithNoun(Dictionary<string, string> verbDictionary, string verb, string noun)
    {
        if (verbDictionary.ContainsKey(noun))
        {
            return verbDictionary[noun];
        }

        return "You can't " + verb + " " + noun;
    }

    void ClearCollectionsForNewRoom() //will clear everything from the previous place, so the player doesnt encounter them again
    {
        interactableItems.ClearCollections();
        interactionDescriptionsInRoom.Clear();
        roomNavigation.ClearExits();
    }

    public void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
        //this will always add a new line between the list
    }
}