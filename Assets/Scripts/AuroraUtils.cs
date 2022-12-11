using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class AuroraUtils
{
    public static void UpdateEmissionColor(float normalizedTimer, Material material, Color startColor, Color endColor)
    {
        material.SetColor("_EmissionColor", Color.Lerp(startColor, endColor, normalizedTimer));
    }

    public static ParticleSystem.MinMaxCurve UpdateParticleEffectCurve(ParticleSystem.MinMaxCurve curve, float minValue, float maxValue, float normalizedTimer)
    {
        curve.constant = Lerp(minValue, maxValue, normalizedTimer);
        return curve;
    }

    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }
}