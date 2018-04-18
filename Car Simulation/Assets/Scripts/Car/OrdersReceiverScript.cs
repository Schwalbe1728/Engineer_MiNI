using Assets.Scripts.AI;
using EngPlayerCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersReceiverScript : MonoBehaviour {

    [SerializeField]
    private InputSource inputSource;

    public Rigidbody CarBody;
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    //public float torquePerSecond;
    public float breakTorque;
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float turnDegreePerSec;
    public float steerResetDelay;
    public float maxSpeed;

	public bool log;

    //private float velocity;
    private float turnDegree;
    //private float torque;

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
        //torque = 0;

        if (inputSource != null)
        {
            Command dump;

            while (!inputSource.ListEmpty)
            {
                dump = inputSource.FirstCommand;
            }

            if(inputSource is AIInputSource)
            {
                //Debug.LogWarning("OrdersReceiver Deactivate");
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
        GameplayScript temp = gameObject.GetComponent<GameplayScript>(); //GameObject.Find("Gameplay Manager").GetComponent<GameplayScript>();

        temp.OnGameEnded += GameEnded;
        temp.OnGameStarted += GameStarted;
    }

	// Update is called once per frame
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
            /*
            if (axle.motor)
            {
                axle.rightWheel.motorTorque = torque;
                axle.leftWheel.motorTorque = torque;
            }

            axle.rightWheel.brakeTorque = breakTorque; //maxMotorTorque;
            axle.leftWheel.brakeTorque = breakTorque; //maxMotorTorque; 
            */
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
        /*
        if(com is Accelerate)
        {
            InterpretAccelerate(com as Accelerate);
        }
        else

        if(com is Break)
        {
            InterpretBreak(com as Break);
        }
        else

        if(com is Turn)
        {
            InterpretTurn(com as Turn);
        }
        */

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
        //Debug.Log("Accelerate");

        //torque = Mathf.Clamp(torque + torquePerSecond * Time.fixedDeltaTime, 0, maxMotorTorque);

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
        //torque = Mathf.Clamp(torque - breakTorque, 0, maxMotorTorque);

        foreach (AxleInfo axle in axleInfos)
        {
            axle.rightWheel.brakeTorque = val * breakTorque;
            axle.leftWheel.brakeTorque = val * breakTorque;

            axle.rightWheel.motorTorque = 0;
            axle.leftWheel.motorTorque = 0;
        }
    }

    private void InterpretTurn(/*Turn turn */ float turn)
    {
        /*
        turnDegree =
            Mathf.Clamp(
                turnDegree - turn * turnDegreePerSec * Time.fixedDeltaTime,
                -maxSteeringAngle, 
                maxSteeringAngle
            );
        */

        turnDegree = maxSteeringAngle * turn;

        /*
        foreach (AxleInfo axle in axleInfos)
        {
            if (axle.steering)
            {
                UpdateSteeringWheels(axle.rightWheel, axle.rightWheelTransform);
                UpdateSteeringWheels(axle.leftWheel, axle.leftWheelTransform);
            }
        }
        */
    }

    private void UpdateSteeringWheels(WheelCollider wheel, Transform wheelTransform)
    {
        wheel.steerAngle = -turnDegree;
        //wheelTransform.localRotation = Quaternion.Euler(0, -turnDegree, 90);
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public Transform leftWheelTransform;
    public Transform rightWheelTransform;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}
