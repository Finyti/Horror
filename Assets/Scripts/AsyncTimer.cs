using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;


public class AsyncTimer
{
    public class EventTimer
    {
        public event EventHandler ProcessCompleted;
        public int Ms;

        public EventTimer(int ms)
        {
            Ms = ms;
        }
        public async void StartProcess()
        {
            await Task.Delay(Ms);
            OnProcessCompleted(EventArgs.Empty);
        }


        protected virtual void OnProcessCompleted(EventArgs e)
        {
            ProcessCompleted?.Invoke(this, e);
        }
    }
}

