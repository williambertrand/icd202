using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class PlayerHealthBar : MonoBehaviour
{

    public Image emptyImage;
    public Image fullImage;
    public GameObject containerPanel;

    public int MaxValue;

    private Image[] fullImages;
    private Image[] emptyImages;

    public static PlayerHealthBar Instance;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {

        var rectTransform = containerPanel.GetComponent<RectTransform>();
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        float delta = width / MaxValue;

        const float paddingLeft = 10.0f;

        float startX = containerPanel.transform.position.x - width / 2 + paddingLeft;


        fullImages = new Image[MaxValue];
        emptyImages = new Image[MaxValue];

        for (int i = 0; i < MaxValue; i++)
        {

            float xPos = i * delta;
            Debug.Log("Adding health max:" + MaxValue + " at xPos: " + xPos);

            fullImages[i] = Instantiate<Image>(fullImage);
            fullImages[i].enabled = true;
            fullImages[i].transform.SetParent(containerPanel.transform, false);
            fullImages[i].transform.position = new Vector3(startX + xPos, containerPanel.transform.position.y, 0);

            emptyImages[i] = Instantiate<Image>(emptyImage);
            emptyImages[i].enabled = false;
            emptyImages[i].transform.SetParent(containerPanel.transform, false);
            emptyImages[i].transform.position = new Vector3(startX + xPos, containerPanel.transform.position.y, 0);

        }
    }


    public void UpdateHealthValue(int value)
    {

        for(int i = 0; i < MaxValue; i++)
        {
            //display filled heart
            if(i < value)
            {
                fullImages[i].enabled = true;
                emptyImages[i].enabled = false;
            }
            else
            {
                //add an empty canvas
                fullImages[i].enabled = false;
                emptyImages[i].enabled = true;
            }
        }

    }

}
