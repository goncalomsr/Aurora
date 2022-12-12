using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aurora_Calm_State:AStateBehaviour
{
    [SerializeField] private InputGestureTracker gestureTracker;

    private ParticleSystem ps;

    [Header("Particles Behavior")]
    [SerializeField] private float strength;
    [SerializeField] private float frequency;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float positionAmount;

    [Space(10)]

    [Header("Particles Behavior")]
    [SerializeField] private Color newColor;
    [SerializeField] private float colorChangeDuration;
    [SerializeField] private float noiseChangeDuration;

    private float timer = 0.0f;
    private float noiseTimer = 0.0f;
    private Material mat;
    private Color startColor = Color.white;
    private float startStrength;
    private float startFrequency;
    private float startScrollSpeed;
    private float startPositionAmount;

    private float stateTimer = 0.0f;
    public override bool InitializeState()
    {
        ps = GetComponent<ParticleSystem>();
        mat = GetComponent<ParticleSystemRenderer>().material;
        return true;
    }

    public override void OnStateStart()
    {
        var noise = ps.noise;

        startColor = mat.GetColor("_EmissionColor");

        startStrength = noise.strength.constant;
        startFrequency = noise.frequency;
        startScrollSpeed = noise.scrollSpeed.constant;
        startPositionAmount = noise.positionAmount.constant;

        timer = 0.0f;
        noiseTimer = 0.0f;

        stateTimer = 0.0f;

        Debug.Log("Calm_State STARTED");

        gestureTracker.OnChillMovementDetectedEvent += OnChillMovement;
        gestureTracker.OnSpinningMovementDetectedEvent += OnSpiningMovement;
    }

    public override void OnStateUpdate()
    {
        timer = Mathf.Clamp01(timer + Time.deltaTime / colorChangeDuration);

        AuroraUtils.UpdateEmissionColor(timer, mat, startColor, newColor);

        noiseTimer = Mathf.Clamp01(noiseTimer + Time.deltaTime / noiseChangeDuration);

        var noise = ps.noise;
        noise.strength = AuroraUtils.UpdateParticleEffectCurve(ps.noise.strength, startStrength, strength, noiseTimer);
        noise.scrollSpeed = AuroraUtils.UpdateParticleEffectCurve(ps.noise.scrollSpeed, startScrollSpeed, scrollSpeed, noiseTimer);
        noise.positionAmount = AuroraUtils.UpdateParticleEffectCurve(ps.noise.positionAmount, startPositionAmount, positionAmount, noiseTimer);
        noise.frequency = AuroraUtils.Lerp(startFrequency, frequency, noiseTimer);


        if (noiseTimer == 1)
        {
            stateTimer += Time.deltaTime;
        }
    }

    public override void OnStateEnd()
    {
        gestureTracker.OnChillMovementDetectedEvent -= OnChillMovement;
        gestureTracker.OnSpinningMovementDetectedEvent -= OnSpiningMovement;

        stateTimer = 0.0f;
    }

    public override int StateTransitionCondition()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            return (int)EAuroraStates.Steady;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            return (int)EAuroraStates.Idle;
        }


        return (int)EAuroraStates.Invalid;
    }

    private void OnChillMovement()
    {
        if (stateTimer < 3f)
            return;

        AssociatedStateMachine.SetState((int)EAuroraStates.Idle);
    }

    private void OnSpiningMovement()
    {
        if (stateTimer < 3f)
            return;

        AssociatedStateMachine.SetState((int)EAuroraStates.Steady);
    }
}