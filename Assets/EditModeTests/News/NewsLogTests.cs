// All code was written by the team

using NUnit.Framework;

[TestFixture]
public class NewsLogTests
{
    [Test]
    public void Constructor_InitialisesLogsArray()
    {
        // Arrange
        News[] newsArray = { new News(1, "Description1", "Explanation1"), new News(2, "Description2", "Explanation2") };

        // Act
        NewsLog newsLog = new NewsLog(newsArray);

        // Assert
        Assert.AreEqual(newsArray, newsLog.Logs, "NewsLog's logs array should match the provided array");
    }
}
