using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class cursorChanger : MonoBehaviour
{
    public Image cursorImage;
    public LayerMask layerToHit;
    [Space]
    public List<Sprite> cursors;
    public List<string> tagsToChangeCursor;
    [HideInInspector] public RaycastHit hit;

    private void Update()
    {
        if(cursors.Count <= 0 || tagsToChangeCursor.Count <= 0)
        {
            Debug.LogWarning("There are no cursors to change to, or tags to compare. \n Please fill these and try again");
            return;
        }

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, layerToHit))
        {
            if (tagsToChangeCursor.Contains(hit.transform.tag)) cursorImage.sprite = cursors.Where(Sprite => Sprite.name == hit.transform.tag).FirstOrDefault();
        }
        else cursorImage.sprite = cursors[0];
    }
}
