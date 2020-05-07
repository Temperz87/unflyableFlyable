using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;
class infAmmo : MonoBehaviour
{
    private void Update()
    {
        if (wepMan != null)
        {
            foreach (HPEquippable toChange in wepMan.GetCombinedEquips())
            {
                if (toChange is HPEquipGun)
                {
                    ((HPEquipGun)toChange).gun.currentAmmo = ((HPEquipGun)toChange).gun.maxAmmo;
                }
                else if (toChange is HPEquipGunTurret)
                {
                    ((HPEquipGunTurret)toChange).gun.currentAmmo = ((HPEquipGunTurret)toChange).gun.maxAmmo;
                }
                else if (toChange is HPEquipIRML)
                {
                    if (((HPEquipIRML)toChange).GetCount() < ((HPEquipIRML)toChange).GetMaxCount())
                    {
                        ((HPEquipIRML)toChange).ml.LoadCount((((HPEquipIRML)toChange).GetMaxCount()));
                    }
                }
                else if (toChange is HPEquipRadarML)
                {
                    if (((HPEquipRadarML)toChange).GetCount() < ((HPEquipRadarML)toChange).GetMaxCount())
                    {
                        ((HPEquipRadarML)toChange).ml.LoadCount((((HPEquipRadarML)toChange).GetMaxCount()));
                    }
                }
                else if (toChange is HPEquipOpticalML)
                {
                    if (((HPEquipOpticalML)toChange).GetCount() < ((HPEquipOpticalML)toChange).GetMaxCount())
                    {
                        ((HPEquipOpticalML)toChange).ml.LoadCount((((HPEquipOpticalML)toChange).GetMaxCount()));
                    }
                }
                else if (toChange is RocketLauncher)
                {
                    if (((RocketLauncher)toChange).GetCount() < ((RocketLauncher)toChange).GetMaxCount())
                    {
                        ((RocketLauncher)toChange).LoadCount((((RocketLauncher)toChange).GetMaxCount()));
                    }
                }
                else if (toChange is HPEquipBombRack)
                {
                    if (((HPEquipBombRack)toChange).GetCount() < ((HPEquipBombRack)toChange).GetMaxCount())
                    {
                        ((HPEquipBombRack)toChange).ml.LoadCount((((HPEquipBombRack)toChange).GetMaxCount()));
                    }
                }
                else if (toChange is HPEquipGPSBombRack)
                {
                    if (((HPEquipGPSBombRack)toChange).GetCount() < ((HPEquipGPSBombRack)toChange).GetMaxCount())
                    {
                        ((HPEquipGPSBombRack)toChange).ml.LoadCount((((HPEquipGPSBombRack)toChange).GetMaxCount()));
                    }
                }
                else if (toChange is HPEquipASML)
                {
                    if (((HPEquipASML)toChange).GetCount() < ((HPEquipASML)toChange).GetMaxCount())
                    {
                        ((HPEquipASML)toChange).ml.LoadCount((((HPEquipASML)toChange).GetMaxCount()));
                    }
                }
                else if (toChange is HPEquipARML)
                {
                    if (((HPEquipARML)toChange).GetCount() < ((HPEquipARML)toChange).GetMaxCount())
                    {
                        ((HPEquipARML)toChange).ml.LoadCount((((HPEquipARML)toChange).GetMaxCount()));
                    }
                }
            }
        }
    }
    public WeaponManager wepMan;
}