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
    bool goOn = false;
    bool GoOn = false;
    string speed;
    string Weapon;
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
        foreach (var thing in targetVehicle.GetComponents(typeof(Component)))
        {
            Debug.Log(thing);
        }
        WeaponManager wm = targetVehicle.GetComponent<WeaponManager>();
        wm.SetMasterArmed(true);

        foreach (var internalBays in wm.internalWeaponBays)
        {
            internalBays.openOnAnyWeaponMatch = true;
        }

        float t = 0f;
        float v = 0f;
        CameraScreenshot CS = new CameraScreenshot();
        PitchYawRoll = new Vector3(0f, 0f, 0f);
        FlightInfo FI = targetActor.flightInfo;
        RefuelPlane rPlane = targetVehicle.GetComponent<RefuelPlane>();
        Debug.Log("SET VECTOR3!!!");
        //Debug.Log("BLOODILY KILLED THE AI PILOT!");
        //FieldInfo FIYA = LOL.GetType().GetField("firing", BindingFlags.NonPublic | BindingFlags.Instance);
        Debug.Log("Controlling " + targetVehicle);
        // float factor = 1f;
        float x = 0;
        float y = 0;
        float z = 0;
        while (true)
        {
            if (AP.steerMode != AutoPilot.SteerModes.Aim)
            {
                AP.steerMode = AutoPilot.SteerModes.Aim; 
                Debug.Log("Changed autopilot steer mode to aim.");
            }
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
            if (Input.GetKey(KeyCode.B))
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
            else
            {
                // Brake Code
            }
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
            }
            // LOL.flareCMs[0].SetCount(9999);
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (gear != null)
                {
                    gear.Toggle();
                }
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (t > 0)
                {
                    t -= 0.0125f;
                }
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (t < 1)
                {
                    t += 0.0125f;
                }
            }
            if (Input.GetKey(KeyCode.C))
            {
                toControl.FireFlares();
                toControl.FireChaff();
            }
            if (Input.GetKey(KeyCode.K))
            {
            }

            PitchYawRoll.Set(x, y, z);
            control.SetJoystickPYR(PitchYawRoll);
            foreach (ModuleEngine Engine in Engines)
            {
                Engine.SetThrottle(t);
            }
            Weapon = wm.currentEquip.name;
            speed = FI.surfaceSpeed.ToString();
            yield return null;
        }
    }
    private void OnGUI()
    {
        if (GoOn)
        {
            GUI.Label(new Rect(20f, 20f, 150000f, 20f), "Speed: " + speed);
            GUI.Label(new Rect(20f, 40f, 150000f, 20f), "Weapon: " + Weapon);
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