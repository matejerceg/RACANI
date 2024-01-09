using UnityEngine;

public class ChopTree : MonoBehaviour
{
    private bool isSwinging = false;
    public float detectionRadius = 3f;
    public float detectionRadiusVerticalOffset = 1f;

    public void StartSwing()
    {
        isSwinging = true;
    }

    private void EndSwing()
    {
        isSwinging = false;
    }

    public void DealDamage()
    {
        if (isSwinging)
        {
            if (Physics.CheckSphere(transform.position, detectionRadius))
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(0, detectionRadiusVerticalOffset, 0), detectionRadius);

                foreach (var collider in hitColliders)
                {
                    if (collider.CompareTag("Tree"))
                    {
                        TreeController treeController = collider.GetComponent<TreeController>();
                        if (treeController != null)
                        {
                            treeController.chopTree();
                        }
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, detectionRadiusVerticalOffset, 0), detectionRadius);
    }
}
