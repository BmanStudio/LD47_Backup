using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float _delayFindingTargetsCoroutine = 0.2f;
    [SerializeField] private Light _visionSpotLight = null;
    
    public float ViewRadius = 0;
    [Range(0,360)]
    public float ViewAngle = 0;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    
    public List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        SetupLight(Color.white);
        StartCoroutine("FindTargetsWithDelay", _delayFindingTargetsCoroutine);
    }
    
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    
    void FindVisibleTargets()
    {
        visibleTargets.Clear();

        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, targetMask);

        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            
            Transform target = targetInViewRadius[i].transform;

            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < ViewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                // If there are no objects with collider and a layer mask of obstacles:
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleGlobal)
    {
        if (!angleGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),
            0,
            Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void SetupLight(Color color, float intensity = 15)
    {
        if (_visionSpotLight != null)
        {
            _visionSpotLight.intensity = intensity;
            _visionSpotLight.type = LightType.Spot;
            _visionSpotLight.color = color;
            _visionSpotLight.range = ViewRadius;
            _visionSpotLight.spotAngle = ViewAngle + (ViewAngle / 2);
            _visionSpotLight.innerSpotAngle = ViewAngle;
        }
    }
}
