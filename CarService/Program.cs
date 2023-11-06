namespace CarService
{
    class Programm
    {
        static void Main()
        {
            //Menu menu = new Menu();
            //menu.Run();
            CarFactory factory = new CarFactory();
            Car car = new Car(factory.CreateCar());
            car.ShowCar();
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
        private int _serviceMoney;

    }

    class Storage // содержит контейнеры с деталями
    {
        private List<Container> _containers = new List<Container>();

        public Storage(List<Container> containers)
        {
            _containers = containers;
        }

        private void AddPartsToExistingContainers(List<Container> containers) //Добавляет детали в контейнеры
        {
            for (int i = 0; i < containers.Count; i++)
            {
                for (int j = 0; j < _containers.Count; j++)
                {
                    if (containers[i].Name == _containers[j].Name)
                    {
                        _containers[i].AddParts(containers[j].TransferAllParts());
                    }
                }
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
            
            if (_parts == null)
            {
                _parts = parts;
            }
            else
            {
                _parts.AddRange(parts);
            }

            if (_parts != null)
            {
                Ammount = _parts.Count;
            }
            else
            {
                Ammount = 0;
            }

            if (_parts != null)
            {
                Name = _parts[0].Name;
            }
            else
            {
                Name = "заготовка";
            }
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        private int Ammount { get; set; }

        public Part TransferPart()
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

        public List<Part> TransferAllParts()
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
    }

    class CarFactory
    {
        PartProvider parts = new PartProvider();

        public List<Part> CreateCar() //собираем тачку
        {
            int ammountOfWheels = 4;
            int ammountOfSeats = 4;
            List<Part> car = new List<Part>();

            for (int i = 0; i < parts.ProvideAllPartsType().Length; i++)
            {
                if (parts.ProvideAllPartsType()[i].Name == "wheel")
                {
                    for (int j = 0; j < ammountOfWheels; j++)
                    {
                        car.Add(new Part(parts.ProvideAllPartsType()[i])); //добавляем не из массива, чтобы были уникальными (используется другой конструктор).
                    }
                }
                else if (parts.ProvideAllPartsType()[i].Name == "seat")
                {
                    for (int j = 0; j < ammountOfSeats; j++)
                    {
                        car.Add(new Part(parts.ProvideAllPartsType()[i]));
                    }
                }
                else
                {
                    car.Add(parts.ProvideAllPartsType()[i]);
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