using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class OnRestartGameSystem : ReactiveSystem<InputEntity>
{
    private Contexts _contexts;
    public OnRestartGameSystem(Contexts context) : base(context.input)
    {
        _contexts = context;
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return null;// context.CreateCollector(InputMatcher.re)
    }

    protected override bool Filter(InputEntity entity)
    {
        throw new System.NotImplementedException();
    }

    protected override void Execute(List<InputEntity> entities)
    {
        throw new System.NotImplementedException();
    }
}
