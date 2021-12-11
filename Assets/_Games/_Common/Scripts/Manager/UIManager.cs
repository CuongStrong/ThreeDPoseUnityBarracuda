using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public partial class UIManager : MonoBehaviourPersistence<UIManager>
{
    public Dictionary<string, BaseMenu> menus = new Dictionary<string, BaseMenu>();
    public Dictionary<string, BasePopup> popups = new Dictionary<string, BasePopup>();

    private Transform menuRoot, popupRoot;

    private void Awake()
    {
        menuRoot = new GameObject("MenuRoot").transform;
        menuRoot.SetParent(transform);

        popupRoot = new GameObject("PopupRoot").transform;
        popupRoot.SetParent(transform);

        GetMenu(ResourcesPath.PREFAB_JOYSTICK).Show();
    }

    public BaseMenu GetMenu(string path)
    {
        if (!menus.TryGetValue(path, out BaseMenu menu))
        {
            BaseMenu prefab = Resources.Load<BaseMenu>(path);

            if (prefab == null) Debug.LogError(string.Format("Cannot load menu {0}", path));

            menu = Instantiate(prefab, menuRoot);
            menus.Add(path, menu);
        }

        return menu;
    }

    public BasePopup GetPopup(string path)
    {
        if (!popups.TryGetValue(path, out BasePopup popup))
        {
            BasePopup prefab = Resources.Load<BasePopup>(path);

            if (prefab == null) Debug.LogError(string.Format("Cannot load popup {0}", path));

            popup = Instantiate(prefab, popupRoot);
            popups.Add(path, popup);
        }

        return popup;
    }
}
