using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Extensions;
using Interfaces;
using UnityEngine;
using UniRx;

[DisallowMultipleComponent]
public abstract class ReactiveViewBase : ViewBase
{
    [Flags]
    protected enum ObserveFlags
    {
        None = 0,
        OnGroupAdded = 1 << 0,
        OnGroupRemoved = 1 << 1,
        WhenViewEnabled = 1 << 5,
        WhenViewDisabled = 1 << 6,
        DisposeOnDisable = 1 << 10,
        IncludeExistingEntities = 1 << 15,
        DestroyEntityAfterObserve = 1 << 16,
    }

    private readonly Dictionary<KeyValuePair<IDisposable, IDisposable>, ObserveFlags> _observers
        = new Dictionary<KeyValuePair<IDisposable, IDisposable>, ObserveFlags>();

    protected InputEntity CreateInputCallback()
    {
        return Contexts.sharedInstance.input.CreateEntity();
    }

    protected void SubscribeEntityWithComponents<TEntity, TResult>(
        IMatcher<TEntity> matcher,
        Func<TEntity, TResult> selector,
        Func<TResult, bool> filter,
        Action<TResult> onNext,
        Action<Exception> onError = null,
        ObserveFlags modifiers =
            ObserveFlags.OnGroupAdded |
            ObserveFlags.WhenViewEnabled |
            ObserveFlags.IncludeExistingEntities,
        ObserveFlags additionalModifiers = ObserveFlags.None)
        where TEntity : class, IEntity
    {
        ObserveEntityWithComponents(matcher, selector, filter, modifiers | additionalModifiers)
            .Subscribe(onNext.SafeInvoke, ex =>
            {
                Debug.LogError("[Reactive View] " + ex.ToString());

                onError.SafeInvoke(ex);
            });
    }

    protected void SubscribeEntitiesWithComponents<TEntity, TResult>(
        IMatcher<TEntity> matcher,
        Func<TEntity[], TResult> selector,
        Func<TResult, bool> filter,
        Action<TResult> onNext,
        Action<Exception> onError = null,
        ObserveFlags modifiers =
            ObserveFlags.OnGroupAdded |
            ObserveFlags.OnGroupRemoved |
            ObserveFlags.WhenViewEnabled |
            ObserveFlags.IncludeExistingEntities,
        ObserveFlags additionalModifiers = ObserveFlags.None)
        where TEntity : class, IEntity
    {
        ObserveEntitiesWithComponents(matcher, selector, filter, modifiers | additionalModifiers)
            .Subscribe(onNext.SafeInvoke, ex =>
            {
                Debug.LogError("[Reactive View] " + ex.ToString());

                onError.SafeInvoke(ex);
            });
    }

    protected UniRx.IObservable<TResult> ObserveEntityWithComponents<TEntity, TResult>(
        IMatcher<TEntity> matcher,
        Func<TEntity, TResult> selector = null,
        Func<TResult, bool> filter = null,
        ObserveFlags modifiers =
            ObserveFlags.OnGroupAdded |
            ObserveFlags.WhenViewEnabled |
            ObserveFlags.IncludeExistingEntities,
        ObserveFlags additionalModifiers = ObserveFlags.None)
        where TEntity : class, IEntity
    {
        var trigger = ObserveEntityFromEventInternal(matcher, 
            selector ?? (e => e.GetType() == typeof(TResult) ? (TResult) (object) e : default(TResult)),
            filter ?? (e => true), modifiers | additionalModifiers);

        var runtime = new ReplaySubject<TResult>();
        _observers.Add(new KeyValuePair<IDisposable, IDisposable>(trigger.Subscribe(runtime), runtime),
            modifiers | additionalModifiers);

        return runtime;
    }

    protected UniRx.IObservable<TResult> ObserveEntitiesWithComponents<TEntity, TResult>(
        IMatcher<TEntity> matcher,
        Func<TEntity[], TResult> selector = null,
        Func<TResult, bool> filter = null,
        ObserveFlags modifiers =
            ObserveFlags.OnGroupAdded |
            ObserveFlags.OnGroupRemoved |
            ObserveFlags.WhenViewEnabled |
            ObserveFlags.IncludeExistingEntities,
        ObserveFlags additionalModifiers = ObserveFlags.None)
        where TEntity : class, IEntity
    {
        var trigger = ObserveEntitiesFromEventInternal(matcher,
            selector ?? (e => e.GetType() == typeof(TResult) ? (TResult) (object) e : default(TResult)),
            filter ?? (e => true), modifiers | additionalModifiers);

        var runtime = new ReplaySubject<TResult>();
        _observers.Add(new KeyValuePair<IDisposable, IDisposable>(trigger.Subscribe(runtime), runtime),
            modifiers | additionalModifiers);

        return runtime;
    }

    protected IReadOnlyReactiveProperty<TResult> PersistEntityWithComponents<TEntity, TResult>(
        IMatcher<TEntity> matcher,
        Func<TEntity, TResult> selector = null,
        Func<TResult, bool> filter = null,
        ObserveFlags modifiers =
            ObserveFlags.OnGroupAdded |
            ObserveFlags.WhenViewEnabled |
            ObserveFlags.IncludeExistingEntities,
        ObserveFlags additionalModifiers = ObserveFlags.None)
        where TEntity : class, IEntity
    {
        var trigger = ObserveEntityFromEventInternal(matcher,
            selector ?? (e => e.GetType() == typeof(TResult) ? (TResult) (object) e : default(TResult)),
            filter ?? (e => true), modifiers | additionalModifiers);
        
        var runtime = new ReactiveProperty<TResult>(trigger);
        _observers.Add(new KeyValuePair<IDisposable, IDisposable>(runtime, null), modifiers | additionalModifiers);

        return runtime;
    }

    protected IReadOnlyReactiveProperty<TResult> PersistEntitiesWithComponents<TEntity, TResult>(
        IMatcher<TEntity> matcher,
        Func<TEntity[], TResult> selector = null,
        Func<TResult, bool> filter = null,
        ObserveFlags modifiers =
            ObserveFlags.OnGroupAdded |
            ObserveFlags.OnGroupRemoved |
            ObserveFlags.WhenViewEnabled |
            ObserveFlags.IncludeExistingEntities,
        ObserveFlags additionalModifiers = ObserveFlags.None)
        where TEntity : class, IEntity
    {
        var trigger = ObserveEntitiesFromEventInternal(matcher,
                selector ?? (e => e.GetType() == typeof(TResult) ? (TResult) (object) e : default(TResult)), 
                filter ?? (e => true), modifiers | additionalModifiers);
        ;
        var runtime = new ReactiveProperty<TResult>(trigger);
        _observers.Add(new KeyValuePair<IDisposable, IDisposable>(runtime, null), modifiers | additionalModifiers);

        return runtime;
    }

    private UniRx.IObservable<TResult> ObserveEntityFromEventInternal<TEntity, TResult>(
        IMatcher<TEntity> matcher,
        Func<TEntity, TResult> selector,
        Func<TResult, bool> filter,
        ObserveFlags modifiers) where TEntity : class, IEntity
    {
        var whenGroupAdded = (modifiers & ObserveFlags.OnGroupAdded) != 0;
        var whenGroupRemoved = (modifiers & ObserveFlags.OnGroupRemoved) != 0;
        if (!whenGroupAdded && !whenGroupRemoved)
        {
            return Observable.Throw<TResult>(new Exception("Empty subscription list is used"));
        }

        if ((modifiers & (ObserveFlags.WhenViewDisabled | ObserveFlags.WhenViewEnabled)) == 0)
        {
            return Observable.Throw<TResult>(new Exception("Empty trigger list is used"));
        }

        var targetCtx = Contexts.sharedInstance.allContexts.OfType<IContext<TEntity>>().FirstOrDefault();
        if (targetCtx == null)
        {
            return Observable.Throw<TResult>(new Exception("Unable to find any matching context for that entity"));
        }
        
        var group = targetCtx.GetGroup(matcher);
        return Observable.FromEvent<GroupChanged<TEntity>, TEntity>(
                handler => (GroupChanged<TEntity>) ((g, entity, ind, cmp) => handler(entity)),
                addHandler =>
                {
                    if (whenGroupAdded)
                    {
                        group.OnEntityAdded += addHandler;
                    }

                    if (whenGroupRemoved)
                    {
                        group.OnEntityRemoved += addHandler;
                    }
                },
                removeHandler =>
                {
                    if (whenGroupAdded)
                    {
                        group.OnEntityAdded -= removeHandler;
                    }

                    if (whenGroupRemoved)
                    {
                        group.OnEntityRemoved -= removeHandler;
                    }
                })
            .Where(entity => isActiveAndEnabled && (ObserveFlags.WhenViewEnabled & modifiers) != 0
                             || !isActiveAndEnabled && (ObserveFlags.WhenViewDisabled & modifiers) != 0)
            .StartWith((ObserveFlags.IncludeExistingEntities & modifiers) != 0 ? group : Enumerable.Empty<TEntity>())
            .Do(entity =>
            {
                var destroyableEntity = entity as IReadyToDestroy;
                if ((ObserveFlags.DestroyEntityAfterObserve & modifiers) != 0 && destroyableEntity != null)
                {
                    destroyableEntity.isReadyToDestroy = true;
                }
            })
            .Select(selector)
            .Where(filter)
            .CatchIgnore((Exception ex) =>
            {
                Debug.LogError(string.Format("[Reactive View {0}] {1}", ViewId, ex));
            });
    }

    private UniRx.IObservable<TResult> ObserveEntitiesFromEventInternal<TEntity, TResult>(
        IMatcher<TEntity> matcher,
        Func<TEntity[], TResult> selector,
        Func<TResult, bool> filter,
        ObserveFlags modifiers) where TEntity : class, IEntity
    {
        var whenGroupAdded = (modifiers & ObserveFlags.OnGroupAdded) != 0;
        var whenGroupRemoved = (modifiers & ObserveFlags.OnGroupRemoved) != 0;
        if (!whenGroupAdded && !whenGroupRemoved)
        {
            return Observable.Throw<TResult>(new Exception("Empty subscription list is used"));
        }

        if ((modifiers & (ObserveFlags.WhenViewDisabled | ObserveFlags.WhenViewEnabled)) == 0)
        {
            return Observable.Throw<TResult>(new Exception("Empty trigger list is used"));
        }

        var targetCtx = Contexts.sharedInstance.allContexts.OfType<IContext<TEntity>>().FirstOrDefault();
        if (targetCtx == null)
        {
            return Observable.Throw<TResult>(new Exception("Unable to find any matching context for that entity"));
        }

        var group = targetCtx.GetGroup(matcher);
        return Observable.FromEvent<GroupChanged<TEntity>, TEntity[]>(
                handler => (GroupChanged<TEntity>) ((g, entity, ind, cmp) => handler(group.ToArray())),
                addHandler =>
                {
                    if (whenGroupAdded)
                    {
                        group.OnEntityAdded += addHandler;
                    }

                    if (whenGroupRemoved)
                    {
                        group.OnEntityRemoved += addHandler;
                    }
                },
                removeHandler =>
                {
                    if (whenGroupAdded)
                    {
                        group.OnEntityAdded -= removeHandler;
                    }

                    if (whenGroupRemoved)
                    {
                        group.OnEntityRemoved -= removeHandler;
                    }
                })
            .StartWith((ObserveFlags.IncludeExistingEntities & modifiers) != 0
                ? new[] {group.ToArray()}
                : Enumerable.Empty<TEntity[]>())
            .Select(entities => group.ToArray())
            .Where(entities => entities != null)
            .Do(entities =>
            {
                if ((ObserveFlags.DestroyEntityAfterObserve & modifiers) == 0)
                {
                    return;
                }

                foreach (var entity in entities)
                {
                    var destroyableEntity = entity as IReadyToDestroy;
                    if (destroyableEntity != null)
                    {
                        destroyableEntity.isReadyToDestroy = true;
                    }
                }
            })
            .Select(selector)
            .Where(entities => isActiveAndEnabled && (ObserveFlags.WhenViewEnabled & modifiers) != 0
                               || !isActiveAndEnabled && (ObserveFlags.WhenViewDisabled & modifiers) != 0)
            .Where(filter)
            .CatchIgnore((Exception ex) =>
            {
                Debug.LogError(string.Format("[Reactive View {0}] {1}",ViewId, ex));
            });
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        foreach (var observer in _observers)
        {
            if (observer.Key.Key != null)
            {
                observer.Key.Key.Dispose();
            }

            if (observer.Key.Value != null)
            {
                observer.Key.Value.Dispose();
            }
        }

        _observers.Clear();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        var toRemove = new List<KeyValuePair<IDisposable, IDisposable>>();
        foreach (var observer in _observers)
        {
            if ((ObserveFlags.DisposeOnDisable & observer.Value) == 0)
            {
                continue;
            }

            if (observer.Key.Key != null)
            {
                observer.Key.Key.Dispose();
            }

            if (observer.Key.Value != null)
            {
                observer.Key.Value.Dispose();
            }

            toRemove.Add(observer.Key);
        }

        foreach (var observer in toRemove)
        {
            _observers.Remove(observer);
        }
    }
}