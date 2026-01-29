// =============================================================================
// MongoDB Initialization Script
// Creates initial database structure and indexes
// =============================================================================

// Switch to senado database
db = db.getSiblingDB('senado');

// =============================================================================
// Create Collections
// =============================================================================

// Users collection (application users who manage the system)
db.createCollection('users');

// People collection (people who request help)
db.createCollection('people');

// Solicitations collection (help requests)
db.createCollection('solicitations');

// Budgets collection (office budgets)
db.createCollection('budgets');

// Audit log collection
db.createCollection('auditLogs');

// =============================================================================
// Create Indexes
// =============================================================================

// Users indexes
db.users.createIndex({ "email": 1 }, { unique: true });
db.users.createIndex({ "username": 1 }, { unique: true });
db.users.createIndex({ "role": 1 });
db.users.createIndex({ "isActive": 1 });

// People indexes
db.people.createIndex({ "dni": 1 }, { unique: true });
db.people.createIndex({ "email": 1 });
db.people.createIndex({ "lastName": 1, "firstName": 1 });
db.people.createIndex({ "createdAt": -1 });

// Solicitations indexes
db.solicitations.createIndex({ "personId": 1 });
db.solicitations.createIndex({ "status": 1 });
db.solicitations.createIndex({ "officeId": 1 });
db.solicitations.createIndex({ "createdAt": -1 });
db.solicitations.createIndex({ "createdBy": 1 });

// Budgets indexes
db.budgets.createIndex({ "officeId": 1 }, { unique: true });
db.budgets.createIndex({ "year": 1 });

// Audit logs indexes
db.auditLogs.createIndex({ "timestamp": -1 });
db.auditLogs.createIndex({ "userId": 1 });
db.auditLogs.createIndex({ "action": 1 });

// =============================================================================
// Insert Sample Data (Development Only)
// =============================================================================

// Sample admin user (password: Admin123!)
db.users.insertOne({
    _id: ObjectId(),
    username: "admin",
    email: "admin@senado.gob.ar",
    passwordHash: "$2a$11$K5KFqPqFqPqFqPqFqPqFqu", // Change in production!
    firstName: "Administrador",
    lastName: "Sistema",
    role: "Admin",
    isActive: true,
    createdAt: new Date(),
    updatedAt: new Date()
});

// Sample offices/budgets
const offices = [
    { name: "Despacho Principal", code: "DP001" },
    { name: "Secretaría General", code: "SG001" },
    { name: "Asuntos Sociales", code: "AS001" }
];

offices.forEach(office => {
    db.budgets.insertOne({
        _id: ObjectId(),
        officeId: office.code,
        officeName: office.name,
        year: new Date().getFullYear(),
        totalBudget: 1000000,
        usedBudget: 0,
        availableBudget: 1000000,
        createdAt: new Date(),
        updatedAt: new Date()
    });
});

print('MongoDB initialization completed successfully!');
