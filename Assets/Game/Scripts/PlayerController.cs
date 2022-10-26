using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MoveToTarget MoveToTarget { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        MoveToTarget = GetComponent<MoveToTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MoveToCursor();
        }
    }

    private void MoveToCursor()
    {
        bool hasHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

        if (hasHit)
        {
            MoveToTarget.MoveTo(hit.point);
        }
    }
}
