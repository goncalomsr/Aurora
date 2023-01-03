using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnMovementComboFoundEvent : UnityEvent<string>
{
}

// Each One Has A 45 Degree Cone
public enum EDirections
{
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
}

[System.Serializable]
public class DirectionComboCombination
{
    public string Name;
    public List<EDirections> comboDirections;

    // Find An Exact Match, No Possible Inbetween matches
    public bool HasCompleteCombination(List<EDirections> directions)
    {
        if (comboDirections.Count != directions.Count)
        {
            return false;
        }

        for (int i = 0; i < comboDirections.Count; ++i)
        {
            if (comboDirections[i] != directions[i])
            {
                return false;
            }
        }

        return true;
    }
}

public class ComboMovementFinder : MonoBehaviour
{
    [Header("Config Mouse Sensitivity")]
    [SerializeField] private float magnitudeThreshold;
    
    [Header("Config Idle Timer")]
    [SerializeField] private float maxIdleTimeBeforeReset = 1.0f;

    [Header("Config Idle Timer")]
    [SerializeField] private float onFindCombinationGracePeriod = 0.2f;

    [Header("Config Max List Size")]
    [SerializeField] private int maxCachedDirectionListSize = 5;

    [Header("Config Whether To 4DoF or 8DoF")] 
    [SerializeField] private bool useCardinalDirectionsOnly;
    
    [Header("Possible Action Combinations List")]
    [SerializeField] private List<DirectionComboCombination> possibleActionList = new List<DirectionComboCombination>();

    public OnMovementComboFoundEvent OnComboFoundEvent = new OnMovementComboFoundEvent() ;
    
    private float onFindCombinationGraceTimer = 0.0f;

    private List<EDirections> cachedComboDirections = new List<EDirections>();

    private Vector3 initialPosition;
    private Vector3 nextPosition;

    private float timer = 0.0f;
    
    private void Start()
    {
        initialPosition = Input.mousePosition;
        timer = maxIdleTimeBeforeReset;
    }

    void Update()
    {
        onFindCombinationGraceTimer -= Time.deltaTime;
        if (onFindCombinationGraceTimer > 0)
        {
            initialPosition = Input.mousePosition;
            return;
        }

        // Start Tracking A Position When We Start Moving
        nextPosition = Input.mousePosition;

        if (Vector3.Distance(initialPosition, nextPosition) > magnitudeThreshold)
        {
            // We Have A Movement That Exceeds The Threshold
            Vector3 direction = nextPosition - initialPosition;
            float upDotProduct = Vector3.Dot(Vector3.up, direction.normalized);
            float rightDotProduct = Vector3.Dot(Vector3.right, direction.normalized);

            EDirections currentDirection = FindDirectionFromDotProducts(upDotProduct, rightDotProduct);
            if ( (cachedComboDirections.Count == 0) || currentDirection != cachedComboDirections[cachedComboDirections.Count - 1])
            {
                cachedComboDirections.Add(currentDirection);
            }
            
            // We have a bare minimum to chekc against! :D
            DirectionComboCombination foundCombo = FindCombo();
            if (foundCombo != null)
            {
                cachedComboDirections.Clear();
                
                onFindCombinationGraceTimer = onFindCombinationGracePeriod;
                
                OnComboFoundEvent.Invoke(foundCombo.Name);
            }

            if (cachedComboDirections.Count >= maxCachedDirectionListSize)
            {
                cachedComboDirections.Clear();
            }

            initialPosition = nextPosition;

            // Reset The Combo Reset Timer
            timer = maxIdleTimeBeforeReset;
            
            return;
        }
        
        // Reset Timer
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            timer = maxIdleTimeBeforeReset;

            cachedComboDirections.Clear();
        }
    }

    private EDirections FindDirectionFromDotProducts(float upDotProduct, float rightDotProduct)
    {
        if (useCardinalDirectionsOnly)
            return Get4DoFDirectionFromDotProducts(upDotProduct, rightDotProduct);
        return Get8DoFDirectionFromDotProducts(upDotProduct, rightDotProduct);
    }

    private EDirections Get4DoFDirectionFromDotProducts(float upDotProduct, float rightDotProduct)
    {
        // Convert To Eulers As It's Easier To Deal With
        float upDotAsEuler = Mathf.Acos(upDotProduct) * Mathf.Rad2Deg;
        float rightDotAsEuler = Mathf.Acos(rightDotProduct) * Mathf.Rad2Deg;

        // Cardinal Direction Checks First
        if (upDotAsEuler < 45.0f)
        {
            return EDirections.North;
        }

        if (upDotAsEuler > 135.0f)
        {
            return EDirections.South;
        }

        if (rightDotAsEuler < 45.0f)
        {
            return EDirections.East;
        }

        // Clearly Has To Be West
        //if (rightDotAsEuler > 135.0f)
        //{
            return EDirections.West;
        //}
    }

    
    private EDirections Get8DoFDirectionFromDotProducts(float upDotProduct, float rightDotProduct)
    {
        // Convert To Eulers As It's Easier To Deal With
        float upDotAsEuler = Mathf.Acos(upDotProduct) * Mathf.Rad2Deg;
        float rightDotAsEuler = Mathf.Acos(rightDotProduct) * Mathf.Rad2Deg;

        // Cardinal Direction Checks First
        if (upDotAsEuler < 22.5f)
        {
            return EDirections.North;
        }

        if (upDotAsEuler > 157.5f)
        {
            return EDirections.South;
        }

        if (rightDotAsEuler < 22.5f)
        {
            return EDirections.East;
        }

        if (rightDotAsEuler > 157.5f)
        {
            return EDirections.West;
        }
        
        // Intercardinal Directions
        if (upDotAsEuler >= 22.5f && upDotAsEuler <= 67.5f && rightDotAsEuler <= 67.5)
        {
            return EDirections.NorthEast;
        }

        if (upDotAsEuler >= 22.5f && upDotAsEuler <= 67.5f && rightDotAsEuler >= 67.5)
        {
            return EDirections.NorthWest;
        }
        
        if (upDotAsEuler <= 157.5f && upDotAsEuler >= 112.5f && rightDotAsEuler <= 67.5)
        {
            return EDirections.SouthEast;
        }
    
        // Technically This Is The Only Other Available Path So No Need To Check
        //if (upDotAsEuler <= 157.5f && upDotAsEuler >= 112.5f && rightDotProduct >= 157.5f)
        //{
            return EDirections.SouthWest;
        //}
    }

    // We Find Full Combos, Nothing Like Finding N S in a W E N S W list
    private DirectionComboCombination FindCombo()
    {
        for (int i = 0; i < possibleActionList.Count; ++i)
        {
            if (possibleActionList[i].HasCompleteCombination(cachedComboDirections))
            {
                return possibleActionList[i];
            }
        }

        return null;
    }
}
