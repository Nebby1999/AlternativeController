using Nebula;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC.UI
{
    public class HUD : SingletonMonoBehaviour<HUD>
    {
        [Header("Infos")]
        public HeadQuarters headQuarters;
        public Base redBase;
        public Base blackBase;

        [Header("Elements")]
        public ResourcesManagerUI hqUI;
        public ResourcesManagerUI blackBaseUI;
        public ResourcesManagerUI redBaseUI;

        public void Start()
        {
            hqUI.tiedManager = headQuarters.resourcesManager;
            blackBaseUI.tiedManager = blackBase.resourcesManager;
            redBaseUI.tiedManager = redBase.resourcesManager;
        }
    }
}