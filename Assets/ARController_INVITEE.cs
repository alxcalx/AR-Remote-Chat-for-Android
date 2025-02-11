﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using Firebase;
    using Firebase.Unity.Editor;
    using Firebase.Database;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;

#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class ARController_INVITEE : MonoBehaviour
    {

        
        

        private Animation anim;
        string inviter_charcater;


        public GameObject andyObject { get; private set; }

        GameObject prefab;
        public int numberOfObjectsAllowed = 1;
        private int currentNumberOfObjects = 0;

        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject CharacterPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a feature point.
        /// </summary>
        public GameObject CharacterPointPrefab;

        /// <summary>
        /// A game object parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;

        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        public GameObject PlaceCharacterOnSurface;

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        /// 
        
        public void Start()
        {
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");
            SceneController sceneController = new SceneController();

            DetectedPlanePrefab.SetActive(true);
            SearchingForPlaneUI.SetActive(true);

            FirebaseDatabase.DefaultInstance
         .GetReference("sessions").Child(sceneController.GetReceiverId()).Child(sceneController.GetSessionID()).Child("inviter_animation")
    .ValueChanged += (object sender2, ValueChangedEventArgs e3) =>
    {
        if (e3.DatabaseError != null)
        {
            Debug.LogError(e3.DatabaseError.Message);
            return;
        }


             anim = andyObject.GetComponent<Animation>();
             anim.Play(e3.Snapshot.Value.ToString());





    };

            FirebaseDatabase.DefaultInstance
         .GetReference("sessions").Child(sceneController.GetReceiverId()).Child(sceneController.GetSessionID()).Child("inviter_message")
                 .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
                 {
                     if (e2.DatabaseError != null)
                     {
                         Debug.LogError(e2.DatabaseError.Message);
                         return;
                     }

                     string messagetext = e2.Snapshot.Value.ToString();

                     andyObject.transform.Find("Message").GetComponent<TextMesh>().text = messagetext;

                     Debug.Log(messagetext);



                 };

            FirebaseDatabase.DefaultInstance
          .GetReference("sessions").Child(sceneController.GetReceiverId()).Child(sceneController.GetSessionID()).Child("inviter_character")
              .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
              {
                  if (e2.DatabaseError != null)
                  {
                      Debug.LogError(e2.DatabaseError.Message);
                      return;
                  }



                  inviter_charcater = e2.Snapshot.Value.ToString();
                  Debug.Log(inviter_charcater);


              };


        }
        public void Update()
        {

            _UpdateApplicationLifecycle();
            bool TapOnSurface = false;
           
            // Hide snackbar when currently tracking at least one plane.
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            bool showSearchingUI = true;
            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    showSearchingUI = false;

                    if (currentNumberOfObjects == 0)
                    {

                        TapOnSurface = true;
                    }
                    else
                    {

                        TapOnSurface = false;
                        DetectedPlanePrefab.SetActive(false);
                    }

                    break;
                }
            }

            PlaceCharacterOnSurface.SetActive(TapOnSurface);
            SearchingForPlaneUI.SetActive(showSearchingUI);

            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                if (currentNumberOfObjects < numberOfObjectsAllowed)
                {
                    currentNumberOfObjects = currentNumberOfObjects + 1;

                    if(andyObject != null)
                    {
                        TapOnSurface = false;

                    }
                    
                    // Use hit pose and camera pose to check if hittest is from the
                    // back of the plane, if it is, no need to create the anchor.
                    if ((hit.Trackable is DetectedPlane) &&
                        Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                            hit.Pose.rotation * Vector3.up) < 0)
                    {
                        Debug.Log("Hit at back of the current DetectedPlane");
                    }
                    else
                    {
                        // Choose the Andy model for the Trackable that got hit.

                        if (hit.Trackable is FeaturePoint)
                        {
                            prefab = CharacterPointPrefab;
                        }
                        else
                        {
                            prefab = CharacterPlanePrefab;
                        }

                        // Instantiate Andy model at the hit pose.
                        andyObject = Instantiate(prefab.transform.Find(inviter_charcater).gameObject, hit.Pose.position, hit.Pose.rotation);

                        // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                        andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

                        // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                        // world evolves.
                        var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                        // Make Andy model a child of the anchor.
                        andyObject.transform.parent = anchor.transform;
                    }

                }
                
            }
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
