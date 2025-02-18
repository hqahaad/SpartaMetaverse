using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisitor 
{
    void Visit<T>(T visitable) where T : Component, IVisitable;
    void Leave<T>(T visitable) where T : Component, IVisitable;
}
