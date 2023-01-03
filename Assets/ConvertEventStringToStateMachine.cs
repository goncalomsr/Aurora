using UnityEngine;

public class ConvertEventStringToStateMachine : MonoBehaviour
{
    private StateMachine stateMachine;

    private void Start()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    public void OnComboFound(string comboName)
    {
        Debug.Log(comboName);
        
        if (comboName.CompareTo("Test Combiantion 1") == 0)
        {
            stateMachine.SetState( (int)EAuroraStates.Calm );
        }

        if (comboName.CompareTo("Test Combination 2") == 0)
        {
            stateMachine.SetState((int)EAuroraStates.Chaotic);
        }

        if (comboName.CompareTo("Test Combination 3") == 0)
        {
            stateMachine.SetState((int)EAuroraStates.Steady);
        }
    }
}
