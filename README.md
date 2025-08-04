# ShareBear – Tools Sharing Platform

**Author**: Makar Shcherbiak  
**Institution**: Polish-Japanese Academy of Computer Technology  

---

## 📘 Overview

**ShareBear** is a full-featured platform designed to promote community-based sharing of work tools. In the spirit of the sharing economy, this project helps reduce costs, limit environmental waste, and increase accessibility to expensive or rarely used tools. It connects individual users and businesses to share or rent tools in a secure, sustainable, and user-friendly environment.

---

## 🎯 Aims and Objectives

### Aims
- To develop a user-friendly platform that connects tool owners and borrowers.
- To support sustainability through shared use of underutilized resources.
- To ensure secure and transparent transactions for all users.

### Objectives
- Enable easy tool listing, searching, and booking.
- Provide a secure payment and transaction flow.
- Include feedback systems for accountability and trust.
- Offer administrative tools for moderation and analytics.
- Implement features for real-time availability and dispute resolution.

---

## 🌐 Context

Tools are often expensive, task-specific, and underused. ShareBear is designed to help freelancers, homeowners, and small businesses reduce costs by borrowing tools when needed. It supports efficient resource use, encourages local cooperation, and serves as a real-world application of the circular economy.

---

## ⚙️ Functional Requirements

### User Management
- User registration and role-based access (owner, borrower, moderator)
- Profile and account management

### Tool Listings
- Add/edit/delete tool entries
- Upload images, set availability, describe usage

### Search & Discovery
- Filtered search by location, price, type, availability

### Booking System
- Calendar availability selection
- Booking confirmations and notifications

### Payment Gateway
- Secure online payments
- Invoicing and receipt generation

### Review System
- User and tool ratings
- Moderated feedback

### Admin Dashboard
- Manage users, disputes, listings, and data insights

---

## 🖼️ Use Case Scenario

**Actor:** Customer  
**Purpose:** Booking a tool  
**Flow Summary:**
1. Customer initiates a booking.
2. System displays tool owner and availability.
3. Date is selected and confirmed.
4. System processes payment and sends confirmation.
5. Owner is notified of the booking.

**Failure Flow:** Payment fails – system retries or halts process if timeout occurs.

---

## 📈 Non-Functional Requirements

- **Performance**: Supports 10,000+ users; < 2s response time.
- **Security**: Encrypted communication and secure authentication.
- **Usability**: Simple, responsive, and accessible UI/UX.

---

## 🗃️ Database Design

Entities:
- **Users** – profile, roles, and activity
- **Tools** – listings, availability, and ownership
- **Borrows** – booking history, multi-tool per session
- **Payments** – payment status, history, and receipts

Relationships:
- One-to-many between Users and Tools
- Many-to-many between Tools and Borrows

---

## 🛠️ Technologies Used

### 🗄️ Database
- **MySQL**: Lightweight, fast, and scalable RDBMS

### 🔧 Backend
- **.NET + Entity Framework**: Fast server-side performance, LINQ integration, scalable
- **NUnit**: Unit testing framework with parallel execution

### 🎨 Frontend
- **React (JavaScript)**: Component-based, responsive, scalable UI

### ☁️ Hosting
- **AWS Cloud**: Scalable, secure, with cross-region backup and failover

---

## 🖌️ Design Features

- **Main Page**: Tool listings with search and filters
- **Sign In / Log In Pages**: Account access and creation
- **Tool Detail Page**: Tool description, availability calendar, pricing
- **Shipment Option Page**: Input for delivery preferences
- **Confirmation Window**: Displays successful transactions and next steps

---

## 🌱 Future Enhancements

- Augmented Reality tool previews  
- Real-time tool tracking  
- Blockchain-based transaction logs  
- Gamification (leaderboards, badges, portraits)  
- Multi-language and multi-currency support

---

## 🌍 Social & Environmental Impact

- Encourages resource sharing and local collaboration
- Reduces waste from unused tools and overproduction
- Supports circular economy models (potential to extend into car sharing, office rentals, etc.)


## 👤 Author

**Makar Shcherbiak**  

