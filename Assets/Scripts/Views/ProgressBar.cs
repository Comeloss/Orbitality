using UnityEngine;

namespace Views
{
    public class ProgressBar : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private SpriteRenderer _barFill;
#pragma warning restore 0649

        private float _fullBar;
        [Range(0f, 1f)] private float _currentProportion;
        
        private void Awake()
        {
            _fullBar = _barFill.size.x;
        }

        public void UpdateBar(float proportion)
        {
            _currentProportion = proportion;

            var size = _barFill.size;
            size.x = _fullBar * _currentProportion;
            _barFill.size = size;
        }
    }
}
