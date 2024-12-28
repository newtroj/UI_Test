using UnityEngine;
using UnityEngine.Playables;

public class CraftController : MonoBehaviour
{
    [SerializeField] private PlayableDirector _showCraftDetailsScreenDirector;
    [SerializeField] private PlayableDirector _showCraftProgressDirector;
    [SerializeField] private PlayableDirector _showCraftDNAScreenDirector;

    private void OnEnable()
    {
        return;
        _showCraftDetailsScreenDirector.Play();
        _showCraftDetailsScreenDirector.stopped += OnDetailsDirectorFinished;
    }

    private void OnDetailsDirectorFinished(PlayableDirector director)
    {
        director.stopped -= OnDetailsDirectorFinished;
        _showCraftProgressDirector.Play();
        _showCraftProgressDirector.stopped += OnProgressDirectorFinished;
    }

    private void OnProgressDirectorFinished(PlayableDirector director)
    {
        director.stopped -= OnProgressDirectorFinished;
        _showCraftDNAScreenDirector.Play();
    }
}