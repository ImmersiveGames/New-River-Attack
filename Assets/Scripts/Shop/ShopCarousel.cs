using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Shopping
{
    public class ShopCarousel
    {
        readonly Transform m_ContentProduct;
        readonly GameObject m_PanelProduct;
        readonly RectTransform m_PanelCenter;

        enum CarouselDirection { Horizontal, Vertical }
        [Header("Carousel")]
        public float spaceBetweenPanels = 50;
        public bool infinityLooping = false;
        public float maxPosition = 1500;
        CarouselDirection Direction = CarouselDirection.Horizontal;

        public GameObject[] getProducts { get; private set; }
        public int getActualProduct { get; private set; }

    #region Internal varables
        float[] m_Distance;
        float[] m_DistanceReposition;
        int m_ProductDistance;
        bool m_Dragging = false;
        bool m_TargetNearestButton = true;
        //private Vector2 productSize;
    #endregion

        #region UNITY METHODS
        public void Update()
        {
            for (int i = 0; i < getProducts.Length; i++)
            {
                var productRectTransform = getProducts[i].GetComponent<RectTransform>();
                m_DistanceReposition[i] = VectorDistance(m_PanelCenter.position, productRectTransform.position);
                //Debug.Log(distanceReposition[i]);
                m_Distance[i] = Mathf.Abs(m_DistanceReposition[i]);
                var anchoredPosition = productRectTransform.anchoredPosition;
                float curX = anchoredPosition.x;
                float curY = anchoredPosition.y;
                if (infinityLooping && m_DistanceReposition[i] > maxPosition)
                {
                    switch (Direction)
                    {
                        case CarouselDirection.Horizontal:
                            curX += getProducts.Length * m_ProductDistance;
                            break;
                        case CarouselDirection.Vertical:
                            curY += getProducts.Length * m_ProductDistance;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (infinityLooping && m_DistanceReposition[i] < -maxPosition)
                {
                    switch (Direction)
                    {
                        case CarouselDirection.Horizontal:
                            curX -= getProducts.Length * m_ProductDistance;
                            break;
                        case CarouselDirection.Vertical:
                            curY -= getProducts.Length * m_ProductDistance;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                productRectTransform.anchoredPosition = new Vector2(curX, curY);
                Canvas.ForceUpdateCanvases();
            }
            if (m_TargetNearestButton)
            {
                float minDistance = Mathf.Min(m_Distance);

                const double tolerance = 0.10;
                for (int a = 0; a < getProducts.Length; a++)
                {
                    if (Math.Abs(minDistance - m_Distance[a]) < tolerance)
                        getActualProduct = a;
                }
            }

            if (m_Dragging)  return;
            if (Direction == CarouselDirection.Horizontal)
                LerpToProduct(-getProducts[getActualProduct].GetComponent<RectTransform>().anchoredPosition.x);
            if (Direction == CarouselDirection.Vertical)
                LerpToProduct(-getProducts[getActualProduct].GetComponent<RectTransform>().anchoredPosition.y);

            Canvas.ForceUpdateCanvases();
        }
        void Drag(bool drag)
        {
            m_TargetNearestButton = true;
            m_Dragging = drag;
        }
  #endregion

        public ShopCarousel(Transform contentProduct, GameObject panelProduct, RectTransform panelCenter)
        {
            m_ContentProduct = contentProduct;
            m_PanelProduct = panelProduct;
            m_PanelCenter = panelCenter;

        }
        public void CreateShopping(List<ShopProductStock> productStocks)
        {
            ClearShopping(productStocks.Count);
            m_Distance = new float[productStocks.Count];
            m_DistanceReposition = new float[productStocks.Count];
            var distanceGap = Vector2.zero;
            for (int i = 0; i < productStocks.Count; i++)
            {
                var item = Object.Instantiate(m_PanelProduct, m_ContentProduct);
                if (item.GetComponent<UIItemShop>() != null)
                    item.GetComponent<UIItemShop>().SetupDisplay(productStocks[i]);
                item.transform.localPosition += (Vector3)distanceGap;
                Canvas.ForceUpdateCanvases();
                //productSize = item.GetComponent<RectTransform>().rect.size;
                distanceGap += VectorCarouselDirection(item.GetComponent<RectTransform>().rect.size, spaceBetweenPanels);
                getProducts[i] = item;
            }
            Canvas.ForceUpdateCanvases();
            // pegas a distancia entre botões
            m_ProductDistance = (int)Mathf.Abs(VectorDistance(getProducts[1].GetComponent<RectTransform>().anchoredPosition, getProducts[0].GetComponent<RectTransform>().anchoredPosition));
            //maxposition = productDistance * 1.8f;
        }
        public void ButtonNavegation(int next)
        {
            m_TargetNearestButton = false;
            getActualProduct += next;
            if (!infinityLooping)
            {
                if (getActualProduct + next >= getProducts.Length)
                    getActualProduct = getProducts.Length - 1;
                if (getActualProduct + next < 0)
                    getActualProduct = 0;
            }
            else
            {
                if (getActualProduct < 0)
                    getActualProduct = getProducts.Length - 1;
                if (getActualProduct > getProducts.Length - 1)
                    getActualProduct = 0;
            }
            Canvas.ForceUpdateCanvases();
            //targetNearestButton = true;
        }

        void LerpToProduct(float pos)
        {
            float newX = 0, newY = 0;
            var anchoredPosition = m_ContentProduct.GetComponent<RectTransform>().anchoredPosition;
            switch (Direction)
            {
                case CarouselDirection.Horizontal:
                    newX = Mathf.Lerp(anchoredPosition.x, pos, Time.deltaTime * 5f);
                    break;
                case CarouselDirection.Vertical:
                    newY = Mathf.Lerp(anchoredPosition.y, pos, Time.deltaTime * 5f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            m_ContentProduct.GetComponent<RectTransform>().anchoredPosition = new Vector2(newX, newY);
        }

        Vector2 VectorCarouselDirection(Vector2 increment, float distance)
        {
            return Direction switch
            {
                CarouselDirection.Horizontal => new Vector2(increment.x + distance, 0),
                CarouselDirection.Vertical => new Vector2(0, increment.y + distance),
                _ => increment
            };
        }

        private float VectorDistance(Vector3 anchor1, Vector3 anchor0)
        {
            return Direction switch
            {
                CarouselDirection.Horizontal => anchor1.x - anchor0.x,
                CarouselDirection.Vertical => anchor1.y - anchor0.y,
                _ => 0
            };
        }

        private void ClearShopping(int count)
        {
            for (int i = 0; i < m_ContentProduct.childCount; i++)
            {
                Object.Destroy(m_ContentProduct.GetChild(i).gameObject);
            }
            getProducts = new GameObject[count];
        }
    }
}
