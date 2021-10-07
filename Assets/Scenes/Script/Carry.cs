//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Basic throwable object
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class Carry : MonoBehaviour
    {
        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.TurnOnKinematic;

        [Tooltip("The local point which acts as a positional and rotational offset to use while held")]
        public Transform attachmentOffset;

        public float scaleReleaseVelocity = 1.1f;

        [Tooltip("When detaching the object, should it return to its original parent?")]
        public bool restoreOriginalParent = false;


        protected bool attached = false;


        public UnityEvent onPickUp;
        public UnityEvent onDetachFromHand;




        [HideInInspector]
        public Interactable interactable;


        //-------------------------------------------------
        protected virtual void Awake()
        {

            interactable = GetComponent<Interactable>();
            if (attachmentOffset != null)
            {
                interactable.handFollowTransform = attachmentOffset;
            }

        }


        //-------------------------------------------------
        protected virtual void HandHoverUpdate(Hand hand)
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();

            if (startingGrabType == GrabTypes.Grip)
            {
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags, attachmentOffset);

            }
        }

        //-------------------------------------------------
        protected virtual void OnAttachedToHand(Hand hand)
        {


            attached = true;

            onPickUp.Invoke();

           // hand.HoverLock(null);

        }


        //-------------------------------------------------
        protected virtual void OnDetachedFromHand(Hand hand)
        {
            attached = false;

            onDetachFromHand.Invoke();

            //hand.HoverUnlock(null);

        }


      
        //-------------------------------------------------
        protected virtual void HandAttachedUpdate(Hand hand)
        {
          

            if (hand.IsGrabEnding(this.gameObject))
            {
                hand.DetachObject(gameObject, restoreOriginalParent);

            }
        }


        
    }

  
}

