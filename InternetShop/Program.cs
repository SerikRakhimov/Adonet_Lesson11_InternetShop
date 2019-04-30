using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop
{
    class Program
    {
        static void Main(string[] args)
        {

            string menu;
            User user, user_buffer;          // выбранный и дополнительный пользователи
            Product product;                 // выбранный продукт

            var choice = new Choice();
            choice.DataInit();
            Console.WriteLine("\n\t\t И Н Т Е Р Н Е Т  -  М А Г А З И Н");

            user = new User();
            user_buffer = choice.LoginUser();

            if (user_buffer == null)
            {
                return;
            }
            user = user_buffer;

            while (true)
            {
                Console.WriteLine("\n\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine($"\tПользователь: {user.Login}");
                Console.WriteLine($"\t{choice.BasketInfo(user)}");
                Console.WriteLine("\n\t\tГлавное меню:\n");
                Console.WriteLine("\t1 - Интернет-магазин");
                Console.WriteLine("\t2 - Перейти в Корзину");
                Console.WriteLine("\t3 - Сменить пользователя");
                Console.WriteLine("\t0 - Выход\n");
                Console.Write("\tВаше выбор = ");
                menu = Console.ReadLine();

                if (menu == "0")
                {
                    break;
                }
                if (menu == "1")   // интернет-магазин
                {
                    product = choice.ChoiceProduct(user);
                    if (product!=null)
                    {
                        // choice.ProductActions(user, product);
                    }
                }
                if (menu == "2")   // перейти в Корзину
                {

                    choice.BasketWork(user);
                }
                if (menu == "3")   // сменить пользователя
                {

                    user_buffer = choice.LoginUser();

                    if (user_buffer != null)
                    {
                        user = user_buffer;
                    }
                }
            }

        }
    }
}
