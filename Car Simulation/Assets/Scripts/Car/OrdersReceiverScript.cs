using Assets.Scripts.AI;
using EngPlayerCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersReceiverScript : MonoBehaviour {

    [SerializeField]
    private InputSource inputSource;

    public Rigidbody CarBody;
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float breakTorque;
    public float maxSteeringAngle;
    public float turnDegreePerSec;
    public float steerResetDelay;
    public float maxSpeed;

	public bool log;
    
    private float turnDegree;

    private bool InProgress;
    private bool Human;

    public void GameEnded(int score)
    {
        InProgress = false;

        foreach(AxleInfo axle in axleInfos)
        {
            axle.rightWheel.motorTorque = 0;
            axle.rightWheel.brakeTorque = Mathf.Infinity;
            axle.rightWheel.steerAngle = 0;

            axle.leftWheel.motorTorque = 0;
            axle.leftWheel.brakeTorque = Mathf.Infinity;
            axle.leftWheel.steerAngle = 0;            

            axle.rightWheelTransform.localRotation = Quaternion.Euler(0, 0, 90);
            axle.leftWheelTransform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        
        CarBody.velocity = Vector3.zero;
        CarBody.angularVelocity = Vector3.zero;        
        turnDegree = 0;
        
        if (inputSource != null)
        {
            Command dump;

            while (!inputSource.ListEmpty)
            {
                dump = inputSource.FirstCommand;
            }

            if(inputSource is AIInputSource)
            {
                (inputSource as AIInputSource).DeativateSensors();
            }
        }
    }

    public void GameStarted()
    {
        InProgress = true;

        if (inputSource is AIInputSource)
        {
            (inputSource as AIInputSource).ActivateSensors();
        }
    }

    public void SetInputSource(InputSource input)
    {
        inputSource = input;
    }

    void Awake()
    {
        InProgress = false;
        GameplayScript temp = gameObject.GetComponent<GameplayScript>();

        temp.OnGameEnded += GameEnded;
        temp.OnGameStarted += GameStarted;
    }

	void FixedUpdate ()
    {        
        if (InProgress && inputSource != null && !inputSource.ListEmpty)
        {                   
            do
            {
				var command = inputSource.FirstCommand;
				if (log) {
					Debug.Log("command: " +  command);
				}
				AssertCommand(command);
            }
            while (!inputSource.ListEmpty) ;
        }       
        
        foreach (AxleInfo axle in axleInfos)
        {
            if (axle.steering)
            {
                UpdateSteeringWheels(axle.rightWheel, axle.rightWheelTransform);
                UpdateSteeringWheels(axle.leftWheel, axle.leftWheelTransform);
            }
        }

        if (CarBody.velocity.magnitude * 3.6f > maxSpeed)
        {
            CarBody.velocity = CarBody.velocity.normalized * maxSpeed / 3.6f;
        }

		if (log) {
			Debug.Log ("velocity: " + CarBody.velocity);
		}
    }

    private void AssertCommand(Command com)
    {
        SimplifiedCommand temp = com as SimplifiedCommand;

        switch(temp.ComType)
        {
            case CommandType.Accelerate:
                InterpretAccelerate(temp.Value);
                break;

            case CommandType.Break:
                InterpretBreak(temp.Value);
                break;

            case CommandType.TurnLeft:
                InterpretTurn(-temp.Value);
                break;

            case CommandType.TurnRight:
                InterpretTurn(temp.Value);
                break;
        }
    }

    private void InterpretAccelerate(float val, Accelerate acc = null)
    {
        val = Mathf.Lerp(0.5f, 1, val);

        foreach(AxleInfo axle in axleInfos)
        {
            if(axle.motor)
            {
                axle.rightWheel.motorTorque = val * maxMotorTorque;
                axle.leftWheel.motorTorque = val * maxMotorTorque;
            }

            axle.rightWheel.brakeTorque = 0;
            axle.leftWheel.brakeTorque = 0;
        }
    }

    private void InterpretBreak(float val, Break br = null)
    {
        foreach (AxleInfo axle in axleInfos)
        {
            axle.rightWheel.brakeTorque = val * breakTorque;
            axle.leftWheel.brakeTorque = val * breakTorque;

            axle.rightWheel.motorTorque = 0;
            axle.leftWheel.motorTorque = 0;
        }
    }

    private void InterpretTurn(float turn)
    {
        turnDegree = maxSteeringAngle * turn;       
    }

    private void UpdateSteeringWheels(WheelCollider wheel, Transform wheelTransform)
    {
        wheel.steerAngle = -turnDegree;
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public Transform leftWheelTransform;
    public Transform rightWheelTransform;
    public bool motor;
    public bool steering;
}
