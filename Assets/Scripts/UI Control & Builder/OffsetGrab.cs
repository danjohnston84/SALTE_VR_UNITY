using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OffsetGrab : XRGrabInteractable
{

    private Vector3 interactorPostion = Vector3.zero;
    private Quaternion interactionRotation = Quaternion.identity;


    protected override void OnSelectEnter(XRBaseInteractor interactor)
    {
        base.OnSelectEnter(interactor);
        StoreInteractor(interactor);
        MatchAttachmentPoints(interactor);
    }

    private void StoreInteractor(XRBaseInteractor interactor)
    {
        interactorPostion = interactor.attachTransform.localPosition;
        interactionRotation = interactor.attachTransform.localRotation;
    }

    private void MatchAttachmentPoints(XRBaseInteractor interactor)
    {
        bool hasAttach = attachTransform != null;
        interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
        interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
    }

    protected override void OnSelectExit(XRBaseInteractor interactor)
    {
        base.OnSelectExit(interactor);
        ResetAttchmentPoint(interactor);
        ClearInteractor(interactor);
    }

   private void ResetAttchmentPoint(XRBaseInteractor interactor)
    {
         interactor.attachTransform.localPosition = interactorPostion;
         interactor.attachTransform.localRotation = interactionRotation;
    }

    private void ClearInteractor(XRBaseInteractor interactor)
    {
        interactorPostion = Vector3.zero;
        interactionRotation = Quaternion.identity;
    }


}
