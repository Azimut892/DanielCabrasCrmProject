# Mircrosoft Dynamics CRM Project

A comprehensive Microsoft Dynamics 365 CRM solution demonstrating advanced plugin development, custom business logic implementation, and role-based security controls.

## Project Overview

This project was developed as a practical exploration of Dynamics 365 customization capabilities, focusing on creating a robust data management system with hierarchical relationships between Header and Line entities. The solution showcases modern CRM development practices including service-oriented architecture, comprehensive security controls, and automated business processes.

## Key Features

### 🔐 Advanced Security Controls
- **Role-Based Access Control**: Implements granular permission checks for sensitive operations
- **Status Change Protection**: Prevents unauthorized modifications to closed/inactive records
- **User Role Validation**: Validates user permissions against specific security roles

### 📊 Entity Relationship Management
- **Header-Line Architecture**: Parent-child relationship with automated status synchronization
- **State Distribution**: Automatic propagation of state and status changes from headers to associated lines
- **Data Integrity**: Ensures consistency across related records

### ⚡ Business Process Automation
- **Record Duplication**: One-click record copying functionality with selective attribute exclusion
- **Status Synchronization**: Automatic alignment of child records with parent status changes
- **Validation Rules**: Real-time form validation with user-friendly notifications

### 🌐 Client-Side Enhancements
- **Dynamic Form Controls**: JavaScript-based form validation and user experience improvements
- **Custom Ribbon Actions**: Integration with Dynamics 365 command bar for enhanced functionality
- **Progress Indicators**: User feedback during long-running operations

## Technical Architecture

### Backend Components

#### Plugins
- **CheckRolesUser**: Security validation plugin ensuring only authorized users can modify protected records
- **CopyHeaderButton**: Custom action for duplicating header records with business rule compliance
- **HeaderStatusReasonDistribution**: Automated status synchronization between parent and child entities

#### Services Layer
- **HeaderService**: Centralized header entity operations and data retrieval
- **LineService**: Line entity management and relationship handling
- **SecurityRoleService**: Role-based security validation and query building

#### Interfaces
- Clean separation of concerns through well-defined interfaces
- Dependency injection ready architecture
- Testable and maintainable code structure

### Frontend Components

#### JavaScript Modules
- **Account Validation**: Real-time form validation with contextual messaging
- **Record Copy Service**: Client-side integration with custom CRM actions
- **Status Reason Checks**: Dynamic form behavior based on record state

## Project Structure

```
DanielCabrasCrmProject/
├── Interfaces/               # Service contracts and abstractions
│   ├── IHeaderService.cs
│   ├── ILineService.cs
│   └── ISecurityRoleInterface.cs
├── Services/                 # Business logic implementations
│   ├── HeaderService.cs
│   ├── LineService.cs
│   └── SecurityRoleService.cs
├── Plugins/                  # CRM plugin implementations
│   ├── CheckRolesUser.cs
│   ├── CopyHeaderButton.cs
│   └── HeaderStatusReasonDistribution.cs
├── JavaScript/               # Client-side enhancements
│   ├── CheckStatusReasonAccount.js
│   └── CopyRecordServiceButton.js
└── EarlyBound/              # Generated entity classes and metadata
```

## Development Approach

This project demonstrates a comprehensive understanding of:

- **Microsoft Dynamics 365 SDK**: Extensive use of CRM SDK for server-side development
- **Plugin Architecture**: Event-driven plugin development with proper error handling
- **Service Layer Pattern**: Clean architecture with separated concerns
- **Security Best Practices**: Implementation of role-based access controls
- **Client-Server Integration**: Seamless integration between JavaScript and server-side components

## Technical Specifications

- **Framework**: .NET Framework 4.7.1
- **CRM SDK Version**: 9.0.2.59
- **Development Environment**: Visual Studio 2017+
- **Deployment**: Assembly signing with strong name key
- **Package Management**: NuGet package restoration

## Business Value

The solution addresses common enterprise CRM challenges:

1. **Data Governance**: Ensures data integrity through automated validation and synchronization
2. **User Experience**: Provides intuitive interfaces with real-time feedback
3. **Security Compliance**: Implements enterprise-grade security controls
4. **Process Efficiency**: Automates routine tasks and reduces manual errors
5. **Scalability**: Service-oriented architecture supports future enhancements

## Getting Started

### Prerequisites
- Microsoft Dynamics 365 environment
- Visual Studio 2017 or later
- .NET Framework 4.7.1
- CRM SDK references

### Installation
1. Clone the repository
2. Restore NuGet packages
3. Build the solution
4. Deploy plugins to your CRM environment
5. Import JavaScript resources as web resources

### Configuration
- Configure plugin registration for appropriate entities and messages
- Set up security roles as defined in the validation logic
- Register custom actions for record copying functionality

## Contributing

This project serves as a foundation for CRM customization and can be extended with additional business logic, enhanced security features, or integration capabilities.

---

*Developed with Microsoft Dynamics 365 CRM SDK and modern .NET development practices.*
