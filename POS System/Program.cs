using POS_System.Data;
using POS_System.Entities;
using POS_System.Services;
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        UserService userService = new UserService();
        ProductService productService = new ProductService();
        SaleTransactionService saleTransactionService = new SaleTransactionService();

        // Create and register an admin user
        User admin = new User("Admin", "admin@gmail.com", "admin", UserRole.Admin);
        userService.RegisterUser(admin);

        // Create and register a cashier user
        User cashier = new User("Cashier", "cashier@gmail.com", "cashier", UserRole.Cashier);
        userService.RegisterUser(cashier);

        // Add categories
        Category electronicsCategory = new Category("Electronics");
        DataContext.Categories.Add(electronicsCategory);

        // Add products
        Product product1 = new Product("Laptop", 1200.00m, 10, "Gadget", electronicsCategory);
        Product product2 = new Product("Smartphone", 800.00m, 20, "Gadget", electronicsCategory);
        productService.AddProduct(product1, admin);
        productService.AddProduct(product2, admin);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("        POS System Main Menu     ");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Register User");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            if (option == "1")
            {
                Console.Write("Name: ");
                string name = Console.ReadLine();
                Console.Write("Email: ");
                string email = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                User newUser = new User(name, email, password, UserRole.Cashier); // Default role is cashier
                userService.RegisterUser(newUser);
                Console.WriteLine("User registered successfully. Please contact admin to assign role.");
                Console.ReadLine();
            }
            else if (option == "2")
            {
                Console.Write("Email: ");
                string email = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                User loggedInUser = userService.AuthenticateUser(email, password);

                if (loggedInUser != null)
                {
                    if (loggedInUser.Role == UserRole.Admin)
                    {
                        AdminMenu(loggedInUser, productService, userService);
                    }
                    else if (loggedInUser.Role == UserRole.Cashier)
                    {
                        CashierMenu(loggedInUser, saleTransactionService, productService);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid credentials. Please try again.");
                    Console.ReadLine();
                }
            }
            else if (option == "3")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please try again.");
                Console.ReadLine();
            }
        }
    }

    static void AdminMenu(User admin, ProductService productService, UserService userService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("          Admin Menu             ");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. View Products");
            Console.WriteLine("3. Update Product");
            Console.WriteLine("4. Remove Product");
            Console.WriteLine("5. Assign User Role");
            Console.WriteLine("6. View Sales");
            Console.WriteLine("7. Logout");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            if (option == "1")
            {
                Console.Write("Product Name: ");
                string name = Console.ReadLine();
                Console.Write("Price: ");
                decimal price = decimal.Parse(Console.ReadLine());
                Console.Write("Quantity: ");
                int quantity = int.Parse(Console.ReadLine());
                Console.Write("Type: ");
                string type = Console.ReadLine();

                Console.WriteLine("Select Category:");
                for (int i = 0; i < DataContext.Categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {DataContext.Categories[i].Name}");
                }
                int categoryIndex = int.Parse(Console.ReadLine()) - 1;
                Category category = DataContext.Categories[categoryIndex];

                Product product = new Product(name, price, quantity, type, category);
                productService.AddProduct(product, admin);
                Console.WriteLine("Product added successfully.");
                Console.ReadLine();
            }
            else if (option == "2")
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("         Product List            ");
                Console.WriteLine("=================================");
                foreach (var product in DataContext.Products)
                {
                    Console.WriteLine($"Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}, Type: {product.Type}, Category: {product.Category.Name}");
                    Console.WriteLine("---------------------------------");
                }
                Console.ReadLine();
            }
            else if (option == "3")
            {
                Console.Write("Enter product name to update: ");
                string name = Console.ReadLine();
                var product = DataContext.Products.FirstOrDefault(p => p.Name == name);
                if (product != null)
                {
                    Console.Write("New Price: ");
                    decimal price = decimal.Parse(Console.ReadLine());
                    Console.Write("New Quantity: ");
                    int quantity = int.Parse(Console.ReadLine());
                    Console.Write("New Type: ");
                    string type = Console.ReadLine();

                    Console.WriteLine("Select New Category:");
                    for (int i = 0; i < DataContext.Categories.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {DataContext.Categories[i].Name}");
                    }
                    int categoryIndex = int.Parse(Console.ReadLine()) - 1;
                    Category category = DataContext.Categories[categoryIndex];

                    product.UpdateProduct(name, price, quantity, type, category);
                    Console.WriteLine("Product updated successfully.");
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }
                Console.ReadLine();
            }
            else if (option == "4")
            {
                Console.Write("Enter product name to remove: ");
                string name = Console.ReadLine();
                var product = DataContext.Products.FirstOrDefault(p => p.Name == name);
                if (product != null)
                {
                    productService.RemoveProduct(product, admin);
                    Console.WriteLine("Product removed successfully.");
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }
                Console.ReadLine();
            }
            else if (option == "5")
            {
                Console.Write("Enter user email to assign role: ");
                string email = Console.ReadLine();
                var user = DataContext.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    Console.WriteLine("Select Role:");
                    Console.WriteLine("1. Admin");
                    Console.WriteLine("2. Cashier");
                    int role = int.Parse(Console.ReadLine());
                    userService.SetUserRole(user, (UserRole)(role - 1));
                    Console.WriteLine("Role assigned successfully.");
                }
                else
                {
                    Console.WriteLine("User not found.");
                }
                Console.ReadLine();
            }
            else if (option == "6")
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("           Sales List            ");
                Console.WriteLine("=================================");
                foreach (var sale in DataContext.Sales)
                {
                    Console.WriteLine($"Date: {sale.Date}");
                    Console.WriteLine($"Cashier: {sale.Cashier.Name}");
                    foreach (var saleProduct in sale.SaleProducts)
                    {
                        for (int i = 0; i < saleProduct.Products.Count; i++)
                        {
                            Console.WriteLine($"Product: {saleProduct.Products[i].Name}, Quantity: {saleProduct.Quantities[i]}, Total Price: {saleProduct.Products[i].Price * saleProduct.Quantities[i]}");
                        }
                    }
                    Console.WriteLine($"Total Sale Amount: {sale.TotalAmount}");
                    Console.WriteLine("---------------------------------");
                }
                Console.ReadLine();
            }
            else if (option == "7")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please try again.");
                Console.ReadLine();
            }
        }
    }

    static void CashierMenu(User cashier, SaleTransactionService saleTransactionService, ProductService productService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("         Cashier Menu            ");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Start New Sale");
            Console.WriteLine("2. Add Product to Sale");
            Console.WriteLine("3. Complete Sale");
            Console.WriteLine("4. View Current Sale");
            Console.WriteLine("5. Logout");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            if (option == "1")
            {
                saleTransactionService.StartNewSale(cashier);
                Console.WriteLine("New sale started.");
                Console.ReadLine();
            }
            else if (option == "2")
            {
                Console.WriteLine("=================================");
                Console.WriteLine("         Product List            ");
                Console.WriteLine("=================================");
                for (int i = 0; i < DataContext.Products.Count; i++)
                {
                    var product = DataContext.Products[i];
                    Console.WriteLine($"{i + 1}. Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}");
                }

                Console.Write("Select product by number: ");
                int productIndex = int.Parse(Console.ReadLine()) - 1;

                if (productIndex >= 0 && productIndex < DataContext.Products.Count)
                {
                    var selectedProduct = DataContext.Products[productIndex];
                    Console.Write("Enter quantity: ");
                    int quantity = int.Parse(Console.ReadLine());

                    if (saleTransactionService.AddProductToSale(selectedProduct, quantity, cashier))
                    {
                        Console.WriteLine("Product added to sale.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add product to sale. Check product quantity.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid product selection.");
                }
                Console.ReadLine();
            }
            else if (option == "3")
            {
                saleTransactionService.CompleteSale(cashier);
                Console.WriteLine("Sale completed.");
                Console.ReadLine();
            }
            else if (option == "4")
            {
                var currentSale = saleTransactionService.GetCurrentSale();
                if (currentSale != null)
                {
                    Console.Clear();
                    Console.WriteLine("=================================");
                    Console.WriteLine("       Current Sale Details      ");
                    Console.WriteLine("=================================");
                    foreach (var saleProduct in currentSale.SaleProducts)
                    {
                        for (int i = 0; i < saleProduct.Products.Count; i++)
                        {
                            Console.WriteLine($"Product: {saleProduct.Products[i].Name}, Quantity: {saleProduct.Quantities[i]}, Price: {saleProduct.Products[i].Price * saleProduct.Quantities[i]}");
                        }
                    }
                    Console.WriteLine($"Total Sale Amount: {currentSale.TotalAmount}");
                }
                else
                {
                    Console.WriteLine("No sale in progress.");
                }
                Console.ReadLine();
            }
            else if (option == "5")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please try again.");
                Console.ReadLine();
            }
        }
    }
}
