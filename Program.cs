using Stock;
using Stock.Models;

//список фабрик
List<Factory> factories = new();
List<Truck> trucks = new();

//input n > 50
Console.WriteLine("factory count: ");
int factoryCount = int.Parse(Console.ReadLine());

//input n > 50
Console.WriteLine("input N: ");
int n = int.Parse(Console.ReadLine());

//m
Console.WriteLine("input M: ");
int m = int.Parse(Console.ReadLine());


Console.WriteLine("инициализация фабрик с их коэфициентами");
for (int i = 1; i < factoryCount + 1; i++)
{
    Factory factory = new();
    factory.Name = Convert.ToChar(i + 64).ToString();          //65 - A, 66 - B, etc
    factory.ProductName = Convert.ToChar(i + 96).ToString();    //97 - a, 98 - b, etc

    float coef = (i / 10f - 0.1f) + 1;  // коэффициент для умножения на n: 1.1n, 1.2n -> coef * n
    float factoryCapacity = coef * n;
    factory.SetFactorySpeed(factoryCapacity);
    factories.Add(factory);

    string fabricString = $"Фабрика: {factory.Name}, Продукции в час: {factory.ProductPerHour} продукции в минуту: {factory.ProductPerMinute}";
    Console.WriteLine(fabricString);
}



Console.WriteLine("инициализация грузовиков ");
Random rnd = new();
int truckQuantity = rnd.Next(10, 40);

for (int i = 1; i < truckQuantity + 1; i++)
{
    Truck truck = new();
    truck.Number = i;
    truck.Capacity = rnd.Next(1000, 2000);
    truck.Fullness = 0;
    truck.OnStock = true;
    truck.Supplies = new();
    truck.Moves = rnd.Next(10, 40);
    truck.CurrentMoves = 0;

    string truckInfo = $"грузовик номер: {truck.Number}, грузоподъемность: {truck.Capacity}, ходов до магазина: {truck.Moves}, состояние: готов к погрузке";
    Console.WriteLine(truckInfo);
    trucks.Add(truck);
}


Console.WriteLine("инициализация склада ");

double capacity = 0.0d;
foreach (var factory in factories)
{
    capacity += factory.ProductPerHour;
}

int _capacity = (int)(m * capacity);
int transportationCoef = (int)(_capacity / 100d * 95);

Stockd stock = new(_capacity, transportationCoef);
stock.Factories = factories;
stock.Trucks = trucks;
Console.WriteLine("Инициировано");


Console.WriteLine("Склад приступает к работе");
await  stock.Initializ();

Console.WriteLine("ened");