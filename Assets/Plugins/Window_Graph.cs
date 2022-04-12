using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace M2MqttUnity.Examples
{
    public class Window_Graph : MonoBehaviour
    {
        [SerializeField] private Sprite circleSprite;
        [SerializeField] private Sprite squareSprite;
        [SerializeField] private float yMaximum;
        [SerializeField] private int xMaximum;
        private RectTransform graphContainer;

        List<GameObject> children = new List<GameObject>();

        private void Awake()
        {
            graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();

            // CreateCircle(new Vector2(200, 200));
            // List<int> valueList = new List<int>() { 1, 50, 100, 50, 20, 25, 25, 20, 50 };
            // ShowGraph(valueList);
        }

        private GameObject CreateCircle(Vector2 anchoredPosition, int typeSprite = 1)
        {
            GameObject gameObject = new GameObject("circle", typeof(Image));
            children.Add(gameObject);
            gameObject.transform.SetParent(graphContainer, false);
            if (typeSprite == 1) gameObject.GetComponent<Image>().sprite = circleSprite;
            else if (typeSprite == 2) gameObject.GetComponent<Image>().sprite = squareSprite;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(11, 11);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);

            return gameObject;
        }
        public void ShowGraph(List<int> valueList)
        {
            children.ForEach(child => Destroy(child));
            float graphHeight = graphContainer.sizeDelta.y;
            float xSize = graphContainer.sizeDelta.x / xMaximum;

            GameObject lastCircleGameObject = null;
            int offset = 0;
            if (valueList.Count > xMaximum) offset = valueList.Count - xMaximum;
            for (int i = offset; i < valueList.Count; i++)
            {
                float xPosition = (i - offset) * xSize;
                float yPosition = (valueList[i] / yMaximum) * graphHeight;
                GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));

                if (lastCircleGameObject != null)
                {
                    CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                }
                lastCircleGameObject = circleGameObject;
            }
        }
        private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, string color = "white")
        {
            GameObject gameObject = new GameObject("dotConnection", typeof(Image));
            children.Add(gameObject);
            gameObject.transform.SetParent(graphContainer, false);
            if (color == "red") gameObject.GetComponent<Image>().color = Color.red;
            else if (color == "blue") gameObject.GetComponent<Image>().color = Color.blue;
            else gameObject.GetComponent<Image>().color = Color.white;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);

            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;

            rectTransform.sizeDelta = new Vector2(distance, 1f);
            double radians = Math.Atan(dir.y / dir.x);
            double angle = radians * (180 / Math.PI);
            rectTransform.localEulerAngles = new Vector3(0, 0, (float)angle);
        }

        public void ShowDoubleGraph(List<int> valueList, List<int> valueList2)
        {
            children.ForEach(child => Destroy(child));
            ShowSubGraph(valueList, this.circleSprite, "red");
            ShowSubGraph(valueList2, this.squareSprite, "blue");
        }

        private void ShowSubGraph(List<int> valueList, Sprite sprite, string color)
        {
            float graphHeight = graphContainer.sizeDelta.y;
            float xSize = graphContainer.sizeDelta.x / xMaximum;

            int offset = 0;
            if (valueList.Count > xMaximum) offset = valueList.Count - xMaximum;

            GameObject lastSpriteGameObject = null;
            for (int i = offset; i < valueList.Count; i++)
            {
                float xPosition = (i - offset) * xSize;
                float yPosition = (valueList[i] / yMaximum) * graphHeight;
                GameObject spriteGameObject = CreateSprite(new Vector2(xPosition, yPosition), sprite);

                if (lastSpriteGameObject != null)
                {
                    CreateDotConnection(lastSpriteGameObject.GetComponent<RectTransform>().anchoredPosition, spriteGameObject.GetComponent<RectTransform>().anchoredPosition, color);
                }
                lastSpriteGameObject = spriteGameObject;
            }
        }

        private GameObject CreateSprite(Vector2 anchoredPosition, Sprite sprite)
        {
            GameObject gameObject = new GameObject("sprite", typeof(Image));
            children.Add(gameObject);
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().sprite = sprite;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(11, 11);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);

            return gameObject;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}