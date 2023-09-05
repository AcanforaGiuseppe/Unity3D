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

namespace UnityEditor.AssetsPostProcessing
{
    public class TexturesPostprocessor : AssetPostprocessor
    {
        #region Private methods
        void OnPreprocessTexture()
        {
            //	Do not preprocess textures belonging to others
            if (!assetPath.ToLower().StartsWith("assets/graphics/"))
                return;

            //	The name of the imported file, without extension
            string fileName = Path.GetFileNameWithoutExtension(assetPath);

            //	Do not process excluded files
            if (AssetDatabase.GetLabels(assetImporter).Any<string>(l => !string.IsNullOrEmpty(l) && l.ToLower().Trim() == "custom"))
                return;

            //	The specialized asset importer for textures
            TextureImporter texImporter = assetImporter as TextureImporter;

            //	Handle sprites
            if (assetPath.ToLower().Contains(@"/sprites/") || assetPath.ToLower().Contains(@"/ui/"))
                texImporter.textureType = TextureImporterType.Sprite;

            //	Handle Normal Maps
            else if (fileName.ToLower().EndsWith("_normal") || fileName.ToLower().EndsWith("_nrm"))
                texImporter.textureType = TextureImporterType.NormalMap;

            //	Handle linear textures
            else if (fileName.ToLower().EndsWith("_orm") || fileName.ToLower().EndsWith("_ao") || fileName.ToLower().EndsWith("_occlusion") || fileName.ToLower().EndsWith("_ambientocclusion") || fileName.ToLower().EndsWith("_ambient_occlusion") || fileName.ToLower().EndsWith("_metallic") || fileName.ToLower().EndsWith("_metallicsmoothness"))
                texImporter.sRGBTexture = false;
        }
        #endregion
    }
}