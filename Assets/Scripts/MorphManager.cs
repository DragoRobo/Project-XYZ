using UnityEngine;

public class MorphManager : MonoBehaviour
{
    public GameObject defaultForm;
    public GameObject flyingForm;
    public GameObject powerForm;
    public GameObject agileForm;

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
        if (currentForm != null)
        {
            Vector3 pos = currentForm.transform.position;
            Destroy(currentForm);
            currentForm = Instantiate(formPrefab, pos, Quaternion.identity);
        }
        else
        {
            currentForm = Instantiate(formPrefab, transform.position, Quaternion.identity);
        }
    }
}