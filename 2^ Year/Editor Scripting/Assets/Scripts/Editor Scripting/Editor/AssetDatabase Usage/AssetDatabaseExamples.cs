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
using System.IO;
using System.Linq;

namespace UnityEditor.Examples
{
    public static class AssetDatabaseExamples
    {
        #region Private methods
        [MenuItem("Assets/Create/Default Character Animator Controller")]
        private static void DoCreateDefaultCharacterAnimatorController()
        {
            //	Get destination path
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (!Directory.Exists(path))
                path = Path.GetDirectoryName(path);

            Debug.Log(path);

            //	Create assets
            Animations.AnimatorController controller = Animations.AnimatorController.CreateAnimatorControllerAtPath(Path.Combine(path, "Default Character Animator Controller.asset"));
            AnimationClip idleAnimation = MakeAnimationClip("Default Idle", true, controller);
            AnimationClip walkLeft180Animation = MakeAnimationClip("Default Walk Left 180°", true, controller);
            AnimationClip walkLeft135Animation = MakeAnimationClip("Default Walk Left 135°", true, controller);
            AnimationClip walkLeft90Animation = MakeAnimationClip("Default Walk Left 90°", true, controller);
            AnimationClip walkLeft45Animation = MakeAnimationClip("Default Walk Left 45°", true, controller);
            AnimationClip walkForward0Animation = MakeAnimationClip("Default Walk Forward 0°", true, controller);
            AnimationClip walkRight45Animation = MakeAnimationClip("Default Walk Right 45°", true, controller);
            AnimationClip walkRight90Animation = MakeAnimationClip("Default Walk Right 90°", true, controller);
            AnimationClip walkRight135Animation = MakeAnimationClip("Default Walk Right 135°", true, controller);
            AnimationClip walkRight180Animation = MakeAnimationClip("Default Walk Right 180°", true, controller);

            //	Create parameters
            controller.AddParameter("Speed", UnityEngine.AnimatorControllerParameterType.Float);
            controller.AddParameter("Angle", UnityEngine.AnimatorControllerParameterType.Float);

            //	Get reference to state machine
            Animations.AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;

            //	Create states
            Animations.AnimatorState idleState = stateMachine.AddState("Idle");
            Animations.AnimatorState walkState = stateMachine.AddState("Walk");

            //	Create blend trees
            Animations.BlendTree walkBlendTree = new Animations.BlendTree();
            walkBlendTree.name = "Walk Blend Tree";
            walkBlendTree.blendParameter = "Angle";
            walkBlendTree.blendType = Animations.BlendTreeType.Simple1D;
            walkBlendTree.useAutomaticThresholds = false;
            walkBlendTree.AddChild(walkLeft180Animation, -180.0f);
            walkBlendTree.AddChild(walkLeft135Animation, -135.0f);
            walkBlendTree.AddChild(walkLeft90Animation, -90.0f);
            walkBlendTree.AddChild(walkLeft45Animation, -45.0f);
            walkBlendTree.AddChild(walkForward0Animation, 0.0f);
            walkBlendTree.AddChild(walkRight45Animation, 45.0f);
            walkBlendTree.AddChild(walkRight90Animation, 90.0f);
            walkBlendTree.AddChild(walkRight135Animation, 135.0f);
            walkBlendTree.AddChild(walkRight180Animation, 180.0f);

            //	Add motion to states
            idleState.motion = idleAnimation;
            walkState.motion = walkBlendTree;
            stateMachine.defaultState = idleState;

            //	Create transitions
            Animations.AnimatorStateTransition idleToWalk = idleState.AddTransition(walkState, false);
            idleToWalk.AddCondition(Animations.AnimatorConditionMode.Greater, 0.01f, "Speed");
            Animations.AnimatorStateTransition walkToIdle = walkState.AddTransition(idleState, false);
            walkToIdle.AddCondition(Animations.AnimatorConditionMode.Less, 0.01f, "Speed");

            //	Consolidate AssetDatabase
            EditorUtility.SetDirty(controller);
            AssetDatabase.SaveAssetIfDirty(controller);

            //	Select created asset
            Selection.activeObject = controller;
        }

        private static AnimationClip MakeAnimationClip(string name, bool loop = true, Object addToAsset = null)
        {
            AnimationClip clip = new AnimationClip();
            clip.name = name;

            if (loop)
            {
                AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
                settings.loopTime = true;
                settings.loopBlend = true;
                AnimationUtility.SetAnimationClipSettings(clip, settings);
            }

            if (addToAsset != null)
                AssetDatabase.AddObjectToAsset(clip, addToAsset);

            return clip;
        }
        #endregion
    }
}