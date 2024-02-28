using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code written by Benjamin Griffin
// based on code from the following tutorials
// https://www.youtube.com/watch?v=GqHFGMy_51c <- audioFilterRead information
// https://www.youtube.com/watch?v=OSCzKOqtgcA <- envelope information
public class SynthTemplate : MonoBehaviour
{
    public GameObject self;
    private readonly string[] _keyboard = new string[] {"z","s","x","d","c","v","g","b","h","n","j","m",",", };
    private readonly float[] _frequencies = new float[] {
        130.81f,//c3
        138.59f,
        146.83f,
        155.56f,
        164.81f,
        174.61f,
        185f,
        196f,
        207.65f,
        220f,
        233.08f,
        246.94f,
        261.63f,//c4
        
        277.18f,
        293.66f,
        311.13f,
        329.63f,
        349.23f,
        369.99f,
        392f,
        415.30f,
        440f,
        466.16f,
        493.88f,
        523.25f,//c5
        
    };

    private float _timer;
    public struct SynthData
    {
        public float Attack; // Attack time - time for Attack to finish
        public float Decay; // Decay time - time for the Decay to finish
        public float Release; //Release time - time for Release to finish
        
        public float Sustain; // Sustain amplitude
        public float Start; // Start amplitude

        public float TriggerOn; // time when note triggered
        public float TriggerOff; // time when note no longer triggered

        public bool IsKeyDown;

        public void Init()
        {
            Attack = .01f;
            Decay = .5f;
            Release = 1f;

            Sustain = .5f;
            Start = .80f;

            TriggerOn = 0.0f;
            TriggerOff = 0.0f;
            
            IsKeyDown = false;
        }

        public float GetAmplitude(float time)
        {
            float amplitude = 0.0f;
            float noteTime = time - TriggerOn;
            if (IsKeyDown)
            {
                //Attack - time between 0 and end of Attack
                if (noteTime < Attack)
                {
                    // value between 0 & 1 from Start to end of the Attack. amplitude is the Start amplitude at 1
                    amplitude = (noteTime / Attack) * Start;
                }

                else if (noteTime > Attack && noteTime < Attack + Decay)
                {
                    // value between 0 & 1 Starting at the peak going to the Sustain (note that because the value is 
                    // decreasing, Sustain - Start will be negative, adding Start ensures a positive amplitude
                    amplitude = ((noteTime - Attack) / Decay) * (Sustain - Start) + Start;
                }

                else if (noteTime > Attack + Decay)
                {
                    amplitude = Sustain; // past the Attack/Decay the amplitude will just be the Sustain value
                }
            }
            else
            {
                // Release
                // value between 0 & 1 Starting at the Sustain ending at 0. Because it is decreasing 0 - Sustain will be
                // negative, adding Sustain ensures a positive amplitude
                amplitude = (time - TriggerOff) / Release * (0.0f-Sustain) + Sustain;
            }

            if (amplitude <= .0001f)
            {
                amplitude = 0.0f;
            }
            return amplitude;
        }
    };
    private SynthData _synth;

    // synthesizer function template, will be used anywhere in the code so that in order to make new instruments
    // all I would need to do is replace this struct
    public struct SynthFunc
    {
        public float CycleLength;

        public void Init()
        {
            CycleLength = Mathf.PI * 2f;
        }

        public float WaveFunction(float phase)
        {
            return phase/(2*Mathf.PI);
            //return Mathf.Sin(phase);
        }        
    }

    private SynthFunc _func;

    public AudioSource source;

    // frequency played by the audio source
    public float frequency = 440f;
    // volume modifier of audio source (used to calculate the current volume)
    public float gain;
    // volume of audio source (sets gain when audio is playing)
    public float volume = .1f;

    // how much to increment the phase
    private float _increment;
    // where we are in the waveform
    private float _phase;
    // frequency of the sample reading/playing
    private readonly float _sampFreq = 48000f;

    void OnAudioFilterRead(float[] data, int channels)
    {
        
        // _increment is the amount to increment based on the length of the Cycle
        _increment = frequency * _func.CycleLength / _sampFreq;
        gain = + volume * _synth.GetAmplitude(_timer);
        // for the duration of the sampling put the audio waveform data into the given data array
        for (int i = 0; i < data.Length; i += channels)
        {
            _phase += _increment;
            data[i] = gain * _func.WaveFunction(_phase);
            // if there are 2 channels (i.e. left/right ear) copy data from one channel into the other
            if (channels == 2)
            {
                data[i + 1] = data[i];
            }

            // if the phase has reached the end of the waveform reset it back to the Start
            if (_phase > (Mathf.PI * 2f))
            {
                _phase = 0f;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        _synth.IsKeyDown = false;
        for (int i = 0; i < _keyboard.Length; i++)
        {
            if (Input.GetKey(_keyboard[i]))
            {
                // if frequency is turned into a list then waves can be added in order to produce chords
                frequency = _frequencies[i];
                if (Input.GetKeyDown(_keyboard[i]))
                {
                    _synth.TriggerOn = _timer;
                }
                _synth.IsKeyDown = true;
                
            }

            if (Input.GetKeyUp(_keyboard[i]))
            {
                _synth.TriggerOff = _timer;
            }
        }

        

        _timer += Time.deltaTime;
        //Debug.Log(_timer);
        if (Input.GetMouseButtonDown(0))
        {
            self.SetActive(false);
        }
    }

    
    private void Start()
    {
        _timer = 0.0f;
        _synth.Init();
        _func.Init();
    }
}
