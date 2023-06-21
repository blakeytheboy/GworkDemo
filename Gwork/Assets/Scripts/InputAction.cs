using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputAction : ScriptableObject
    //abstract modifier in a class declaration to indicate that a class is intended only to be a base class of other classes
    //marked or included in an abstract class, must be implimented by classes that derive from the abstract class
{
    public string keyWord;

    public abstract void RespondToInput(GameController controller, string[] separatedInputWords);
    //will create an array of input words, passed in from the input script
}