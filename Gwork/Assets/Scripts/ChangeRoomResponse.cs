using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/ActionResponses/ChangeRoom")]
public class ChangeRoomResponse : ActionResponse
{
    public Room roomToChangeTo;

    public override bool DoActionResponse(GameController controller)
    {
        if (controller.roomNavigation.currentRoom.roomName == requiredString)
            //checking room name
        {
            controller.roomNavigation.currentRoom = roomToChangeTo;
            //this will change he room where the 'secret door is open'
            controller.DisplayRoomText();
            return true;
            //will load an entire new room, if the right item is used in the intended room
        }

        return false;
        //if wrong place, it wont work
    }
}