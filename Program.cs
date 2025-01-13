using System;
using System.Collections.Generic;

namespace OnlineShoppingCart
{
    // Product Class
    public class Product
    {
        private int ProductID;
        private string Name;
        private decimal Price;
        private int QuantityInStock;

        public Product(int productID, string name, decimal price, int quantityInStock)
        {
            ProductID = productID;
            Name = name;
            Price = price;
            QuantityInStock = quantityInStock;
        }

        public int GetProductID() => ProductID;
        public string GetName() => Name;
        public decimal GetPrice() => Price;
        public int GetQuantityInStock() => QuantityInStock;

        public void ReduceStock(int quantity)
        {
            if (QuantityInStock >= quantity)
                QuantityInStock -= quantity;
            else
                Console.WriteLine($"Insufficient stock for {Name}. Only {QuantityInStock} left.");
        }

        public void ReplenishStock(int quantity)
        {
            QuantityInStock += quantity;
        }

        public bool CanAddToCart(int quantity)
        {
            return QuantityInStock >= quantity;
        }
    }

    // Cart Class
    public class Cart
    {
        private List<Product> Products;

        public Cart()
        {
            Products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
            Console.WriteLine($"{product.GetName()} has been added to the cart.");
        }

        public bool RemoveProduct(Product product)
        {
            if (Products.Contains(product))
            {
                Products.Remove(product);
                Console.WriteLine($"{product.GetName()} has been removed from the cart.");
                return true;
            }
            else
            {
                Console.WriteLine($"{product.GetName()} is not in the cart.");
                return false;
            }
        }

        public decimal GetTotal()
        {
            decimal total = 0;
            foreach (Product product in Products)
                total += product.GetPrice();
            return total;
        }

        public void DisplayCartContents()
        {
            Console.WriteLine("Cart Contents:");
            if (Products.Count == 0)
            {
                Console.WriteLine("Your cart is empty.");
            }
            else
            {
                foreach (Product product in Products)
                {
                    Console.WriteLine($"- {product.GetName()}, ${product.GetPrice()}");
                }
                Console.WriteLine($"Total: ${GetTotal()}");
            }
        }

        public bool IsProductInCart(Product product)
        {
            return Products.Contains(product);
        }
    }

    // Customer Class
    public class Customer
    {
        private int CustomerID;
        private string Name;
        private Cart Cart;

        public Customer(int customerID, string name)
        {
            CustomerID = customerID;
            Name = name;
            Cart = new Cart();
        }

        public void ViewCart()
        {
            Cart.DisplayCartContents();
        }

        public void AddToCart(Product product)
        {
            if (product.CanAddToCart(1)) // Check if stock is available for 1 unit
            {
                Cart.AddProduct(product);
                product.ReduceStock(1);
            }
            else
            {
                Console.WriteLine($"Cannot add {product.GetName()} to the cart. Not enough stock.");
            }
        }

        public void RemoveFromCart(Product product)
        {
            if (Cart.IsProductInCart(product) && Cart.RemoveProduct(product))
            {
                product.ReplenishStock(1); // Replenish stock when removed from the cart
            }
            else
            {
                Console.WriteLine($"{product.GetName()} cannot be removed because it is not in the cart.");
            }
        }

        public void Checkout()
        {
            decimal total = Cart.GetTotal();
            if (total > 0)
            {
                Console.WriteLine($"Checkout complete. Total amount due: ${total}");
                Cart = new Cart();
            }
            else
            {
                Console.WriteLine("Your cart is empty. Add items to checkout.");
            }
        }
    }

    // Main Program
    public class Program
    {
        public static void Main(string[] args)
        {
            Product apple = new Product(1, "Apple", 0.5m, 50);
            Product bread = new Product(2, "Bread", 1.5m, 20);
            Product milk = new Product(3, "Milk", 2.0m, 20);

            Customer customer = new Customer(101, "Sergiu Contu");

            Console.WriteLine("Welcome to the online store!");

            Console.WriteLine("\nAvailable products:");
            Console.WriteLine($"1. {apple.GetName()} - ${apple.GetPrice()}");
            Console.WriteLine($"2. {bread.GetName()} - ${bread.GetPrice()}");
            Console.WriteLine($"3. {milk.GetName()} - ${milk.GetPrice()}");

            while (true)
            {
                Console.WriteLine("\nEnter the number of the product to add, or enter a negative number to remove a product (e.g., -1 to remove Apple). Enter 0 to view cart/checkout:");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            customer.AddToCart(apple);
                            break;
                        case 2:
                            customer.AddToCart(bread);
                            break;
                        case 3:
                            customer.AddToCart(milk);
                            break;
                        case -1:
                            customer.RemoveFromCart(apple);
                            break;
                        case -2:
                            customer.RemoveFromCart(bread);
                            break;
                        case -3:
                            customer.RemoveFromCart(milk);
                            break;
                        case 0:
                            Console.WriteLine("\n1. View Cart");
                            Console.WriteLine("2. Checkout");
                            Console.WriteLine("3. Continue Shopping");
                            string cartChoice = Console.ReadLine();
                            if (int.TryParse(cartChoice, out int cartAction))
                            {
                                switch (cartAction)
                                {
                                    case 1:
                                        customer.ViewCart();
                                        break;
                                    case 2:
                                        customer.Checkout();
                                        Console.WriteLine("\nThank you for shopping with us!");
                                        return;
                                    case 3:
                                        break;
                                    default:
                                        Console.WriteLine("Invalid choice.");
                                        break;
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid product number.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
        }
    }
}


