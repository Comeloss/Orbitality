using UnityEngine;

namespace Views
{
    public class InputSystem : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var playerId = Contexts.sharedInstance.game.player.PlanetId; 
                
                Contexts.sharedInstance.input.CreateEntity().ReplaceTriggerFire(playerId);
            }
        }
    }
}
