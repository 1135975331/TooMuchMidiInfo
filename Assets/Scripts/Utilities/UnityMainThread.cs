using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// https://stackoverflow.com/questions/53916533/setactive-can-only-be-called-from-the-main-thread
/// </summary>
internal class UnityMainThread : MonoBehaviour
{
	internal static UnityMainThread Wkr;
	private readonly Queue<Action> jobs = new Queue<Action>();

	private void Awake() {
		Wkr = this;
	}

	private void Update() {
		while (jobs.Count > 0) 
			jobs.Dequeue().Invoke();
	}

	internal void AddJob(Action newJob) {
		jobs.Enqueue(newJob);
	}
}