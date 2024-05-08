// All code was written by the team

[System.Serializable]
public class OrderDataList
{
    public OrderData[] Orders;

    public OrderDataList(OrderData[] orders)
    {
        this.Orders = orders;
    }
}
