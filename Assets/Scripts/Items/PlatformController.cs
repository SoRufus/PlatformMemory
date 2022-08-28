using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    public bool Breakable { get; set; }
    public bool IsLeft { get; set; }

    private LevelManager levelManager = null;
    private Animator animator = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
        levelManager = LevelManager.Instance;
    }

    public void Highlight()
    {
        animator.SetBool("Light", true);
        Invoke(nameof(Fade), levelManager.GetCurrentLevelData().previewTime);
    }

    private void Fade()
    {
        animator.SetBool("Light", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Breakable) return;

        gameObject.SetActive(false);
    }
}
