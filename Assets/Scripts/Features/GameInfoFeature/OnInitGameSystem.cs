using System;
using System.Collections.Generic;
using Entitas;
using UniRx;

namespace Features.GameInfoFeature
{
    public class OnInitGameSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public OnInitGameSystem(Contexts context) : base(context.input)
        {
            _contexts = context;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.Restart);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.isRestart;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            Contexts.sharedInstance.game.CleanUp();

            Observable.Timer(TimeSpan.FromTicks(1)).Subscribe((_) =>
            {
                _contexts.input.CreateEntity().isInitGameplay = true;
                _contexts.game.ReplaceGamePlayState(GamePlayStateComponent.GamePlayStateType.Play);
            });
        }
    }
}