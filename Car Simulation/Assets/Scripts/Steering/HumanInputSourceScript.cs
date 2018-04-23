using Assets.Scripts.AI;
using EngPlayerCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInputSourceScript : InputSource {

    [SerializeField]
    private KeyCode TurnLeftKey;

    [SerializeField]
    private KeyCode TurnRightKey;

    [SerializeField]
    private KeyCode AccelerateKey;

    [SerializeField]
    private KeyCode BreakKey;
        
	void Awake ()
    {
        CommandsList = new List<Command>();	
	}
	
	void Update ()
    {
        bool rightPressed = Input.GetKey(TurnRightKey);
        bool leftPressed = Input.GetKey(TurnLeftKey);
        bool accPressed = Input.GetKey(AccelerateKey);
        bool breakPressed = Input.GetKey(BreakKey);

        if(rightPressed && !leftPressed)
        {
            CommandsList.Add(new SimplifiedCommand(CommandType.TurnRight));
        }

        if(leftPressed && !rightPressed)
        {
            CommandsList.Add(new SimplifiedCommand(CommandType.TurnLeft));
        }

        if(accPressed && !breakPressed)
        {
            CommandsList.Add(new SimplifiedCommand(CommandType.Accelerate));
        }

        if (!accPressed && breakPressed)
        {
            CommandsList.Add(new SimplifiedCommand(CommandType.Break));
        }

    }
}

public abstract class InputSource : MonoBehaviour
{
    protected List<Command> CommandsList;

    public Command FirstCommand
    {
        get
        {
            Command result = CommandsList[0];
            CommandsList.RemoveAt(0);

            return result;
        }
    }

    public bool ListEmpty { get { return CommandsList.Count == 0; } }
}
