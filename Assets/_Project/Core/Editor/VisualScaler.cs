using UnityEditor;
using UnityEngine;
using SpaceChaser.Core.Building;

public class VisualScaler
{
    [MenuItem("Tools/SetScale/Build")]
    public static void ScaleAllBuildVisuals()
    {
        if (Application.isPlaying)
        {
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab.TryGetComponent(out Build _))
            {
                Transform visualTransform = prefab.transform.Find("Visual");

                if (visualTransform != null)
                {
                    if (visualTransform.TryGetComponent(out SpriteRenderer _))
                    {
                        visualTransform.localScale = new Vector3(4f, 4f, 1f);
                    }
                }

                // if (prefab.TryGetComponent(out PolygonCollider2D col))
                // {
                //     Vector2[] pentagonPoints = new Vector2[3]
                //     {
                //         new Vector2(0f, 1f),             // Góra
                //         new Vector2(-0.951f, 0.309f),    // Lewy górny
                //         new Vector2(-0.588f, -0.809f),   // Lewy dół
                //     };

                //     col.pathCount = 1;
                //     col.SetPath(0, pentagonPoints);
                // }

                EditorUtility.SetDirty(prefab);
            }
        }

        AssetDatabase.SaveAssets();
    }
}