using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisitable
{
    void Accept(IVisitor visitor);
    void Cancel(IVisitor visitor);
}
