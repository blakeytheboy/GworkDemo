using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionResponse : ScriptableObject
{
    public string requiredString;
    //key to check against if something is possible
    //using this to check if you are in the right room to use an item (e.g. skull on podium)

    public abstract bool DoActionResponse(GameController controller);
    //passing reference to game controller

}