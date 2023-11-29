using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;

namespace AmazingGraceJester
{
	[BepInPlugin("ShimmyMySherbet.AmazingGraceJester", "AmazingGraceJester", "1.0.0")]
	public class AmazingJester : BaseUnityPlugin
	{
		/// <summary>
		/// An in-memory cached instance of the creepy Amazing Grace audio effect
		/// </summary>
		public static AudioClip AmazingGraceSFX { get; private set; }

		/// <summary>
		/// Gets the path to the BepInEx cache directory, where we can store temporary files
		/// </summary>
		public string CacheDirectory => Path.Combine("BepInEx", "cache");

		/// <summary>
		/// The path to the amazing grace sound effect in the cache
		/// </summary>
		public string AmazingGraceFile => Path.Combine(CacheDirectory, "AmazingGraceEffect.ogg");

		private Harmony m_Harmony;

		/// <summary>
		/// Applies all harmony patches, and runs <seealso cref="AwakeAsync"/> on the thread pool
		/// </summary>
		private void Awake()
		{
			// Patches
			m_Harmony = new Harmony("AmazingGraceJester");
			m_Harmony.PatchAll(typeof(AmazingJester).Assembly);

			// Load assets
			Task.Run(AwakeAsync);
		}

		/// <summary>
		/// Runs <seealso cref="LoadAssets"/>, reporting any errors, and reporting plugin load complete
		/// </summary>
		private async Task AwakeAsync()
		{
			try
			{
				await LoadAssets();
			}
			catch (Exception ex)
			{
				Logger.LogError($"Failed to load Amazing Grace Jester: {ex.Message}, {ex.StackTrace}");
				throw;
			}
			Logger.LogInfo($"Plugin Amazing Grace Jester is loaded!");
		}

		/// <summary>
		/// Writes the amazing grace sound effect to the cache if needed, loads the audio effect, and sets <seealso cref="AmazingGraceSFX"/>
		/// </summary>
		/// <returns></returns>
		private async Task LoadAssets()
		{
			if (!File.Exists(AmazingGraceFile))
			{
				await CacheSoundEffect();
			}
			LoadSoundEffect();
		}

		/// <summary>
		/// Writes the amazing grace sound effect to `BepInEx\Cache\AmazingGraceEffect.ogg`, so we can load it without the use of third part libraries like NLayer
		/// </summary>
		private async Task CacheSoundEffect()
		{
			using (var manifestStream = typeof(AmazingJester).Assembly.GetManifestResourceStream("AmazingGraceJester.Assets.AmazingGraceEffect.ogg"))
			using (var file = new FileStream(AmazingGraceFile, FileMode.Create, FileAccess.Write))
			{
				await manifestStream.CopyToAsync(file);
				await file.FlushAsync();
			}
		}

		/// <summary>
		/// Loads the cached sound file as an <seealso cref="AudioClip"/>, and sets <seealso cref="AmazingGraceSFX"/>
		/// </summary>
		private void LoadSoundEffect()
		{
			var path = "file:///" + Path.GetFullPath(AmazingGraceFile).Replace('\\', '/');
			using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS))
			{
				www.SendWebRequest();

				SpinWait.SpinUntil(() => www.isDone);

				if (www.result == UnityWebRequest.Result.ConnectionError)
				{
					Debug.Log(www.error);
					return;
				}

				AmazingGraceSFX = DownloadHandlerAudioClip.GetContent(www);
			}
		}
	}
}