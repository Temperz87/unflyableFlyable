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


namespace unflyableFlyable 
{
    class HUDMindFuckery : MonoBehaviour
    {
        private void Start()
        {
            HUDGunDirectorSight HGDS = new HUDGunDirectorSight();

        }
        public void weaponInfo()
        {
            
        }
        HUDWeaponInfo HWI = new HUDWeaponInfo();
        public WeaponManager wm;
    }
}
