using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private Dictionary<InputContext, IInputActionCollection2> inputDict = new();

    public void RegisterInputAction(InputContext context, IInputActionCollection2 collection)
    {
        if (!inputDict.ContainsKey(context))
        {
            inputDict.Add(context, collection);
        }
    }

    public IInputActionCollection2 GetInputAction(InputContext context)
    {
        if (!inputDict.ContainsKey(context))
            return null;

        return inputDict[context];
    }
}

public enum InputContext
{
    Player
}