using UnityEngine;
using TMPro;
public class WeponSwitching : MonoBehaviour
{
    //Variables
    public int selectedWepon = 0;
    [SerializeField] private GameObject handgunImage;
    [SerializeField] private GameObject arImage;
    [SerializeField] private TextMeshProUGUI gunText;

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

        switch(selectedWepon)
        {
            case 1:
                arImage.SetActive(false);
                handgunImage.SetActive(true);
                gunText.gameObject.SetActive(true);
                break;
            case 2:
                arImage.SetActive(true);
                handgunImage.SetActive(false);
                gunText.gameObject.SetActive(true);
                break;
            default:
                arImage.SetActive(false);
                handgunImage.SetActive(false);
                gunText.gameObject.SetActive(false);
                break;
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
