namespace IntraDotNet.DapperExtensions.Tests.Models;

public class Category
{
    public required int Id { get; set; }
    public required string Name { get; set; }

    public static readonly string GetQuery = "SELECT ID, Name FROM dbo.Category";
}