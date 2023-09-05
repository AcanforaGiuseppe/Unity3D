using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.Callbacks;
#endif

using S = System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.ScenePostprocessing
{
	using EditorScripting.BuildUtils;

	public static class BuildVersionHandler
	{
		#region Private methods
		[PostProcessScene]
		private static void PostProcessSceneExcludeFromBuild()
		{
			//	Make sure this runs only during build
			if(!BuildPipeline.isBuildingPlayer)
				return;
			//	Find ExcludeFromBuild references in scene
			BuildVersionInfo[] buildInfoObjects = Resources.FindObjectsOfTypeAll<BuildVersionInfo>();
			//	Handle exclusions
			foreach(BuildVersionInfo buildInfoObject in buildInfoObjects)
				buildInfoObject.RefreshVersion();
		}
		#endregion
	}
}
