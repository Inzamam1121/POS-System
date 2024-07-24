using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POS_System.Data;
using POS_System.Entities;
using POS_System.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        //var host = CreateHostBuilder().Build();

        //using (var scope = host.Services.CreateScope())
        //{
        //    var services = scope.ServiceProvider;

        //    var userService = services.GetRequiredService<UserService>();
        //    var productService = services.GetRequiredService<ProductService>();
        //    var saleTransactionService = services.GetRequiredService<SaleTransactionService>();
        //    var context = services.GetRequiredService<DataContextEntity>();

        //    // Create and register an admin user
        //    var admin = new User("Admin", "admin@gmail.com", "admin", UserRole.Admin);
        //    userService.RegisterUser(admin);

        //    // Create and register a cashier user
        //    var cashier = new User("Cashier", "cashier@gmail.com", "cashier", UserRole.Cashier);
        //    userService.RegisterUser(cashier);

        //    // Add categories
        //    var electronicsCategory = new Category { Name = "Electronics" };
        //    context.Categories.Add(electronicsCategory);
        //    context.SaveChanges();

        //    // Add products
        //    var product1 = new Product { Name = "Laptop", Price = 1200.00m, Quantity = 10, Type = "Gadget", Category = electronicsCategory };
        //    var product2 = new Product { Name = "Smartphone", Price = 800.00m, Quantity = 20, Type = "Gadget", Category = electronicsCategory };
        //    productService.AddProduct(product1, admin);
        //    productService.AddProduct(product2, admin);
        //    context.SaveChanges();

        //    // Main Menu loop
        //    while (true)
        //    {
        //        Console.Clear();
        //        Console.WriteLine("=================================");
        //        Console.WriteLine("        POS System Main Menu     ");
        //        Console.WriteLine("=================================");
        //        Console.WriteLine("1. Register User");
        //        Console.WriteLine("2. Login");
        //        Console.WriteLine("3. Exit");
        //        Console.Write("Select an option: ");
        //        var option = Console.ReadLine();

        //        if (option == "1")
        //        {
        //            Console.Write("Name: ");
        //            var name = Console.ReadLine();
        //            Console.Write("Email: ");
        //            var email = Console.ReadLine();
        //            Console.Write("Password: ");
        //            var password = Console.ReadLine();

        //            var newUser = new User(name, email, password, UserRole.Cashier); // Default role is cashier
        //            userService.RegisterUser(newUser);
        //            context.SaveChanges();
        //            Console.WriteLine("User registered successfully. Please contact admin to assign role.");
        //            Console.ReadLine();
        //        }
        //        else if (option == "2")
        //        {
        //            Console.Write("Email: ");
        //            var email = Console.ReadLine();
        //            Console.Write("Password: ");
        //            var password = Console.ReadLine();

        //            var loggedInUser = userService.AuthenticateUser(email, password);

        //            if (loggedInUser != null)
        //            {
        //                if (loggedInUser.Role == UserRole.Admin)
        //                {
        //                    await AdminMenuAsync(loggedInUser, productService, userService, context);
        //                }
        //                else if (loggedInUser.Role == UserRole.Cashier)
        //                {
        //                    await CashierMenuAsync(loggedInUser, saleTransactionService, productService, context);
        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine("Invalid credentials. Please try again.");
        //                Console.ReadLine();
        //            }
        //        }
        //        else if (option == "3")
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            Console.WriteLine("Invalid option. Please try again.");
        //            Console.ReadLine();
        //        }
        //    }
        //}
    }

    //static async Task AdminMenuAsync(User admin, ProductService productService, UserService userService, DataContextEntity context)
    //{
    //    while (true)
    //    {
    //        Console.Clear();
    //        Console.WriteLine("=================================");
    //        Console.WriteLine("          Admin Menu             ");
    //        Console.WriteLine("=================================");
    //        Console.WriteLine("1. Add Product");
    //        Console.WriteLine("2. View Products");
    //        Console.WriteLine("3. Update Product");
    //        Console.WriteLine("4. Remove Product");
    //        Console.WriteLine("5. Assign User Role");
    //        Console.WriteLine("6. View Sales");
    //        Console.WriteLine("7. Logout");
    //        Console.Write("Select an option: ");
    //        var option = Console.ReadLine();

    //        if (option == "1")
    //        {
    //            Console.Write("Product Name: ");
    //            var name = Console.ReadLine();
    //            Console.Write("Price: ");
    //            var price = decimal.Parse(Console.ReadLine());
    //            Console.Write("Quantity: ");
    //            var quantity = int.Parse(Console.ReadLine());
    //            Console.Write("Type: ");
    //            var type = Console.ReadLine();

    //            Console.WriteLine("Select Category:");
    //            for (int i = 0; i < context.Categories.Count(); i++)
    //            {
    //                Console.WriteLine($"{i + 1}. {context.Categories.ToList()[i].Name}");
    //            }
    //            var categoryIndex = int.Parse(Console.ReadLine()) - 1;
    //            var category = context.Categories.ToList()[categoryIndex];

    //            var product = new Product { Name = name, Price = price, Quantity = quantity, Type = type, Category = category };
    //            productService.AddProduct(product, admin);
    //            context.SaveChanges();
    //            Console.WriteLine("Product added successfully.");
    //            Console.ReadLine();
    //        }
    //        else if (option == "2")
    //        {
    //            Console.Clear();
    //            Console.WriteLine("=================================");
    //            Console.WriteLine("         Product List            ");
    //            Console.WriteLine("=================================");
    //            foreach (var product in context.Products.Include(p => p.Category).ToList())
    //            {
    //                Console.WriteLine($"Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}, Type: {product.Type}, Category: {product.Category.Name}");
    //                Console.WriteLine("---------------------------------");
    //            }
    //            Console.ReadLine();
    //        }
    //        else if (option == "3")
    //        {
    //            Console.Write("Enter product name to update: ");
    //            var name = Console.ReadLine();
    //            var product = context.Products.Include(p => p.Category).FirstOrDefault(p => p.Name == name);
    //            if (product != null)
    //            {
    //                Console.Write("New Price: ");
    //                var price = decimal.Parse(Console.ReadLine());
    //                Console.Write("New Quantity: ");
    //                var quantity = int.Parse(Console.ReadLine());
    //                Console.Write("New Type: ");
    //                var type = Console.ReadLine();

    //                Console.WriteLine("Select New Category:");
    //                for (int i = 0; i < context.Categories.Count(); i++)
    //                {
    //                    Console.WriteLine($"{i + 1}. {context.Categories.ToList()[i].Name}");
    //                }
    //                var categoryIndex = int.Parse(Console.ReadLine()) - 1;
    //                var category = context.Categories.ToList()[categoryIndex];

    //                product.UpdateProduct(name, price, quantity, type, category);
    //                context.SaveChanges();
    //                Console.WriteLine("Product updated successfully.");
    //            }
    //            else
    //            {
    //                Console.WriteLine("Product not found.");
    //            }
    //            Console.ReadLine();
    //        }
    //        else if (option == "4")
    //        {
    //            Console.Write("Enter product name to remove: ");
    //            var name = Console.ReadLine();
    //            var product = context.Products.FirstOrDefault(p => p.Name == name);
    //            if (product != null)
    //            {
    //                productService.RemoveProduct(product, admin);
    //                context.SaveChanges();
    //                Console.WriteLine("Product removed successfully.");
    //            }
    //            else
    //            {
    //                Console.WriteLine("Product not found.");
    //            }
    //            Console.ReadLine();
    //        }
    //        else if (option == "5")
    //        {
    //            Console.Write("Enter user email to assign role: ");
    //            var email = Console.ReadLine();
    //            var user = context.Users.FirstOrDefault(u => u.Email == email);
    //            if (user != null)
    //            {
    //                Console.WriteLine("Select Role:");
    //                Console.WriteLine("1. Admin");
    //                Console.WriteLine("2. Cashier");
    //                var role = int.Parse(Console.ReadLine());
    //                userService.SetUserRole(user, (UserRole)(role - 1));
    //                context.SaveChanges();
    //                Console.WriteLine("Role assigned successfully.");
    //            }
    //            else
    //            {
    //                Console.WriteLine("User not found.");
    //            }
    //            Console.ReadLine();
    //        }
    //        else if (option == "6")
    //        {
    //            Console.Clear();
    //            Console.WriteLine("=================================");
    //            Console.WriteLine("           Sales List            ");
    //            Console.WriteLine("=================================");
    //            foreach (var sale in context.Sales.Include(s => s.Cashier).Include(s => s.SaleProducts).ThenInclude(sp => sp.Product).ToList())
    //            {
    //                Console.WriteLine($"Sale ID: {sale.SaleId}, Cashier: {sale.Cashier.Name}, Date: {sale.Date}");
    //                foreach (var saleProduct in sale.SaleProducts)
    //                {
    //                    Console.WriteLine($"    Product: {saleProduct.Product.Name}, Quantity: {saleProduct.Quantity}, Price: {saleProduct.Product.Price}");
    //                }
    //                Console.WriteLine("---------------------------------");
    //            }
    //            Console.ReadLine();
    //        }
    //        else if (option == "7")
    //        {
    //            break;
    //        }
    //        else
    //        {
    //            Console.WriteLine("Invalid option. Please try again.");
    //            Console.ReadLine();
    //        }
    //    }
    //}

    //static async Task CashierMenuAsync(User cashier, SaleTransactionService saleTransactionService, ProductService productService, DataContextEntity context)
    //{
    //    while (true)
    //    {
    //        Console.Clear();
    //        Console.WriteLine("=================================");
    //        Console.WriteLine("         Cashier Menu            ");
    //        Console.WriteLine("=================================");
    //        Console.WriteLine("1. Start New Sale");
    //        Console.WriteLine("2. Add Product to Sale");
    //        Console.WriteLine("3. Complete Sale");
    //        Console.WriteLine("4. View Current Sale");
    //        Console.WriteLine("5. Logout");
    //        Console.Write("Select an option: ");
    //        var option = Console.ReadLine();

    //        if (option == "1")
    //        {
    //            saleTransactionService.StartNewSale(cashier);
    //            Console.WriteLine("New sale started.");
    //            Console.ReadLine();
    //        }
    //        else if (option == "2")
    //        {
    //            var currentSale = saleTransactionService.GetCurrentSale();
    //            if (currentSale != null)
    //            {
    //                Console.Write("Enter product name to add: ");
    //                var productName = Console.ReadLine();
    //                var product = context.Products.FirstOrDefault(p => p.Name == productName);
    //                if (product != null)
    //                {
    //                    Console.Write("Enter quantity: ");
    //                    var quantity = int.Parse(Console.ReadLine());

    //                    var success = await saleTransactionService.AddProductToSaleAsync(product, quantity, cashier);
    //                    if (success)
    //                    {
    //                        Console.WriteLine("Product added to sale.");
    //                    }
    //                }
    //                else
    //                {
    //                    Console.WriteLine("Product not found.");
    //                }
    //            }
    //            else
    //            {
    //                Console.WriteLine("No active sale. Please start a new sale first.");
    //            }
    //            Console.ReadLine();
    //        }
    //        else if (option == "3")
    //        {
    //            var currentSale = saleTransactionService.GetCurrentSale();
    //            if (currentSale != null)
    //            {
    //                await saleTransactionService.CompleteSaleAsync(cashier);
    //                Console.WriteLine("Sale completed.");
    //            }
    //            else
    //            {
    //                Console.WriteLine("No active sale. Please start a new sale first.");
    //            }
    //            Console.ReadLine();
    //        }
    //        else if (option == "4")
    //        {
    //            var currentSale = saleTransactionService.GetCurrentSale();
    //            if (currentSale != null)
    //            {
    //                Console.WriteLine("=================================");
    //                Console.WriteLine("         Current Sale            ");
    //                Console.WriteLine("=================================");
    //                Console.WriteLine($"Cashier: {currentSale.Cashier.Name}");
    //                foreach (var saleProduct in currentSale.SaleProducts)
    //                {
    //                    Console.WriteLine($"Product: {saleProduct.Product.Name}, Quantity: {saleProduct.Quantity}, Price: {saleProduct.Product.Price}");
    //                }
    //                Console.WriteLine("---------------------------------");
    //            }
    //            else
    //            {
    //                Console.WriteLine("No active sale.");
    //            }
    //            Console.ReadLine();
    //        }
    //        else if (option == "5")
    //        {
    //            break;
    //        }
    //        else
    //        {
    //            Console.WriteLine("Invalid option. Please try again.");
    //            Console.ReadLine();
    //        }
    //    }
    //}

    //static IHostBuilder CreateHostBuilder() =>
    //    Host.CreateDefaultBuilder()
    //        .ConfigureServices((context, services) =>
    //        {
    //            services.AddDbContext<DataContextEntity>(options =>
    //                options.UseInMemoryDatabase("POSSystemDB"));

    //            services.AddScoped<UserService>();
    //            services.AddScoped<ProductService>();
    //            services.AddScoped<SaleTransactionService>();
    //        });
}
