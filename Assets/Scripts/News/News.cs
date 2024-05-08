// All code was written by the team

[System.Serializable]
public class News
{
    public int Round;
    public string Description;
    public string Explanation;

    public News(int round, string description, string explanation)
    {
        this.Round = round;
        this.Description = description;
        this.Explanation = explanation;
    }
}
