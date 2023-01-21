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

namespace UnityEditor.EditorTools
{
    public class ArrayInstancingWindow : EditorWindow
    {
        #region Private constants
        private const string PREFS_NAMESPACE = "ARRAY_INSTANCING_WINDOW::";
        #endregion

        #region Private enums
        private enum ArrayMode : byte
        {
            Grid,
            Radial
        }
        #endregion

        #region Private variables
        private GameObject prefabToInstantiate = null;
        private bool autoDetectPefabToInstantiate = false;
        private ArrayMode arrayMode = ArrayMode.Grid;
        private int count = 5;
        private float extent = 10.0f;
        #endregion

        #region Public properties
        public bool canInstantiate => prefabToInstantiate != null && AssetDatabase.Contains(prefabToInstantiate);
        #endregion

        #region Lifecycle
        [MenuItem("Tools/Instancing/Array Insancer")]
        private static void ShowArrayInstancingWindow() => GetWindow<ArrayInstancingWindow>("Array Instancer").Show();

        void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                prefabToInstantiate = EditorGUILayout.ObjectField(new GUIContent("Prefab"), prefabToInstantiate, typeof(GameObject), false) as GameObject;
                autoDetectPefabToInstantiate = EditorGUILayout.ToggleLeft(new GUIContent("Auto Detect"), autoDetectPefabToInstantiate);
            }
            arrayMode = (ArrayMode)EditorGUILayout.EnumPopup(new GUIContent("Array Mode"), arrayMode);
            using (new GUILayout.HorizontalScope())
            {
                count = Mathf.Max(0, EditorGUILayout.IntField(new GUIContent(arrayMode == ArrayMode.Radial ? "Amount" : "Per side"), count));
                extent = Mathf.Max(0.1f, EditorGUILayout.FloatField(new GUIContent(arrayMode == ArrayMode.Radial ? "Radius" : "Extent"), extent));
            }
            bool wasGUIEnabled = GUI.enabled;
            GUI.enabled &= canInstantiate;

            if (GUILayout.Button("Instantiate"))
                DoInstantiateArrayOfObjects();

            GUI.enabled = wasGUIEnabled;
        }

        void OnEnable()
        {
            //	Store persistent fields
            autoDetectPefabToInstantiate = EditorPrefs.GetBool(PREFS_NAMESPACE + "autoDetectPefabToInstantiate", false);
            arrayMode = (ArrayMode)EditorPrefs.GetInt(PREFS_NAMESPACE + "arrayMode", (int)ArrayMode.Grid);
            count = EditorPrefs.GetInt(PREFS_NAMESPACE + "count", 5);
            extent = EditorPrefs.GetFloat(PREFS_NAMESPACE + "extent", 10.0f);

            //	Get current selection as prefab to instantiate
            if (prefabToInstantiate == null)
                SetPrefabToInstantiateFromSelection();

            //	Respond to selection changes
            Selection.selectionChanged += HandleSelectionChanged;
        }

        void OnDisable()
        {
            //	Unsubscribe from selection change callback
            Selection.selectionChanged -= HandleSelectionChanged;

            //	Store persistent fields
            EditorPrefs.SetBool(PREFS_NAMESPACE + "autoDetectPefabToInstantiate", autoDetectPefabToInstantiate);
            EditorPrefs.SetInt(PREFS_NAMESPACE + "arrayMode", (int)arrayMode);
            EditorPrefs.SetInt(PREFS_NAMESPACE + "count", count);
            EditorPrefs.SetFloat(PREFS_NAMESPACE + "extent", extent);
        }
        #endregion

        #region Public methods
        public void DoInstantiateArrayOfObjects()
        {
            if (!canInstantiate)
                return;

            switch (arrayMode)
            {
                case ArrayMode.Grid:
                    InstantiateGrid();
                    break;
                case ArrayMode.Radial:
                    InstantiateRadial();
                    break;
                default:
                    Debug.LogWarning($"{arrayMode.ToString()} not handled.");
                    break;
            }
        }
        #endregion

        #region Private methods
        private void HandleSelectionChanged()
        {
            if (autoDetectPefabToInstantiate)
            {
                SetPrefabToInstantiateFromSelection();
                Repaint();
            }
        }

        private void SetPrefabToInstantiateFromSelection()
        {
            if (Selection.activeGameObject != null && Selection.activeGameObject is GameObject prefab && AssetDatabase.Contains(prefab))
                prefabToInstantiate = prefab;
        }

        private void InstantiateGrid()
        {
            float step = extent / count;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    GameObject instance = PrefabUtility.InstantiatePrefab(prefabToInstantiate) as GameObject;
                    instance.transform.position = new Vector3(-extent + 2 * j * step, 0.0f, -extent + 2 * i * step);
                    Undo.RegisterCreatedObjectUndo(instance, "Grid instancing");
                }
            }
        }

        private void InstantiateRadial()
        {
            float step = (2.0f * Mathf.PI) / count;
            for (int i = 0; i < count; i++)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(prefabToInstantiate) as GameObject;
                instance.transform.position = new Vector3(Mathf.Cos(i * step), 0.0f, Mathf.Sin(i * step)) * extent;
                Undo.RegisterCreatedObjectUndo(instance, "Radial instancing");
            }
        }
        #endregion
    }
}