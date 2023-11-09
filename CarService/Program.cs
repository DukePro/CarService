namespace CarService
{
    class Programm
    {
        static void Main() //Доделать - меню, очередь клиентов и её заполнение, цены деталей, цену работы.
        {
            //Menu menu = new Menu();
            //menu.Run();
            CarFactory factory = new CarFactory();
            Car car = new Car(factory.CreateCar());
            //car.ShowCar();
            Service service = new Service();
            service.RepearCar(car);
            //Storage _storage = new Storage();
            //_storage.ShowStorage();
        }
    }

    class Menu
    {
        private const string FishAdd = "1";
        private const string FishRemove = "2";
        private const string RemoveAllFishes = "3";
        private const string SkipTime = "4";
        private const string Exit = "0";

        public void Run()
        {
            string userInput;
            bool isExit = false;
            int menuPositionY = 0;

            while (isExit == false)
            {
                Console.SetCursorPosition(0, menuPositionY);
                Console.WriteLine(FishAdd + "ghghghgh");
                Console.WriteLine(FishRemove + " - Remove fish");
                Console.WriteLine(RemoveAllFishes + " - Clear Aquarium");
                Console.WriteLine(SkipTime + " - Skip 1 month");
                Console.WriteLine(Exit + " - Exit\n");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case FishAdd:
                        UiOperations.CleanString();
                        break;

                    case FishRemove:
                        UiOperations.CleanString();
                        break;

                    case RemoveAllFishes:
                        UiOperations.CleanString();
                        break;

                    case SkipTime:
                        UiOperations.CleanString();
                        break;

                    case Exit:
                        isExit = true;
                        break;
                }
            }
        }
    }

    class Service //содержит склад и счёт сервиса и ремонтирует машину клиента.
    {
        private int _account = 1000;
        private Storage _storage = new Storage();
        private Queue<Client> _clients = new Queue<Client>();

        public void CreateClients() //создаём клиентов и их тачки, ставим в очередь.
        {
            int clientsCount = 10;
            int clientsEntered = 0;
            CarFactory factory = new CarFactory();

            for (int i = 0; i < clientsCount; i++)
            {
                Client client = new Client(new Car(factory.CreateCar()));
                _clients.Enqueue(client);
                clientsEntered++;
            }

            Console.WriteLine($"В очередь встало {clientsEntered} клиентов");
            Console.WriteLine($"Всего клиентов в очереди - {_clients.Count()}");
        }

        public void RepearCar(Car car)
        {
            bool isExit = false;
            string Exit = "0";
            int idFromUser;

            while (isExit == false)
            {
                car.ShowCar();
                
                Console.WriteLine("\nВведите номер детали для замены:");
                Console.WriteLine(Exit + " - Выход\n");
                
                string userInput = Console.ReadLine();

                if (userInput == Exit)
                {
                    isExit = true;
                }
                else if (int.TryParse(userInput, out idFromUser) == false)
                {
                    Console.Write("Неверный Id детали\n");
                }
                else
                {
                    Car tempCar = new Car(ReplaceCarPart(car, idFromUser));
                    car = tempCar;
                }
            }
        }

        private List<Part> ReplaceCarPart(Car car, int id) //замена детали
        {
            string partName;
            List<Part> tempCarParts = car.ProvideCarParts(); // получаем детали из тачки клиента

            for (int i = 0; i < tempCarParts.Count; i++)
            {
                if (tempCarParts[i].Id == id) // ищем деталь в списке по id
                {
                    if (tempCarParts[i].IsBroken == true)
                    {
                        partName = tempCarParts[i].Name; // получаем название детали

                        if (_storage.CheckPartAveilable(partName)) // проверяем наличие детали
                        {
                            tempCarParts.RemoveAt(i); // удаляем неисправную деталь
                            tempCarParts.Insert(i, _storage.TransferPartFromStorage(partName)); //вставляем на её место исправную со склада
                        }
                    }
                    else
                    {
                        Console.WriteLine("Вы пытаетесь заенить исправную деталь.");
                    }
                }
            }

            return tempCarParts;
        }

        public void TakeMoney(int money)
        {
            _account += money;
        }

        public int GiveMoney(int money)
        {
            if (_account - money < 0)
            {
                Console.WriteLine("Не хватает денег. Сектор банкрот на барабане! Кое-что уходит в зрительный зал.");
                return 0;
            }
            else
            {
                _account -= money;

                return money;
            }
        }
    }

    class Storage // содержит контейнеры с деталями
    {
        private int _initialAmmountOfParts = 10;
        private PartProvider _parts = new PartProvider();
        private List<Container> _storage = new List<Container>();

        public Storage()
        {
            FillStorage();
        }

        public void AddPartsToExistingContainers(List<Container> containers) //Добавляет детали в контейнеры (возможно переделать)
        {
            for (int i = 0; i < containers.Count; i++)
            {
                for (int j = 0; j < _storage.Count; j++)
                {
                    if (containers[i].Name == _storage[j].Name)
                    {
                        _storage[i].AddParts(containers[j].TransferAllPartsFromContainer());
                    }
                }
            }
        }

        public bool CheckPartAveilable(string name)
        {
            for (int i = 0; i < _storage.Count; i++)
            {
                if (_storage[i].Name == name)
                {
                    return true;
                }
            }

            Console.WriteLine($"На складе кончились датали - {name}");
            
            return false;
        }

        public Part TransferPartFromStorage(string name) // Если получен null из этого метода, значит кончились детали, сервис платит штраф.
        {
            for (int i = 0; i < _storage.Count; i++)
            {
                if (_storage[i].Name == name)
                {
                    return _storage[i].TransferPartFromContainer();
                }
            }

            Console.WriteLine($"На складе кончились датали - {name}");
            return null;
        }

        public void ShowStorage()
        {
            Console.WriteLine($"Содержание склада:");

            for (int i = 0; i < _storage.Count; i++)
            {
                Console.WriteLine($"Позиция {i + 1} - {_storage[i].Name}, колличество - {_storage[i].Ammount}");
            }
        }

        private void FillStorage()
        {
            for (int i = 0; i < _parts.ProvideAllPartsType().Length; i++) //проходим столько раз, какой длины список
            {
                List<Part> parts = new List<Part>(); //создаём список деталей

                for (int j = 0; j < _initialAmmountOfParts; j++) //добавляем каждую деталь по очереди,столько раз, сколько указано в переменной
                {
                    parts.Add(new Part(_parts.ProvideAllPartsType()[i])); //создаём и добавляем новую деталь
                }

                Container container = new Container(parts); // создаём контейнер и помещаем в него детали
                _storage.Add(container); // добавляем контейнер на склад
            }
        }
    }

    class Container
    {
        private static int _id = 0;
        private List<Part> _parts;

        public Container(List<Part> parts)
        {
            Id = _id++;
            _parts = parts;
            Ammount = _parts.Count;
            Name = _parts[0].Name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Ammount { get; private set; }

        public Part TransferPartFromContainer()
        {
            if (_parts.Count > 0)
            {
                Part part = _parts[0];
                _parts.RemoveAt(0);

                return part;
            }
            else
            {
                return null;
            }
        }

        public List<Part> TransferAllPartsFromContainer()
        {
            List<Part> parts = _parts;
            _parts.Clear();

            return parts;
        }

        public void AddParts(List<Part> parts)
        {
            _parts.AddRange(parts);
        }
    }

    class Client //содержит деньги и машину
    {
        private static int _id = 0;

        public Client(Car car)
        {
            Id = _id++;
            Money = 1000;
            ClientCar = car;
        }

        public int Id { get; private set; }
        public int Money { get; private set; }
        public Car ClientCar { get; private set; }

        public void ReciveRepearedCar(Car car)
        {
            ClientCar = car;
        }

        public int GiveMoney(int money)
        {
            if (Money - money < 0)
            {
                Console.WriteLine("Не хватает денег!");
                return 0;
            }

            Money =- money;
            Console.WriteLine($"Клиент заплатил {money}");

            return money;
        }

        public void ReciveMoney(int money)
        {
            Money =+ money;
            Console.WriteLine($"Клиент получил {money}");
        }
    }

    class PartProvider
    {
        private Part[] _partTypes = new Part[]
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

        public Part[] ProvideAllPartsType()
        {
            Part[] parts = new Part[] { };
            parts = _partTypes;

            return parts;
        }

        public Part ProvidePart(string name) //Создаём новую деталь по запросу через имя.
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

        public List<Part> CreateCar() //собираем тачку
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
                        car.Add(new Part(_parts.ProvideAllPartsType()[i])); //добавляем не из массива, чтобы были уникальными (используется другой конструктор).
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

            car = BrakeCar(car); //ломаем тачку

            return car;
        }

        private List<Part> BrakeCar(List<Part> car) // ломаем тачку
        {
            int brokenPartsProbability = 2; //на это число делим все детали в машине, чтобы получить макс допустимое кол-во поломок
            int minAmmountOfBrokenParts = 1;
            int maxAmmountOfBrokenParts = car.Count / brokenPartsProbability;
            int ammountOfBrokenParts = Utils.GetRandomNumber(minAmmountOfBrokenParts, maxAmmountOfBrokenParts);
            List<Part> tempCar = new List<Part>();

            for (int i = 0; i < ammountOfBrokenParts; i++) //проходим по тачке столько раз, сколько выпало сломанных деталей, ломаем и перекидываем их во временную тачку
            {
                int brokenItemIndex = Utils.GetRandomNumber(car.Count - 1);
                car[brokenItemIndex].BrakePart();
                tempCar.Add(car[brokenItemIndex]);
                car.RemoveAt(brokenItemIndex);
            }

            if (car.Count > 0) //докидываем целые детали во временную тачку
            {
                for (int i = 0; i < car.Count; i++)
                {
                    tempCar.Add(car[i]);
                }

                car = tempCar; //перекидываем из временной все детали в основную
            }
            else
            {
                car = tempCar;
            }

            //можно добавить сортировку по сломаным деталям и Id
            return car;
        }
    }

    class Car //содержит машну в виде листа деталей
    {
        private List<Part> _car;

        public Car(List<Part> car)
        {
            _car = car;
        }

        public void ShowCar()
        {
            int wheelCount = 0;// временно
            int seatCount = 0;// временно

            for (int i = 0; i < _car.Count; i++)
            {
                _car[i].ShowPartInfo();

                if (_car[i].Name == "wheel")// временно
                {
                    wheelCount++;// временно
                }
                if (_car[i].Name == "seat")// временно
                {
                    seatCount++;// временно
                }
            }

            Console.WriteLine($"Колёс - {wheelCount}"); // временно
            Console.WriteLine($"Сидений - {seatCount}");// временно
            Console.WriteLine($"Всего деталей в авто - {_car.Count}");// временно
        }

        public List<Part> ProvideCarParts()
        {
            List<Part> carParts = _car;

            return carParts;
        }
    }

    class Part
    {
        private static int _id = 0;

        public Part(string name)
        {
            Id = _id++;
            Name = name;
            IsBroken = false;
        }

        public Part(Part name) //конструктор для копирования детали, а не клонирования.
        {
            Id = _id++;
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

            for (int i = currentLineCursor; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void MoveCursourMessageLine()
        {
            int mesagePositionY = 5;
            Console.SetCursorPosition(0, mesagePositionY);
        }

        public static void MoveCursourUserInputLine()
        {
            int userInputPositionY = 7;
            Console.SetCursorPosition(0, userInputPositionY);
        }

        public static void MoveCursourFishesListLine()
        {
            int fishesListPositionY = 9;
            Console.SetCursorPosition(0, fishesListPositionY);
        }

        public static void CleanString()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }
}