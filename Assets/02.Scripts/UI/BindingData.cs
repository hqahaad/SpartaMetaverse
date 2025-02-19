using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingData<T>
{
    private T value;

    public event Action<T> OnValueChanged = delegate { };

    public T Value
    {
        get { return value; }
        set
        {
            this.value = value;
            OnValueChanged?.Invoke(this.value);
        }
    }

    public BindingData(T value)
    {
        this.value = value;
    }

    public BindingData()
    {

    }
}
