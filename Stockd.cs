using Stock.Models;
using System.Diagnostics;

namespace Stock;

public class Stockd
{
    /// <summary>
    /// фабрики, которые обслуживает склад 
    /// </summary>
    public List<Factory> Factories { get; set; } = new();

    /// <summary>
    /// Грузовики, которые обслуживают склад 
    /// </summary>
    public List<Truck> Trucks { get; set; } = new();


    List<Supply>  SummaryProducts = new();  // количество 1-го вида продукции на складе

    List<Supply> ProductsHistory = new();   // история поставок продукции на склад

    public int Capacity = 0;    // Вместимость склада( в единицах продукции)

    public int Fullness = 0;    // Заполненность склада( в единицах продукции)

    public int TransportationCoef = 0; //количество продукции равное 95% заполненности

    public Stockd(int capacity,  int trCoef)
    {
        Capacity = capacity;
        TransportationCoef = trCoef;
    }


    private int i = 0;// - 1 минута в рамках системы

    public async Task Initializ()
    {
        for (i = 0; true; i++)
        {
            Task truckMove = TruckMove();
            Task receiveProduct = ReceiveProducts();

            await Task.WhenAll(truckMove, receiveProduct);

            int quantity = 0;
            SummaryProducts.ForEach(x => quantity += x.Quantity); // общее количество продукции на складе
            Console.WriteLine($"Всего на складе {quantity} единиц продукции.");
            foreach (var summary in SummaryProducts)
            {
                double percent = summary.Quantity / (double)quantity * 100;
                Console.WriteLine($"\t продукция {summary.Product} кол-во {summary.Quantity}, процент {percent} %");
            }
            double t = quantity / (double)Capacity * 100;//процент занятости склада
            Console.WriteLine($"общее кол-во {quantity}, {t} % занято");


            if (Fullness > TransportationCoef) // если заполненность > 95 %
            {
                Console.WriteLine("Склад заполнен > 95%");

                await Transportation();
            }
        }
    }

    SemaphoreSlim semaphore = new(1);
    async Task Transportation()
    {
        //выборка грузовиков ожидающих погрузки
        List<Truck> EmptyTruck = Trucks.Where(x=>x.OnStock == true).ToList();

        semaphore.Wait();

        foreach (var truck in EmptyTruck)
        {
            while(truck.Capacity != truck.Fullness)
            {
                Supply supply = ProductsHistory.Last(); // последняя погрузка на склад
                 
                Supply truckSupply = truck.Supplies.FirstOrDefault(x => x.Factory == supply.Factory);

                if (truckSupply is null)
                {
                    truckSupply = new()
                    {
                        Factory = supply.Factory,
                        Product = supply.Product,
                    };

                    truck.Supplies.Add(truckSupply);
                }

                if (truck.Capacity >=  truck.Fullness + supply.Quantity)
                { 
                    truckSupply.Quantity += supply.Quantity;
                    truck.Fullness += supply.Quantity;
                    ProductsHistory.Remove(supply);
                }
                else
                {
                    int difference = truck.Capacity - truck.Fullness;
                    supply.Quantity -= difference;
                     
                    truckSupply.Quantity += difference;
                    truck.Fullness += difference;
                }
            }


            string info = $"Грузовик: {truck.Number}";
            Console.WriteLine(info);

            int quantity = 0;
            truck.Supplies.ForEach(x => quantity += x.Quantity); // общее количество продукции в грузовике

            foreach (var supply in truck.Supplies)
            {
                Supply summary = SummaryProducts.First(x => x.Factory == supply.Factory);
                summary.Quantity -= supply.Quantity;

                double percent = supply.Quantity / (double)quantity * 100;
                Console.WriteLine($" погрузил: продукцию {supply.Product} кол-во {supply.Quantity} процент {percent} %");
            } 

            Console.WriteLine($" общее кол-во {quantity}");
        } 
        semaphore.Release();
    }

    async Task TruckMove()
    {
        foreach (var truck in Trucks)
        {
            if(truck.OnStock == false)
            {
                truck.CurrentMoves += 1;
            }
            
            if(truck.CurrentMoves == truck.Moves)
            {
                truck.CurrentMoves = 0;
                truck.Supplies = new();
                truck.OnStock = true;
            }
        }
    }

    async Task ReceiveProducts()
    {
        foreach (var factory in Factories)
        {
            Supply supply = factory.GetProductPerMinute(30);     //supply - 1 поставка продукции за n минут. По умолчанию 30 минут

            int plannedQuantity = Fullness + supply.Quantity;   // ожидаемое количество продукции после принятия поставки 

            if (plannedQuantity > Capacity)
            {
                int excess = Fullness + supply.Quantity - Capacity; // излишек

                supply.Quantity -= excess;
                factory.ReturnProduct(excess);  
            }

            if(supply.Quantity >0)
                ProductsHistory.Add(supply); //добавляем поставку в историю поставок.

            Fullness += supply.Quantity;

            var sumProduct = SummaryProducts.FirstOrDefault(x => x.Factory == supply.Factory);
            if (sumProduct is null)
            {
                Supply s = new()
                {
                    Factory = supply.Factory,
                    Product = supply.Product,
                    Quantity = supply.Quantity
                };
                SummaryProducts.Add(s);
            }
            else
            {
                sumProduct.Quantity += supply.Quantity;
            }

            string info = $"Фабрика: {supply.Factory} поставка: {supply.Quantity}";
            Console.WriteLine(info);
        }
    }
}
