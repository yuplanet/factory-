namespace Stock.Models;

/// <summary>
/// Модель грузовика для первозки продукции
/// </summary>
public class Truck
{
    /// <summary>
    /// номер грузовика
    /// </summary>
    public int Number { get; set; }     

    /// <summary>
    /// грузоподьемность 
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Заполненность
    /// </summary>
    public int Fullness { get; set; }

    /// <summary>
    /// Виды продукции в багаже
    /// </summary>
    public List<Supply> Supplies { get; set; } = new();


    /// <summary>
    /// количество ходов до магазина и обратно
    /// </summary>
    public int Moves { get; set; }


    /// <summary>
    /// текущее количество ходов
    /// </summary>
    public int CurrentMoves { get; set; }

/// <summary>
/// на Складе?
/// </summary>
    public bool OnStock { get; set; }
}
