using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Adventurer_Ability1 : Ability
{
    public Damage damageToDo;

    public Vector2 originOffset;
    public Vector2 direction;
    public float checkDistance;
    public ContactFilter2D checkContactFilter;
    
    public override void Use()
    {
        Debug.Log("Adventurer Uses " + AbilityName);

        Vector2 origin = new Vector2(transform.position.x + originOffset.x, transform.position.y + originOffset.y);
        
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        Physics2D.Raycast(origin, direction, checkContactFilter, hits, checkDistance);

        if(hits.Count > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                hit.transform.gameObject.TryGetComponent<Health>(out Health targetHealth);
                if(targetHealth != null) 
                {
                    targetHealth.Damage(damageToDo);
                    Debug.Log("Adventurer Ability " + AbilityName + ": target found");
                }
            }
        }
        else
        {
            Debug.Log("Adventurer Ability " + AbilityName + ": no targets found");
        }

        base.Use();
    }
}