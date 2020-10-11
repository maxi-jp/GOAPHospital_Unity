using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject patientPrefab;
    public int numPatients;

    public List<GameObject> patients = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // get the references of the current patients of the scene
        Patient[] patientsP = FindObjectsOfType<Patient>();
        foreach (Patient patient in patientsP)
            patients.Add(patient.gameObject);

        for (int i = 0; i < numPatients; i++)
        {
            
        }

        Invoke("SpawnPatient", 5f);
    }

    private void SpawnPatient()
    {
        GameObject newPatient = Instantiate(patientPrefab, transform.position, Quaternion.identity);
        patients.Add(newPatient);

        Invoke("SpawnPatient", Random.Range(2f, 10f));
    }

}
