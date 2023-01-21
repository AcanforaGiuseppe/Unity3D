using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.Build.Reporting;
#endif

using S = System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityEditor.CustomBuildCommands
{
	public static class CustomBuildTargetPlatform
	{
		#region Menu items
		[MenuItem("File/Build for Windows", true)]
		private static bool CanBuildForWindows() => CanBuild();
		[MenuItem("File/Build for Windows", false)]
		private static void DoBuildForWindows()
		{
			string destinationDirectory = EditorUtility.OpenFolderPanel($"Build All Standaolne", "", "Build");
			if(
				destinationDirectory == null ||
				!Directory.Exists(destinationDirectory)
			)
				Debug.LogWarning("Build All Standalone canceled.");
			DoBuild(new BuildPlayerOptions
			{
				scenes = EditorBuildSettings.scenes.Select<EditorBuildSettingsScene, string>(s => s.path).ToArray<string>(),
				targetGroup = BuildTargetGroup.Standalone,
				target = BuildTarget.StandaloneWindows64,
				locationPathName = destinationDirectory,
				extraScriptingDefines = new string[] { },   //	Use this whe you want to do specific builds like DSM-free builds
				options = BuildOptions.None //	Use this to make development builds, to run builds after complete and so on (this is a bitmask!)
			});
		}
		[MenuItem("File/Build for Mac", true)]
		private static bool CanBuildForMac() => CanBuild();
		[MenuItem("File/Build for Mac", false)]
		private static void DoBuildForMac()
		{
			string destinationDirectory = EditorUtility.OpenFolderPanel($"Build All Standaolne", "", "Build");
			if(
				destinationDirectory == null ||
				!Directory.Exists(destinationDirectory)
			)
				Debug.LogWarning("Build All Standalone canceled.");
			DoBuild(new BuildPlayerOptions
			{
				scenes = EditorBuildSettings.scenes.Select<EditorBuildSettingsScene, string>(s => s.path).ToArray<string>(),
				targetGroup = BuildTargetGroup.Standalone,
				target = BuildTarget.StandaloneOSX,
				locationPathName = destinationDirectory,
				extraScriptingDefines = new string[] { },   //	Use this whe you want to do specific builds like DSM-free builds
				options = BuildOptions.None //	Use this to make development builds, to run builds after complete and so on (this is a bitmask!)
			});
		}
		[MenuItem("File/Build All Standalone", true)]
		private static bool CanBuildAllStandalone() => CanBuild();
		[MenuItem("File/Build All Standalone", false)]
		private static void BuildAllStandaolne()
		{
			string destinationDirectory;
			if(!GetBuildDirectory(out destinationDirectory))
			{
				Debug.LogWarning("Build All Standalone canceled.");
				return;
			}
			BuildTarget[] failedBuilds = DoBuildMultiple(
				new BuildPlayerOptions
				{
					scenes = EditorBuildSettings.scenes.Select<EditorBuildSettingsScene, string>(s => s.path).ToArray<string>(),
					extraScriptingDefines = new string[] { },   //	Use this whe you want to do specific builds like DSM-free builds
					options = BuildOptions.None //	Use this to make development builds, to run builds after complete and so on (this is a bitmask!)
				},
				destinationDirectory,
				BuildTarget.StandaloneWindows64, BuildTarget.StandaloneOSX
			);
			if(failedBuilds.Length > 0)
				Debug.LogError($"The following platforms failed to build:{S.Environment.NewLine}{string.Join($"{S.Environment.NewLine}- ", failedBuilds.Select<BuildTarget, string>(t => t.ToString()).ToArray<string>())}");
		}
		private static bool GetBuildDirectory(out string directoryPath)
		{
			directoryPath = EditorUtility.SaveFolderPanel($"Build All Standaolne", "", "Build");
			return !string.IsNullOrEmpty(directoryPath);
		}
		#endregion
		#region Private methods
		/// <summary>
		/// Tells if a build is currently possible.
		/// </summary>
		/// <returns>True if it is possible to bluild a player, false otherwise</returns>
		private static bool CanBuild() => !EditorApplication.isPlaying && !EditorApplication.isCompiling;
		/// <summary>
		/// Build the player with the given options and return the output directory path;
		/// </summary>
		/// <param name="options">Build player options</param>
		/// <returns>The path of the directory containing the build, if succeeded, null otherwise</returns>
		private static string DoBuild(BuildPlayerOptions options, bool switchBackBuildTarget = true)
		{
			if(!BuildPipeline.IsBuildTargetSupported(options.targetGroup, options.target))
			{
				Debug.Log($"Skipping build for {options.target} since the editor does not support it. Check installed modules.");
				return null;
			}
			BuildTarget editorTarget = EditorUserBuildSettings.activeBuildTarget;
			try
			{
				//	Ensure directory exists
				if(!Directory.Exists(options.locationPathName))
					Directory.CreateDirectory(options.locationPathName);
				//	Switch build target
				if(editorTarget != options.target)
					EditorUserBuildSettings.SwitchActiveBuildTarget(options.targetGroup, options.target);
				//	Build
				options.locationPathName = CompleteBuildPath(options.locationPathName, options.target);
				BuildReport report = BuildPipeline.BuildPlayer(options);
				//	Handle result
				switch(report.summary.result)
				{
					case BuildResult.Succeeded:
						Debug.Log($"Custom build for {options.target} succeeded in {report.summary.totalTime}, total size {report.summary.totalSize.ToReadableSize()} in {report.summary.outputPath}");
						//	FEATURE	Copy documentation into build directory
						//	FEATURE	Build an installer
						//	FEATURE	Zip up the build and copy to network drive or upload
						//	FEATURE	Send an e-mail to notify the team a new build is ready for testing
						//	Return the output path
						return report.summary.outputPath;
					case BuildResult.Cancelled:
						break;
					case BuildResult.Unknown:
					case BuildResult.Failed:
					default:
						Debug.LogError($"Could not complete build for {options.target}. Check console for details.");
						break;
				}
			}
			catch(S.Exception ex)
			{
				Debug.LogError($"Could not complete the build process due to an exception.{S.Environment.NewLine}{ex.Message}{S.Environment.NewLine}{ex.StackTrace}");
			}
			finally
			{
				if(
					switchBackBuildTarget &&
					EditorUserBuildSettings.activeBuildTarget != editorTarget
				)
					EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(editorTarget), editorTarget);
			}
			//	If didn't return yet, something went wrong
			return null;
		}
		/// <summary>
		/// Builds multiple platforms sequentially.
		/// </summary>
		/// <param name="options">Build player options</param>
		/// <param name="containerDirectory">The directory which will contain all the sub-directoryies, one per platform</param>
		/// <param name="platformsToBuild">List of the platforms to build</param>
		/// <returns>A list of the FAILED platforms</returns>
		private static BuildTarget[] DoBuildMultiple(BuildPlayerOptions options, string containerDirectory, params BuildTarget[] platformsToBuild)
		{
			List<BuildTarget> failedBuilds = new List<BuildTarget>();
			for(int t = 0; t < platformsToBuild.Length; t++)
			{
				options.target = platformsToBuild[t];
				options.targetGroup = BuildPipeline.GetBuildTargetGroup(options.target);
				options.locationPathName = Path.Combine(containerDirectory, platformsToBuild[t].ToString());
				if(string.IsNullOrEmpty(DoBuild(options, t == platformsToBuild.Length - 1)))
					failedBuilds.Add(platformsToBuild[t]);
			}
			return failedBuilds.ToArray();
		}
		/// <summary>
		/// Manipulate build path to prepare it to be used by BuildPipeline.
		/// </summary>
		/// <param name="baseBuildPath">The path of the directory chosen by the user which will contain the built player</param>
		/// <param name="platform">The BuildTarget for which the player is built</param>
		/// <returns>The build path ready for BuildPipeline</returns>
		private static string CompleteBuildPath(string baseBuildPath, BuildTarget platform)
		{
			switch(platform)
			{
				case BuildTarget.StandaloneOSX:
					return Path.Combine(baseBuildPath, PlayerSettings.productName);
				case BuildTarget.StandaloneWindows:
				case BuildTarget.StandaloneWindows64:
					return Path.Combine(baseBuildPath, PlayerSettings.productName + ".exe");
				case BuildTarget.StandaloneLinux64:
					return Path.Combine(baseBuildPath, PlayerSettings.productName + ".x86_64");
				case BuildTarget.iOS:
					return Path.Combine(baseBuildPath, PlayerSettings.productName + ".ipa");
				case BuildTarget.Android:
				case BuildTarget.WebGL:
				case BuildTarget.WSAPlayer:
				case BuildTarget.PS4:
				case BuildTarget.tvOS:
				case BuildTarget.Switch:
				case BuildTarget.Lumin:
				case BuildTarget.Stadia:
				case BuildTarget.CloudRendering:
				case BuildTarget.GameCoreXboxOne:
				case BuildTarget.PS5:
				case BuildTarget.NoTarget:
				default:
					return Path.Combine(baseBuildPath, PlayerSettings.productName);
			}
		}
		/// <summary>
		/// Convert file size from unsigned long to readable string.
		/// </summary>
		/// <param name="totalBytes">File size in bytes</param>
		/// <returns>Redable file size</returns>
		private static string ToReadableSize(this ulong totalBytes)
		{
			string[] orderLabels = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
			if(totalBytes == 0)
				return $"0 {orderLabels[0]}";
			int order = S.Convert.ToInt32(S.Math.Floor(S.Math.Log(totalBytes, 1024)));
			double num = S.Math.Round(totalBytes / S.Math.Pow(1024, order), 2);
			return string.Format("{0:0.##} {1}", num, orderLabels[order]);
		}
		#endregion
	}
}
