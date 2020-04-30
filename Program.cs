using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;
using System.Windows.Forms;

class LOL : VTOLMOD
{
    string Stuff = "";
    string toEdit = "What Controller? Delete the text and enter the number next to the controller, then hit the giant button.";
    string radarTargets = "";
    string sRadarTargets = "";
    string lRadarTargets = "No lock";
    bool goOn = false;
    bool GoOn = false;
    string speed;
    string heading;
    string altitude;
    string Weapon;
    string Ammo;
    bool radarActive = false;
    bool hasWM = false;
    public IEnumerator main()
    {
        Vector3 PitchYawRoll = new Vector3();
        bool tiltC = false;
        while (VTMapManager.fetch == null || !VTMapManager.fetch.scenarioReady)
        {
            yield return null;
        }
        foreach (var thing in FindObjectsOfType<AIPilot>())
        {
            Debug.Log("Controller " + thing);
            Stuff += thing.gameObject.name + " ";
        }
        goOn = true;
        while (GoOn == false)
        {
            yield return null;
        }
        AIPilot toControl = new AIPilot();
        bool found = false;
        AIPilot[] wut = FindObjectsOfType<AIPilot>();
        for (int i = 0; i < wut.Length; i++)
        {
            if (wut[i].gameObject.name.Contains(toEdit))
            {
                toControl = wut[i];
                found = true;
                break;
            }
        }
        if (!found)
        {
            Debug.LogError("Couldn't find AIPilot, defaulting to 0.");
            toControl = wut[0];
        }
        toControl.commandState = AIPilot.CommandStates.Override;
        foreach (var thing in FindObjectsOfType<AIPilot>())
        {
            Debug.Log(thing);
        }
        GameObject targetVehicle = toControl.gameObject;
        if (targetVehicle != null)
        {
            Debug.Log("GOT THE TARGET VEHICLE!!!");
        }
        TiltController TC = targetVehicle.GetComponent<TiltController>();
        if (TC != null)
        {
            tiltC = true;
        }
        Transform transform = FlightSceneManager.instance.playerActor.transform;
        FloatingOriginShifter playerfos = transform.GetComponent<FloatingOriginShifter>();
        if (playerfos)
        {
            playerfos.enabled = false;
        }
        GameObject empty = new GameObject();
        GameObject Cammy = Instantiate(empty, transform.position, Quaternion.identity);
        Actor targetActor = targetVehicle.GetComponent<Actor>();
        Camera CammyCam = Cammy.AddComponent<Camera>();
        CameraFollowMe cam = Cammy.AddComponent<CameraFollowMe>();
        cam.targets = new List<Transform>();
        Debug.Log("Target list created.");
        cam.AddTarget(targetActor.transform);
        Debug.Log("Added our actor to the target.");
        if (targetActor.transform == null)
        {
            Debug.LogError("Actor transform is null.");
        }
        if (cam.gameObject == null)
        {
            Debug.LogError("cam gameObject is null.");
        }
        cam.cam = CammyCam;
        cam.gameObject.SetActive(true);
        cam.AddTarget(FlightSceneManager.instance.playerActor.transform);
        GameSettings.SetGameSettingValue("HIDE_HELMET", true, true);
        Debug.Log("Cam should be set.");
        VehicleInputManager control = targetVehicle.AddComponent<VehicleInputManager>();
        AutoPilot AP = targetVehicle.GetComponent<AutoPilot>();
        control.pyrOutputs = AP.outputs;
        AP.steerMode = AutoPilot.SteerModes.Aim;
        List<ModuleEngine> Engines = AP.engines;
        AP.enabled = false;
        control.wheelSteerOutputs = targetVehicle.GetComponents<WheelsController>();
        GearAnimator gear = toControl.gearAnimator;
        targetVehicle.GetComponent<AIPilot>().enabled = false;
        Radar targetRadar = targetVehicle.GetComponent<Radar>();
        LockingRadar lTargetRadar = new LockingRadar();
        if (targetRadar != null)
        {
            lTargetRadar = toControl.lockingRadar;
            radarActive = true;
            targetRadar.detectMissiles = true;
        }
        foreach (var thing in targetVehicle.GetComponents(typeof(Component)))
        {
            Debug.Log(thing);
        }
        WeaponManager wm = targetVehicle.GetComponent<WeaponManager>();
        if (wm != null)
        {
            hasWM = true;
            wm.SetMasterArmed(true);
            foreach (var internalBays in wm.internalWeaponBays)
            {
                internalBays.openOnAnyWeaponMatch = true;
            }
        }
        float t = 0f;
        float v = 0f;
        CameraScreenshot CS = new CameraScreenshot();
        PitchYawRoll = new Vector3(0f, 0f, 0f);
        FlightInfo FI = targetActor.flightInfo;
        // RefuelPlane rPlane = targetVehicle.GetComponent<RefuelPlane>();
        Debug.Log("SET VECTOR3!!!");
        Debug.Log("Controlling " + targetVehicle);
        float x = 0f;
        float y = 0f;
        float z = 0f;
        float flaps = 0f;
        float brakes = 0f;
        int idx = -1;
        float headingNum = 0;
        bool locked = false;
        Actor lockedTarget = new Actor();
        while (true)
        {

            // Pitch Yaw Roll controls
            if (Input.GetKey(KeyCode.S))
            {
                x = -1f;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                x = 1f;
            }
            else
            {
                x = 0f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                y = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                y = 1f;
            }
            else
            {
                y = 0f;
            }
            if (Input.GetKey(KeyCode.E))
            {
                z = -1f;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                z = 1f;
            }
            else
            {
                z = 0f;
            }

            // Tilt Controller
            if (Input.GetKey(KeyCode.Z))
            {
                if (v >= 90)
                { }
                else
                {
                    if (TC)
                    {
                        v += 1;
                        TC.SetTiltImmediate(v);
                    }
                }
            }
            if (Input.GetKey(KeyCode.X))
            {
                if (v <= 0)
                { }
                else
                {
                    if (tiltC)
                    {
                        v -= 1;
                        TC.SetTiltImmediate(v);
                    }
                }
            }

            // Screen Shot
            if (Input.GetKeyDown(KeyCode.L))
            {
                try
                {
                    if (CS.cam == null)
                    {
                        CS.cam = CameraFollowMe.instance.cam;
                        Debug.Log("NULL in assigning cam to CS");
                    }
                    CS.Screenshot();
                }
                catch (NullReferenceException)
                {
                    Debug.Log("This dude really tried screenshotting without having the debug cam on, LOSER!");
                }
            }

            // WeaponManager code
            if (wm != null)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    wm.CycleActiveWeapons();
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    wm.StartFire();
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    wm.EndFire();
                }
                Weapon = wm.currentEquip.name;
                Ammo = wm.currentEquip.GetCount().ToString();
            }

            // Gear Toggle
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (gear != null)
                {
                    gear.Toggle();
                }
            }

            // Thrrottle code
            if (Input.GetKey(KeyCode.LeftControl)) // Increase
            {
                if (t > 0)
                {
                    t -= 0.0125f;
                }
            }
            if (Input.GetKey(KeyCode.LeftShift)) // Decrease
            {
                if (t < 1)
                {
                    t += 0.0125f;
                }
            }

            // CMS dispenser
            if (Input.GetKey(KeyCode.C))
            {
                toControl.FireFlares();
                toControl.FireChaff();
            }

            // Radar code
            if (targetRadar != null)
            {
                if (Input.GetKeyDown(KeyCode.M)) // Move right in the array
                {
                    idx += 1;
                    if (idx > targetRadar.detectedUnits.Count - 1)
                    {
                        idx = targetRadar.detectedUnits.Count - 1;
                    }
                    if (targetRadar.detectedUnits.Count > 0)
                    {
                        lockedTarget = targetRadar.detectedUnits[idx];
                        sRadarTargets = lockedTarget.actorName;
                    }
                }
                if (Input.GetKeyDown(KeyCode.N)) // Move left in the array
                {
                    idx -= 1;
                    if (idx < 0)
                    {
                        idx = 0;
                    }
                    if (targetRadar.detectedUnits.Count > 0)
                    {
                        lockedTarget = targetRadar.detectedUnits[idx];
                        sRadarTargets = lockedTarget.actorName;
                    }
                }
                if (Input.GetKeyDown(KeyCode.J)) // Lock
                {
                    if (!locked)
                    {
                        if (targetRadar.detectedUnits.Count > 0)
                        {
                            if (idx >= 0)
                            {
                                if (lTargetRadar.GetLock(lockedTarget))
                                {
                                    lRadarTargets = lockedTarget.actorName;
                                    locked = !locked;
                                }
                            }
                        }
                    }
                    else
                    {
                        lTargetRadar.Unlock();
                        lRadarTargets = "No lock";
                        locked = !locked;
                    }
                }
                radarTargets = "";
                foreach (var thing in targetRadar.detectedUnits) 
                {
                    radarTargets += thing + " " + Mathf.Round(VectorUtils.Bearing(targetRadar.transform.position, thing.position)).ToString() + " " + Mathf.Round((targetRadar.transform.position - thing.position).magnitude).ToString() + " " + Mathf.Round((WaterPhysics.GetAltitude(thing.position))).ToString() + "\n";
                }
            }

            // Flaps
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (flaps == 0f)
                {
                    flaps = .5f;
                }
                else if (flaps == .5f)
                {
                    flaps = 1f;
                }
                else
                {
                    flaps = 0f;
                }
                foreach (var thing in toControl.autoPilot.outputs)
                {
                    thing.SetFlaps(flaps);
                }
            }


            // Brakes
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (brakes == 0f)
                {
                    brakes = 1f;
                }
                else
                {
                    brakes = 0f;
                }
                foreach (var thing in toControl.autoPilot.outputs)
                {
                    thing.SetBrakes(brakes);
                }
            }
            // Misc Stuff
            PitchYawRoll.Set(x, y, z);
            control.SetJoystickPYR(PitchYawRoll);
            foreach (ModuleEngine Engine in Engines)
            {
                Engine.SetThrottle(t);
            }
            headingNum = VectorUtils.SignedAngle(Vector3.forward, targetActor.transform.forward, Vector3.right);
            if (headingNum < 0f)
            {
                headingNum += 360f;
            }
            heading = Mathf.Round(headingNum).ToString();
            altitude = Mathf.Round((WaterPhysics.GetAltitude(targetActor.position))).ToString();
            speed = FI.surfaceSpeed.ToString();
            yield return null;
        }
    }
    private void OnGUI()
    {
        if (GoOn)
        {
            GUI.Label(new Rect(20f, 20f, 150000f, 150000f), "Speed: " + speed);
            GUI.Label(new Rect(20f, 40f, 150000f, 150000f), "Altitude: " + altitude);
            GUI.Label(new Rect(20f, 60f, 150000f, 150000f), "Heading: " + heading);
            if (hasWM)
            {
                GUI.Label(new Rect(20f, 80f, 150000f, 20f), "Weapon: " + Weapon);
                GUI.Label(new Rect(20f, 100f, 150000f, 20f), "Ammo: " + Ammo);
            }
            if (radarActive)
            {
                GUI.Label(new Rect(2200f, 20f, 150000f, 150000f), "Radar Targets - BRA" + "\n" + radarTargets);
                GUI.Label(new Rect(2200f, 1220f, 150000f, 21f), "Selected Target: " + sRadarTargets);
                GUI.Label(new Rect(2200f, 1240f, 150000f, 21f), "Locked Target: " + lRadarTargets);
            }
        }
        if (goOn& !GoOn)
        {
            GUI.Label(new Rect(20f, 40f, 1500f, 20f), "Available Controllers: " + Stuff);
            toEdit = GUI.TextField(new Rect(20f, 60f, 1500f, 20f), toEdit, 2000);
            if (GUI.Button(new Rect(1600, 60f, 120, 20), "Control."))
            {
                GoOn = true;
            }
            
        }
    }
    private void Start()
    {
        ModLoaded();
    }
    public override void ModLoaded()
    {
        VTOLAPI.SceneLoaded += lol;
        base.ModLoaded();
    }
    private void lol(VTOLScenes scenes)
    {
        if (scenes == VTOLScenes.Akutan || scenes == VTOLScenes.CustomMapBase)
        {
            StartCoroutine(main());
        }
    }
}