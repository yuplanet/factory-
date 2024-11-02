namespace Stock.Models;

/// <summary>
/// Фабрика
/// </summary>
public class Factory
{
    /// <summary>
    /// Название фабрики
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Название продукции фабрики
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Коэфициент изготовляемой продукции(за минуту)
    /// </summary>
    public double ProductPerMinute;

    /// <summary>
    /// Коэфициент изготовляемой продукции(за час)
    /// </summary>
    public double ProductPerHour;


    /// <summary>
    /// скорость работы фабрики
    /// </summary>
    /// <param name="productPerHour"></param>
    public void SetFactorySpeed(double productPerHour)
    {
        ProductPerHour = productPerHour;
        ProductPerMinute = productPerHour / 60d;
    }

    /// <summary>
    /// получить партию продукции за n минут
    /// </summary>
    public Supply GetProductPerMinute(int minute)
    {
        double prod = ProductPerMinute * minute + OstProduct;

        OstProduct = prod - (int)prod;

        Supply supply = new()
        {
            Factory = Name,
            Product = ProductName,
            Quantity = (int)prod
        };

        return supply;
    }

    /// <summary>
    /// недоделанная продукция за минуту
    /// Суммируется со следующей партией
    /// </summary>
    public double OstProduct = 0.0d;


    /// <summary>
    /// продукция, которая не влезла на склад, возвращается обратно
    /// </summary>
    /// <param name="quantity"></param>
    public void ReturnProduct(int quantity)
    {
        OstProduct += quantity;
    }
}
