using System;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager : GameSingleton
{
    internal delegate void TransitionAction();

    [Header("Screen Fade")]
    [SerializeField] CanvasGroup fadeCanvas;
    [SerializeField] float fadeDuration = 1;

    internal void MakeScreenTransition(Func<Task> duringTransition = null, Action finished = null, float delayFadeOut = 0.5f)
    {
        LeanTween.alphaCanvas(fadeCanvas, 1, fadeDuration).setOnComplete(async () =>
        {
            if (duringTransition != null) await duringTransition();
            LeanTween.alphaCanvas(fadeCanvas, 0, fadeDuration).setDelay(delayFadeOut);
        });

        if (finished != null) finished();

    }

}
