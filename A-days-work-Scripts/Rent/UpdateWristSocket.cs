using Pixelplacement;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UpdateWristSocket : XRSocketInteractor
{
    // Sizing
    public float targetSize = 0.25f;
    public float sizingDuration = 0.25f;

    // Runtime
    private Vector3 originalScale = Vector3.one;
    private Vector3 objectSize = Vector3.zero;

    private BoxCollider box;
    private SphereCollider sphere;
    private Vector3 originalBColliderSize = Vector3.one;
    private float originalSColliderSize = 1;
    private Vector3 originalBColliderCenter = Vector3.one;
    private Vector3 objectColliderSize = Vector3.zero;

    // Prevents random objects from being selected
    private bool canSelect = false;

    private AudioSource _audio;

    protected override void Start()
    {
        base.Start();
        _audio = GetComponent<AudioSource>();
    }

    protected override void OnHoverEntering(XRBaseInteractable interactable)
    {
        //Debug.Log("OnHoverEntering");
        base.OnHoverEntering(interactable);

        // If the object is already selected, wrist can grab it
        if (interactable.isSelected)
            canSelect = true;
    }

    protected override void OnHoverExiting(XRBaseInteractable interactable)
    {
        //Debug.Log("OnHoverExiting");
        base.OnHoverExiting(interactable);

        // If the wrist didn't grab the object, we can no longer select
        if (!selectTarget)
            canSelect = false;
    }

    protected override void OnSelectEntering(XRBaseInteractable interactable)
    {
        // Store object when select begins
        base.OnSelectEntering(interactable);
        StoreObjectSizeScale(interactable);
    }

    protected override void OnSelectEntered(XRBaseInteractable interactable)
    {
        // Once select has occured, scale object to size
        base.OnSelectEntered(interactable);
        TweenSizeToSocket(interactable);

        _audio.PlayOneShot(_audio.clip);
    }

    protected override void OnSelectExited(XRBaseInteractable interactable)
    {
        // Once the user has grabbed the object, scale to original size
        base.OnSelectExited(interactable);
        SetOriginalScale(interactable);
        canSelect = false;
    }

    private void StoreObjectSizeScale(XRBaseInteractable interactable)
    {
        // Find the object's size
        objectSize = FindObjectSize(interactable.gameObject);

        /*変えた*/
        Transform s = interactable.transform.GetChild(0);
        originalScale = s.localScale;

        box = interactable.GetComponent<BoxCollider>();
        if (box != null)
        {
            originalBColliderSize = box.size;
            originalBColliderCenter = box.center;
        }
        sphere = interactable.GetComponent<SphereCollider>();
        if (sphere != null)
            originalSColliderSize = sphere.radius;

        //originalScale = interactable.transform.localScale;

        //Debug.Log("オブジェクトのサイズを探ります" + objectSize);
    }

    private Vector3 FindObjectSize(GameObject objectToMeasure)
    {
        Vector3 size = Vector3.one;

        // Assumes the interactable has one mesh on the root
        if (objectToMeasure.TryGetComponent(out MeshFilter meshFilter))
            size = ColliderMeasurer.Instance.Measure(meshFilter.mesh);

        return size;
    }

    private void TweenSizeToSocket(XRBaseInteractable interactable)
    {
        /*変えた*/
        //Debug.Log("オブジェクトのサイズを変更します");
        // Find the new scale based on the size of the collider, and scale
        //Vector3 targetScale = FindTargetScale();
        //Debug.Log(targetScale);

        interactable.GetComponent<Collider>().isTrigger = true;

        float largestSize = FindLargestSize(objectSize);
        float scaleDifference = targetSize / largestSize;
        //Debug.Log("変わる量は" + scaleDifference);

        Transform s = interactable.transform.GetChild(0);
        s.localScale = new Vector3(scaleDifference, scaleDifference, scaleDifference);

        box = interactable.GetComponent<BoxCollider>();
        if (box != null)
        {
            box.center = Vector3.zero;
            box.size = new Vector3(0.05f, 0.05f, 0.05f);
        }
        sphere = interactable.GetComponent<SphereCollider>();
        if (sphere != null)
            sphere.radius = 0.05f;

        //interactable.transform.localScale = new Vector3(scaleDifference, scaleDifference, scaleDifference);

        //ChangeObjectSize t = interactable.GetComponent<ChangeObjectSize>();
        //t.Resize(new Vector3(scaleDifference, scaleDifference, scaleDifference));

        //var size = interactable.GetComponent<XRGrabInteractable>().m_TargetLocalScale;
        //size = new Vector3(scaleDifference, scaleDifference, scaleDifference);

        // Tween to our new scale
        //Tween.LocalScale(interactable.transform, new Vector3(scaleDifference, scaleDifference, scaleDifference), sizingDuration, 0);
    }

    private Vector3 FindTargetScale()
    {
        // Figure out new scale, we assume the object is originally uniformly scaled
        float largestSize = FindLargestSize(objectSize);
        float scaleDifference = targetSize / largestSize;
        //Debug.Log("変わる量は"+scaleDifference);
        return new Vector3(scaleDifference, scaleDifference, scaleDifference);
    }

    private void SetOriginalScale(XRBaseInteractable interactable)
    {
        // This isn't necessary, but prevents an error when exiting play
        if (interactable)
        {
            //Debug.Log("オブジェクトのサイズを元に戻します");
            // Restore object to original scale
            /*変えた*/
            Transform s = interactable.transform.GetChild(0);
            s.localScale = originalScale;

            interactable.GetComponent<Collider>().isTrigger = false;

            //interactable.transform.localScale = originalScale;

            // Reset just incase
            originalScale = Vector3.one;
            objectSize = Vector3.zero;

            box = interactable.GetComponent<BoxCollider>();
            if (box != null)
            {
                box.size = originalBColliderSize;
                box.center = originalBColliderCenter;
            }
            sphere = interactable.GetComponent<SphereCollider>();
            if (sphere != null)
                sphere.radius = originalSColliderSize;
        }
    }

    private float FindLargestSize(Vector3 value)
    {
        //Debug.Log("一辺の最大は・・・" + value);
        float largestSize = Mathf.Max(value.x, value.y);
        largestSize = Mathf.Max(largestSize, value.z);
        return largestSize;
    }

    public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride
    {
        // Move while ignoring physics, and no smoothing
        get { return XRBaseInteractable.MovementType.Instantaneous; }
    }

    // Is the socket active, and object is being held by different interactor
    public override bool isSelectActive => base.isSelectActive && canSelect;

    private void OnDrawGizmos()
    {
        // Draw the approximate size of the socketed object
        //Gizmos.DrawWireSphere(transform.position, targetSize * 0.5f);
    }
}
