using System;
using System.Collections.Generic;

using UnityEngine;

/* Example
public class SomeCoolState : MonoBehaviour
{
    [SerializeField] private InputGestureTracker gestureTracker;

    void OnStartState()
    {
        gestureTracker.OnErraticMovementDetectedEvent += OnErraticMovement;

    }

    void OnENdsState()
    {
        gestureTracker.OnErraticMovementDetectedEvent -= OnErraticMovement;
    }

    private void OnErraticMovement()
    {
        // Chekc if you are the active state first!
        // AssociatedStateMachine.GetCurrentState == this;
        // AssociatedStateMachine.ChangeState(Chaotic);
    }

}
*/

public class InputGestureTracker : MonoBehaviour
{
    public enum TrackableGestures
    {
        Inactive,
        Erratic,
        Spinning
    }

    // Chill Movement
    public delegate void OnChillMovementDetectedDelegate();
    public event OnChillMovementDetectedDelegate OnChillMovementDetectedEvent;

    // Slow Movement
    public delegate void OnSlowMovementDetectedDelegate();
    public event OnSlowMovementDetectedDelegate OnSlowMovementDetectedEvent;

    // Erratic Movement
    public delegate void OnErraticMovementDetectedDelegate();
    public event OnErraticMovementDetectedDelegate OnErraticMovementDetectedEvent;

    // Spinning Movement
    public delegate void OnSpinningMovementDetectedDelegate();
    public event OnSpinningMovementDetectedDelegate OnSpinningMovementDetectedEvent;

    // Sampling
    private float timer = 0.0f;
    private static float sampleSize = 1.0f; // Time Per Samples And Checking
    private List<Vector2> MousePositionSamples = new List<Vector2>();

    private void Update()
    {
        MousePositionSamples.Add(Input.mousePosition);

        timer += Time.deltaTime;
        if (timer >= sampleSize)
        {
            ProcessPossibleGesturesByPriority();

            timer = 0;
            MousePositionSamples.Clear();

            return;
        }
    }

    private void ProcessPossibleGesturesByPriority()
    {
        if (IsSampleAValidCircularGesture())
        {
            Debug.Log("Circular Gesture Found!");
            OnSpinningMovementDetectedEvent?.Invoke();

            return;
        }

        if (IsSampleAValidErraticGesture())
        {
            Debug.Log("Sporadic Gesture Found!");
            OnErraticMovementDetectedEvent?.Invoke();

            return;
        }

        if (IsSampleAValidSlowMovementGesture())
        {
            Debug.Log("Slow Movement Gesture Found!");
            OnSlowMovementDetectedEvent?.Invoke();

            return;
        }

        if (IsSampleAValidChillGesture())
        {
            Debug.Log("No Gesture Found!");
            OnChillMovementDetectedEvent?.Invoke();

            return;
        }
    }

    private bool IsSampleAValidCircularGesture()
    {
        // How Fast And How Many Inversions Of The Partial Or Complete Sign?
        const float DotMinTolerance = 0.6f;
        const float DotMaxTolerance = 1.0f;
        const int sampleTolerance = 30;

        List<Vector2> directions = GetDirectionsFromPoints( GetNonDuplicatedVectors(MousePositionSamples) );
        if (directions.Count < sampleTolerance)
        {
            return false;
        }

        for (int i = 0; i < directions.Count - 1; ++i)
        {
            Vector2 currentDirection = directions[i];
            Vector2 nextDirection = directions[i+1];

            float dot = Vector2.Dot(currentDirection.normalized, nextDirection.normalized);
            if (dot < DotMinTolerance || dot > DotMaxTolerance)
                return false;
        }

        return true;
    }

    private bool IsSampleAValidErraticGesture()
    {
        // How Fast And How Many Inversions Of The Partial Or Complete Sign?
        const int MovementThresholdSqr = 100;
        const int InversionThreshold = 6;

        List<Vector2> directions = GetDirectionsFromPoints(GetNonDuplicatedVectors(MousePositionSamples));

        // As We Compare The Start To The Next Sample We Need To Make Sure We Dont Hit An IndexOutOfRangeException!
        // Also The First Sample Will Dictate Whether We're Moving A Specific Direction Continuously Or Not
        int SignInversionCounts = 0;
        for (int i = 0; i < directions.Count - 1; ++i)
        {
            Vector2 CurrentDirection = directions[i];
            Vector2 NextDirection = directions[i+1];

            int CurrentDirectionX = Math.Sign(CurrentDirection.x);
            int CurrentDirectionY = Math.Sign(CurrentDirection.y);

            int NextDirectionX = Math.Sign(NextDirection.x);
            int NextDirectionY = Math.Sign(NextDirection.y);

            // When Multiplying 1 means they are facing the same direction, 0 means one is stopped and -1 they are facing opposite ways
            if (CurrentDirectionX * NextDirectionX < 0)
            {
                ++SignInversionCounts;
            }

            // All other samples must be facing the same way!
            if (CurrentDirectionY * NextDirectionY < 0)
            {
                ++SignInversionCounts;
            }
        }

        return SignInversionCounts > InversionThreshold;
    }

    private bool IsSampleAValidSlowMovementGesture()
    {
        const int MovementThresholdSqr = 400;

        List<Vector2> directions = GetDirectionsFromPoints(GetNonDuplicatedVectors(MousePositionSamples));
        if (directions.Count == 0)
            return false;

        // As We Compare The Start To The Next Sample We Need To Make Sure We Dont Hit An IndexOutOfRangeException!
        // Also The First Sample Will Dictate Whether We're Moving A Specific Direction Continuously Or Not
        int SignInversionCounts = 0;
        for (int i = 0; i < directions.Count - 1; ++i)
        {
            if (directions[i].sqrMagnitude > MovementThresholdSqr)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsSampleAValidChillGesture()
    {
        return true;
    }

    // None are zero as a mouse but a accelerometer we might have 0's
    private List<Vector2> GetNonDuplicatedVectors(List<Vector2> points)
    {
        List<Vector2> cleanedList = new List<Vector2>();
        for (int i = 0; i < points.Count - 1; ++i)
        {
            if (points[i] != Vector2.zero && points[i] != points[i + 1])
            {
                cleanedList.Add(points[i]);
            }
        }
        return cleanedList;
    }

    private List<Vector2> GetDirectionsFromPoints(List<Vector2> points)
    {
        List<Vector2> directions = new List<Vector2>();
        for (int i = 0; i < points.Count - 1; ++i)
        {
            directions.Add(points[i+1] - points[i]);
        }
        return directions;
    }
}

/*

// Horizontal
public delegate void OnHorizontalMovementDetectedDelegate();
public event OnHorizontalMovementDetectedDelegate OnHorizontalMovementDetectedEvent;

// Vertical
public delegate void OnVerticalMovementDetectedDelegate();
public event OnVerticalMovementDetectedDelegate OnVerticalMovementDetectedEvent;

// Circular Gesture
public delegate void OnCircularGestureDetectedDelegate();
public event OnCircularGestureDetectedDelegate OnCircularGestureDetectedEvent;

if (IsSampleAValidHorizontalGesture())
{
    Debug.Log("Horizontal Gesture Found!");
    OnHorizontalMovementDetectedEvent?.Invoke();
}

if (IsSampleAValidVerticalGesture())
{
    Debug.Log("Vertical Gesture Found!");
    OnVerticalMovementDetectedEvent?.Invoke();
}


private bool IsSampleAValidHorizontalGesture()
{
    const int MovementThreshold = 10;

    int firstSampleFound = -1;
    Vector2 InitialDirection = Vector2.zero;

    for (int i = 0; i < MousePositionSamples.Count - 1; ++i)
    {
        InitialDirection.x = (MousePositionSamples[i + 1] - MousePositionSamples[i]).x;

        if (Mathf.Abs(InitialDirection.x) > MovementThreshold)
        {
            firstSampleFound = i;
            //Debug.Log($"FoundSameple From: {MousePositionSamples[i + 1]}-{MousePositionSamples[i]}={MousePositionSamples[i + 1] - MousePositionSamples[i]}");
            break;
        }
    }

    // If We Found No Samples Or Samples Are At The End Of The Sample Range ... Well Disregard This Gesture This Time Around
    if (firstSampleFound < 0 || firstSampleFound >= MousePositionSamples.Count - 1)
    {
        return false;
    }

    // As We Compare The Start To The Next Sample We Need To Make Sure We Dont Hit An IndexOutOfRangeException!
    // Also The First Sample Will Dictate Whether We're Moving A Specific Direction Continuously Or Not
    for (int i = firstSampleFound; i < MousePositionSamples.Count - 1; ++i)
    {
        // All other samples must be facing the same way!
        float currentX = MousePositionSamples[i].x;
        float nextX = MousePositionSamples[i + 1].x;

        float direction = nextX - currentX;

        int InitialDirectionSign = (int)Math.Sign(InitialDirection.x);
        int CurrentMouseDirectionSign = (int)Math.Sign(direction);

        if (InitialDirectionSign != CurrentMouseDirectionSign && CurrentMouseDirectionSign != 0)
        {
            return false;
        }
    }

    return true;
}

private bool IsSampleAValidVerticalGesture()
{
    const int MovementThreshold = 10;

    int firstSampleFound = -1;
    Vector2 InitialDirection = Vector2.zero;

    for (int i = 0; i < MousePositionSamples.Count - 1; ++i)
    {
        InitialDirection.y = (MousePositionSamples[i + 1] - MousePositionSamples[i]).y;

        if (Mathf.Abs(InitialDirection.y) > MovementThreshold)
        {
            firstSampleFound = i;
            break;
        }
    }

    // If We Found No Samples Or Samples Are At The End Of The Sample Range ... Well Disregard This Gesture This Time Around
    if (firstSampleFound < 0 || firstSampleFound >= MousePositionSamples.Count - 1)
    {
        return false;
    }

    // As We Compare The Start To The Next Sample We Need To Make Sure We Dont Hit An IndexOutOfRangeException!
    // Also The First Sample Will Dictate Whether We're Moving A Specific Direction Continuously Or Not
    for (int i = firstSampleFound; i < MousePositionSamples.Count - 1; ++i)
    {
        // All other samples must be facing the same way!
        float currentX = MousePositionSamples[i].y;
        float nextX = MousePositionSamples[i + 1].y;

        float direction = nextX - currentX;

        int InitialDirectionSign = (int)Math.Sign(InitialDirection.y);
        int CurrentMouseDirectionSign = (int)Math.Sign(direction);

        if (InitialDirectionSign != CurrentMouseDirectionSign && CurrentMouseDirectionSign != 0)
        {
            return false;
        }
    }

    return true;
}

*/
