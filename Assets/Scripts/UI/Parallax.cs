using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pallax : MonoBehaviour
{
    [SerializeField] private float _parallaxStrenght = 100;

    [SerializeField] private Vector2 _parallaxClamp;

    private Vector2 _StartPosition;

    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _StartPosition = transform.position;
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = _camera.ScreenToViewportPoint(Input.mousePosition);
        float posX = Mathf.Lerp(transform.position.x, _StartPosition.x + (mousePos.x * _parallaxStrenght),
            5f * Time.deltaTime);
        float posY = Mathf.Lerp(transform.position.y, _StartPosition.y + (mousePos.y * _parallaxStrenght),
            5f * Time.deltaTime);
        posX = Mathf.Clamp(posY, _StartPosition.x - _parallaxClamp.x, _StartPosition.x + _parallaxClamp.x);
        posY = Mathf.Clamp(posY, _StartPosition.y - _parallaxClamp.y, _StartPosition.y + _parallaxClamp.y);
        transform.position = new Vector3(posX, posY, transform.position.z);

    }
}
