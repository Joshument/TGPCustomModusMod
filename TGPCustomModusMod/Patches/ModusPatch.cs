using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace TGPCustomModusMod.Patches
{
    [HarmonyPatch(typeof(Modus), "SetIcon")]
    static public class ModusSetIconPatch
    {
        static public bool Prefix(Sylladex ___sylladex, string title)
        {
            string assetName = "assets/" + title.ToLower() + "modus.png";

            if (CustomModus.modiBundle.Contains(assetName))
            {
                MelonLogger.Msg("Loading and applying sprite " + assetName);

                ___sylladex.modusIcon.sprite = CustomModus.modiBundle.LoadAsset<Sprite>(assetName);

                return false;
            }
            return true;
        }
    }
}
