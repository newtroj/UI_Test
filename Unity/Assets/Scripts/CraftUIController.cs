using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Playables;

public class CraftUIController : MonoBehaviour
{
    [SerializeField] private PlayableDirector _enableCraftButtonDirector;
    [SerializeField] private PlayableDirector _showCraftDetailsScreenDirector;
    [SerializeField] private PlayableDirector _loopCraftDetailsScreenDirector;
    [SerializeField] private PlayableDirector _showCraftProgressDirector;
    [SerializeField] private PlayableDirector _showCraftDNAScreenDirector;
    [SerializeField] private PlayableDirector _loopCraftDNAScreenDirector;
    [SerializeField] private ParticleSystem _buttonParticles;

    [SerializeField] private float _scaleUp = 1.2f;
    [SerializeField] private float _fastAnimationDuration = 0.5f;
    [SerializeField] private float _slowAnimationDuration = 1f;
    [SerializeField] private Transform _craftButtonTransform;

    private TweenerCore<Vector3, Vector3, VectorOptions> currentTween = null;

    public void EnableCraftButton()
    {
        _enableCraftButtonDirector.Play();
        _enableCraftButtonDirector.stopped += OnCraftButtonEnabled;
    }
    
    public void DisableCraftButton()
    {
        StopCraftButtonAnimation();
        ResetTimeline(_enableCraftButtonDirector);
    }

    private void OnCraftButtonEnabled(PlayableDirector director)
    {
        director.stopped -= OnCraftButtonEnabled;
        PlayCraftButtonAnimation(_slowAnimationDuration);
        OpenDetailsScreen();
        _buttonParticles.Play();
    }

    private void PlayCraftButtonAnimation(float duration)
    {
        StopCraftButtonAnimation();
        currentTween = _craftButtonTransform.DOScale(Vector3.one * _scaleUp, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void StopCraftButtonAnimation()
    {
        currentTween?.Kill();
        _craftButtonTransform.localScale = Vector3.one;
        _buttonParticles.Stop();
    }

    public void OnCraftButtonPressed()
    {
        if(!CraftController.Instance.CanCraft() || _showCraftProgressDirector.state == PlayState.Playing)
            return;
        
        StartCraftProgress();
        PlayCraftButtonAnimation(_fastAnimationDuration);
    }
    
    public void OnCraftButtonReleased()
    {
        if(_showCraftProgressDirector.state != PlayState.Playing)
            return;
        
        _showCraftProgressDirector.stopped -= OnProgressDirectorFinished;
        ResetTimeline(_showCraftProgressDirector);

        PlayCraftButtonAnimation(_slowAnimationDuration);
    }

    private void ResetTimeline(PlayableDirector director)
    {
        director.Stop();
        director.time = 0;
        director.Evaluate();
    }

    private void OpenDetailsScreen()
    {
        _showCraftDetailsScreenDirector.Play();
        _showCraftDetailsScreenDirector.stopped += OnShowDetailsScreenDirectorFinished;
    }

    private void OnShowDetailsScreenDirectorFinished(PlayableDirector obj)
    {
        obj.stopped -= OnShowDetailsScreenDirectorFinished;
        _loopCraftDetailsScreenDirector.Play();
    }

    private void StartCraftProgress()
    {
        _showCraftProgressDirector.Play();
        _showCraftProgressDirector.stopped += OnProgressDirectorFinished;
    }

    private void OnProgressDirectorFinished(PlayableDirector director)
    {
        director.stopped -= OnProgressDirectorFinished;
        _showCraftDNAScreenDirector.Play();
        _showCraftDNAScreenDirector.stopped += Cleanup;
    }

    private void Cleanup(PlayableDirector obj)
    {
        obj.stopped -= Cleanup;
        
        _loopCraftDNAScreenDirector.Play();
        
        StopCraftButtonAnimation();
        
        _loopCraftDetailsScreenDirector.Stop();
        ResetTimeline(_showCraftProgressDirector);
        ResetTimeline(_showCraftDetailsScreenDirector);
        ResetTimeline(_enableCraftButtonDirector);

        CraftController.Instance.OnCraftFinished();
    }
}