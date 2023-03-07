using UnityEngine;

public class MassCalculator : MonoBehaviour 
{
    public static float density = 1.0f; // density of the object material, in kilograms per cubic meter

    public static float CalculateMass(GameObject gameObject)
    {
        // Calculate the volume of the object based on its scale
        float volume = gameObject.transform.localScale.x * gameObject.transform.localScale.y * gameObject.transform.localScale.z;

        // Calculate the mass of the object using the formula: mass = density * volume
        float mass = density * volume;

        return mass;
    }
}
