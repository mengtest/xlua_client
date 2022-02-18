using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFull : MonoBehaviour
{
    //����SpriteRender��������ľ���
    public float distance = 1.0f;
    //ע�⣺Sprite��Cam��������
    private SpriteRenderer spriteRender = null;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        spriteRender.material.renderQueue = 2980;//��δ���ǳ���Ҫ������Ҫ���ϣ�����͸������Ⱦ�㼶�����
        if (cam != null)
        {
            cam.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        FixSpriteSize();
    }

    //����ӦSprite��Size
    public void FixSpriteSize()
    {
        if(cam == null)
        {
            return;
        }

        float width = spriteRender.sprite.bounds.size.x;
        float height = spriteRender.sprite.bounds.size.y;

        float worldScreenWidth, worldScreenHeight;
        if(cam.orthographic)
        {
            //���������
            worldScreenHeight = cam.orthographicSize * 2;
            worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
            Debug.Log("worldScreenHeight = " + worldScreenHeight.ToString() + " worldScreenWidth = " + worldScreenWidth.ToString());
        }
        else
        {
            //ͶӰ���
            worldScreenHeight = 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            worldScreenWidth = worldScreenHeight * cam.aspect;
        }
        transform.localPosition = new Vector3(0, 0, distance);
        transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 0);
    }
}
