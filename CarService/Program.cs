using System.Linq;

namespace CarService
{
    class Programm
    {
        static void Main()
        {
            //Menu menu = new Menu();
            //menu.Run();
            Car car = new Car();
            car.CreateCar();
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

    class Service //содержит склад и счёт сервиса
    {
        private int _serviceMoney;

    }

    class Storage // содержит детали на замену
    {

    }

    class Container
    {
        public Container(string name)
        {
            Name = name;
            Index = 0;
            _parts = null;

            if (_parts != null)
            {
                Ammount = _parts.Count;
            }
            else
            {
                Ammount = 0;
            }
        }

        public int Index { get; private set; }
        public string Name { get; private set; }
        private int Ammount { get; set; }
        private List<Part> _parts { get; set; }
    }

    class Client //содержит деньги и машину
    {
        private int _clientMoney;

    }

    class Car //содержит детали
    {
        private List<Part> _car = new List<Part>();
        private Part[] _partTypes = new Part[] // Надо придумать как дать доступ на чтение этого массива другим классам... (возможно метод с выдачей копии массива)
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

        public void CreateCar() //собираем тачку
        {
            int ammountOfWheels = 4;
            int ammountOfSeats = 4;
            //int ammountByDefault = 1;

            for (int i = 0; i < _partTypes.Length; i++)
            {
                if (_partTypes[i].Name == "wheel")
                {
                    for (int j = 0; j < ammountOfWheels; j++)
                    {
                        _car.Add(new Part(_partTypes[i])); //добавляем не из массива, чтобы были уникальными.
                    }
                }
                else if (_partTypes[i].Name == "seat")
                {
                    for (int j = 0; j < ammountOfSeats; j++)
                    {
                        _car.Add(new Part(_partTypes[i]));
                    }
                }
                else
                {
                    _car.Add(_partTypes[i] );
                }
            }

            BrakeCar(); //ломаем тачку
        }

        public void ShowCar()
        {
            int wheelCount = 0;//
            int seatCount = 0;//

            for (int i = 0; i < _car.Count; i++)
            {
                _car[i].ShowPartInfo();
                
                if (_car[i].Name == "wheel")//
                {
                    wheelCount++;//
                }
                if (_car[i].Name == "seat")//
                {
                    seatCount++;//
                }
            }

            Console.WriteLine($"Колёс - {wheelCount}"); //
            Console.WriteLine($"Сидений - {seatCount}");//
        }

        private void BrakeCar() // ломаем тачку
        {
            int brokenPartsProbability = 2; //на это число делим все детали в машине, чтобы получить макс допустимое кол-во поломок
            int minAmmountOfBrokenParts = 1;
            int maxAmmountOfBrokenParts = _car.Count / brokenPartsProbability;
            int ammountOfBrokenParts = Utils.GetRandomNumber(minAmmountOfBrokenParts, maxAmmountOfBrokenParts);
            List<Part> tempCar = new List<Part>();
            
            for (int i = 0; i < ammountOfBrokenParts; i++) //проходим по тачке столько раз, сколько выпало сломанных деталей, ломаем и перекидываем их во временную тачку
            {
                int brokenItemIndex = Utils.GetRandomNumber(_car.Count - 1);
                _car[brokenItemIndex].IsBroken = true;
                tempCar.Add(_car[brokenItemIndex]);
                _car.RemoveAt(brokenItemIndex);
            }

            if (_car.Count > 0) //докидываем целые детали во временную тачку
            {
                for (int i = 0; i < _car.Count; i++)
                {
                    tempCar.Add(_car[i]);
                }

                _car = tempCar; //перекидываем из временной все детали в основную
            }
            else
            {
                _car = tempCar;
            }
        }
    }

    class Part
    {
        public Part(string name)
        {
            //Id = 0;
            Name = name;
            IsBroken = false;
        }

        public Part(Part name) //конструктор для копирования детали, а не клонирования.
        {
            Name = name.Name;
        }

        //public int Id { get; private set; }
        public string Name { get; private set; }
        public bool IsBroken { get; set; }

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

            Console.WriteLine($"{Name} - {status}");
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