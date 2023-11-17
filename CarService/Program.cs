namespace CarService
{
    class Programm
    {
        static void Main()
        {
            Menu menu = new Menu();
            menu.Run();
        }
    }

    class Menu
    {
        private const string ServeClientCommand = "1";
        private const string CreateClientsCommand = "2";
        private const string CheckStorageCommand = "3";
        private const string Exit = "0";

        private int _clientsToCreate = 5;
        private Service _service = new Service();

        public void Run()
        {
            string userInput;
            bool isExit = false;
            int menuPositionY = 0;

            _service.CreateClients(_clientsToCreate);

            while (isExit == false)
            {
                Console.SetCursorPosition(0, menuPositionY);
                UiOperations.CleanConsoleBelowLine();
                Console.SetCursorPosition(0, menuPositionY);

                _service.ShowServiceStatus();
                Console.WriteLine(ServeClientCommand + " - Обслужить клиента");
                Console.WriteLine(CreateClientsCommand + " - Создать клиентов");
                Console.WriteLine(CheckStorageCommand + " - Склад");
                Console.WriteLine(Exit + " - Выход\n");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case ServeClientCommand:
                        _service.ServeClientMenu();
                        break;

                    case CreateClientsCommand:
                        _service.CreateClients(_clientsToCreate);
                        break;

                    case CheckStorageCommand:
                        _service.StorageMenu();
                        break;

                    case Exit:
                        isExit = true;
                        break;
                }
            }
        }
    }

    class Service
    {
        private int _account = 1000;
        private Storage _storage;
        private Queue<Client> _clients = new Queue<Client>();
        private PartRecord[] _partPrices;

        public Service()
        {
            _storage = new Storage();
            _partPrices = new PartRecord[]
    {
    new PartRecord("wheel", 80),
    new PartRecord ("right headlight", 50),
    new PartRecord ("left headlight", 50),
    new PartRecord ("transmission", 120),
    new PartRecord ("drive shaft", 110),
    new PartRecord ("windshield", 70),
    new PartRecord ("rear window", 70),
    new PartRecord ("left window", 50),
    new PartRecord ("right window", 50),
    new PartRecord ("hood", 90),
    new PartRecord ("trunk", 90),
    new PartRecord ("left door", 100),
    new PartRecord ("right door", 100),
    new PartRecord ("steering wheel", 30),
    new PartRecord ("engine", 300),
    new PartRecord ("radiator", 100),
    new PartRecord ("front bumper", 80),
    new PartRecord ("rear bumper", 80),
    new PartRecord ("seat", 70),
    new PartRecord ("speedometer", 30)
    };
        }

        public void ServeClientMenu()
        {
            const string ShowCarCommand = "1";
            const string CheckRepearPriceCommand = "2";
            const string ChangePartCommand = "3";
            const string ExitRepearCommand = "0";
            
            bool isExit = false;
            string userInput;

            if (_clients.Count == 0)
            {
                Console.WriteLine("Очередь клиентов пуста.");

                return;
            }

            Client client = GetClient();

            while (isExit == false)
            {
                Console.WriteLine("\nМеню ремонта:");
                Console.WriteLine(ShowCarCommand + " - Осмотреть машину:");
                Console.WriteLine(CheckRepearPriceCommand + " - Оценить стоимость ремонта:");
                Console.WriteLine(ChangePartCommand + " - Заменить деталь:");
                Console.WriteLine(ExitRepearCommand + " - Закончить обслуживание и отпустить клиента:");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case ShowCarCommand:
                        client.CustomerCar.ShowCar();
                        break;

                    case CheckRepearPriceCommand:
                        CalculateRepearPrice(client.CustomerCar, true);
                        break;

                    case ChangePartCommand:
                        ServeClient(client);
                        break;

                    case ExitRepearCommand:
                        isExit = true;
                        break;
                }
            }
        }

        public void StorageMenu()
        {
            const string OrderMorePartsCommand = "1";
            const string ShowStorageCommand = "2";
            const string ExitStorageCommand = "0";
            
            bool isExit = false;
            string userInput;

            while (isExit == false)
            {
                Console.WriteLine("\nМеню склада:");
                Console.WriteLine(OrderMorePartsCommand + " - Заказать запчастей:");
                Console.WriteLine(ShowStorageCommand + " - Показать склад:");
                Console.WriteLine(ExitStorageCommand + " - Назад:");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case OrderMorePartsCommand:
                        _storage.AddPartsToExistingContainers();
                        break;

                    case ShowStorageCommand:
                        _storage.ShowStorage();
                        break;

                    case ExitStorageCommand:
                        isExit = true;
                        break;
                }
            }
        }

        public void ShowServiceStatus()
        {
            Console.WriteLine($"Счёт - {_account}, Клиентов в очереди - {_clients.Count}");
        }

        public Client GetClient()
        {
            if ( _clients.Count == 0 )
            {
                Console.WriteLine("Очередь клиентов пуста.");
                
                return new Client();
            }
            else
            {
                return _clients.Dequeue();
            }
        }

        public void CreateClients(int clientsCount)
        {
            CarFactory factory = new CarFactory();

            for (int i = 0; i < clientsCount; i++)
            {
                Client client = new Client(new Car(factory.CreateCar()));
                _clients.Enqueue(client);
            }
        }

        public int CalculateRepearPrice(Car car, bool showInfo)
        {
            int totalPrice = 0;
            int partsPrice = 0;
            int jobPrice = 0;

            List<Part> tempCarParts = car.ProvideCarParts();

            for (int i = 0; i < tempCarParts.Count; i++)
            {
                for (int j = 0; j < _partPrices.Length; j++)
                {
                    if (tempCarParts[i].IsBroken == true && tempCarParts[i].Name == _partPrices[j].Name)
                    {
                        partsPrice += _partPrices[j].Price;
                        jobPrice += _partPrices[j].JobPrice;
                        totalPrice += _partPrices[j].Price + _partPrices[j].JobPrice;

                        if (showInfo == true)
                        {
                            Console.WriteLine($"{_partPrices[j].Name} - цена детали - {_partPrices[j].Price} - стоимость ремонта - {_partPrices[j].JobPrice}");
                        }
                    }
                }
            }

            if (showInfo == true)
            {
                Console.WriteLine($"Стоимость деталей - {partsPrice}, стоимость ремонта - {jobPrice}, ИТОГО: {totalPrice}");
            }

            return totalPrice;
        }

        public void ServeClient(Client client)
        {
            int idFromUser;

            Console.WriteLine("\nВведите номер детали для замены:");

            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out idFromUser) == false)
            {
                Console.Write("Неверный Id детали\n");
            }
            else
            {
                ReplaceCarPart(client, idFromUser);
            }
        }

        private void ReplaceCarPart(Client client, int id)
        {
            bool isPartFound = false;
            string partName;
            List<Part> tempCarParts = client.CustomerCar.ProvideCarParts();

            for (int i = 0; i < tempCarParts.Count; i++)
            {
                if (tempCarParts[i].Id == id)
                {
                    isPartFound = true;
                    partName = tempCarParts[i].Name;

                    if (_storage.CheckPartAveilable(partName) == true)
                    {
                        if (RecivePayment(partName, client, true))
                        {
                            if (tempCarParts[i].IsBroken == false)
                            {
                                Console.WriteLine("Вы заменили исправную деталь!");
                                client.ReciveMoney(PayFine());
                            }

                            Part newPart = _storage.TransferPart(partName);
                            
                            if (client.CustomerCar.ReplacePart(newPart, id))
                            {
                                Console.WriteLine("Ремонт прошел успешно");
                            }
                            else
                            {
                                Console.WriteLine("Ремонт не удался");
                            }

                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Деталь - {partName} не установлена. Придётся заплатить штраф.");
                        client.ReciveMoney(PayFine());
                    }
                }
            }

            if (isPartFound == false)
            {
                Console.WriteLine("Такой детали не существует, возможно Вы ошиблись в номере.");
            }
        }

        private bool RecivePayment(string partName, Client client, bool showInfo)
        {
            bool isPayed = false;
            int totalPrice = 0;

            for (int i = 0; i < _partPrices.Length; i++)
            {
                if (partName == _partPrices[i].Name)
                {
                    totalPrice += _partPrices[i].Price + _partPrices[i].JobPrice;

                    if (showInfo == true)
                    {
                        Console.WriteLine($"{_partPrices[i].Name} - цена детали - {_partPrices[i].Price} - стоимость ремонта - {_partPrices[i].JobPrice}, ИТОГО: {totalPrice}");
                    }
                }
            }

            if (client.Money >= totalPrice)
            {
                _account += client.Pay(totalPrice);

                isPayed = true;
                Console.WriteLine($"Деталь заменена");
            }
            else
            {
                Console.WriteLine("Деталь не заменена (не оплачено)");
            }

            return isPayed;
        }

        private int PayFine()
        {
            int fine = 1000;

            return GiveMoney(fine);
        }

        public void TakeMoney(int money)
        {
            _account += money;
        }

        public int GiveMoney(int money)
        {
            if (_account - money < 0)
            {
                Console.WriteLine("Не хватает денег. Сектор банкрот на барабане! Кое-что уходит в зрительный зал. (Конец игры)");
                return 0;
            }
            else
            {
                _account -= money;

                return money;
            }
        }
    }

    class Storage
    {
        private int _initialAmmountOfParts = 1;
        private PartProvider _parts = new PartProvider();
        private List<Container> _containers = new List<Container>();

        public Storage()
        {
            _containers = Fill();
        }

        public void AddPartsToExistingContainers()
        {
            List<Container> containers = Fill();

            for (int i = 0; i < containers.Count; i++)
            {
                for (int j = 0; j < _containers.Count; j++)
                {
                    if (containers[i].Name == _containers[j].Name)
                    {
                        _containers[j].AddParts(containers[i].TransferAllParts());
                    }
                }
            }
        }

        public bool CheckPartAveilable(string name)
        {
            for (int i = 0; i < _containers.Count; i++)
            {
                if (_containers[i].Name == name && _containers[i].Ammount != 0)
                {
                    return true;
                }
            }

            Console.WriteLine($"На складе кончились датали - {name}");

            return false;
        }

        public Part TransferPart(string name)
        {
            for (int i = 0; i < _containers.Count; i++)
            {
                if (_containers[i].Name == name)
                {
                    return _containers[i].GetPart();
                }
            }

            Console.WriteLine($"Невозможно передать - {name}");

            return null;
        }

        public void ShowStorage()
        {
            Console.WriteLine($"Содержание склада:");

            for (int i = 0; i < _containers.Count; i++)
            {
                Console.WriteLine($"Позиция {i + 1} - {_containers[i].Name}, колличество - {_containers[i].Ammount}");
            }
        }

        public List<Container> Fill()
        {
            List<Container> storage = new List<Container>();

            for (int i = 0; i < _parts.ProvideAllPartsType().Length; i++)
            {
                List<Part> parts = new List<Part>();

                for (int j = 0; j < _initialAmmountOfParts; j++)
                {
                    parts.Add(new Part(_parts.ProvideAllPartsType()[i]));
                }

                Container container = new Container(parts);
                storage.Add(container);
            }

            return storage;
        }
    }

    class Container
    {
        private static int s_id = 0;
        private List<Part> _parts;

        public Container(List<Part> parts)
        {
            Id = s_id++;
            _parts = parts;
            Name = _parts[0].Name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Ammount => _parts.Count;

        public Part GetPart()
        {
            if (_parts.Count > 0)
            {
                Part part = _parts[0];
                _parts.Remove(part);

                return part;
            }
            else
            {
                return null;
            }
        }

        public List<Part> TransferAllParts()
        {
            List<Part> parts = new List<Part>(_parts);
            _parts.Clear();

            return parts;
        }

        public void AddParts(List<Part> parts)
        {
            _parts.AddRange(parts);
        }
    }

    class Client
    {
        private static int s_id = 0;

        public Client(Car car)
        {
            Id = s_id++;
            Money = 2000;
            CustomerCar = car;
        }

        public Client()
        {
        }

        public int Id { get; private set; }
        public int Money { get; private set; }
        public Car CustomerCar { get; private set; }

        public void ReciveRepearedCar(Car car)
        {
            CustomerCar = car;
        }

        public int Pay(int money)
        {
            if (Money - money < 0)
            {
                Console.WriteLine("Не хватает денег!");
                return 0;
            }

            Money -= money;
            Console.WriteLine($"Клиент заплатил {money}");

            return money;
        }

        public void ReciveMoney(int money)
        {
            Money += money;
            Console.WriteLine($"Клиент получил {money}");
        }
    }

    class PartProvider
    {
        private Part[] _partTypes;

        public PartProvider()
        {
            _partTypes = new Part[]
    {
    new Part("wheel"),
    new Part("right headlight"),
    new Part("left headlight"),
    new Part("transmission"),
    new Part("drive shaft"),
    new Part("windshield"),
    new Part("rear window"),
    new Part("left window"),
    new Part("right window"),
    new Part("hood"),
    new Part("trunk"),
    new Part("left door"),
    new Part("right door"),
    new Part("steering wheel"),
    new Part("engine"),
    new Part("radiator"),
    new Part("front bumper"),
    new Part("rear bumper"),
    new Part("seat"),
    new Part("speedometer")
    };
        }

        public Part[] ProvideAllPartsType()
        {
            Part[] parts = new Part[] { };
            parts = _partTypes;

            return parts;
        }

        public Part ProvidePart(string name)
        {
            Part part = null;

            for (int i = 0; i < _partTypes.Length; i++)
            {
                if (_partTypes[i].Name == name)
                {
                    part = new Part(_partTypes[i]);
                }
            }

            if (part == null)
            {
                Console.WriteLine("Нет такой детали.");
            }

            return part;
        }
    }

    class CarFactory
    {
        private PartProvider _parts = new PartProvider();

        public List<Part> CreateCar()
        {
            int ammountOfWheels = 4;
            int ammountOfSeats = 4;
            List<Part> car = new List<Part>();

            for (int i = 0; i < _parts.ProvideAllPartsType().Length; i++)
            {
                if (_parts.ProvideAllPartsType()[i].Name == "wheel")
                {
                    for (int j = 0; j < ammountOfWheels; j++)
                    {
                        car.Add(new Part(_parts.ProvideAllPartsType()[i]));
                    }
                }
                else if (_parts.ProvideAllPartsType()[i].Name == "seat")
                {
                    for (int j = 0; j < ammountOfSeats; j++)
                    {
                        car.Add(new Part(_parts.ProvideAllPartsType()[i]));
                    }
                }
                else
                {
                    car.Add(_parts.ProvideAllPartsType()[i]);
                }
            }

            car = BrakeCar(car);

            return car;
        }

        private List<Part> BrakeCar(List<Part> car)
        {
            int brokenPartsProbability = 8;
            int minAmmountOfBrokenParts = 1;
            int maxAmmountOfBrokenParts = car.Count / brokenPartsProbability;
            int ammountOfBrokenParts = Utils.GetRandomNumber(minAmmountOfBrokenParts, maxAmmountOfBrokenParts);
            List<Part> tempCar = new List<Part>();

            for (int i = 0; i < ammountOfBrokenParts; i++)
            {
                int brokenItemIndex = Utils.GetRandomNumber(car.Count - 1);
                car[brokenItemIndex].BrakePart();
                tempCar.Add(car[brokenItemIndex]);
                car.RemoveAt(brokenItemIndex);
            }

            if (car.Count > 0)
            {
                for (int i = 0; i < car.Count; i++)
                {
                    tempCar.Add(car[i]);
                }

                car = tempCar;
            }
            else
            {
                car = tempCar;
            }

            return car;
        }
    }

    class Car
    {
        private List<Part> _car;

        public Car(List<Part> car)
        {
            _car = car;
        }

        public void ShowCar()
        {
            Console.WriteLine("Осмотр машины клиента:");

            for (int i = 0; i < _car.Count; i++)
            {
                if (_car[i] != null)
                {
                    _car[i].ShowPartInfo();
                }
                else
                {
                    Console.WriteLine("Деталь отсутствует!");
                }
            }
        }

        public bool ReplacePart(Part part, int id)
        {
            bool isPartChanged = false;

            for (int i = 0; i < _car.Count; i++)
            {
                if (_car[i].Id == id)
                {
                    _car.RemoveAt(i);
                    _car.Insert(i, part);
                    isPartChanged = true;

                    return isPartChanged;
                 }
            }

            return isPartChanged;
        }

        public List<Part> ProvideCarParts()
        {
            List<Part> carParts = _car.ToList();

            return carParts;
        }
    }

    class Part
    {
        private static int s_id = 0;

        public Part(string name)
        {
            Id = s_id++;
            Name = name;
            IsBroken = false;
        }

        public Part(Part name)
        {
            Id = s_id++;
            Name = name.Name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool IsBroken { get; private set; }

        public void ShowPartInfo()
        {
            string status;

            if (IsBroken == true)
            {
                status = "сломано";
            }
            else
            {
                status = "исправно";
            }

            Console.WriteLine($"{Id} - {Name} - {status}");
        }

        public void BrakePart()
        {
            IsBroken = true;
        }
    }

    class PartRecord : Part
    {
        private int _jobPriceModificator = 5;

        public PartRecord(string name, int price) : base(name)
        {
            Price = price;
            JobPrice = price / _jobPriceModificator;
        }

        public int Price { get; private set; }
        public int JobPrice { get; private set; }
    }

    class Utils
    {
        private static Random s_random = new Random();

        public static int GetRandomNumber(int minValue, int maxValue)
        {
            return s_random.Next(minValue, maxValue);
        }

        public static int GetRandomNumber(int maxValue)
        {
            return s_random.Next(maxValue);
        }
    }

    static class UiOperations
    {
        public static void CleanConsoleBelowLine()
        {
            int currentLineCursor = Console.CursorTop;
            int numberOfCleanScreens = 10;

            for (int i = currentLineCursor; i < Console.WindowHeight * numberOfCleanScreens; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}