using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Features.GameInfoFeature
{
    public class ResetTimeSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public ResetTimeSystem(Contexts context) : base(context.input)
        {
            _contexts = context;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.InitGameplay);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.isInitGameplay;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            _contexts.game.ReplaceGameTime(0);
            _contexts.game.ReplaceGameTimeMilliseconds(0);
            _contexts.game.ReplaceGameDeltaTime(Time.time);
        }
    }
}