﻿//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: For controlling in-game objects with tracked devices.
//
//=============================================================================

using UnityEngine;
using Valve.VR;

namespace Valve.VR
{
    public class SteamVR_TrackedObject_Tracker : MonoBehaviour
    {
        //public TrackerManager tracker_Manager;
        public TrackerManager_edited tracker_Manager;
        public enum EIndex
        {
            None = -1,
            Hmd = (int)OpenVR.k_unTrackedDeviceIndex_Hmd,
            Device1,
            Device2,
            Device3,
            Device4,
            Device5,
            Device6,
            Device7,
            Device8,
            Device9,
            Device10,
            Device11,
            Device12,
            Device13,
            Device14,
            Device15
        }

        public EIndex index;

        [Tooltip("If not set, relative to parent")]
        public Transform origin;

        public bool isValid { get; private set; }

        bool isListed = false;

        private void OnNewPoses(TrackedDevicePose_t[] poses)
        {
            if (index == EIndex.None)
                return;

            var i = (int)index;

            isValid = false;

            //트래커인지 판단.
            if (OpenVR.System.GetTrackedDeviceClass((uint)index) != ETrackedDeviceClass.GenericTracker)
                gameObject.SetActive(false);
            else
            {
                if(!isListed)
                {
                    tracker_Manager.TrackerList.Add(gameObject);
                    
                    Debug.Log("Tracker detect : " + gameObject.name);

                    isListed = true;
                }
                    
            }

            if (poses.Length <= i)
                return;

            if (!poses[i].bDeviceIsConnected)
                return;

            if (!poses[i].bPoseIsValid)
                return;

            isValid = true;

            var pose = new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

            if (origin != null)
            {
                transform.position = origin.transform.TransformPoint(pose.pos);
                transform.rotation = origin.rotation * pose.rot;
            }
            else
            {
                transform.localPosition = pose.pos;
                transform.localRotation = pose.rot;
            }
        }

        SteamVR_Events.Action newPosesAction;

        SteamVR_TrackedObject_Tracker()
        {
            newPosesAction = SteamVR_Events.NewPosesAction(OnNewPoses);
        }

        private void Awake()
        {
            OnEnable();
            //tracker_Manager = GameObject.Find("Player").GetComponent<TrackerManager>();
            tracker_Manager = GameObject.Find("Player").GetComponent<TrackerManager_edited>();
        }

        void OnEnable()
        {
            var render = SteamVR_Render.instance;
            if (render == null)
            {
                enabled = false;
                return;
            }

            newPosesAction.enabled = true;
        }

        void OnDisable()
        {
            newPosesAction.enabled = false;
            isValid = false;
        }

        public void SetDeviceIndex(int index)
        {
            if (System.Enum.IsDefined(typeof(EIndex), index))
                this.index = (EIndex)index;
        }
    }
}