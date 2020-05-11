using AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace IdentityMapKlyuchnikovaPolina
{
    class IdentityMap
    {
        public List<Student> StudentsList { get; set; }
        public List<List<Student>> GroupsStudents { get; set; }
        private IdentityMap()
        {
            StudentsList = new List<Student>();
            GroupsStudents = new List<List<Student>>();
        }
        public static IdentityMap Instance { get; } = new IdentityMap();
        internal bool AddStudent(Student s)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.students.Add(s);
                db.SaveChanges();
                Console.WriteLine("Cтудент: "+s.Name+", Группы:"+s.Group+": Успешно добавлен!");
                return true;
            }

        }
        internal Student GetStudent(int numberGradeBook)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Student s = FindStudent(numberGradeBook);
                if (s.Name != "not found")
                {
                    Console.WriteLine("Взято из Identity Map");
                    return s;
                }

                Student student = db.students.FirstOrDefault
                    (p => p.NumberGradeBook == 100001);
                List<Rates> rates = db.Rates.Where
                    (p => p.StudentNumberGradeBook == numberGradeBook).ToList();
                student.Rates = db.Rates.
                    Include(u => u.Student).
                    Where(p => p.StudentNumberGradeBook == numberGradeBook).ToList();

                if (student == null)
                    return new Student("Not Found");
                StudentsList.Add(student);
                return  student;
            }
        }

        internal List<Student> findName(string name)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Student> s = db.students.Where(p => EF.Functions.Like(p.Name, "%"+name+"%")).ToList();
                return s;
            }
        }

        internal Student DelStud(int numberGradeBook)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Student DEl1 = db.students.FirstOrDefault(p => p.NumberGradeBook == numberGradeBook);
                if (DEl1 != null)
                {
                    db.students.Remove(DEl1);
                    StudentsList.Remove(DEl1);
                    db.SaveChanges();
                }
                else
                    DEl1 = new Student("Not Found");
                return DEl1;
            }
        }

        internal Rates EditRate(string subject, string rate, int StudentNumberGradeBook)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Rates newrate = db.Rates.FirstOrDefault(p => p.StudentNumberGradeBook == StudentNumberGradeBook);
                if (newrate != null)
                {
                    newrate = db.Rates.FirstOrDefault(p => p.StudentNumberGradeBook == StudentNumberGradeBook & p.Subject == subject);
                    if (newrate != null)
                    {
                        newrate.Rate = rate;
                        foreach (var item in StudentsList) if (item.NumberGradeBook == StudentNumberGradeBook)
                            {
                                foreach (var rates in item.Rates) if (rates.Subject == subject)
                                    {
                                        rates.Rate = rate; break;
                                    }
                                break;
                            }
                    }
                    else
                    {
                        newrate = new Rates(subject, rate, db.students.FirstOrDefault(p => p.NumberGradeBook == StudentNumberGradeBook));
                        db.Rates.Add(newrate);
                        foreach (var item in StudentsList) if (item.NumberGradeBook == StudentNumberGradeBook)
                            { item.Rates.Add(newrate); break; }
                    }
                    db.SaveChanges();
                    return newrate;
                }
                else
                    return new Rates("Not ", "found");
            }
        }
        private Student FindStudent(int numberGradeBook)
        {
            foreach (var item in StudentsList)
            {
                if (item.NumberGradeBook == numberGradeBook)
                    return item;
                
            }
            return new Student("not found", "not found");
        }
        internal  List<Student> GetGroup(string group)
        {
            for (int i = 0; i < GroupsStudents.Count; i++)
                if (GroupsStudents[i][0].Group == group)
                {
                    Console.WriteLine("Взято из Identity Map");
                    return GroupsStudents[i];
                }
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Student> students = db.students.Where(p => EF.Functions.Like(p.Group, "%" + group + "%")).ToList();
                GroupsStudents.Add(students);
                
                return students;
            }
        }
    }

}
