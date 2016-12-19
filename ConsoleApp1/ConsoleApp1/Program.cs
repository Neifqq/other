using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp1
{
    #region Все классы
    class Employee
    {
        private string EmploymentDate { get; set; } // Дата найма.
        private string FullName { get; set; } // ФИО.
        public bool IsMemberOfLaborUnion { get; private set; } // Член профсоюза.
        private string Number { get; set; } // Табельный номер.  Он содержит буквы?
        public byte Rating { get; private set; } // Разряд.

        public void GetInformation()
        {
            Console.Write("Введите ФИО сотрудника: ");
            FullName = Console.ReadLine();
            Console.Write("Введите дату найма: ");
            EmploymentDate = Console.ReadLine();
            Console.Write("Введите табельный номер: ");
            Number = Console.ReadLine();
            Console.Write("Принадлежит ли сотрудник профсоюзу (Да/Нет)? ");
            if (Console.ReadLine() == "Да")
            {
                IsMemberOfLaborUnion = true;
            }
            else
            {
                IsMemberOfLaborUnion = false;
            }
            Console.Write("Введите разряд сотрудника: ");
            Rating = Convert.ToByte(Console.ReadLine());
        }

    }

    class TimeBoard
    {
        public DateTime StartDate { get; private set; } // Нижняя граница.
        public DateTime EndDate { get; private set; } // Верхняя граница.
        public List<TimeSheet> Board = new List<TimeSheet>(); // Список дней.

        // Заполнение списка.
        public void AddTimeSheets()
        {

            for (DateTime date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                Console.Write($"Часы за {Convert.ToString(date)}: ");
                TimeSheet t = new TimeSheet(date, Convert.ToByte(Console.ReadLine()));
                Board.Add(t);
            }
            Console.Clear();
        }


        // Инпут дат.
        public void SetTimeFrames()
        {
            string input;
            string[] parsed;
            Console.Write("Введите начальную дату (ДД.ММ.ГГГГ): ");
            input = Console.ReadLine();
            parsed = input.Split('.');
            StartDate = new DateTime(int.Parse(parsed[2]), int.Parse(parsed[1]), int.Parse(parsed[0]));
            Console.Write("Введите конечную дату (ДД.ММ.ГГГГ): ");
            input = Console.ReadLine();
            parsed = input.Split('.');
            EndDate = new DateTime(int.Parse(parsed[2]), int.Parse(parsed[1]), int.Parse(parsed[0]));
            Console.Clear();
        }
    }

    class TimeSheet
    {
        public DateTime Date { get; private set; } // Дата.
        public byte Hours { get; private set; } // Кол-во отработанных часов.

        public TimeSheet(DateTime d, byte h)
        {
            Date = d;
            Hours = h;
        }
    }

    class Position
    {
        public int[] Code = new int[5]; // Возможно, шифр содержит информацию о том, какая оплата у должностей.
        public string Name { get; private set; } // Должность
        public int BaseHourlyRate { get; private set; } // Оплата

        public Position()
        {
            Name = null;
            BaseHourlyRate = 0;
            Code[0] = 500;
            Code[1] = 400;
            Code[2] = 250;
            Code[3] = 150;
            Code[4] = 100;
        }



        public void SetPayment()
        {
            BaseHourlyRate = Code[Convert.ToInt32(Name) - 1];
        }

        public void GetInformation()
        {
            Console.Write("Введите должность сотрудника: ");
            Name = Console.ReadLine();
            BaseHourlyRate = Code[Convert.ToInt32(Name) - 1];
            Console.Clear();
        }
    }
    #endregion


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Payroll v0.1");
            Thread.Sleep(1000);
            Employee Person = new Employee();
            TimeBoard Board = new TimeBoard();
            Position Position = new Position();

            Person.GetInformation();
            Position.GetInformation();
            Board.SetTimeFrames();
            Board.AddTimeSheets();

            Console.Write($"Оплата сотрудника: {GetPayment(Person, Board, Position)}");

            Console.ReadKey();
        }
        public static double GetPayment(Employee e, TimeBoard t, Position p)
        {
            double multiplier = ((e.Rating - 1) / 10) + 1;
            double sum = 0;
            for (int i = 0; i < t.Board.Count(); i++)
            {
                if (Convert.ToString(t.Board[i].Date.DayOfWeek) == "Saturday" || Convert.ToString(t.Board[i].Date.DayOfWeek) == "Sunday")
                {
                    sum += p.BaseHourlyRate * t.Board[i].Hours * 2 * multiplier;
                }
                else
                {
                    if (t.Board[i].Hours > 6)
                    {
                        sum += (t.Board[i].Hours - 6) * p.BaseHourlyRate * 2 * multiplier + p.BaseHourlyRate * 6 * multiplier;
                    }
                    else
                    {
                        sum += p.BaseHourlyRate * t.Board[i].Hours * multiplier;
                    }
                }
            }
            if (e.IsMemberOfLaborUnion)
            {
                sum -= sum * 0.15;
            }
            else
            {
                sum -= sum * 0.13;
            }
            return sum;
        }

    }



}
