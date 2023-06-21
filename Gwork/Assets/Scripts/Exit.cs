using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// will allow you to see the public varriables in the inspector
public class Exit
{
    public string keyString; //what the player must input to go to the room
    public string exitDescription; //exit description (where it is in current room)
    public Room valueRoom; //used to set it in a dictonary 
}