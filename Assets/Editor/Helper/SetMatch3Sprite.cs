using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class SetMatch3Sprite : MonoBehaviour
{
    private const string MENU_ROOT = "Tools/Link Sprite/";
    
    [MenuItem(MENU_ROOT + "Set", false, 50)]
    
    public static void SetSpriteCurrentScene()
    {
        var sceneObjects = Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go))
                         && go.hideFlags == HideFlags.None).ToArray();

        List<TogglesStatus> findedToggleStatus = new List<TogglesStatus>();
        SpriteContainer spriteContainer = null;
        foreach (var go in sceneObjects)
        {
            var components = go.GetComponents<Component>();
            foreach (var component in components)
            {
                var Container = component as SpriteContainer;
                if (Container != null)
                {
                    spriteContainer = Container;
                }
                
                var c = component as TogglesStatus;
                if (c != null)
                {
                    findedToggleStatus.Add(c);
                }    
            }
        }

        foreach (var ts in findedToggleStatus)
        {
            if (ts.GetStatus._status == ToggleStatus.StatusKinds.EID)
            {
                Sprite sp = spriteContainer.GetSpriteOrNull((EID)ts.GetStatus.Value, (EColor) 0, 1);
                if (sp != null)
                {
                    Image img = ts.GetComponent<Image>();
                    if (null != img)
                    {
                        Debug.Log(sp.name);
                        img.sprite = sp;
                        EditorUtility.SetDirty(ts);
                    }
                        
                }
                else
                {
                    Debug.LogError((EID)ts.GetStatus.Value);
                }
            }
        }
    }

    [MenuItem(MENU_ROOT + "SetCell", false, 50)]
    public static void SetCell()
    {
        var sceneObjects = Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go))
                         && go.hideFlags == HideFlags.None).ToArray();

        foreach (var go in sceneObjects)
        {
            var components = go.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component.name.Contains("Cover"))
                {
                    
                }
            }
        }
    }
}
