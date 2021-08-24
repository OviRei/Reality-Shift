using UnityEngine;

public class WeponSwitching : MonoBehaviour
{
    //Variables
    public int selectedWepon = 0;

    //Unity Functions
    private void Start()
    {
        SelectWepon();
    }

    private void Update()
    {
        int previousSelectedWepon = selectedWepon;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(selectedWepon >= transform.childCount - 1) selectedWepon = 0;
            else selectedWepon++;
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if(selectedWepon <= 0) selectedWepon = transform.childCount - 1;
            else selectedWepon--;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) selectedWepon = 0;
        else if(Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2) selectedWepon = 1;
        else if(Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3) selectedWepon = 2;

        if(previousSelectedWepon != selectedWepon)
        {
            SelectWepon();
        }
    }

    //My Functions
    private void SelectWepon()
    {
        int i = 0;
        foreach(Transform wepon in transform)
        {
            if(i == selectedWepon) wepon.gameObject.SetActive(true);
            else wepon.gameObject.SetActive(false);
            i++;
        }
    }
}
