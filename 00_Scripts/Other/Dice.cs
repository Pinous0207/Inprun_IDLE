using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Animator animator;
    public GameObject CoinParticle;
    public TextMeshPro CoinText;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Base_Mng.Sound.Play(Sound.Effect, "Dice");
        StartCoroutine(CoinBlast());
    }

    IEnumerator CoinBlast()
    {
        for(int i = 0; i< 10; i++)
        {
            int RandomValue = Random.Range(1, 7);
            CoinText.text = RandomValue.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        animator.SetTrigger("Dice_Open");
        CoinParticle.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        animator.SetTrigger("Dice_Close");

        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
