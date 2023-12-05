using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Pulse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool UseMouse;
    [SerializeField] float Ascelation;

    // Update is called once per frame
    void Update()
    {
        
    }

    Tween myTween;

    void Start()
    {
        
        // Create and store the tween so you can reuse it,
        // also add infinite loops (so it pulses until it's manually stopped/rewinded)
        // and set its autoKill status to FALSE, so it won't be automatically
        // destroyed when completed.
        // Finally, set its state to paused, so it will be played only on mouse enter
        myTween = transform.DOScale(Ascelation, 1).SetLoops(-1, LoopType.Yoyo).SetAutoKill(false).Pause();
        if (!UseMouse) myTween.Play();
    }

   

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UseMouse)
            myTween.Play();
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // CHOOSE:
        /*if (UseMouse)
            myTween.Rewind();*/
        Debug.Log("EXIT");
        if (UseMouse)
            myTween.SmoothRewind();
        if (UseMouse)
            myTween = transform.DOScale(Ascelation, 1).SetLoops(-1, LoopType.Yoyo).SetAutoKill(false).Pause();
    }
}
