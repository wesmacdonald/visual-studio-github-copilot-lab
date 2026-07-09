using DataEntities;

namespace TinyShop.Tests;

[TestClass]
public class ProductTests
{
    [TestMethod]
    public void Product_NewInstance_HasDefaultValues()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        Assert.AreEqual(0, product.Id);
        Assert.IsNull(product.Name);
        Assert.IsNull(product.Description);
        Assert.AreEqual(0m, product.Price);
        Assert.IsNull(product.ImageUrl);
    }

    [TestMethod]
    public void Product_Id_SetterAndGetter_RoundTripsValue()
    {
        // Arrange
        var product = new Product();

        // Act
        product.Id = 42;

        // Assert
        Assert.AreEqual(42, product.Id);
    }

    [TestMethod]
    public void Product_Name_SetterAndGetter_RoundTripsValue()
    {
        // Arrange
        var product = new Product();

        // Act
        product.Name = "Trail Pack";

        // Assert
        Assert.AreEqual("Trail Pack", product.Name);
    }

    [TestMethod]
    public void Product_Description_SetterAndGetter_RoundTripsValue()
    {
        // Arrange
        var product = new Product();

        // Act
        product.Description = "A durable weekend pack.";

        // Assert
        Assert.AreEqual("A durable weekend pack.", product.Description);
    }

    [TestMethod]
    [DataRow(19.99)]
    [DataRow(0)]
    [DataRow(999.99)]
    public void Product_Price_SetterAndGetter_RoundTripsValue(double price)
    {
        // Arrange
        var product = new Product();

        // Act
        product.Price = (decimal)price;

        // Assert
        Assert.AreEqual((decimal)price, product.Price);
    }

    [TestMethod]
    [DataRow("product1.png")]
    [DataRow("product2.png")]
    [DataRow("")]
    public void Product_ImageUrl_SetterAndGetter_RoundTripsValue(string imageUrl)
    {
        // Arrange
        var product = new Product();

        // Act
        product.ImageUrl = imageUrl;

        // Assert
        Assert.AreEqual(imageUrl, product.ImageUrl);
    }

    [TestMethod]
    [DataRow(1, "Trail Pack", "A durable weekend pack.", 129.99, "/images/trail-pack.jpg")]
    [DataRow(2, "", "", 0.00, "")]
    [DataRow(3, null, null, -10.50, null)]
    public void Product_Properties_UsingDataRow_RoundTripMultipleValues(int id, string? name, string? description, double price, string? imageUrl)
    {
        // Arrange & Act
        var product = new Product
        {
            Id = id,
            Name = name,
            Description = description,
            Price = (decimal)price,
            ImageUrl = imageUrl
        };

        // Assert
        Assert.AreEqual(id, product.Id);
        Assert.AreEqual(name, product.Name);
        Assert.AreEqual(description, product.Description);
        Assert.AreEqual((decimal)price, product.Price);
        Assert.AreEqual(imageUrl, product.ImageUrl);
    }
}
