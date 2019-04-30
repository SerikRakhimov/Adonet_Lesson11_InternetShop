using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InternetShop
{
    public class Choice
    {
        private string check;
        private int i, number;


        public void DataInit()
        {

            using (DataContext context = new DataContext())
            {
                var products = context.Products.ToList();
                if (products.Count == 0)
                {
                    context.Products.AddRange(new List<Product> {
                    new Product{Name = "Картофель", Price = 80},
                    new Product{Name = "Лук речатый", Price = 120},
                    new Product{Name = "Морковь", Price = 150},
                    new Product{Name = "Яблоки Golden", Price = 400},
                    new Product{Name = "Банан", Price = 350},
                    new Product{Name = "Мандарин", Price = 700},
                    new Product{Name = "Апельсин", Price = 800}}
                    );
                }
                context.SaveChanges();
            }
        }

        public User LoginUser()
        {
            string login, password, phoneNumber, menu;
            User result = null;

            List<User> users;
            using (DataContext context = new DataContext())
            {
                while (true)
                {

                    Console.WriteLine("\n\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine("\n\tВвод пользователя:");
                    Console.WriteLine("\n\t1 - Регистрация пользователя");
                    Console.WriteLine("\t2 - Вход для зарегистрированных пользователей");
                    Console.WriteLine("\t0 - Выход");
                    Console.Write("\n\tВаш выбор = ");
                    menu = Console.ReadLine();

                    if (menu == "0")
                    {
                        break;
                    }

                    if (menu == "1" || menu == "2")
                    {

                        do
                        {
                            Console.Write("\n\tВаш логин = ");
                            login = Console.ReadLine();
                        } while (login == "");

                        do
                        {
                            Console.Write("\tВаш пароль = ");
                            password = Console.ReadLine();
                        } while (password == "");


                        if (menu == "1") // регистрация
                        {
                            users = context.Users.Where(user => user.Login == login).ToList();

                            if (users.Count != 0)
                            {
                                Console.WriteLine($"\n\tТакой пользователь уже зарегистрирован в базе данных.");
                                continue;
                            }
                            else
                            {
                                do
                                {
                                    Console.Write("\tВаш номер телефона = ");
                                    phoneNumber = Console.ReadLine();
                                } while (phoneNumber == "");

                                context.Users.Add(new User { Login = login, Password = password, PhoneNumber = phoneNumber });
                                context.SaveChanges();

                                users = context.Users.Where(user => user.Login == login).ToList();
                                result = users.FirstOrDefault();

                                Console.WriteLine($"\n\tИнформация о пользователе добавлена.");

                                break;
                            }

                        }
                        if (menu == "2") // вход
                        {
                            users = context.Users.Where(user => user.Login == login).ToList();
                            User find = users.FirstOrDefault();

                            if (users.Count != 0)
                            {
                                if (password == find.Password)
                                {

                                    Console.WriteLine($"\n\tПароль введен правильно.");
                                    result = find;
                                    break;

                                }
                                else
                                {
                                    Console.WriteLine($"\n\tПароль введен неправильно.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"\n\tЛогин пользователя не найден!");
                            }

                        }

                    }
                }
            }
            return result;
        }

        public Product ChoiceProduct(User user)
        {

            int count;
            Product result = null;
            List<Product> products;
            using (DataContext context = new DataContext())
            {
                products = context.Products.ToList();
                if (products.Count == 0)
                {
                    Console.WriteLine("\n\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine($"\tПользователь: {user.Login}");
                    Console.WriteLine($"\tСписок товаров пуст.");
                }
                else
                {
                    while (true)
                    {
                        products = context.Products.OrderBy(item => item.Name).ToList();

                        Console.WriteLine("\n\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                        Console.WriteLine($"\tПользователь: {user.Login}");
                        Console.WriteLine($"\t{BasketInfo(user)}");
                        Console.WriteLine("\n\tСписок товаров:\n");
                        int i = 0;
                        foreach (var item in products)
                        {
                            i++;
                            Console.WriteLine($"\t{i} - {item.Name} - {item.Price} тнг");
                        };

                        Console.WriteLine($"\n\t0 - выход, '+' - перейти в Корзину");
                        Console.Write($"\n\tВведите номер товара для покупки (1-{products.Count}) = ");
                        check = Console.ReadLine();

                        if (check == "+")
                        {
                            BasketWork(user);
                            break;
                        }
                        
                        try
                        {
                            number = int.Parse(check);
                            if (number == 0)  // выход с программы
                            {
                                break;
                            }
                            else if (1 <= number && number <= products.Count)
                            {

                                Product product = products[number - 1];
                                Console.WriteLine("\n\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                                Console.WriteLine("\tПокупка товара");
                                Console.WriteLine($"\n\t {product.Name} цена = {product.Price} тнг");

                                while (true)
                                {
                                    Console.Write("\t Количество = ");
                                    check = Console.ReadLine();
                                    try
                                    {
                                        count = int.Parse(check);
                                        break;
                                    }
                                    catch
                                    {
                                    }
                                };

                                User find = context.Users.Where(user1 => user1.Id == user.Id).ToList().FirstOrDefault();

                                context.Baskets.Add(new Basket { User = find, Product = product, Count = count, Pay = false, CardNumber = "", PayDate = null });
                                context.SaveChanges();

                                Console.WriteLine($"\n\tИнформация о покупке добавлена в Корзину.");
                            }
                        }
                        catch
                        {
                        }

                        if (number != 0)
                        {
                            number--;
                            result = products[number];
                        }
                    }
                }
                return result;
            }
        }

        public string BasketInfo(User user)
        {
            int sumItog;
            string result;
            using (DataContext context = new DataContext())
            {
                var users = context.Users.Where(item => item.Id == user.Id).ToList();
                if (users.Count == 0)
                {
                    result = "Корзина пуста.";
                }
                else
                {
                    User find = users.FirstOrDefault();
                    context.Entry(find).Collection("Baskets").Load();
                    var baskets = find.Baskets.Where(item => item.Pay == false).ToList();

                    //var baskets1 = users.SelectMany(sm => sm.Baskets).ToList();

                    if (baskets.Count == 0)
                    {
                        result = "Корзина пуста.";
                    }
                    else
                    {
                        sumItog = 0;
                        foreach (var item in baskets)
                        {
                            sumItog += item.Product.Price * item.Count;
                        }
                        result = $"В Корзине {baskets.Count} товаров на сумму {sumItog} тнг.";
                    }
                }

            }
            return result;
        }

        public void BasketWork(User user)
        {
            int sumItog, sum;
            using (DataContext context = new DataContext())
            {
                Console.WriteLine("\n\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine($"\tПользователь: {user.Login}");

                var users = context.Users.Where(item => item.Id == user.Id).ToList();
                User find = users.FirstOrDefault();
                context.Entry(find).Collection("Baskets").Load();
                var baskets = find.Baskets.Where(item => item.Pay == false).ToList();

                if (baskets.Count == 0)
                {
                    Console.WriteLine("\tКорзина пуста.");
                }
                else
                {
                    Console.WriteLine("\tСписок товаров в Корзине:\n");
                    sumItog = 0;
                    int i = 0;
                    foreach (var item in baskets)
                    {
                        i++;
                        sum = item.Product.Price * item.Count;
                        sumItog += sum;
                        Console.WriteLine($"\t{i} - {item.Product.Name} - Цена = {item.Product.Price}  Кол-во = {item.Count}  Сумма = {sum} тнг.");
                    }
                    Console.WriteLine($"\n\tИтого: {baskets.Count} товаров на сумму {sumItog} тнг.");

                    Console.Write("\n\t1 - оформить покупку, 0 - выход = ");
                    string menu = Console.ReadLine();

                    if (menu == "1")
                    {
                        Console.Write("\n\tВведите номер карточки для оплаты = ");
                        string cardnumber = Console.ReadLine();
                        if (cardnumber!="")
                        {
                            foreach (var item in baskets)
                            {
                                item.Pay = true;
                                item.PayDate = DateTime.Now;
                                item.CardNumber = cardnumber;
                            }
                            context.SaveChanges();
                            Console.WriteLine("\n\tОплата произведена.");
                        }
                    }
                }
            }
        }
    }
}