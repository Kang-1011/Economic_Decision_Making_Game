// All code was written by the team

[System.Serializable]
public class OrderData
{
    public string TradeType;
    public string Ticker;
    public double EntryPrice;
    public double Quantity;
    public double TotalValue;
    public double Sl;
    public double Tp;
    public string Status;
    public double ExitPrice;
    public double ExitValue;
    public double PnL;

    public OrderData(int mode, string ticker, double entryPrice, double quantity, double totalValue, double sl, double tp)
    {
        setTradeType(mode);

        this.Ticker = ticker;
        this.EntryPrice = entryPrice;
        this.Quantity = quantity;
        this.TotalValue = totalValue;
        this.Sl = sl;
        this.Tp = tp;
        this.ExitPrice = 0;
        this.ExitValue = 0;
        this.PnL = 0;

        setStatus();
    }

    private void setTradeType(int mode)
    {
        switch (mode)
        {
            case 0:
                this.TradeType = "Long";
                break;
            case 1:
                this.TradeType = "Short";
                break;
            case 2:
                this.TradeType = "Buy";
                break;
            case 3:
                this.TradeType = "Sell";
                break;
        }
    }

    private void setStatus()
    {
        if (this.TradeType == "Long" || this.TradeType == "Short")
        {
            this.Status = "Ongoing";
        }
        else if (this.TradeType == "Buy" || this.TradeType == "Sell")
        {
            this.Status = "Filled";
        }
    }
}
