using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityMapKlyuchnikovaPolina
{
    class ActionDescription
    {
        public ActionDescription(string ActionNameRus, string ActionNameEng, string Description)
        {
            this.ActionNameRus = ActionNameRus;
            this.ActionNameEng = ActionNameEng;
            this.Description = Description;
        }
        public string ActionNameRus { get; set; }
        public string ActionNameEng { get; set; }
        public string Description { get; set; }
    }

    static class Action
    {
        public static bool DoAction(string ActionName)
        {
            switch (ActionName)
            {
                case "добавить":
                case "add":
                    AddStudent();
                    break;
                case "помощь":
                case "help":
                    Help();
                    break;
                case "показать группу":
                case "group":
                    ShowGroup();
                    break;
                case "s":
                case "оценки студента":
                case "rate":
                    StudRate();
                    break;
                case "удалить студента":
                case "del":
                    StudDel();
                    break;
                case "найти студента":
                case "find":
                    Find();
                    break;
                case "добавить генер":
                case "generated":
                    AddNewStudent();
                    break;
                case "закончить":
                case "end":
                    return false;
                default:
                    NotFind();
                    break;
            }
            return true;
        }

        private static void Find()
        {
            Console.WriteLine("Введите Имя студента:");
            List<Student> sList = Student.FindName(Console.ReadLine());
            Console.WriteLine("Найдены студенты:");
            foreach (var s in sList)
            {
                Console.WriteLine(s.Name + ". Группа: " + s.Group  + ".  Номер зачетки:" + s.NumberGradeBook+".");
            }
        }

        private static void StudDel()
        {
            Console.WriteLine("Введите номер зачетки:");
            int x;
            int.TryParse(Console.ReadLine(), out x);
            Student s = Student.DelStud(x);
            Console.WriteLine("Cтудент:"+
                s.Name + 
                ". Группа: " + s.Group + 
                ".  Номер зачетк:" + s.NumberGradeBook + "." +
                "Удален!");

        }

        private static void StudRate()
        {
            Console.WriteLine("Введите номер зачетной книжки");
            int x;
            if (!int.TryParse(Console.ReadLine(), out x))
            {
                Console.WriteLine("Не номер");
                return;
            }
            Student s = Student.GetStudent(x);
            Console.WriteLine(s.Name + " " + s.Group);
            for (int i = 0; i < (s.Rates).Count; i++)
            {
                Console.WriteLine(s.Name + " " + s.Rates[i].Subject + ": " + s.Rates[i].Rate);
            }
            Console.WriteLine("Добавить или Изменить оценки(y/n)");
            if (Console.Read() == 'n')
                return;
            string Agree = "y";
            while (Agree == "y")
            {
                Console.WriteLine("Введите предмет");
                string subject = Console.ReadLine();
                subject = Console.ReadLine();
                Console.WriteLine("Введите оценку");
                string rate = Console.ReadLine();

                Rates r = Student.EditRate(subject, rate, x);
                Console.WriteLine(s.Name + " " 
                    + r.Subject 
                    + ": " + r.Rate);

                Console.WriteLine("Продолжить?(y/n)");
                Agree = Console.ReadLine();
            }
        }

        private static void ShowGroup()
        {
            Console.WriteLine("Введите группу");
            string Group = Console.ReadLine();
            List<Student> students = Student.ShowGroup(Group);
            students.Sort(needToReOrder);
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine(students[i].NumberGradeBook+" " + students[i].Name);
            }
        }

        public static void AddStudent()
        {
            Console.WriteLine("Введите Имя студента:");
            string Name = Console.ReadLine();
            Console.WriteLine("Введите Группу:");
            string Group = Console.ReadLine();
            Student s = new Student(Name, Group);
            Student.AddStudent(s);
        }

        public static void Help()
        {
            List<ActionDescription> adList = new List<ActionDescription>();
            adList.Add(new ActionDescription("помощь", "help", "Показывает Доступные команды"));
            adList.Add(new ActionDescription("добавить", "add", "Добавляет нового студента"));
            adList.Add(new ActionDescription("закончиь", "end", "Закнчивает работу порграммы"));
            adList.Add(new ActionDescription("показать группу", "group", "Выводит список группы"));
            adList.Add(new ActionDescription("оценки студента", "rate", "Выводит оценки студента (Доступно измение)"));
            adList.Add(new ActionDescription("удалить студента", "del", "Удаляет студента (Безворатно)"));
            adList.Add(new ActionDescription("найти студента", "find", "Находит  информацию студентов с указзанным именем"));
            adList.Add(new ActionDescription("добавить генер", "generated", "Добавить в базу данных сгенерированных студентов"));

            for (int i = 0; i < adList.Count; i++)
                Console.WriteLine(adList[i].ActionNameEng + ", " + adList[i].ActionNameRus + ": " + adList[i].Description);
        }

        public static void NotFind()
        {
            Console.WriteLine("Несуществующая команда:");
        }

        public static int needToReOrder(Student S1, Student S2)
        {
            string s1 = S1.Name, s2 = S2.Name;
            for (int i = 0; i < (s1.Length > s2.Length ? s2.Length : s1.Length); i++)
            {
                if (s1.ToCharArray()[i] < s2.ToCharArray()[i]) return 1;
                if (s1.ToCharArray()[i] > s2.ToCharArray()[i]) return -1;
            }
            return 0;
        }
        static void AddNewStudent()
        {
            Console.WriteLine("Сколько хотите добавить новых студентов?(<100)");
            int x;
            while (!int.TryParse(Console.ReadLine(), out x))
            {
                Console.WriteLine("Введите число!");
            }
            Random rand = new Random();
            List<string> name = listgen.newListName();
            List<string> surename = listgen.newListSurename();
            string[] groups = { "931701", "931702", "931703", "931704", "931705", "931706", "007" };

            for (int i1 = 0; i1 < x; i1++)
            {
                List<Rates> rates = new List<Rates>();
                List<string> objects = listgen.newListObject();
                List<string> rate = listgen.newListRates();
                for (int i = 0; i < rand.Next(3, 7); i++)
                {
                    string ob = objects[rand.Next(5)];
                    rates.Add(new Rates(ob, rate[rand.Next(7)]));
                    objects.Remove(ob);
                }
                Student s = new Student(name[rand.Next(7)] + " " + surename[rand.Next(7)], groups[rand.Next(7)], rates);
                Console.WriteLine(s.Name + "" + s.Group);
                foreach (var item in s.Rates)
                    Console.WriteLine("   " + item.Subject + ": " + item.Rate);
                Student.AddStudent(s);
            }
        }

    }

    static class listgen
    {
        
        public static List<string> newListName()
        {
            List<string> objects = new List<string>();
            objects.Add("Шац");
            objects.Add("Шмидт");
            objects.Add("Гришко");
            objects.Add("Павленко");
            objects.Add("Никитенко");
            objects.Add("Киплинг");
            objects.Add("Танненбаум");
            return objects;
        }
        public static List<string> newListSurename()
        {
            List<string> objects = new List<string>();
            objects.Add("Анастасия");
            objects.Add("Мария");
            objects.Add("Полина");
            objects.Add("Сергйе");
            objects.Add("Николай");
            objects.Add("Генадий");
            objects.Add("Петр");
            return objects;
        }
        public static List<string> newListObject()
        {
            List<string> objects = new List<string>();
            objects.Add("Math");
            objects.Add("OOAD");
            objects.Add("Optimn methods");
            objects.Add("Data Base");
            objects.Add("Physical cult");
            objects.Add("Computer science");
            objects.Add("Web prog");
            objects.Add("Theory of probability");
            objects.Add("Statistics");
            return objects;
        }
        public static List<string> newListRates()
        {
            List<string> rates = new List<string>();
            rates.Add("awfully!");
            rates.Add("disgustingly!!!");
            rates.Add("Magically");
            rates.Add("so-so...");
            rates.Add("Physical");
            rates.Add("Satisfactorily");
            rates.Add("Not Bad");
            rates.Add("Bad");
            return rates;
        }


    }

}
