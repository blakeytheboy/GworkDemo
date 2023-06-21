using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNavigation : MonoBehaviour
{

    public Room currentRoom; 
    //record what room we are in

    Dictionary<string, Room> exitDictionary = new Dictionary<string, Room>();
    //dictionary is composed of a collection (key, value pairs) so that each possible key only appears once in the collection
    //a dictonary allows us to specifcy a type for both the and and value
    //Dictionary<string, GameObject> myDictionary;
    GameController controller;

    void Awake()
    {
        controller = GetComponent<GameController>(); //find game controller connected to same game object and store reference in variable
    }

    public void UnpackExitsInRoom()
    {
        for (int i = 0; i < currentRoom.exits.Length; i++) //interger to display all the exits in the current room
        {
            exitDictionary.Add(currentRoom.exits[i].keyString, currentRoom.exits[i].valueRoom);
            //key string is north, value is room
            controller.interactionDescriptionsInRoom.Add(currentRoom.exits[i].exitDescription); //add all exits to the interactions of the current room
            //just entered a room, unpack our exits, send a list of descriptions and prepare to show them on screen
        }
    }

    public void AttemptToChangeRooms(string directionNoun) //directionNoun is NESW
    {
        if (exitDictionary.ContainsKey(directionNoun)) //if exit contains NESW
        {
            currentRoom = exitDictionary[directionNoun]; //okay have the key? set the current room to that room
            controller.LogStringWithReturn("You go to the " + directionNoun);
            controller.DisplayRoomText();
        }
        else //if the key is not there (no room)
        {
            controller.LogStringWithReturn("There is no path to the " + directionNoun); //then it wont let you go somewhere without a room set that way
        }

    }

    public void ClearExits()
    {
        exitDictionary.Clear(); //clears everything unpacked, ready for the new room the player enters so these options dont persist
    }
}
