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

namespace UnityEditor.MenuExtensions
{
    public static class PingSelection
    {
        #region Private methods
        [MenuItem("Edit/Ping Selection %#&P", true)]
        private static bool CanPingSelection() => Selection.activeObject != null;
        [MenuItem("Edit/Ping Selection %#&P", false)]
        private static void DoPingSelection() => EditorGUIUtility.PingObject(Selection.activeObject);
        #endregion
    }

}