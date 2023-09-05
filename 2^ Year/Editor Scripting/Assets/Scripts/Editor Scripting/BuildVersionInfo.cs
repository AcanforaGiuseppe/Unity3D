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
	[RequireComponent(typeof(Text))]
	public class BuildVersionInfo : MonoBehaviour
	{
		#region Private variables
		private Text buildVersionLabel = null;
		[HideInInspector]
		[SerializeField]
		private string buildVersion = "EDITOR";
		#endregion
		#region Lifecycle
		void Awake() => buildVersionLabel = GetComponent<Text>();
		void Start() => buildVersionLabel.text = $"DEVELOPMENT-BUILD {buildVersion}";
		#endregion
		#region Public methods
#if UNITY_EDITOR
		public void RefreshVersion()
		{
			buildVersion = S.Guid.NewGuid().ToString();
			//	FEATURE	Read version from source control commit id
		}
#endif
		#endregion
	}
}
