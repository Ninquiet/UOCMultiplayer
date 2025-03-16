using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Complete.CineMachine
{
    public class CMCameraController : MonoBehaviour
    {
        [SerializeField] 
        private CinemachineCamera _cinemachineCamera;
        [SerializeField]
        private CinemachineBrain _cinemachineBrain;
        [SerializeField]
        private Camera _camera;

        private Tween _currentTween;
        private const float TRANSITION_DURATION = 1.4f;
        
        public void SetTarget(Transform target, int channel)
        {
            _cinemachineCamera.Follow = target;
            OutputChannels enumChannel = GetEnumChannel(channel);
            
            _cinemachineCamera.OutputChannel = enumChannel;
            _cinemachineBrain.ChannelMask = enumChannel;
        }

        public void SetViewportRect(float x, float y, float w, float h)
        {
            if (_currentTween != null)
                _currentTween.Kill();
            
            var sequence = DOTween.Sequence();
            
            sequence.Append( DOTween.To(() => _camera.rect, x => _camera.rect = x, new Rect(x, y, w, h), TRANSITION_DURATION) );
        }
 
        private OutputChannels GetEnumChannel(int channel)
        {
            var bitShift = 1 << (channel - 1);
            var enumChannel = (OutputChannels)bitShift;
            return enumChannel;
        }
    }
}