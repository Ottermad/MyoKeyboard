using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class MyoState : MonoBehaviour {

	public GameObject myo = null;
	public Text test;

	private Pose _lastPose = Pose.Unknown;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Access the ThalmicMyo component attached to the Myo game object.
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		// Check if the pose has changed since last update.
		// The ThalmicMyo component of a Myo game object has a pose property that is set to the
		// currently detected pose (e.g. Pose.Fist for the user making a fist). If no pose is currently
		// detected, pose will be set to Pose.Rest. If pose detection is unavailable, e.g. because Myo
		// is not on a user's arm, pose will be set to Pose.Unknown.
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;

			// Vibrate the Myo armband when a fist is made.
			if (thalmicMyo.pose == Pose.Fist) {
				thalmicMyo.Vibrate (VibrationType.Medium);

				test.text = "Fist";

				ExtendUnlockAndNotifyUserAction (thalmicMyo);

				// Change material when wave in, wave out or double tap poses are made.
			} else if (thalmicMyo.pose == Pose.WaveIn) {
				test.text = "WaveIn";
			} else if (thalmicMyo.pose == Pose.WaveOut) {
				test.text = "WaveOut";
			} else if (thalmicMyo.pose == Pose.DoubleTap) {
				test.text = "Double Tap";
			} else if (thalmicMyo.pose == Pose.FingersSpread) {
				test.text = "Fingers Spread";
			} else if (thalmicMyo.pose == Pose.Rest) {
				test.text = "Rest";
			} else {
				test.text = "Unknown";
			}
		}
	}

	// Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
	// recognized.
	void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo)
	{
		ThalmicHub hub = ThalmicHub.instance;

		if (hub.lockingPolicy == LockingPolicy.Standard) {
			myo.Unlock (UnlockType.Timed);
		}

		myo.NotifyUserAction ();
	}
}
