﻿using Microsoft.EntityFrameworkCore;

namespace mercuryfireTest.Data;

public class DBContext:DbContext
{
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
}