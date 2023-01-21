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

namespace UnityEditor.Examples
{
    public static class EditorUtilityExample
    {
        #region Private methods

        [MenuItem("Tools/Examples/Display Dialog")]
        private static void DoDisplayDialog() => EditorUtility.DisplayDialog("Title", "Contents of the message", "OK button label");

        [MenuItem("Tools/Examples/Display Progress")]
        private static void DoDisplayProgress() => EditorUtility.DisplayProgressBar("Title", "Current operation in progress...", 0.35f);

        [MenuItem("Tools/Examples/Display Cancelable Progress")]
        private static void DoDisplayCancelableProgress() => EditorUtility.DisplayCancelableProgressBar("Title", "Current operation in progress...", 0.35f);

        [MenuItem("Tools/Examples/Clear Progress")]
        private static void DoClearProgress() => EditorUtility.ClearProgressBar();

        [MenuItem("Tools/Examples/Save File")]
        private static void DoSaveFileDialog()
        {
            string filePath = EditorUtility.SaveFilePanel("Save File Panel", ".", "", "");
            EditorUtility.DisplayDialog("File to save", "Chosen path: " + (string.IsNullOrEmpty(filePath) ? "<none>" : filePath), "Close");
        }

        [MenuItem("Tools/Examples/Open File")]
        private static void DoOpenFileDialog()
        {
            string filePath = EditorUtility.OpenFilePanel("Open File Panel", ".", "");
            EditorUtility.DisplayDialog("File to open", "Chosen path: " + (string.IsNullOrEmpty(filePath) ? "<none>" : filePath), "Close");
        }

        [MenuItem("Tools/Examples/Open Directory")]
        private static void DoOpenDirectoryDialog()
        {
            string directoryPath = EditorUtility.OpenFolderPanel("Open Directory Panel", ".", "");
            EditorUtility.DisplayDialog("File to open", "Chosen path: " + (string.IsNullOrEmpty(directoryPath) ? "<none>" : directoryPath), "Close");
        }

        [MenuItem("Tools/Examples/Display Custom Menu")]
        private static void DoShowMenu()
        {
            Dictionary<int, string> items = new Dictionary<int, string>{
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
                { 4, "four" },
                { 5, "five" }
            };
            EditorUtility.DisplayCustomMenu(new Rect(), items.Select<KeyValuePair<int, string>, GUIContent>(item => new GUIContent($"Squared value of {item.Value}")).ToArray<GUIContent>(), -1, HandleCustomMenuSelection, items);
        }

        private static void HandleCustomMenuSelection(object userData, string[] options, int selected)
        {
            int number = -1;
            Dictionary<int, string> items = userData as Dictionary<int, string>;

            if (items != null)
                number = items.Keys.ToArray<int>()[selected];

            EditorUtility.DisplayDialog("Custom Menu Selection", $"{number} squared is {number * number}", "Close");
        }
        #endregion

    }
}