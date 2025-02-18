using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingData<T>
{
    private T _value;

    public event Action<T> OnValueChanged = delegate { };

    public T Value
    {
        get { return _value; }
        set
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }

    public BindingData(T value)
    {
        _value = value;
    }

    public BindingData()
    {

    }
}
