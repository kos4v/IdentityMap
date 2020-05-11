using AppContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace IdentityMapKlyuchnikovaPolina
{
    class Student
    {
        [Key]
        public int NumberGradeBook { get; private set; }
        internal static bool AddStudent(Student s)
        {
            IdentityMap Im = IdentityMap.Instance;

            return Im.AddStudent(s);
        }
        public string Name { get; set; }
        public string Group { get; set; }
        public List<Rates> Rates { get; set; }

        internal static Student GetStudent(int NumberGradeBook)
        {
            return IdentityMap.Instance.GetStudent(NumberGradeBook);
        }

        public Student(string Name, string Group)
        {
            this.Name = Name;
            this.Group = Group;
        }

        public Student(string notFound)
        {
            this.Name = notFound;
            this.Group = notFound;
            this.NumberGradeBook = 0;
        }

        public Student(string Name, string Group, List<Rates> rates)
        {
            Rates = new List<Rates>();
            this.Name = Name;
            this.Group = Group;
            Rates.AddRange(rates);
        }

        internal static List<Student> ShowGroup(string group)
        {
            return IdentityMap.Instance.GetGroup(group); ;
        }

        internal static Rates EditRate(string subject, string rate, int StudentNumberGradeBook)
        {
            return IdentityMap.Instance.EditRate(subject, rate, StudentNumberGradeBook);
        }

        internal static Student DelStud(int NumberGradeBook)
        {
            return IdentityMap.Instance.DelStud(NumberGradeBook);
        }

        internal static List<Student> FindName(string Name)
        {
            return IdentityMap.Instance.findName(Name);
        }
    }

    class Rates
    {
        [Key]
        public int Id { get; private set; }
        public string Subject { get; set; }
        public string Rate { get; set; }

        public int StudentNumberGradeBook { get; internal set; }

        [ForeignKey("StudentNumberGradeBook")]
        public Student Student { get; set; }

        public Rates(string Subject, string Rate)
        {
            this.Subject = Subject;
            this.Rate = Rate;
        }
        public Rates(string Subject, string Rate, Student student)
        {
            this.Subject = Subject;
            this.Rate = Rate;
            this.Student = student;
        }
    }
}
