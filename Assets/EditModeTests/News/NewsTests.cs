// All code was written by the team

using NUnit.Framework;

[TestFixture]
public class NewsTests
{
    [Test]
    public void Constructor_InitializesFields()
    {
        // Arrange
        int round = 1;
        string description = "Test Description";
        string explanation = "Test Explanation";

        // Act
        News news = new News(round, description, explanation);

        // Assert
        Assert.AreEqual(round, news.Round, "Round value should match the provided integer.");
        Assert.AreEqual(description, news.Description, "Description should match the provided string.");
        Assert.AreEqual(explanation, news.Explanation, "Explanation should match the provided string.");
    }
}
