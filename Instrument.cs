using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// container class to make object selection easy
public class Instrument : MonoBehaviour
{
    public GameObject instrument;
    public void Activate()
    {
        instrument.SetActive(true);
    }
}
