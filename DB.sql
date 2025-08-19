-- Database
-- Kiểm tra và xóa database nếu tồn tại
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'demo')
BEGIN
    ALTER DATABASE demo SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE demo;
END
GO

-- Tạo database demo
CREATE DATABASE demo;
GO

-- Sử dụng database demo
USE demo;
GO

-- Tạo bảng class (lớp học)
CREATE TABLE class (
    class_id INT IDENTITY(1,1) PRIMARY KEY,
    class_name NVARCHAR(50) NOT NULL,
    class_code NVARCHAR(20) NOT NULL UNIQUE,
    teacher_name NVARCHAR(100),
    room_number NVARCHAR(10),
    created_date DATETIME DEFAULT GETDATE(),
    max_students INT DEFAULT 30
);

-- Tạo bảng system_account (phải tạo trước để làm khóa ngoại)
CREATE TABLE system_account (
    account_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) NOT NULL UNIQUE,
    password_hash NVARCHAR(255) NOT NULL,
    full_name NVARCHAR(100),
    email NVARCHAR(100),
    role NVARCHAR(20) DEFAULT N'User', -- Có thể là Admin, User, Teacher, v.v.
    created_at DATETIME DEFAULT GETDATE(),
    status NVARCHAR(20) DEFAULT N'Active'
);

-- Tạo bảng student (học sinh)
CREATE TABLE student (
    student_id INT IDENTITY(1,1) PRIMARY KEY,
    student_code NVARCHAR(20) NOT NULL UNIQUE,
    full_name NVARCHAR(100) NOT NULL,
    date_of_birth DATE,
    email NVARCHAR(100),
    phone NVARCHAR(15),
    address NVARCHAR(255),
    class_id INT,
    enrollment_date DATETIME DEFAULT GETDATE(),
    status NVARCHAR(20) DEFAULT N'Active',
    
    -- Tạo foreign key constraint
    CONSTRAINT FK_student_class 
        FOREIGN KEY (class_id) REFERENCES class(class_id)
        ON UPDATE CASCADE
        ON DELETE SET NULL
);

-- Tạo bảng product (sản phẩm)
CREATE TABLE product (
    product_id INT IDENTITY(1,1) PRIMARY KEY,
    product_code NVARCHAR(20) NOT NULL UNIQUE,
    product_name NVARCHAR(100) NOT NULL,
    description NVARCHAR(500),
    price DECIMAL(18,2) NOT NULL,
    unit NVARCHAR(20) DEFAULT N'Cái',
    category NVARCHAR(50),
    created_date DATETIME DEFAULT GETDATE(),
    status NVARCHAR(20) DEFAULT N'Active'
);

-- Tạo bảng inventoryProduct (kho sản phẩm)
CREATE TABLE inventoryProduct (
    inventory_id INT IDENTITY(1,1) PRIMARY KEY,
    product_id INT NOT NULL,
    quantity INT NOT NULL DEFAULT 0,
    system_account_id INT,
    warehouse_location NVARCHAR(50),
    last_updated DATETIME DEFAULT GETDATE(),
    notes NVARCHAR(255),
    
    -- Tạo foreign key constraints
    CONSTRAINT FK_inventory_product 
        FOREIGN KEY (product_id) REFERENCES product(product_id)
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    
    CONSTRAINT FK_inventory_account 
        FOREIGN KEY (system_account_id) REFERENCES system_account(account_id)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

-- Tạo bảng transaction (giao dịch kho)
CREATE TABLE [TRANSACTION] (
    transaction_id INT IDENTITY(1,1) PRIMARY KEY,
    inventory_id INT NOT NULL,
    quantity INT NOT NULL,
    system_account_id INT,
    status NVARCHAR(20) DEFAULT N'Pending', -- 'Pending', 'Completed', 'Cancelled'
    
    -- Tạo foreign key constraints
    CONSTRAINT FK_transaction_inventory 
        FOREIGN KEY (inventory_id) REFERENCES inventoryProduct(inventory_id)
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    
    CONSTRAINT FK_transaction_account 
        FOREIGN KEY (system_account_id) REFERENCES system_account(account_id)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

-- Tạo các index để tối ưu hiệu suất
CREATE INDEX IX_student_class_id ON student(class_id);
CREATE INDEX IX_student_full_name ON student(full_name);
CREATE INDEX IX_class_class_code ON class(class_code);
CREATE INDEX IX_product_product_code ON product(product_code);
CREATE INDEX IX_product_product_name ON product(product_name);
CREATE INDEX IX_inventory_product_id ON inventoryProduct(product_id);
CREATE INDEX IX_inventory_account_id ON inventoryProduct(system_account_id);
CREATE INDEX IX_transaction_inventory_id ON [TRANSACTION](inventory_id);
CREATE INDEX IX_transaction_account_id ON [TRANSACTION](system_account_id);
CREATE INDEX IX_transaction_status ON [TRANSACTION](status);
CREATE INDEX IX_system_account_username ON system_account(username);
CREATE INDEX IX_system_account_role ON system_account(role);

-- Chèn dữ liệu mẫu cho bảng system_account (phải chèn trước để làm khóa ngoại)
INSERT INTO system_account (username, password_hash, full_name, email, role)
VALUES 
    ('admin', 'AQAAAAIAAYagAAAAEM5lC8HdvSkZQ+Z06253tIF9vAgjaw++yNQgPymq+grQck3WnfUsyOOAvOTRQK9ZOw==', N'Quản trị viên', 'admin@email.com', 'Admin'),
    ('teacher01', 'AQAAAAIAAYagAAAAEM5lC8HdvSkZQ+Z06253tIF9vAgjaw++yNQgPymq+grQck3WnfUsyOOAvOTRQK9ZOw==', N'Nguyễn Văn An', 'an.nv@email.com', 'Teacher'),
    ('user01', 'AQAAAAIAAYagAAAAEM5lC8HdvSkZQ+Z06253tIF9vAgjaw++yNQgPymq+grQck3WnfUsyOOAvOTRQK9ZOw==', N'Nguyễn Văn User', 'user01@email.com', 'User'),
    ('manager01', 'AQAAAAIAAYagAAAAEM5lC8HdvSkZQ+Z06253tIF9vAgjaw++yNQgPymq+grQck3WnfUsyOOAvOTRQK9ZOw==', N'Trần Thị Manager', 'manager01@email.com', 'Manager');

-- Chèn dữ liệu mẫu cho bảng class
INSERT INTO class (class_name, class_code, teacher_name, room_number, max_students) 
VALUES 
    (N'Lớp 10A1', '10A1', N'Nguyễn Văn An', 'A101', 35),
    (N'Lớp 10A2', '10A2', N'Trần Thị Bình', 'A102', 35),
    (N'Lớp 11B1', '11B1', N'Lê Văn Cường', 'B201', 30),
    (N'Lớp 12C1', '12C1', N'Phạm Thị Dung', 'C301', 32);

-- Chèn dữ liệu mẫu cho bảng student
INSERT INTO student (student_code, full_name, date_of_birth, email, phone, address, class_id) 
VALUES 
    ('SV001', N'Nguyễn Văn Hùng', '2008-05-15', 'hung.nv@email.com', '0123456789', N'123 Đường ABC, Quận 1, TP.HCM', 1),
    ('SV002', N'Trần Thị Lan', '2008-08-20', 'lan.tt@email.com', '0123456790', N'456 Đường DEF, Quận 2, TP.HCM', 1),
    ('SV003', N'Lê Văn Minh', '2008-03-10', 'minh.lv@email.com', '0123456791', N'789 Đường GHI, Quận 3, TP.HCM', 1),
    ('SV004', N'Phạm Thị Nga', '2007-12-25', 'nga.pt@email.com', '0123456792', N'321 Đường JKL, Quận 4, TP.HCM', 2),
    ('SV005', N'Hoàng Văn Phong', '2007-07-18', 'phong.hv@email.com', '0123456793', N'654 Đường MNO, Quận 5, TP.HCM', 2),
    ('SV006', N'Vũ Thị Quỳnh', '2006-11-30', 'quynh.vt@email.com', '0123456794', N'987 Đường PQR, Quận 6, TP.HCM', 3),
    ('SV007', N'Đỗ Văn Sơn', '2005-09-05', 'son.dv@email.com', '0123456795', N'147 Đường STU, Quận 7, TP.HCM', 4);

-- Chèn dữ liệu mẫu cho bảng product
INSERT INTO product (product_code, product_name, description, price, unit, category) 
VALUES 
    ('PRD001', N'Bút bi xanh', N'Bút bi màu xanh, chất lượng cao', 5000.00, N'Cái', N'Văn phòng phẩm'),
    ('PRD002', N'Sổ tay A5', N'Sổ tay kích thước A5, 200 trang', 25000.00, N'Cuốn', N'Văn phòng phẩm'),
    ('PRD003', N'Thước kẻ 30cm', N'Thước kẻ nhựa trong suốt 30cm', 8000.00, N'Cái', N'Dụng cụ học tập'),
    ('PRD004', N'Máy tính Casio', N'Máy tính khoa học Casio FX-580VN', 350000.00, N'Cái', N'Thiết bị điện tử'),
    ('PRD005', N'Bộ compas', N'Bộ dụng cụ vẽ hình học đầy đủ', 45000.00, N'Bộ', N'Dụng cụ học tập'),
    ('PRD006', N'Bút chì 2B', N'Bút chì gỗ 2B, hộp 12 cái', 15000.00, N'Hộp', N'Văn phòng phẩm'),
    ('PRD007', N'Tẩy trắng', N'Tẩy trắng cao su, không độc hại', 3000.00, N'Cái', N'Văn phòng phẩm'),
    ('PRD008', N'Keo dán UHU', N'Keo dán đa năng 40ml', 12000.00, N'Tuýp', N'Văn phòng phẩm');

-- Chèn dữ liệu mẫu cho bảng inventoryProduct
INSERT INTO inventoryProduct (product_id, quantity, system_account_id, warehouse_location, notes) 
VALUES 
    (1, 500, 1, N'Kho A - Kệ 1', N'Hàng mới nhập'),
    (2, 200, 1, N'Kho A - Kệ 2', N'Còn đủ hàng'),
    (3, 150, 2, N'Kho B - Kệ 1', N'Cần bổ sung'),
    (4, 50, 2, N'Kho B - Kệ 3', N'Hàng cao cấp'),
    (5, 80, 1, N'Kho A - Kệ 3', N'Hàng bán chạy'),
    (6, 300, 3, N'Kho C - Kệ 1', N'Hàng học sinh dùng nhiều'),
    (7, 400, 3, N'Kho C - Kệ 2', N'Hàng giá rẻ'),
    (8, 120, 4, N'Kho D - Kệ 1', N'Hàng chất lượng tốt');

-- Chèn dữ liệu mẫu cho bảng transaction
INSERT INTO [TRANSACTION] (inventory_id, quantity, system_account_id, status) 
VALUES 
    (1, 100, 1, 'Completed'),
    (2, -20, 2, 'Completed'),
    (3, 50, 1, 'Completed'),
    (4, -5, 2, 'Pending'),
    (5, -2, 1, 'Completed'),
    (6, 200, 3, 'Completed'),
    (7, -50, 3, 'Completed'),
    (8, 80, 4, 'Pending');

-- Hiển thị thông tin tổng quan
PRINT 'Database demo đã được tạo thành công!';
PRINT 'Các bảng đã được tạo với dữ liệu mẫu:';
PRINT '- class';
PRINT '- system_account';
PRINT '- student';
PRINT '- product';
PRINT '- inventoryProduct';
PRINT '- transaction';
