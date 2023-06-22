using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/Interactable Object")]
//store data and references to other scriptable objects
public class InteractableObject : ScriptableObject
{
    public string noun = "name";
    //the name we refer to the object by e.g. skull
    [TextArea]
    public string description = "Description in room";
    //what it prints when it is avaible to be interacted with in the room
    public Interaction[] interactions;
    //array of interactions that display all the interactions
}