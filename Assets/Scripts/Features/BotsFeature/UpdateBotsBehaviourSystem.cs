using System.Collections.Generic;
using Entitas;

namespace Features.BotsFeature
{
    public class UpdateBotsBehaviourSystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;
        
        public UpdateBotsBehaviourSystem(Contexts context) : base(context.game)
        {
            _contexts = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.BotsClock);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasBotsClock;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var bots = _contexts.game.GetGroup(GameMatcher.BotAi);

            foreach (var botEntity in bots)
            {
                botEntity.ReplaceBotAi(botEntity.botAi.ReactionTicks,
                    botEntity.botAi.CanAct ? 0 : botEntity.botAi.CurrentTick + 1);
            }
        }
    }
}
