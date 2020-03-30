using System;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class ViewBase : MonoBehaviour
{
    public string ViewId;
    private static Contexts _contexts;

    protected Contexts C
    {
        get
        {
            if (_contexts == null)
            {
                _contexts = Contexts.sharedInstance;
            }

            return _contexts;
        }
    }


    public string GetViewId()
    {
        return ViewId;
    }

    public string GetAliasName()
    {
        return GetAliasName(GetType());
    }

    public static string GetAliasName(Type viewType)
    {
        var alias = viewType.Name;
        alias = alias.EndsWith("View")
            ? alias
            : alias + "View";
        return alias;
    }
 
    protected virtual void ViewAwake()
    {
    }

    protected virtual void ViewStart()
    {
    }

    protected virtual void ViewDestroy()
    {
    }

    protected virtual void ViewEnable()
    {
    }

    protected virtual void ViewDisable()
    {
    }
    
    protected virtual void Awake()
    {
        ViewAwake();
    }
    
    protected virtual void Start()
    {
        ViewStart();
    }

    protected virtual void OnDestroy()
    {
        ViewDestroy();
    }

    protected virtual void OnEnable()
    {
        ViewEnable();
    }

    protected virtual void OnDisable()
    {
        ViewDisable();
    }
}