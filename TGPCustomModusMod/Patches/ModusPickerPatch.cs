using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace TGPCustomModusMod.Patches
{
    [HarmonyPatch(typeof(ModusPicker), "AddModus")]
    public static class ModusPickerAddModusPatch
    {
        public static void Prefix(string modus, ref Image ___modusOption, ModusPicker __instance)
        {

        }
        public static void Postfix(string modus, ref Image ___modusOption)
        {
            MelonLogger.Msg("Running postfix for ModusPicker::AddModus()");

            string assetName = "assets/" + modus.ToLower() + "modus.png";

            MelonLogger.Msg("Checking if sprite " + assetName + " exists");
            if (CustomModus.modiBundle.Contains(assetName)) 
            {
                MelonLogger.Msg("Loading and applying sprite " + assetName);

                Image image = ___modusOption.transform.parent.Find("QueuestackModus").gameObject.GetComponent<Image>();

                image.sprite = CustomModus.modiBundle.LoadAsset<Sprite>(assetName);
            }
            
            MelonLogger.Msg("Postfix for ModusPicker::AddModus() finished");
        }
    }

    [HarmonyPatch(typeof(ModusPicker), "ReadDescriptions")]
    public static class ModusPickerReadDescriptionsPatch
    {
        public static bool Prefix()
        {
            Type modusPickerType = typeof(ModusPicker);
            FieldInfo modusDescriptionField = modusPickerType.GetField("modusDescription", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            Dictionary<string, string> modusDescription = (Dictionary<string, string>)modusDescriptionField.GetValue(null);

            if (modusDescription != null)
            {
                return false;
            }
            modusDescription = new Dictionary<string, string>();

            MelonLogger.Msg("Creating list");
            List<string> fetchModiDescriptions = new List<string>(StreamingAssets.ReadLines("fetchmodi.txt"));

            MelonLogger.Msg("Looping");
            foreach (string item in fetchModiDescriptions.Concat(CustomModus.customDescriptions))
            {
                MelonLogger.Msg(item);
                string[] array = item.Split(new char[1] { ':' }, 2);
                modusDescription[array[0]] = array[1].TrimStart();
            }

            modusDescriptionField.SetValue(null, modusDescription);

            MelonLogger.Msg(modusDescription);
            return false;
        }
    }
}
