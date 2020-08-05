using UnityEngine;
using UnityEngine.UI;

public class TrhurserCharg : MonoBehaviour
{
    private Slider _chargeSlider;

    [SerializeField]
    private Image _fill;

    private enum Type
    {
        ThrustCharge,
        Ammo
    }

    [SerializeField] private Type chargerType;

    // Start is called before the first frame update
    private void Start()
    {
        _chargeSlider = GetComponent<Slider>();
        if (_chargeSlider == null)
            Debug.LogError("Charge slider is null");
    }

    public void UpdateCharge(float charge)
    {

        _chargeSlider.value = charge;
        ChangeFillColor(charge);
    }

    public void UpdateAmmo(int ammo)
    {
        _chargeSlider.value = ammo;
        ChangeAmmoFillColor(ammo);
    }

    private void ChangeAmmoFillColor(int ammo)
    {
        if (ammo >= 10)
            _fill.color = new Color(0.0f, 1.0f, 0.0f); // Green
        else if (ammo >= 7)
            _fill.color = new Color(1.0f, 0.54f, 0.0f); // Orange
        else
            _fill.color = new Color(1.0f, 0.0f, 0.0f); // Red
    }

    private void ChangeFillColor(float charge)
    {
        if (charge >= 0.5f)
            _fill.color = new Color(0.0f, 1.0f, 0.0f); // Green
        else if (charge >= 0.3f)
            _fill.color = new Color(1.0f, 0.54f, 0.0f); // Orange
        else
            _fill.color = new Color(1.0f, 0.0f, 0.0f); // Red
    }
}