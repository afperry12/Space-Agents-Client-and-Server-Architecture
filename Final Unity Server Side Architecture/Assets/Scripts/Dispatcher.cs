//using UnityEngine;
//using System.Collections.Generic;
//using System;

//public class ThreadManager
//{
//    private static readonly List<Action> executeOnMainThread = new List<Action>();
//    private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
//    public static bool actionToExecuteOnMainThread = false;

//    /// <summary>Sets an action to be executed on the main thread.</summary>
//    /// <param name="_action">The action to be executed on the main thread.</param>
//    public static void ExecuteOnMainThread(Action _action)
//    {
//        if (_action == null)
//        {
//            Debug.Log("No action to execute on main thread!");
//            return;
//        }

//        lock (executeOnMainThread)
//        {
//            executeOnMainThread.Add(_action);
//            actionToExecuteOnMainThread = true;
//        }
//    }

//    /// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
//    public static void UpdateMain()
//    {
//        if (actionToExecuteOnMainThread)
//        {
//            executeCopiedOnMainThread.Clear();
//            lock (executeOnMainThread)
//            {
//                executeCopiedOnMainThread.AddRange(executeOnMainThread);
//                executeOnMainThread.Clear();
//                actionToExecuteOnMainThread = false;
//            }

//            for (int i = 0; i < executeCopiedOnMainThread.Count; i++)
//            {
//                executeCopiedOnMainThread[i]();
//            }
//        }
//    }
//}


using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public class Dispatcher : MonoBehaviour
{
    public static void RunAsync(Action action)
    {
        ThreadPool.QueueUserWorkItem(o => action());
    }

    public static void RunAsync(Action<object> action, object state)
    {
        ThreadPool.QueueUserWorkItem(o => action(o), state);
    }

    public static void RunOnMainThread(Action action)
    {
        lock (_backlog)
        {
            _backlog.Add(action);
            _queued = true;
        }
    }

    //public static void RunOnMainThread(Action action)
    //{
    //    // Execute the action on the main thread using the InvokeOnAppThread method
    //    UnityEngine.WSA.Application.InvokeOnAppThread(() => { action(); }, false);
    //}



    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (_instance == null)
        {
            _instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();
            DontDestroyOnLoad(_instance.gameObject);
        }
    }

    private void Update()
    {
        if (_queued)
        {
            lock (_backlog)
            {
                var tmp = _actions;
                _actions = _backlog;
                _backlog = tmp;
                _queued = false;
            }

            foreach (var action in _actions)
                action();

            _actions.Clear();
        }
    }

    static Dispatcher _instance;
    static volatile bool _queued = false;
    static List<Action> _backlog = new List<Action>(8);
    static List<Action> _actions = new List<Action>(8);
}