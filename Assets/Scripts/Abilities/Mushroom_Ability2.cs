using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Mushroom_Ability2
 : Ability
{
    public Damage damageToDo;

    public Vector2 originOffset;
    public Vector2 direction;
    public float checkDistance;
    public ContactFilter2D checkContactFilter;
    
    public override void Use()
    {
        Debug.Log("Mushroom Uses " + AbilityName);

        Vector2 origin = new Vector2(transform.position.x + originOffset.x, transform.position.y + originOffset.y);
        
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        Physics2D.Raycast(origin, direction, checkContactFilter, hits, checkDistance);

        if(hits.Count > 0)
        {
            Debug.Log("Mushroom Ability " + AbilityName + ": hit " + hits.Count + " targets");
            
            foreach (RaycastHit2D hit in hits)
            {
                hit.transform.gameObject.TryGetComponent<Health>(out Health targetHealth);
                if(targetHealth != null) 
                {
                    targetHealth.Damage(damageToDo);
                    Debug.Log("Mushroom Ability " + AbilityName + ": target found");
                }
                else
                {
                    Debug.Log("Mushroom Ability " + AbilityName + ": target with no health component " + hit.transform.gameObject.name);
                }
            }
        }
        else
        {
            Debug.Log("Mushroom Ability " + AbilityName + ": no targets found");
        }

        base.Use();
    }
}