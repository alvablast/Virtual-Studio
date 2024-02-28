using UnityEngine;

public class SineWave : IWaveFunction
{
    public float WaveFunction(float phase)
    {
        return Mathf.Sin(phase);
    }
}
