using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;

class LOL : VTOLMOD
{
<<<<<<< HEAD
    private string Stuff = "";
    private string toEdit = "What Controller? Delete the text and enter the number next to the controller, then hit the giant button.";
    private string radarTargets = "";
    private string sRadarTargets = "";
    private string lRadarTargets = "No lock";
    private bool goOn = false;
    private bool GoOn = false;
    private string speed;
    public string SpeedLabel()
=======
    string Stuff = "";
    string toEdit = "What Controller? Delete the text and enter the number next to the controller, then hit the giant button.";
    string radarTargets = "";
    string sRadarTargets = "";
    string lRadarTargets = "No lock";
    bool goOn = false;
    bool GoOn = false;
    string speed;
    public string SpeedLabel() // Too Long?
>>>>>>> c10c0cd3c866a7b14e128bc6540ee64d34d03c44
    {
        switch (MeasurementManager.instance.airspeedMode)
        {
            case MeasurementManager.SpeedModes.MetersPerSecond:
                return "m/s";
            case MeasurementManager.SpeedModes.KilometersPerHour:
                return "KPH";
            case MeasurementManager.SpeedModes.Knots:
                return "kt";
            case MeasurementManager.SpeedModes.MilesPerHour:
                return "MPH";
            case MeasurementManager.SpeedModes.FeetPerSecond:
                return "ft/s";
            case MeasurementManager.SpeedModes.Mach:
                return "M";
            default:
                return "Unhandled unit";
        }
    }
    string altitude;
    public string AltitudeLabel()
    {
        MeasurementManager.AltitudeModes altitudeMode = MeasurementManager.instance.altitudeMode;
        if (altitudeMode == MeasurementManager.AltitudeModes.Meters)
        {
            return "m";
        }
        if (altitudeMode != MeasurementManager.AltitudeModes.Feet)
        {
            return "?";
        }
        return "ft";
    }
    public string DistanceLabel()
    {
        switch (MeasurementManager.instance.distanceMode)
        {
            case MeasurementManager.DistanceModes.Meters:
                return "km";
            case MeasurementManager.DistanceModes.NautMiles:
                return "nm";
            case MeasurementManager.DistanceModes.Feet:
                return "ft";
            case MeasurementManager.DistanceModes.Miles:
                return "mi";
            default:
                return "Unhandled unit";
        }
    }
    string heading;
    string Weapon;
    string Ammo;
    string Missiles;
    bool radarActive = false;
    bool lockingRadar = false;
    bool hasWM = false;
    bool MissileDetected = false;
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
            Stuff += thing.gameObject.name + " ";
        }
        goOn = true;
        while (GoOn == false)
        {
            Stuff = "";
            foreach (var thing in FindObjectsOfType<AIPilot>())
            {
                Stuff += thing.gameObject.name + " ";
            }
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
       //  targetActor.team = Teams.Allied;
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
        Radar targetRadar = toControl.detectionRadar;
        LockingRadar lTargetRadar = new LockingRadar();
        if (targetRadar != null)
        {
            targetRadar.teamsToDetect = Radar.DetectionTeams.Both;
            lTargetRadar = toControl.lockingRadar;
            radarActive = true;
        }
        if (lTargetRadar != null)
        {
            lockingRadar = true; 
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
        MissileDetector rwr = toControl.rwr;
        MeasurementManager MM = MeasurementManager.instance;
        float t = 0f;
        float v = 0f;
        CameraScreenshot CS = new CameraScreenshot();
        PitchYawRoll = new Vector3(0f, 0f, 0f);
        FlightInfo FI = targetActor.flightInfo;
        // RefuelPlane rPlane = targetVehicle.GetComponent<RefuelPlane>();
        float x = 0f;
        float y = 0f;
        float z = 0f;
        float flaps = 0f;
        float brakes = 0f;
        int idx = -1;
        int p = 2;
        float headingNum = 0;
        bool locked = false;
        bool tDep = false;
        bool hDep = false;
        if (toControl.tailHook != null)
        {
            tDep = toControl.tailHook.isDeployed;
        }
        if (toControl.catHook != null)
        {
            hDep = toControl.catHook.deployed;
        }
        infAmmo iA = targetVehicle.AddComponent<infAmmo>();
        iA.wepMan = wm;
        iA.enabled = false;
        Actor lockSelection = new Actor();
        Debug.Log("Controlling " + targetVehicle);
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
                if (iA.enabled)
                {

                    if (Input.GetKey(KeyCode.Space))
                    {
                        if (wm.currentEquip is HPEquipIRML || wm.currentEquip is HPEquipRadarML)
                        {
                            wm.SingleFire();
                        }
                        else
                        {
                            wm.StartFire();
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        if (!(wm.currentEquip is HPEquipIRML || wm.currentEquip is HPEquipRadarML))
                            wm.EndFire();
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (wm.currentEquip is HPEquipIRML || wm.currentEquip is HPEquipRadarML)
                        {
                            wm.SingleFire();
                        }
                        else
                        {
                            wm.StartFire();
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        if (!(wm.currentEquip is HPEquipIRML || wm.currentEquip is HPEquipRadarML))
                            wm.EndFire();
                    }
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
                if (lTargetRadar != null)
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
                            lockSelection = targetRadar.detectedUnits[idx];
                            sRadarTargets = lockSelection.actorName;
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
                            lockSelection = targetRadar.detectedUnits[idx];
                            sRadarTargets = lockSelection.actorName;
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
                                    if (lTargetRadar.GetLock(lockSelection))
                                    {
                                        lRadarTargets = lockSelection.ToString();
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
                    if (!lTargetRadar.IsLocked())
                    {
                        lRadarTargets = "Lock Dropped";
                        if (locked)
                        {
                            locked = !locked;
                        }
                    }
                }
                radarTargets = "";
                foreach (var thing in targetRadar.detectedUnits)
                {
                    headingNum = Mathf.Round(VectorUtils.SignedAngle(Vector3.forward, thing.transform.forward, Vector3.right));
                    if (headingNum < 0)
                    {
                        headingNum += 360;
                    }
                    radarTargets += thing + " " + headingNum.ToString() + " " + MM.ConvertedDistance(Mathf.Round((targetRadar.transform.position - thing.position).magnitude)).ToString() + " " + DistanceLabel() + " " + MM.ConvertedAltitude(Mathf.Round((WaterPhysics.GetAltitude(thing.position)))).ToString() + " " + AltitudeLabel() + "\n";
                }
            }

            // RWR code
            if (rwr != null)
            {
                Missiles = "";
                if (rwr.missileDetected)
                {
                    MissileDetected = true;
                    foreach (var Missile in rwr.detectedMissiles)
                    {
                        headingNum = Mathf.Round(VectorUtils.SignedAngle(Vector3.forward, Missile.transform.forward, Vector3.right));
                        if (headingNum < 0)
                        {
                            headingNum += 360;
                        }
                        Missiles += Missile.ToString() + " " + headingNum.ToString() + " " + MM.ConvertedDistance(Mathf.Round((rwr.transform.position - Missile.transform.position).magnitude)).ToString() + " " + DistanceLabel() + " " + MM.ConvertedAltitude(Mathf.Round((WaterPhysics.GetAltitude(Missile.transform.position)))).ToString() + " " + AltitudeLabel() + "\n";
                    }
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

            // Wing Folding
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (toControl.wingRotator != null)
                {
                    if (toControl.wingRotator.deployed)
                    {
                        toControl.wingRotator.SetDefault();
                    }
                    else
                    {
                        toControl.wingRotator.SetDeployed();
                    }
                }
            }

            // Tail Hook
            if (Input.GetKeyDown(KeyCode.H))
            {
                if (toControl.tailHook != null)
                {
                    if (tDep)
                    {
                        toControl.tailHook.RetractHook();
                        tDep = !tDep;
                    }
                    else
                    {
                        toControl.tailHook.ExtendHook();
                        tDep = !tDep;
                    }
                }
            }

            //  Launch Bar
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (toControl.catHook != null)
                {
                    if (hDep)
                    {
                        toControl.catHook.Retract();
                        hDep = !hDep;
                    }
                    else
                    {
                        toControl.catHook.Extend();
                        hDep = !hDep;
                    }
                }
            }

            // Debug
            if (Input.GetKeyDown(KeyCode.O))
            {
                iA.enabled = !iA.enabled;
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
            heading = FI.heading.ToString();
            altitude = MM.ConvertedAltitude(FI.altitudeASL).ToString() + " " + AltitudeLabel();
            speed = MM.ConvertedSpeed(FI.surfaceSpeed).ToString() + " " + SpeedLabel();
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
                if (lockingRadar)
                {
                    GUI.Label(new Rect(2200f, 1220f, 150000f, 21f), "Selected Target: " + sRadarTargets);
                    GUI.Label(new Rect(2200f, 1240f, 150000f, 21f), "Locked Target: " + lRadarTargets);
                }
            }
            if (MissileDetected)
            {
                GUI.Label(new Rect(20f, 120f, 150000f, 150000f), "MISSILE(S) DETECTED! - BRA\n" + Missiles);
            }
        }
        if (goOn& !GoOn)
        {
            GUI.Label(new Rect(20f, 40f, 150000f, 20f), "Available Controllers: " + Stuff);
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
