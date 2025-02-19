using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractionGameObject : MonoBehaviour, IVisitor
{
    private PlayerEntity entityCache = null;

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerEntity entity)
        {
            entity.OnInteractionCallback += Interaction;
            entityCache = entity;
        }
    }

    public void Leave<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerEntity entity)
        {
            entity.OnInteractionCallback -= Interaction;
            entityCache = null;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<IVisitable>()?.Accept(this);
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<IVisitable>()?.Cancel(this);
    }

    protected void Interaction(InputAction.CallbackContext context)
    {
        if (entityCache != null)
        {
            OnInteraction(entityCache);
        }
    }

    protected abstract void OnInteraction(PlayerEntity entity);
}
