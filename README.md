# RX MEDO CARD

![RX MEDO CARD Logo](rx-medocard/RxMedoWeb/wwwroot/images/logo.png)

> **WHERE HEALTH MEETS SAVINGS**

## About the Project

RX MEDO CARD is a healthcare membership platform developed as an initiative of RX MEDICAL TRUST. The platform connects patients with healthcare providers, offering discounted rates on various medical services through membership plans.

## Website Components

### Main Pages

| Page | Description |
|------|-------------|
| **Home** | Landing page showcasing the main features and benefits |
| **Offers** | Displays membership plans and healthcare service offers |
| **Hospitals** | Lists partner hospitals and healthcare facilities |
| **OPD** | Information about outpatient department services |
| **Pharmacy** | Details about pharmacy services and discounts |
| **Diagnostics** | Booking system for diagnostic tests |
| **Membership** | Registration form for new members |

### Admin Dashboard

A comprehensive healthcare management system that enables administrators to manage diagnostic bookings and membership applications.

#### Default Admin Credentials
```
Username: admin
Password: admin123
```
> **Note**: It is strongly recommended to change these credentials after first login for security purposes.

#### Admin Features

- **Dashboard Overview**: Real-time statistics for diagnostic bookings and memberships
- **Diagnostic Bookings Management**: Track patient details and booking status
- **Membership Management**: Monitor application status and member information
- **Security Features**: Protected routes with role-based authentication

## Features

- **Membership Plans**: Various healthcare membership options with different benefits
- **Hospital Network**: Information about partner hospitals and healthcare providers
- **Diagnostic Test Booking**: Online booking system for medical tests
- **Pharmacy Discounts**: Special rates on medications for members
- **OPD Services**: Discounted outpatient department consultations
- **Mobile-Responsive Design**: Optimized for all device sizes

## Technical Stack

- **Framework**: ASP.NET Core MVC (.NET 8.0)
- **Database**: MySQL with Entity Framework Core
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5, Bootstrap Icons, Font Awesome
- **Animations**: Custom CSS animations
- **Responsive Design**: Mobile-first approach
- **Authentication**: Role-based (Admin)

## UI Components

- Animated dashboard cards
- Responsive sidebar navigation
- Status badges for visual feedback
- Auto-dismissible alerts
- Mobile-optimized layouts

## Project Structure

```
RxMedoWeb/
├── Controllers/
│   └── AdminController.cs    # Admin dashboard logic
├── Views/
│   ├── Admin/
│   │   ├── Dashboard.cshtml  # Main dashboard view
│   │   └── DiagnosticBookings.cshtml
│   └── Shared/
│       └── _AdminLayout.cshtml
├── wwwroot/
│   ├── css/
│   └── js/
└── Data/
    └── ApplicationDbContext.cs
```

## Recent Updates

- Footer department links (cardiology, etc.) now route to the offers section
- Improved navigation and user experience
- Enhanced mobile responsiveness
- Added diagnostic booking functionality
- Updated membership registration process

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- MySQL Server
- Visual Studio 2022 or Visual Studio Code

### Installation

1. Clone the repository
   ```
   git clone https://github.com/nidhisoni22/rx-medocard.git
   ```

2. Update the connection string in `appsettings.json` to point to your MySQL instance

3. Run database migrations
   ```
   dotnet ef database update
   ```

4. Run the application
   ```
   dotnet run
   ```

5. Access Admin Dashboard
   - Navigate to `/Admin/Login`
   - Use the default credentials mentioned above

## Contributors

### Page Design Credits

#### Karan
- Home Page
- Diagnostics Page
- Hospitals Page

#### Nidhi
- Offers Page
- OPD (Outpatient Department) Page
- Pharmacy Page

## Contact Information

- **Address**: Crossing Republic, Ghaziabad, 201016
- **Phone**: +919999307517
- **Email**: RxMedicalTrust@gmail.com

## License

© 2024 - RxMedical Trust - All Rights Reserved
