using System.Collections.Generic;
using UnityEngine;
namespace Shopping
{

    public class ShopCarousel
    {

        private Transform contentProduct;
        private GameObject panelProduct;
        private RectTransform panelCenter;

        public enum CarouselDirection { Horizontal, Vertical }
        [Header("Carousel")]
        public float gapBeteenPanels = 50;
        public bool infinityLooping = false;
        public float maxposition = 1500;
        public CarouselDirection carouselDirection = CarouselDirection.Horizontal;

        public GameObject[] GetProducts { get; private set; }
        public int GetActualProduct { get; private set; }

    #region Internal varables
        private float[] distance;
        private float[] distanceReposition;
        private int productDistance;
        private bool dragging = false;
        private bool targetNearestButton = true;
        //private Vector2 productSize;
    #endregion

        public ShopCarousel(Transform contentProduct, GameObject panelProduct, RectTransform panelCenter)
        {
            this.contentProduct = contentProduct;
            this.panelProduct = panelProduct;
            this.panelCenter = panelCenter;

        }
        public void CreateShopping(List<ShopProductStock> productStocks)
        {
            ClearShopping(productStocks.Count);
            distance = new float[productStocks.Count];
            distanceReposition = new float[productStocks.Count];
            Vector2 distanceGap = Vector2.zero;
            for (int i = 0; i < productStocks.Count; i++)
            {
                GameObject item = Object.Instantiate(panelProduct, contentProduct);
                if (item.GetComponent<UIItemShop>() != null)
                    item.GetComponent<UIItemShop>().SetupDisplay(productStocks[i]);
                item.transform.localPosition += (Vector3)distanceGap;
                Canvas.ForceUpdateCanvases();
                //productSize = item.GetComponent<RectTransform>().rect.size;
                distanceGap += VectorCarouselDirection(item.GetComponent<RectTransform>().rect.size, gapBeteenPanels);
                GetProducts[i] = item;
            }
            Canvas.ForceUpdateCanvases();
            // pegas a distancia entre botões
            productDistance = (int)Mathf.Abs(VectorDistance(GetProducts[1].GetComponent<RectTransform>().anchoredPosition, GetProducts[0].GetComponent<RectTransform>().anchoredPosition));
            //maxposition = productDistance * 1.8f;
        }

        public void Update()
        {
            for (int i = 0; i < GetProducts.Length; i++)
            {
                distanceReposition[i] = VectorDistance(panelCenter.position, GetProducts[i].GetComponent<RectTransform>().position);
                //Debug.Log(distanceReposition[i]);
                distance[i] = Mathf.Abs(distanceReposition[i]);
                float curX = GetProducts[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = GetProducts[i].GetComponent<RectTransform>().anchoredPosition.y;
                if (infinityLooping && distanceReposition[i] > maxposition)
                {
                    if (carouselDirection == CarouselDirection.Horizontal)
                        curX += GetProducts.Length * productDistance;
                    if (carouselDirection == CarouselDirection.Vertical)
                        curY += GetProducts.Length * productDistance;
                }

                if (infinityLooping && distanceReposition[i] < -maxposition)
                {
                    if (carouselDirection == CarouselDirection.Horizontal)
                        curX -= GetProducts.Length * productDistance;
                    if (carouselDirection == CarouselDirection.Vertical)
                        curY -= GetProducts.Length * productDistance;
                }
                GetProducts[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(curX, curY);
                Canvas.ForceUpdateCanvases();
            }
            if (targetNearestButton)
            {
                float minDistance = Mathf.Min(distance);

                for (int a = 0; a < GetProducts.Length; a++)
                {
                    if (minDistance == distance[a])
                        GetActualProduct = a;
                }
            }

            if (!dragging)
            {
                if (carouselDirection == CarouselDirection.Horizontal)
                    LerpToProduct(-GetProducts[GetActualProduct].GetComponent<RectTransform>().anchoredPosition.x);
                else
                    LerpToProduct(-GetProducts[GetActualProduct].GetComponent<RectTransform>().anchoredPosition.y);

                Canvas.ForceUpdateCanvases();
            }
        }

        public void ButtonNavegation(int next)
        {
            targetNearestButton = false;
            GetActualProduct += next;
            if (!infinityLooping)
            {
                if (GetActualProduct + next >= GetProducts.Length)
                    GetActualProduct = GetProducts.Length - 1;
                if (GetActualProduct + next < 0)
                    GetActualProduct = 0;
            }
            else
            {
                if (GetActualProduct < 0)
                    GetActualProduct = GetProducts.Length - 1;
                if (GetActualProduct > GetProducts.Length - 1)
                    GetActualProduct = 0;
            }
            Canvas.ForceUpdateCanvases();
            //targetNearestButton = true;
        }

        private void LerpToProduct(float pos)
        {
            float newX = 0, newY = 0;
            if (carouselDirection == CarouselDirection.Horizontal)
                newX = Mathf.Lerp(contentProduct.GetComponent<RectTransform>().anchoredPosition.x, pos, Time.deltaTime * 5f);
            if (carouselDirection == CarouselDirection.Vertical)
                newY = Mathf.Lerp(contentProduct.GetComponent<RectTransform>().anchoredPosition.y, pos, Time.deltaTime * 5f);

            contentProduct.GetComponent<RectTransform>().anchoredPosition = new Vector2(newX, newY);
        }

        private Vector2 VectorCarouselDirection(Vector2 increment, float distance)
        {
            switch (carouselDirection)
            {
                case CarouselDirection.Horizontal:
                    return new Vector2(increment.x + distance, 0);
                case CarouselDirection.Vertical:
                    return new Vector2(0, increment.y + distance);
                default:
                    return increment;
            }
        }

        private float VectorDistance(Vector3 ancor1, Vector3 ancor0)
        {
            switch (carouselDirection)
            {
                case CarouselDirection.Horizontal:
                    return ancor1.x - ancor0.x;
                case CarouselDirection.Vertical:
                    return ancor1.y - ancor0.y;
                default:
                    return 0;
            }
        }

        private void ClearShopping(int count)
        {
            for (int i = 0; i < contentProduct.childCount; i++)
            {
                Object.Destroy(contentProduct.GetChild(i).gameObject);
            }
            GetProducts = new GameObject[count];
        }

        public void Drag(bool drag)
        {
            targetNearestButton = true;
            dragging = drag;
        }
    }
}
