# MWBAYLY - E-Commerce Application

## Overview
MWBAYLY is a modern E-Commerce web application built with **ASP.NET Core MVC**. It provides a robust platform for managing products, categories, companies, and orders, featuring a secure shopping cart and seamless payment integration using **Stripe**.

## Architecture & Design Patterns
The application follows a clean architecture approach, emphasizing **Separation of Concerns** to ensure maintainability, testability, and scalability.

### 1. Repository Pattern
To decouple the business logic from the data access layer, the project implements the **Repository Pattern**. This abstraction allows the application to work with data objects without needing to know how persistence is implemented.

- **Generic Repository (`IRepository<T>`, `Repository<T>`)**: Handles common CRUD operations (Create, Read, Update, Delete) for any entity type, reducing code duplication.
- **Specific Repositories**: Interfaces like `IProductRepository` and `ICategoryRepository` extend the generic repository to implement entity-specific business logic.
- **Dependency Injection**: Repositories are injected into Controllers, adhering to the Dependency Inversion Principle.

### 2. MVC (Model-View-Controller)
The application is structured using the standard MVC pattern:
- **Models**: Represent the data and business logic (e.g., `Product`, `Category`, `Cart`).
- **Views**: Responsible for the user interface and presentation logic using Razor syntax.
- **Controllers**: Handle user requests, manipulate data using Repositories, and render the appropriate Views.

## Key Features

### 🛍️ Product Management
- **CRUD Operations**: Comprehensive management for Products and Categories.
- **Organization**: Products are categorized for easy browsing.
- **Company Management**: Manage associated companies/suppliers.

### 🛒 Shopping Cart & Checkout with Stripe
The application uses **Stripe** for secure payment processing. The integration handles the checkout session creation and user redirection.

**Implementation Details (`CartController.cs`):**
1. **Session Creation**: When the user clicks "Pay", the `Pay` action creates a Stripe `SessionCreateOptions` object.
2. **Line Items**: It iterates through the user's shopping cart (`_cartRepository.GetAll()`) and maps each item to a Stripe `SessionLineItemOptions`.
   - Sets `PriceData` (Currency: EGP, UnitAmount in cents).
   - Sets `ProductData` (Name, Description).
   - Sets `Quantity`.
3. **Redirection**: A `SessionService` creates the session, and the user is redirected to the Stripe hosted checkout page (`session.Url`).
4. **Callbacks**: Stripe redirects the user back to:
   - `SuccessUrl`: `/checkout/success` (Order confirmed).
   - `CancelUrl`: `/checkout/cancel` (Payment cancelled).

```csharp
// Snippet from CartController.cs
var options = new SessionCreateOptions
{
    PaymentMethodTypes = new List<string> { "card" },
    LineItems = new List<SessionLineItemOptions>(), // Populated from Cart
    Mode = "payment",
    SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
    CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
};
```

### 📧 Email Service (IEmailSender)
The application includes an email service used for sending account notifications (e.g., registration confirmation).

**Implementation Details (`EmailSender.cs`):**
- **Interface**: Implements `IEmailSender` (custom interface wrapping `Microsoft.AspNetCore.Identity.UI.Services`).
- **Protocol**: Uses standard **SMTP** via `System.Net.Mail`.
- **Configuration**: Currently configured for Gmail SMTP (`smtp.gmail.com` on port 587).
- **Security**: Uses `EnableSsl = true` for secure transmission.

```csharp
// Snippet from EmailSender.cs
public Task SendEmailAsync(string email, string subject, string message)
{
    var client = new SmtpClient("smtp.gmail.com", 587)
    {
        EnableSsl = true,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential("your-email@gmail.com", "app-password")
    };
    return client.SendMailAsync(new MailMessage(...));
}
```

### 🔐 Authentication & Security
- **Identity System**: Built on top of **ASP.NET Core Identity** for robust user management.
- **Role-Based Access**: Authorization controls to separate Customer and Admin functionalities.
- **Secure Data**: Implementation of best practices for data protection and user privacy.

## Technology Stack
- **Framework**: ASP.NET Core
- **Language**: C#
- **Database**: SQL Server with Entity Framework Core
- **ORM**: Entity Framework Core
- **Payment Gateway**: Stripe API
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap
