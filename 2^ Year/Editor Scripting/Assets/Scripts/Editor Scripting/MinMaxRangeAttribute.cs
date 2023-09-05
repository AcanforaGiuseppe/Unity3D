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

namespace EditorScripting.Attributes
{
    [S.AttributeUsage(S.AttributeTargets.Field)]
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        #region Public variables
        public readonly float rangeMin;
        public readonly float rangeMax;
        #endregion

        #region Constructors
        public MinMaxRangeAttribute(float rangeMin, float rangeMax)
        {
            this.rangeMin = rangeMin;
            this.rangeMax = rangeMax;
        }
        #endregion
    }
}