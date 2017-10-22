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
        
	// Use this for initialization
	void Awake ()
    {
        CommandsList = new List<Command>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        bool rightPressed = Input.GetKey(TurnRightKey);
        bool leftPressed = Input.GetKey(TurnLeftKey);
        bool accPressed = Input.GetKey(AccelerateKey);
        bool breakPressed = Input.GetKey(BreakKey);

        if(rightPressed && !leftPressed)
        {
            CommandsList.Add(new Turn(TurnDirection.Right));
        }

        if(leftPressed && !rightPressed)
        {
            CommandsList.Add(new Turn(TurnDirection.Left));
        }

        if(accPressed && !breakPressed)
        {
            CommandsList.Add(new Accelerate());
        }

        if (!accPressed && breakPressed)
        {
            CommandsList.Add(new Break());
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
