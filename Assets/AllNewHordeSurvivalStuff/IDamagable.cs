using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IDamagable : MonoBehaviour
{
    [SerializeField] GameObject DamageNumbersPrefab;
    float MaxHealth { get; set; }
    float Health { get; set; }
    float Resistance { get; set; }
    float Worth { get; set; }




    public void OnDamage(Vector3 hitpoint, float DamageAmount,GameObject Damager)
    {
        Health = Mathf.Clamp(Health - DamageAmount, 0, MaxHealth);
        var number = Instantiate(DamageNumbersPrefab, hitpoint, Quaternion.identity, transform);
        number.GetComponent<TextMeshPro>().text = DamageAmount.ToString();
        Destroy(number,1.2f);
        if(Health == 0) OnDie(Damager);
    }

    public void OnDie(GameObject Damager)
    {
        Debug.Log("Deaded");
    }

    public void OnDealDamage()
    {

    }
}
