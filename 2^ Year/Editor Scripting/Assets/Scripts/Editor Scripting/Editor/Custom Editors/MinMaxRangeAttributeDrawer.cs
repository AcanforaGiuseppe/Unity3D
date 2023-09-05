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

namespace UnityEditor.CustomEditors
{
    using EditorScripting.Attributes;

    [CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
    public class MinMaxRangeAttributeDrawer : PropertyDrawer
    {
        #region Lifecycle
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //	Draw label
            Rect editPosition = EditorGUI.PrefixLabel(position, label);

            //	Calculate areas
            float numericInputWidth = 45.0f;
            Rect minDirectInputRect = new Rect(editPosition.x, editPosition.y, numericInputWidth, editPosition.height);
            Rect sliderRect = new Rect(editPosition.x + numericInputWidth, editPosition.y, editPosition.width - 2.0f * numericInputWidth, editPosition.height);
            Rect maxDirectInputRect = new Rect(editPosition.x + editPosition.width - numericInputWidth, editPosition.y, numericInputWidth, editPosition.height);

            //	Get reference to serialized properties and attribute
            SerializedProperty minProp = property.FindPropertyRelative("min");
            SerializedProperty maxProp = property.FindPropertyRelative("max");
            MinMaxRangeAttribute minMaxAttribute = (MinMaxRangeAttribute)attribute;

            //	Draw inspector
            minProp.floatValue = EditorGUI.FloatField(minDirectInputRect, minProp.floatValue);
            float minValue = minProp.floatValue;
            float maxValue = maxProp.floatValue;
            EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, minMaxAttribute.rangeMin, minMaxAttribute.rangeMax);
            minProp.floatValue = minValue;
            maxProp.floatValue = maxValue;
            maxProp.floatValue = EditorGUI.FloatField(maxDirectInputRect, maxProp.floatValue);
        }
        #endregion
    }
}