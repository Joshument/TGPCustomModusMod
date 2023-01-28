using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using System.Reflection.Emit;
using UnityEngine;
using static MelonLoader.MelonLogger;

namespace TGPCustomModusMod
{
    [HarmonyPatch(typeof(Sylladex), "Start")]
    public static class SylladexStartPatch
    {
        private static void Prefix()
        {
            MelonLogger.Msg("Changing the list of active modi");

            Type sylladexType = typeof(Sylladex);

            FieldInfo modiField = sylladexType.GetField("modi", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            List<string> values = (List<string>)modiField.GetValue(null);
            values.Append<string>("Queuestack");

            foreach (string value in values)
            {
                MelonLogger.Msg(value);
            }

            modiField.SetValue(null, values);
        }
    }

    [HarmonyPatch(typeof(Sylladex), "SetModus")]
    public static class SetModusPatch
    {
        /*
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Type sylladexType = typeof(Sylladex);
            FieldInfo modusObjectField = sylladexType.GetField("modusObject", BindingFlags.NonPublic | BindingFlags.Instance);

            int switchStatementStart = -1;
            int jumpLocationStart = -1;

            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; ++i)
            {
                if (codes[i].opcode == OpCodes.Ldarg_1 && codes[i + 1].opcode == OpCodes.Ldstr)
                {
                    switchStatementStart = i;
                    break;
                }

                if (codes[i].opcode == OpCodes.Ldarg_0 && codes[i + 1].opcode == OpCodes.Ldarg_0)
                {
                    jumpLocationStart = i;
                    break;
                }
            }

            object modusObject = codes[jumpLocationStart + 2].operand;

            List<CodeInstruction> jumpOpCodes = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, modusObject),
            };

            return codes;
        }
        */

        
        static bool Prefix(
            Sylladex __instance,  
            ref Modus ___captchaModus, ref GameObject ___modusObject, ref string ___modusName, ref AudioClip ___clipSwitch,
            string modus, bool playSound = false
        )
        {
            MelonLogger.Msg("Starting prefix for SetModus");

            MelonLogger.Msg("Going through switch cases");
            switch (modus)
            {
                case "Stack":
                    ___captchaModus = ___modusObject.AddComponent<FILOModus>();
                    break;
                case "Array":
                    ___captchaModus = ___modusObject.AddComponent<ArrayModus>();
                    break;
                case "Hashmap":
                    ___captchaModus = ___modusObject.AddComponent<HashmapModus>();
                    break;
                case "Tree":
                    ___captchaModus = ___modusObject.AddComponent<TreeModus>();
                    break;
                case "Queuestack":
                    ___captchaModus = ___modusObject.AddComponent<QueueStackModus>();
                    break;
                default:
                    ___captchaModus = ___modusObject.AddComponent<FIFOModus>();
                    break;
            }
            ___modusName = modus;
            if (playSound)
            {
                __instance.PlaySoundEffect(___clipSwitch);
            }

            MelonLogger.Msg("Done");

            return false;
        }
    }
}

