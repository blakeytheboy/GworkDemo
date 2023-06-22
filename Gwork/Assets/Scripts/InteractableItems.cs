using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItems : MonoBehaviour
{
    public List<InteractableObject> usableItemList;

    public Dictionary<string, string> examineDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> takeDictionary = new Dictionary<string, string>();

    [HideInInspector] public List<string> nounsInRoom = new List<string>();
    //name of all the objects in the room. hidden so we dont access in inspect, but public so others scripts can access

    Dictionary<string, ActionResponse> useDictionary = new Dictionary<string, ActionResponse>();
    List<string> nounsInInventory = new List<string>(); //move items in and out of these lists as they change status
    GameController controller;

    void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public string GetObjectsNotInInventory(Room currentRoom, int i)
    {
        InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i];

        if (!nounsInInventory.Contains(interactableInRoom.noun))
            //look at this list and look if it is or isnt in the list
            //if NOT in the list... it will be added to \/
        {
            nounsInRoom.Add(interactableInRoom.noun);
            return interactableInRoom.description;
            //return the description of the interactable because its NOT in the inventory
        }

        return null;
    }

    public void AddActionResponsesToUseDictionary() //called when you take an item
    {
        for (int i = 0; i < nounsInInventory.Count; i++)
        {
            string noun = nounsInInventory[i];

            InteractableObject interactableObjectInInventory = GetInteractableObjectFromUsableList(noun);
            if (interactableObjectInInventory == null)
                continue; //break out of the loop and continue

            for (int j = 0; j < interactableObjectInInventory.interactions.Length; j++)
            {
                Interaction interaction = interactableObjectInInventory.interactions[j];

                if (interaction.actionResponse == null)
                    continue; //break out of the loop and continue

                if (!useDictionary.ContainsKey(noun)) //if we dont have this noun, then...
                {
                    useDictionary.Add(noun, interaction.actionResponse);
                    //allows us to pass in a noun, and get back an action response
                    //to check if they have nouns that are usable, and putting them in the useDictionary
                }
            }

        }
    }

    InteractableObject GetInteractableObjectFromUsableList(string noun)
    {
        for (int i = 0; i < usableItemList.Count; i++)
        {
            if (usableItemList[i].noun == noun)
                //if it is the same as the noun we are looking for...
            {
                return usableItemList[i];
            }
        }
        return null;
    }

    public void DisplayInventory()
    {
        controller.LogStringWithReturn("You look inside your bag, you have: ");

        for (int i = 0; i < nounsInInventory.Count; i++)
        {
            controller.LogStringWithReturn(nounsInInventory[i]);
            //list everything in the inventory (with prefix) 
        }
    }

    public void ClearCollections()
        //clear out collections every room change - so it can get ready to display next room
    {
        examineDictionary.Clear();
        takeDictionary.Clear();
        nounsInRoom.Clear();
    }

    public Dictionary<string, string> Take(string[] separatedInputWords)
    {
        string noun = separatedInputWords[1];
        //checks for the second word in the string

        if (nounsInRoom.Contains(noun))
        {
            nounsInInventory.Add(noun);
            AddActionResponsesToUseDictionary();
            nounsInRoom.Remove(noun);
            return takeDictionary;
            //took item from list in the room, added it to the inventory 
        }
        else
        {
            controller.LogStringWithReturn("There is no " + noun + " here to take.");
            return null;

            //else no noun in room, then it returns empty and displays the message
        }
    }

    public void UseItem(string[] separatedInputWords)
    {
        string nounToUse = separatedInputWords[1];

        if (nounsInInventory.Contains(nounToUse))
        {
            if (useDictionary.ContainsKey(nounToUse))
                //needs to be in our inventory and us dictionary
            {
                bool actionResult = useDictionary[nounToUse].DoActionResponse(controller); //passing in the noun we want to use
                if (!actionResult)
                {
                    controller.LogStringWithReturn("Hmm. Nothing happens."); //if the action failed. right idea, wrong place
                }
            }
            else
            {
                controller.LogStringWithReturn("You can't use the " + nounToUse); //cant use it at all
            }
        }
        else
        {
            controller.LogStringWithReturn("There is no " + nounToUse + " in your inventory to use. Make sure you try: go, take, examine, or use"); 
            
            //dont have it at all, regardless if it might work or not
        }
    }

}
