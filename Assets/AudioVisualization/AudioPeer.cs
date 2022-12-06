using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(AudioSource))]

public class AudioPeer : MonoBehaviour
{
    public AudioSource audioSource;
    public float[] samples = new float[512];

    public float[] frequencyBand = new float[8];

    public float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];

    private float[] freqBandHighest = new float[8];
    public float[] audioBand = new float[8];
    public float[] audioBandBuffer = new float[8];

    public float amplitude, amplitudeBuffer;
    private float amplitudeHighest;

    public float audioProfileFloat;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        AudioProfile(audioProfileFloat);
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBand();
        BandBuffler();
        CreateAudioBands();

        GetAmplitude();
    }

    public void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            freqBandHighest[i] = audioProfile;
        }
    }

    public void CreateAudioBands()
    {
        for(int i = 0; i < 8; i++)
        {
            if(frequencyBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = frequencyBand[i];
            }

            audioBand[i] = (frequencyBand[i] / freqBandHighest[i]);
            audioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);
        }
    }

    public void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;

        for(int i =0; i < 8; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }

        if(currentAmplitude > amplitudeHighest)
        {
            amplitudeHighest = currentAmplitude;
        }

        amplitude = currentAmplitude /  amplitudeHighest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
    }

    public void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    public void BandBuffler()
    {
        for (int g = 0; g < 8; g++)
        {
            if (frequencyBand[g] > bandBuffer[g])
            {
            bandBuffer[g] = frequencyBand[g];
            bufferDecrease[g] = 0.005f;
            }
            if (frequencyBand[g] < bandBuffer[g])
            {
                bandBuffer[g] -= bufferDecrease[g];
                bufferDecrease[g] *= 1f;
            }
        }


    }

    public void MakeFrequencyBand()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            for(int j =0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;
            frequencyBand[i] = average * 10;
        }
    }
}
