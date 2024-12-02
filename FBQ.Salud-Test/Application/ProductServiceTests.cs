using Application.DataAccess;
using Application.Services;
using Domain.Entities;
using Domain.Models;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Tests.Application;
/// <summary>
/// Unit tests for ProductService.
/// </summary>
[TestFixture]
public class ProductServiceTests
{
    private ProductService _productService;
    private Mock<IProductRepository> _mockProductRepository;

    private int _productId;
    private Producto _product;
    private List<Producto> _productList;
    private List<ProductoFindDto> _productDtoList;
    private List<ProductoFindDto> _emptyProductDtoList;

    /// <summary>
    /// Set up the mock dependencies and common data for testing the ProductService.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _productService = new ProductService(_mockProductRepository.Object);

        // Common Product data
        _productId = 1;
        _product = new Producto
        {
            ProductoId = _productId,
            Nombre = "Product1",
            Descripcion = "Description",
            Marca = "Brand1",
            Codigo = "12345",
            Image = "image.jpg",
            Precio = 99.99m,
            Categoria = "Category1"
        };

        // Common lists for tests
        _productList = new List<Producto>
            {
                _product

            };

        _productDtoList = new List<ProductoFindDto>
            {
                new ProductoFindDto
                {
                    ProductoId = _productId,
                    Nombre = _product.Nombre,
                    Descripcion = _product.Descripcion,
                    Marca = _product.Marca,
                    Codigo = _product.Codigo,
                    Image = _product.Image,
                    Precio = _product.Precio,
                    Categoria = _product.Categoria
                }
            };

        _emptyProductDtoList = new List<ProductoFindDto>();  // Empty list for tests
    }

    /// <summary>
    /// Tests the FindProductById method. Verifies that when the product exists,
    /// a Product DTO is returned with the correct details.
    /// </summary>
    [Test]
    public async Task FindProductById_ShouldReturnProductDto_WhenProductExists()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.GetProductById(_productId)).ReturnsAsync(_product);

        // Act
        var result = await _productService.FindProductById(_productId);

        // Assert
        Assert.That(result, Is.InstanceOf<ProductoFindDto>());
        Assert.That(result.Nombre, Is.EqualTo(_product.Nombre));
        Assert.That(result.Descripcion, Is.EqualTo(_product.Descripcion));
    }

    /// <summary>
    /// Tests the FindProductById method. Verifies that when the product does not exist,
    /// the method returns null.
    /// </summary>
    [Test]
    public async Task FindProductById_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.GetProductById(It.IsAny<int>())).ReturnsAsync((Producto)null);

        // Act
        var result = await _productService.FindProductById(_productId);

        // Assert
        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Tests the GetProducts method. Verifies that when products exist, 
    /// the corresponding product DTOs are returned.
    /// </summary>
    [Test]
    public async Task GetProducts_ShouldReturnProductDtos_WhenProductsExist()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.GetProductos(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>())).ReturnsAsync(_productList);

        // Act
        var result = await _productService.GetProducts("Category1");


        // Assert
    
        for (int i = 0; i < _productDtoList.Count; i++)
        {
            var expected = _productDtoList[i];
            var actual = result[i];

            Assert.Multiple(() =>
            {
                Assert.That(actual.Codigo, Is.EqualTo(expected.Codigo), $"Mismatch at index {i}: Expected codigo = {expected.Codigo}, Actual codigo = {actual.Codigo}");
                Assert.That(actual.Image, Is.EqualTo(expected.Image), $"Mismatch at index {i}: Expected Image = {expected.Image}, Actual Name = {actual.Image}");
                Assert.That(actual.Marca, Is.EqualTo(expected.Marca), $"Mismatch at index {i}: Expected Marca = {expected.Marca}, Actual Marca = {actual.Marca}");
                Assert.That(actual.Nombre, Is.EqualTo(expected.Nombre), $"Mismatch at index {i}: Expected Nombre = {expected.Nombre}, Actual Nombre = {actual.Nombre}");
                Assert.That(actual.Descripcion, Is.EqualTo(expected.Descripcion), $"Mismatch at index {i}: Expected Descripcion = {expected.Descripcion}, Actual Name = {actual.Descripcion}");
                Assert.That(actual.Precio, Is.EqualTo(expected.Precio), $"Mismatch at index {i}: Expected Precio = {expected.Precio}, Actual Name = {actual.Precio}");
                Assert.That(actual.Categoria, Is.EqualTo(expected.Categoria), $"Mismatch at index {i}: Expected Categoria = {expected.Categoria}, Actual Name = {actual.Categoria}");
            });
        }
    }

    /// <summary>
    /// Tests the GetProductosByCategoryOrBrand method. Verifies that when products exist, 
    /// the corresponding product DTOs are returned based on the category or brand.
    /// </summary>
    [Test]
    public async Task GetProductosByCategoryOrBrand_ShouldReturnProductDtos_WhenProductsExist()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.GetProductosByCategoryOrBrand(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_productList);

        // Act
        var result = await _productService.GetProductosByCategoryOrBrand("Category1", "brand");

        // Assert
        Assert.That(result.Count, Is.EqualTo(_productDtoList.Count), "The collections have different lengths.");

    }




    

    /// <summary>
    /// Tests the GetProductosByCategoryOrBrand method. Verifies that when no products exist,
    /// the method returns an empty list.
    /// </summary>
    [Test]
        public async Task GetProductosByCategoryOrBrand_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetProductosByCategoryOrBrand(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<Producto>());

            // Act
            var result = await _productService.GetProductosByCategoryOrBrand("NonExistingCategory", "NonExistingBrand");

            // Assert
            Assert.That(result, Is.Empty);
        }
    }

