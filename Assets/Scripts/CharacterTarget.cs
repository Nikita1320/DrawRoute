using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CharacterTarget : MonoBehaviour, IDropHandler
{
    public Action droped;
    [SerializeField] private bool isAccess = true;

    public bool IsAccess { get => isAccess; }

    public void OnDrop(PointerEventData eventData)
    {
        if (isAccess)
        {
            droped?.Invoke();
            isAccess = false;
        }
    }
    public void Reset()
    {
        isAccess = true;
        droped = null;
    }
}
