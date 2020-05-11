using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace IdentityMapKlyuchnikovaPolina
{
    class Program
    {
        static void Main(string[] args)
        {
            string Act;
            do
            {
                Console.WriteLine("Выбирите Действие:");
                Act = Console.ReadLine();
            }
            while (Action.DoAction(Act));
        }

    }
   
}
