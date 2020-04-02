using System;
using System.Collections.Generic;
using Entitas;

namespace Features.GameInfoFeature
{
    public class SwitchGamePlayStateSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public SwitchGamePlayStateSystem(Contexts context) : base(context.input)
        {
            _contexts = context;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.SwitchGamePlayState);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.isSwitchGamePlayState &&
                   _contexts.game.hasGamePlayState;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            switch (_contexts.game.gamePlayState.CurrentState)
            {
                case GamePlayStateComponent.GamePlayStateType.Play:
                    _contexts.game.ReplaceGamePlayState(GamePlayStateComponent.GamePlayStateType.Pause);
                    break;
                case GamePlayStateComponent.GamePlayStateType.Pause:
                    _contexts.game.ReplaceGamePlayState(GamePlayStateComponent.GamePlayStateType.Play);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}