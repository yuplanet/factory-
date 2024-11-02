namespace Stock;

/// <summary>
/// Модель поставки продукции на склад
/// </summary>
public class Supply
{
    /// <summary>
    /// Название фабрики
    /// </summary>
    public string Factory { get; set; } = string.Empty;

    /// <summary>
    /// Название продкции
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// Количество потавляемой продукции
    /// </summary>
    public int Quantity { get; set; }
}
