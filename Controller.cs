using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;

/*public class Controller : MonoBehaviour
{
    public void setPYR(Vector3 PitchYawRoll)
    {
        for (int i = 0; i < apPlane.autoPilot.outputs.Length; i++)
        {
            apPlane.autoPilot.outputs[i].SetPitchYawRoll(PitchYawRoll);
        }
    }
    public void setThrottle(float throttle)
    {
        if (apPlane.commandState != AIPilot.CommandStates.Override)
        {
            apPlane.commandState = AIPilot.CommandStates.Override;
        }
        foreach (ModuleEngine flightControlComponent in apPlane.autoPilot.engines)
        {
            flightControlComponent.SetThrottle(throttle);
        }
    }
    (public void setGear(GearAnimator.GearStates targetGearState)
    {
        if (gear != targetGearState)
        {
            if (targetGearState == GearAnimator.GearStates.Extended)
            {
                apPlane.gearAnimator.Extend();
            }
            if (targetGearState == GearAnimator.GearStates.Retracted)
            {
                apPlane.gearAnimator.Retract();
            }
            gear = targetGearState;
        }
    }
    public void setTilt(float tilt)
    {
        if (tilta != tilt)
        {
            apPlane.tiltController.SetTiltImmediate(tilt);
            tilta = tilt;
        }
    }
    public void fireCM()
    {
        //apPlane.CountermeasureProgram(true, true, 2, 0f);

    }
    private float tilta;
    public VehicleInputManager apPlane;
    private GearAnimator.GearStates gear;
}*/