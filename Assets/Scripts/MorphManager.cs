using UnityEngine;

public class MorphManager : MonoBehaviour
{
    public GameObject defaultForm;
    public GameObject flyingForm;
    public GameObject powerForm;
    public GameObject agileForm;
    public CameraFollow cameraFollow; // reference to camera follow script

    private GameObject currentForm;

    void Start()
    {
        // Spawn default form at start
        MorphTo(defaultForm);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            MorphTo(defaultForm);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            MorphTo(flyingForm);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            MorphTo(powerForm);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            MorphTo(agileForm);
    }

    void MorphTo(GameObject formPrefab)
    {
        Vector3 spawnPos = transform.position;

        if (currentForm != null)
        {
            // Save position before destroying
            spawnPos = currentForm.transform.position;
            Destroy(currentForm);
        }

        // Spawn new form
        currentForm = Instantiate(formPrefab, spawnPos, Quaternion.identity);

        // Update camera to follow new form
        if (cameraFollow != null)
        {
            cameraFollow.target = currentForm.transform;
        }
    }
}