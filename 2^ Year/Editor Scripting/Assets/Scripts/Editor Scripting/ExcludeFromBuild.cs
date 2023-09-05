using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using S = System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EditorScripting.BuildUtils
{
	[DisallowMultipleComponent]
	public sealed class ExcludeFromBuild : MonoBehaviour
	{
#if UNITY_EDITOR
		#region Private enums
		private enum PlatformMode : byte { Exclude, Include }
		#endregion
		#region Private variables
		[SerializeField]
		private PlatformMode mode = PlatformMode.Exclude;
		[SerializeField]
		private List<BuildTarget> platforms = new List<BuildTarget>();
		[SerializeField]
		private bool onlyInDevelopmentBuilds = false;
		[SerializeField]
		private bool hideInEditor = false;
		#endregion
		#region Lifecycle
		void Awake()
		{
			if(hideInEditor)
				DestroyImmediate(gameObject);
		}
		#endregion
		#region Public methods
		public bool IsIncludedOnPlatform(BuildTarget platform)
		{
			switch(mode)
			{
				case PlatformMode.Include:
					return platforms.Contains(platform);
				case PlatformMode.Exclude:
				default:
					return !platforms.Contains(platform);
			}
		}
		public void HandleExclusion(BuildTarget platform)
		{
			if(!BuildPipeline.isBuildingPlayer)
				return;
			if(
				!IsIncludedOnPlatform(platform) ||
				(
					onlyInDevelopmentBuilds &&
					!EditorUserBuildSettings.development
				)
			)
				DestroyImmediate(gameObject);
			else
				DestroyImmediate(this);

		}
		#endregion
#endif
	}
}
