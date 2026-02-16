# ERI-Workflow-Business-Operations-Suite-Lite-
ERI Workflow &amp; Business Operations Suite (Lite) An internal enterprise workflow and approval management system built with ASP.NET Core MVC, designed to streamline business operations, request approvals, and administrative oversight within ERI/ERSS.

ERI Workflow & Business Operations Suite (Lite)
Overview

The ERI Workflow & Business Operations Suite (Lite) is an internally developed enterprise workflow management system designed to streamline operational processes within ERI and ERSS.

This application enables structured request submission, approval workflows, role-based access control, and operational visibility through analytics dashboards.

The system replaces manual approval processes with a secure, auditable, and scalable digital workflow solution.

🎯 Purpose

This system was developed to:

Digitize internal request processes

Improve operational transparency

Enforce role-based approval governance

Reduce administrative overhead

Provide real-time workflow visibility

It serves as a foundation for scalable internal business automation at ERI.

🏗 Technology Stack

ASP.NET Core MVC (.NET 8)

Entity Framework Core

SQLite (local file-based database)

ASP.NET Core Identity (Role-based Authentication)

Bootstrap (UI Framework)

Chart.js (Dashboard Analytics)

🔐 Role-Based Access Control

The system includes three primary roles:

Role	Capabilities
Admin	Full system access, analytics, oversight
Manager	Approve / reject requests
Staff	Submit and track requests
📊 Core Features

Secure authentication system

Role-based authorization

Request submission (Leave, IT, Purchase, General)

Document upload support

Approval / rejection workflow

Dashboard with analytics

Auto database migration and seeding

Internal deployment-ready configuration

🗄 Database

SQLite local storage (App_Data/eri_workflow.db)

Automatic migration on application startup

Seeded default users and roles

🚀 Quick Start
dotnet restore
dotnet ef migrations add InitialCreate
dotnet run


The application will automatically:

Create the database

Apply migrations

Seed default users

🔑 Default Credentials (Development Only)

Admin: admin@eri.co.za
 / Admin@123
Manager: manager@eri.co.za
 / Manager@123
Staff: staff@eri.co.za
 / Staff@123

🏢 Deployment

Designed for:

Internal IIS hosting

Azure App Service (Private)

On-premise Windows Server environments

📌 Future Expansion

Email notifications

Multi-department filtering

Advanced reporting

API integration

Audit trail logging

SaaS conversion capability

🎯 Strategic Positioning

This repository demonstrates:

Enterprise application architecture

Workflow automation design

Role-based security implementation

Internal system modernization capability
