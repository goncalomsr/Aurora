using UnityEngine;

// Made this to act like a component for easy config
// This has to be abstract to ensure we force the functions we need for the state machine.
public abstract class AStateBehaviour : MonoBehaviour
{
    public StateMachine AssociatedStateMachine { get; set; }
    public abstract bool InitializeState();
    public abstract void OnStateStart();
    public abstract void OnStateUpdate();
    public abstract void OnStateEnd();
    public abstract int StateTransitionCondition();
}
