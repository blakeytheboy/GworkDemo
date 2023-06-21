using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/Room")]
//allows you to create asset instansces 
public class Room : ScriptableObject
//ScriptableObjects scripts, but not attached to game objects unlike mono behaviour
//can be used to create assets which store data or execute code
{
    [TextArea] //display as a bigger text entry box
    public string description;  //what the room will display when the player walks in
    public string roomName; //roomname, used to check where player is and item to be used
    public Exit[] exits; 
    public InteractableObject[] interactableObjectsInRoom;

}