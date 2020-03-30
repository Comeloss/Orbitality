using UnityEngine;

namespace Views
{
    public class InputSystem : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                var playerId = Contexts.sharedInstance.game.player.PlaneId; 
                
                Contexts.sharedInstance.input.CreateEntity().ReplaceTriggerFire(playerId);
            }
        }
    }
}
