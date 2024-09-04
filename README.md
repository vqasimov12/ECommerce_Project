
# WPF E-Commerce Application

This is a desktop-based e-commerce application built using Windows Presentation Foundation (WPF) with MVVM architecture. The application includes functionalities for both customers and administrator, allowing for product browsing, cart management, order processing, and admin-level operations such as product and category management, user statistics, and more.

## Features

### Customer Features
- **Product Browsing**: View a wide range of products with detailed information.
- **Shopping Cart**: Add products to the cart and proceed to checkout.
- **favourite Products**: Add products to favourite List and proceed to checkout.
- **Order Management**: Place orders and view order history.
- **User Profile**: Update personal information and manage the account.
- **Email Notifications**:
  - **Sign-Up Confirmation**: Receive a confirmation email upon successful registration.
  - **Order Confirmation**: Receive detailed email notifications after purchasing products, including product details and order summary.
- **Download Invoice**: Download a detailed invoice of the order in PDF format directly from the application.

### Admin Features
- **Product Management**: Create, edit, and delete products.
- **Category Management**: Add, edit, and remove categories.
- **User Management**: View users .
- **Statistics Dashboard**: View sales, user statistics, and income reports.
- **Download Reports**: Download financial reports and order receipts in PDF format.

## Technologies Used

- **WPF**: Windows Presentation Foundation for building the user interface.
- **MVVM**: Model-View-ViewModel architectural pattern for separation of concerns.
- **Entity Framework Core**: For database operations and data persistence.
- **Material Design in XAML**: Provides modern UI components and theming.
- **Syncfusion**: For advanced UI components like rating controls.
- **Cloudinary**: Used for image storage and management.
- **Email Service**: For sending sign-up confirmations and order receipts to users, implemented using SMTP.
- **PDF Generation**: Functionality for generating and downloading invoices and reports in PDF format.

## Prerequisites

- **.NET 8.0**: Ensure you have the latest .NET SDK installed.
- **SQL Server**: Database for storing application data.
- **SMTP Service**: Set up an SMTP server or account for sending emails (e.g., SendGrid, Gmail).
- **Visual Studio**: Recommended IDE for building and running the application.

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/vqasimov12/ECommerce_Project.git
```

### Setup the Database

1. Update the connection string in `appsettings.json`.
2. Run database migrations to set up the database schema:
   ```bash
   dotnet ef database update
   ```

## Usage

### Customer Flow

1. **Sign Up**: Register a new account and receive a confirmation email.
2. **Browse Products**: Start by exploring the product catalog.
3. **Add Filter to Products**: Get more exact search result.
4. **Add to Cart**: Select items and add them to your cart.
5. **Add to Favourites**: Select items and add them to your Favourite Products.
6. **Checkout**: Proceed to payment and complete your order.
7. **Receive Order Email**: Get a detailed order confirmation via email, including product details and order summary.
8. **Download Invoice**: Download your order invoice as a PDF from the application.
9. **View Orders**: Check your order history and status.

### Admin Flow

1. **Login as Admin**: Access the admin panel using admin credentials.
2. **Manage Products**: Add new products or update existing ones.
3. **Manage Categories**: Add new category or update existing ones.
4. **View Statistics**: Use the admin dashboard to monitor sales and user activity.
5. **Download Reports**: Export financial reports and order receipts as PDFs.

## Project Structure

- **Views**: XAML files defining the UI.
- **ViewModels**: Handles UI logic and data binding.
- **Models**: Represents the data structure of the application.
- **Services**: Contains business logic, data access, email, PDF generation, and other services.


## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

# Default Accounts

For ease of testing and initial setup, the following default accounts are provided. Please change these credentials immediately after the initial setup for security purposes.

## Admin Account

- **Username**: admin@gmail.com
- **Password**: admin123

## User Account

- **Username**: qasimov.vaqif512@gmail.com
- **Password**: user123



> **Note**: These default accounts are intended for development and testing purposes only. Ensure to update the passwords and manage accounts securely in a production environment.
