using System.Collections.Generic;
using Entitas;

namespace Features.Input
{
    public class CleanupInputContext : ICleanupSystem
    {
        private readonly InputContext _context;
        private readonly Queue<InputEntity> _queueToDestroy = new Queue<InputEntity>();

        public CleanupInputContext(InputContext inputContext)
        {
            _context = inputContext;
        }

        public void Cleanup()
        {
            while (_queueToDestroy.Count > 0)
            {
                _queueToDestroy.Dequeue().Destroy();
            }

            foreach (var inputEntity in _context.GetEntities())
            {
                _queueToDestroy.Enqueue(inputEntity);
            }
        }
    }
}