using UnityEngine;

public class ButtonActivator : MonoBehaviour
{
    public Animator machineAnimator;

    void Start()
    {
        
            // Get the Animator component from the machine GameObject

            if (machineAnimator == null)
            {
                Debug.LogError("Animator component not found on the machine " + name);
            }
            else if (machineAnimator.runtimeAnimatorController == null)
            {
                Debug.LogError("AnimatorController not assigned to the Animator on " + name);
            }
    }
    

    // This function is called when this GameObject collides with another GameObject
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (machineAnimator != null && machineAnimator.runtimeAnimatorController != null)
        {
            // Set the "Button Hit" bool to true in the machine's Animator
            Debug.Log("Setting Button Hit to true");
            machineAnimator.SetTrigger("Button");
        }
    }
}
