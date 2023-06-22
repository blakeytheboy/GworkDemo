using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interaction
{
    public InputAction inputAction;
    [TextArea]
    public string textResponse;
    public ActionResponse actionResponse; //
    //if the object gives a text response when you examine it /\ will be the text it gives back
}

//all this does is hold data for us 