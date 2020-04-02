using System.Collections.Generic;
using Entitas;

namespace Features.SolarSystemFeature
{
    public sealed class DestroyEntitiesGameSystem : ICleanupSystem 
    {
        private IGroup<GameEntity> _toRemove;
        private readonly GameContext _context;
        private readonly Queue<GameEntity> _queueToDestroy = new Queue<GameEntity>();

        public DestroyEntitiesGameSystem(GameContext gameContext)
        {
            _context = gameContext;
            _toRemove = _context.GetGroup(GameMatcher.ReadyToDestroy);
        }

        public void Cleanup()
        {
            while (_queueToDestroy.Count > 0)
            {
                var e = _queueToDestroy.Dequeue();
                if (e.isEnabled)
                {
                    e.Destroy();
                }
            }
            foreach (var e in _toRemove.GetEntities())
            {
                _queueToDestroy.Enqueue(e);
            }
        }
    }
}