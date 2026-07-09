using System.Net;
using System.Text;
using System.Text.Json;
using Bunit;
using BunitTestContext = Bunit.TestContext;
using DataEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Components.Pages;
using Store.Services;

namespace TinyShop.Tests;

[TestClass]
public class ProductDetailsTests
{
    [TestMethod]
    public async Task GetProductById_ReturnsProduct_ForSuccessfulResponse()
    {
        var service = CreateService(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new Product
            {
                Id = 42,
                Name = "Trail Pack",
                Description = "A durable weekend pack.",
                Price = 129.99m,
                ImageUrl = "/images/trail-pack.jpg"
            }), Encoding.UTF8, "application/json")
        });

        var product = await service.GetProductById(42);

        Assert.IsNotNull(product);
        Assert.AreEqual(42, product!.Id);
        Assert.AreEqual("Trail Pack", product.Name);
        Assert.AreEqual(129.99m, product.Price);
    }

    [TestMethod]
    public async Task GetProductById_ReturnsNull_ForMissingProduct()
    {
        var service = CreateService(new HttpResponseMessage(HttpStatusCode.NotFound));

        var product = await service.GetProductById(999);

        Assert.IsNull(product);
    }

    [TestMethod]
    public async Task ProductDetails_ShowsNotFoundMessage_WhenProductMissing()
    {
        using var context = new BunitTestContext();
        var productService = CreateService(new HttpResponseMessage(HttpStatusCode.NotFound));

        context.Services.AddSingleton<IConfiguration>(CreateConfiguration());
        context.Services.AddSingleton(productService);

        var component = context.RenderComponent<ProductDetails>(parameters => parameters.Add(p => p.Id, 999));

        component.WaitForAssertion(() => Assert.IsTrue(component.Markup.Contains("Product not found")));
        Assert.IsTrue(component.Markup.Contains("Back to products"));
    }

    [TestMethod]
    public async Task ProductDetails_ShowsProductDetails_WhenProductExists()
    {
        using var context = new BunitTestContext();
        var productService = CreateService(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new Product
            {
                Id = 7,
                Name = "Summit Tent",
                Description = "A compact shelter for cold nights.",
                Price = 199.95m,
                ImageUrl = "/images/summit-tent.jpg"
            }), Encoding.UTF8, "application/json")
        });

        context.Services.AddSingleton<IConfiguration>(CreateConfiguration());
        context.Services.AddSingleton(productService);

        var component = context.RenderComponent<ProductDetails>(parameters => parameters.Add(p => p.Id, 7));

        component.WaitForAssertion(() => Assert.IsTrue(component.Markup.Contains("Summit Tent")));
        Assert.IsTrue(component.Markup.Contains("$199.95"));
        Assert.IsTrue(component.Markup.Contains("A compact shelter for cold nights."));
    }

    private static ProductService CreateService(HttpResponseMessage response)
    {
        var handler = new StubHttpMessageHandler(_ => Task.FromResult(response));
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.com")
        };

        return new ProductService(httpClient);
    }

    private static IConfiguration CreateConfiguration()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ImagePrefix"] = "https://example.com"
            })
            .Build();
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _handler;

        public StubHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handler)
        {
            _handler = handler;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _handler(request);
        }
    }
}
