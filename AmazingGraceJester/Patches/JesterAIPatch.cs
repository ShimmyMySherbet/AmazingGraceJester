using System;
using HarmonyLib;

namespace AmazingGraceJester.Patches
{
	/// <summary>
	/// Replaces the jester's wind-up theme with the Amazing Grace effect, loaded by <seealso cref="AmazingJester"/>
	/// </summary>
	[HarmonyPatch(typeof(JesterAI), "SetJesterInitialValues")]
	internal static class JesterAIPatch
	{
		[HarmonyPrefix]
		public static void Prefix(JesterAI __instance)
		{
			if (AmazingJester.AmazingGraceSFX != null)
			{
				// Replace sound effect
				__instance.popGoesTheWeaselTheme = AmazingJester.AmazingGraceSFX;
			}
		}
	}
}
