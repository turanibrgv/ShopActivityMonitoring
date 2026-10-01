namespace ShopActivityMonitoring.Domain.Enums;

// Минимум 3 специфичных типа события — требование индивидуального задания
public enum EventType
{
    ViewProduct = 1,   // просмотр товара
    AddToCart = 2,     // добавление в корзину
    PlaceOrder = 3,    // оформление заказа
    CancelOrder = 4    // отмена заказа
}