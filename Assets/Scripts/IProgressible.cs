using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgressible
{
    public event EventHandler<ProgressChangedEventArgs> progressChanged;
    public class ProgressChangedEventArgs : EventArgs {
        public float currentProgress;
        public float maxProgress;
    }
}
