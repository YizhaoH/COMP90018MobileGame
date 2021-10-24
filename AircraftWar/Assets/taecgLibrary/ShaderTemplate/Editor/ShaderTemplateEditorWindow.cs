/**
 * @file         ShaderTemplateEditorWindow.cs
 * @author       Hongwei Li(taecg@qq.com)
 * @copyright    2017-2019 onemt
 * @created      2020-05-14
 * @updated      2020-05-14
 *
 * @brief        扩展Shader创建时的模版
 */

#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace taecg.tools
{
    public class ShaderTemplateEditorWindow : EditorWindow
    {
        #region 数据成员
        #endregion

        #region 编缉器入口
        [MenuItem("Assets/Create/Shader/Unlit URP Shader")]
        static void UnlitURPShader()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            string templatePath = AssetDatabase.GUIDToAssetPath("20733eab49936844bb0c3331d8e78ffb");
            string newPath = string.Format("{0}/New Unlit URP Shader.shader", path);
            AssetDatabase.CopyAsset(templatePath, newPath);
            AssetDatabase.ImportAsset(newPath);
        }
        #endregion
    }
}
#endif